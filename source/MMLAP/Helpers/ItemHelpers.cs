using Archipelago.Core.Util;
using MMLAP.Models;

namespace MMLAP.Helpers
{
    public class ItemHelpers
    {
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
            int busterInvLength = Addresses.UnequippedBusterInvStart.ByteLength ?? 34;
            for (uint i = 0; i < busterInvLength; i++)
            {
                ulong busterInvSlot = busterInv + i;
                byte invSlotVal = Memory.ReadByte(busterInvSlot);
                if (invSlotVal == 0)
                {
                    // Offset of 1 is intended for buster part code conversion
                    Memory.Write(busterInvSlot, (itemData.ItemCode ?? -1) + 1);
                    return;
                }
            }
            // If buster inventory is full then do nothing
            return;
        }

        public static void ReceiveSpecialItem(ItemData itemData)
        {
            _ = Memory.WriteBit(itemData.InventoryAddressData.Address, itemData.InventoryAddressData.BitNumber ?? 0, true);
            return;
        }

        public static void ReceiveNormalItem(ItemData itemData)
        {
            _ = Memory.WriteBit(itemData.InventoryAddressData.Address, itemData.InventoryAddressData.BitNumber ?? 0, true);
            return;
        }
    }
}
