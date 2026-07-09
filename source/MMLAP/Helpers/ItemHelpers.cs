using Archipelago.Core.Util;
using MMLAP.Models;
using System.Linq;
using static MMLAP.Models.MMLEnums;

namespace MMLAP.Helpers
{
    public class ItemHelpers
    {
        public static bool HasBusterPart(ItemData busterItemData)
        {
            if (busterItemData.Category != ItemCategory.Buster || busterItemData.ItemCode == null)
            {
                return false;
            }

            // Check equipped buster slots
            byte equippedBusterPart1 = Memory.ReadByte(Addresses.EquippedBusterPart1.Address);
            byte equippedBusterPart2 = Memory.ReadByte(Addresses.EquippedBusterPart2.Address);
            byte equippedBusterPart3 = Memory.ReadByte(Addresses.EquippedBusterPart3.Address);
            // Offset of 1 is intended for buster part code conversion
            byte itemCode = (byte)(busterItemData.ItemCode.Value + 1);

            if (itemCode == equippedBusterPart1 ||
                itemCode == equippedBusterPart2 ||
                itemCode == equippedBusterPart3)
            {
                return true;
            }

            // Check unequipped inventory
            ulong busterInv = Addresses.UnequippedBusterInvStart.Address;
            int busterInvLength = Addresses.UnequippedBusterInvStart.ByteLength ?? 34;
            for (uint i = 0; i < busterInvLength; i++)
            {
                ulong busterInvSlot = busterInv + i;
                byte invSlotVal = Memory.ReadByte(busterInvSlot);
                if (invSlotVal == itemCode)
                {
                    return true;
                }
            }

            return false;
        }

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
                case MMLEnums.ItemCategory.SpecialItem:
                    ReceiveSpecialItem(itemData);
                    break;
                case MMLEnums.ItemCategory.Normal:
                    ReceiveNormalItem(itemData);
                    break;
                case MMLEnums.ItemCategory.SpecialWeapon:
                    ReceiveSpecialWeapon(itemData);
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
            if (itemData.Category != ItemCategory.Zenny)
            {
                return;
            }
            uint oldZenny = Memory.ReadUInt(Addresses.CurrentZenny.Address);
            uint amountReceived = itemData.Quantity;
            uint newZenny = ((oldZenny + amountReceived) < oldZenny) ? uint.MaxValue : oldZenny + amountReceived;
            _ = Memory.Write(Addresses.CurrentZenny.Address, newZenny);
            return;
        }

        public static void ReceiveBusterPart(ItemData itemData)
        {
            if (itemData.Category != ItemCategory.Buster || itemData.ItemCode == null)
            {
                return;
            }

            // Prevent giving duplicate item if buster part is already equipped or in inventory
            if (HasBusterPart(itemData))
            {
                return;
            }

            ulong busterInv = Addresses.UnequippedBusterInvStart.Address;
            int busterInvLength = Addresses.UnequippedBusterInvStart.ByteLength ?? 34;
            ulong? busterInvSlotWrite = null;
            for (uint i = 0; i < busterInvLength; i++)
            {
                ulong busterInvSlot = busterInv + i;
                byte invSlotVal = Memory.ReadByte(busterInvSlot);
                if (invSlotVal == 0 && busterInvSlotWrite == null)
                {
                    // Assign the first empty slot for writing
                    busterInvSlotWrite = busterInvSlot;
                }
            }

            if (busterInvSlotWrite != null)
            {
                // Offset of 1 is intended for buster part code conversion
                Memory.WriteByte(busterInvSlotWrite.Value, (byte)(itemData.ItemCode.Value + 1));
            }
            // If buster inventory is full then do nothing
            return;
        }

        public static void ReceiveSpecialItem(ItemData itemData)
        {
            if (itemData.Category != ItemCategory.SpecialItem)
            {
                return;
            }
            _ = MemoryHelpers.WriteAddressDataBit(itemData.InventoryAddressData, true);
            return;
        }

        public static void ReceiveNormalItem(ItemData itemData)
        {
            if (itemData.Category != ItemCategory.Normal)
            {
                return;
            }
            _ = MemoryHelpers.WriteAddressDataBit(itemData.InventoryAddressData, true);
            return;
        }

        public static void ReceiveSpecialWeapon(ItemData itemData)
        {
            if (itemData.Category != ItemCategory.SpecialWeapon)
            {
                return;
            }
            _ = MemoryHelpers.WriteAddressDataBit(itemData.InventoryAddressData, true);
            return;
        }
    }
}
