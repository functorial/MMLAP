using Archipelago.Core.Util;
using MMLAP.Models;
using System.Linq;

namespace MMLAP.Helpers
{
    public class ItemHelpers
    {
        public static bool HasReceivedItem(int itemId)
        {
            return App.APClient?.CurrentSession?.Items?.AllItemsReceived.Any(item => item.ItemId == itemId) ?? false;
        }

        public static void ReceiveGenericItem(ItemData itemData)
        {
            switch (itemData.Category)
            {
                case MMLEnums.ItemCategory.Nothing:
                    ReceiveNothing(itemData);
                    break;
                case MMLEnums.ItemCategory.Zenny:
                    ReceiveZenny(itemData);
                    break;
                case MMLEnums.ItemCategory.Buster:
                    ReceiveBusterPart(itemData);
                    break;
                case MMLEnums.ItemCategory.Special:
                    ReceiveSpecialItem(itemData);
                    break;
                case MMLEnums.ItemCategory.Normal:
                    ReceiveNormalItem(itemData);
                    break;
                default:
                    return;
            }
        }
        public static void ReceiveNothing(ItemData itemData)
        {
            return;
        }

        public static void ReceiveZenny(ItemData itemData)
        {
            uint oldZenny = Memory.ReadUInt(Addresses.CurrentZenny.Address);
            uint amountReceived = itemData.Quantity;
            uint newZenny = ((oldZenny + amountReceived) < oldZenny) ? uint.MaxValue : oldZenny + amountReceived;
            _ = Memory.Write(Addresses.CurrentZenny.Address, newZenny);
            return;
        }

        public static void ReceiveBusterPart(ItemData itemData)
        {
            ulong busterInv = Addresses.UnequippedBusterInvStart.Address;
            int busterInvLength = Addresses.UnequippedBusterInvStart.ByteLength??34;
            ulong? busterInvSlotWrite = null;
            for (uint i = 0; i < busterInvLength; i++)
            {
                ulong busterInvSlot = busterInv + i;
                byte invSlotVal = Memory.ReadByte(busterInvSlot);
                if (
                    invSlotVal == 0 &&
                    busterInvSlotWrite == null
                )
                {
                    // Assign the first empty slot for writing but keep looking in case the item is already in the inventory
                    busterInvSlotWrite = busterInvSlot;
                }
                if(invSlotVal == (itemData.ItemCode??-1)+1)
                {
                    // If the item is already in the inventory then do nothing
                    return;
                }
            }
            if (busterInvSlotWrite != null && itemData.ItemCode != null)
            {
                // Offset of 1 is intended for buster part code conversion
                Memory.WriteByte(busterInvSlotWrite.Value, (byte)(itemData.ItemCode.Value + 1));
            }
            // If buster inventory is full then do nothing
            return;
        }

        public static void ReceiveSpecialItem(ItemData itemData)
        {
            _ = MemoryHelpers.WriteAddressDataBit(itemData.InventoryAddressData, true);
            return;
        }

        public static void ReceiveNormalItem(ItemData itemData)
        {
            _ = MemoryHelpers.WriteAddressDataBit(itemData.InventoryAddressData, true);
            return;
        }
    }
}
