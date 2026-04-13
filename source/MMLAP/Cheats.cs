using MMLAP.Helpers;
using MMLAP.Models;
using System;

namespace MMLAP
{
    public class Cheats
    {
        public static OpCode[] FastForwardCardonSubgate(bool hasTakenYellowRefractor)
        {
            // May be written slowly as it is checked during in-game loop
            int fastForwardState = hasTakenYellowRefractor ? 0x04 : 0x03;
            return
            [
                LoadHalfImmediate(0x00100320, MMLEnums.Register.v1, (byte)fastForwardState),
                LoadHalfImmediate(0x0010E7FC, MMLEnums.Register.v1, (byte)fastForwardState),
                LoadHalfImmediate(0x0010E8B8, MMLEnums.Register.v1, (byte)fastForwardState),
            ];
        }

        public static OpCode[] EnableDoorsInsideJyunSubgate(byte minProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadHalfImmediate(0x001003F8, MMLEnums.Register.v1, Math.Max((byte)0x06, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsInsideClozerSubgate(byte minProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadHalfImmediate(0x00100574, MMLEnums.Register.v1, Math.Max((byte)0x07, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsOutsideCardonSubgate(byte minProgressionCounter, bool hasDefeatedFerdinand, bool hasCompletedCardonTankEvent)
        {
            // Needs to be written fast during loading screen
            int fastForwardState = !hasDefeatedFerdinand ? 0x00 : (hasCompletedCardonTankEvent ? 0x04 : 0x03);
            return
            [
                LoadHalfImmediate(0x00100E04, MMLEnums.Register.a1, Math.Max((byte)0x04, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsCardonForestFlutterBroken(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return [
                LoadHalfImmediate(0x001007E0, MMLEnums.Register.a1, Math.Max((byte)0x01, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsAppleMarket(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return [
                LoadHalfImmediate(0x00100474, MMLEnums.Register.a1, Math.Max((byte)0x01, minProgressionCounter)),
            ];
        }

        public static OpCode[] FastForwardDowntown(byte minProgressionCounter, bool subCitiesAreSurfaced)
        {
            // Needs to be written fast during loading screen
            OpCode[] code = subCitiesAreSurfaced ?
            [
                // Enable Sub-City
                LoadHalfImmediate(0x00106BB0, MMLEnums.Register.v0, 0x09),
                // Open doors
                LoadHalfImmediate(0x00106B50, MMLEnums.Register.a1, Math.Max((byte)0x09, minProgressionCounter)),
                // Opens doors during Tron / dog scene
                LoadHalfImmediate(0x00106B68, MMLEnums.Register.v0, Math.Max((byte)0x09, minProgressionCounter)),
            ] :
            [
                // Open doors
                LoadHalfImmediate(0x00106B50, MMLEnums.Register.a1, Math.Max((byte)0x02, minProgressionCounter)),
                // Opens doors during Tron / dog scene
                LoadHalfImmediate(0x00106B68, MMLEnums.Register.v0, Math.Max((byte)0x02, minProgressionCounter)), 

            ];
            return code;
        }

        public static OpCode[] FastForwardOldCity(byte minProgressionCounter)
        {
            return 
            [
                LoadHalfImmediate(0x001007D0, MMLEnums.Register.v1, Math.Max((byte)0x09, minProgressionCounter))
            ];
        }

        public static OpCode[] FastForwardUptown(byte minProgressionCounter, bool subCitiesAreSurfaced)
        {
            OpCode[] code = subCitiesAreSurfaced ?
            [
                LoadHalfImmediate(0x0001FB58, MMLEnums.Register.v1, Math.Max((byte)0x09, minProgressionCounter)),
                LoadHalfImmediate(0x00100648, MMLEnums.Register.v1, Math.Max((byte)0x09, minProgressionCounter))
            ] :
            [
                LoadHalfImmediate(0x00100648, MMLEnums.Register.v1, Math.Max((byte)0x02, minProgressionCounter))
            ];
            return code;
        }

        public static OpCode[] EnableDoorsCityHall(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadHalfImmediate(0x001006E4, MMLEnums.Register.a1, Math.Max((byte)0x01, minProgressionCounter))
            ];
        }

        public static OpCode[] FastForwardWilysBoat(byte minProgressionCounter, bool boatIsFixed, bool hasDefeatedBalkonGerat)
        {
            // This could go in slow loop, but put in fast loop since NPCs spawn on progression check
            byte fastForwardState = (byte)(hasDefeatedBalkonGerat ? minProgressionCounter : (boatIsFixed ? 0x05 : 0x04));
            return
            [
                // Loads people into the zone (they were evacuated)
                LoadHalfImmediate(0x001003A8, MMLEnums.Register.v1, fastForwardState),
                // 
                LoadHalfImmediate(0x00100374, MMLEnums.Register.a1, fastForwardState),
                // 
                LoadHalfImmediate(0x0001FCA8, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardYassPlains(byte minProgressionCounter, bool hasEarnedClassBLicense, bool HasEarnedClassALicense)
        {
            byte fastForwardState = (byte)(HasEarnedClassALicense ? Math.Max((byte)0x02, minProgressionCounter) : (hasEarnedClassBLicense ? 0x01 : 0x00));
            return
            [
                // Original tests v1 = 1
                LoadHalfImmediate(0x0001FD40, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x0001FD5C, MMLEnums.Register.v1, fastForwardState),
                // In internal game loop, loads tanks?
                LoadHalfImmediate(0x00100754, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x001006D4, MMLEnums.Register.a1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardClozerWoodsWithBridge(byte minProgressionCounter, bool hasEarnedClassBLicense, bool hasEarnedClassALicense)
        {
            byte fastForwardState = (byte)(hasEarnedClassALicense ? Math.Max((byte)0x02, minProgressionCounter) : (hasEarnedClassBLicense ? 0x01 : 0x00));
            return
            [
                // ?
                LoadHalfImmediate(0x0001FD94, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x0001FDB0, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x001002A0, MMLEnums.Register.a1, fastForwardState),
                // In internal game loop
                LoadHalfImmediate(0x00100328, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardClozerWoods(byte minProgressionCounter, bool hasEarnedClassBLicense, bool hasEarnedClassALicense)
        {
            byte fastForwardState = (byte)(hasEarnedClassALicense ? Math.Max((byte)0x02, minProgressionCounter) : (hasEarnedClassBLicense ? 0x01 : 0x00));
            return
            [
                // ?
                LoadHalfImmediate(0x0001FB90, MMLEnums.Register.v1, fastForwardState),
                // In internal game loop
                LoadHalfImmediate(0x00100534, MMLEnums.Register.v1, fastForwardState),
                // Enable sub-gate entrance door
                LoadHalfImmediate(0x00100500, MMLEnums.Register.a1, Math.Max((byte)0x08, minProgressionCounter)),
            ];
        }

        public static OpCode[] EnableFixBoatCallRoll()
        {
            // Enable "Call Roll" option when talking to worker which usually checks 0xBE37B[1]
            // delete branching that checks 0xBE37B[1] (the "has started taking yellow refractor cutscene" flag)
            // This is used by other stuff and can cause soft locks if not rewritten
            // An execution breakpoint here only hits once in this area, so it should be safe as long as it's restored later
            return [
                Nop(Addresses.FixBoatCallRollUtil.Address)
            ];
        }

        public static OpCode[] RestoreFixBoatCallRoll()
        {
            // This is what is overwritten by EnableFixBoatCallRoll
            return [
                new OpCode(Addresses.FixBoatCallRollUtil.Address, 0x10400006)  // beq v0, zero, 0x80055478
            ];
        }

        public static OpCode[] EnableRedRefractorCutscene()
        {
            return
            [
                LoadHalfImmediate(0x001001EC, MMLEnums.Register.v1, 0x06)
            ];
        }

        public static OpCode[] FastForwardCardonForestFlutterFixed(byte minProgressionCounter)
        {
            // Fast load
            return [
                // Can re-enter flutter
                LoadHalfImmediate(0x00100A3C, MMLEnums.Register.a1, Math.Max((byte)0x07, minProgressionCounter)),
                // Flutter is there
                LoadHalfImmediate(0x00100AE8, MMLEnums.Register.v1, Math.Max((byte)0x07, minProgressionCounter))
            ];
        }

        public static OpCode[] FastForwardLakeJyun(bool hasDefeatedBalkonGerat)
        {
            OpCode[] code = hasDefeatedBalkonGerat ? 
            [
                // 0x05 Prevents black screen after phase 1. 0x06 has the boat there after boss
                LoadHalfImmediate(0x0010059C, MMLEnums.Register.v1, 0x06),
                // Prevents crash after starting phase 1
                LoadHalfImmediate(0x0001FBCC, MMLEnums.Register.v1, 0x06),
                // Lets phase 1 load
                //LoadHalfImmediate(0x0011674C, MMLEnums.Register.v1, 0x06),
                // Lets after phase 2 load
                LoadHalfImmediate(0x00101A70, MMLEnums.Register.v1, 0x01)
            ] :
            [
                // 0x05 Prevents black screen after phase 1. 0x06 has the boat there after boss
                LoadHalfImmediate(0x0010059C, MMLEnums.Register.v1, 0x05),
                // Prevents crash after starting phase 1
                LoadHalfImmediate(0x0001FBCC, MMLEnums.Register.v1, 0x05),
                // Lets phase 1 load
                LoadHalfImmediate(0x0011674C, MMLEnums.Register.v1, 0x05)
            ];
            return code;
        }

        public static OpCode[] DecoupleCardonForestSubGateKeys()
        {
            // See related code in 
            AddressData jakkoKeyAddressData = DataDicts.LocationDataDict[60].CheckAddressData;
            AddressData conveyorKeyAddressData = DataDicts.LocationDataDict[61].CheckAddressData;
            AddressData switchesKeyAddressData = DataDicts.LocationDataDict[62].CheckAddressData;
            short jakkoKeyBitNumberOffset    = (short)((jakkoKeyAddressData.Address    - 0xBE378) * 8 + 7 - (jakkoKeyAddressData.BitNumber ?? 0));
            short conveyorKeyBitNumberOffset = (short)((conveyorKeyAddressData.Address - 0xBE378) * 8 + 7 - (conveyorKeyAddressData.BitNumber ?? 1));
            short switchesKeyBitNumberOffset = (short)((switchesKeyAddressData.Address - 0xBE378) * 8 + 7 - (switchesKeyAddressData.BitNumber ?? 2));
            short jakkoRefractorTerminalVirtualOffset = (short)((Addresses.YellowRefractorTerminalVirtual.Address - 0xBE378) * 8 + 1);
            short conveyorRefractorTerminalVirtualOffset = (short)((Addresses.YellowRefractorTerminalVirtual.Address - 0xBE378) * 8);
            short switchesRefractorTerminalVirtualOffset = (short)((Addresses.YellowRefractorTerminalVirtual.Address - 0xBE378) * 8 );
            return [
                // These will make the key pickups write to addresses, set to be distinct from "has key" addresses
                LoadHalfImmediate(0x00108F5C, MMLEnums.Register.a0, jakkoKeyBitNumberOffset), // replace write 0xBE41D with 0xBE377 (jakko key)
                LoadHalfImmediate(0x00108F64, MMLEnums.Register.a0, conveyorKeyBitNumberOffset), // replace write 0xBE41D with 0xBE377 (conveyor key)
                LoadHalfImmediate(0x00108F4C, MMLEnums.Register.a0, switchesKeyBitNumberOffset), // replace write 0xBE41E with 0xBE377 (switches key)

                // These execute on loading into a room, determining if the key is there, so we change to the same values above
                LoadHalfImmediate(0x0010CA24, MMLEnums.Register.a0, jakkoKeyBitNumberOffset), // load keys, switch read 0xBE41D with 0xBE377 (jakko key)
                LoadHalfImmediate(0x0010C9B0, MMLEnums.Register.a0, conveyorKeyBitNumberOffset), // load keys, switch read 0xBE41D with 0xBE377 (conveyor key)
                LoadHalfImmediate(0x0010CA44, MMLEnums.Register.a0, switchesKeyBitNumberOffset), // load keys, switch read 0xBE41E with 0xBE377 (switches key)

                // TODO: Figure out what these do. There is still an issue where Roll says the wrong line after picking up the third key
                //LoadHalfImmediate(0x0010C784, MMLEnums.Register.a0, jakkoKeyBitNumberOffset), // replace read 0xBE41D with 0xBE377
                //LoadHalfImmediate(0x0010C79C, MMLEnums.Register.a0, conveyorKeyBitNumberOffset), // replace read 0xBE41D with 0xBE377
                //LoadHalfImmediate(0x0010C7B4, MMLEnums.Register.a0, switchesKeyBitNumberOffset), // replace read 0xBE41E with 0xBE377

                // ?? These should make the sprite go away when picked up and control Roll's voice line
                //LoadHalfImmediate(0x00108F94, MMLEnums.Register.a0, yellowRefractorTerminalVirtualOffset), // key pickup, switch read 0xBE41D with 0xBE376

                // These should prevent the game writing to the terminal address based on your current key count when changing levels. Handled elsewhere
                Nop(0x0010C7DC),
                Nop(0x0010C850),
                Nop(0x0010C868),
                Nop(0x0010C870),
                Nop(0x0010C87C),

                // The refractor terminal reads whether you picked up the keys. These change the address the terminal is manipulated to another.
                // We need to populate the other address with emulated behavior of the original based on keys in inventory. 
                // This should probably be done in code injection, but I will put that part in the slow game loop because lazy
                //LoadHalfImmediate(0x00051228, MMLEnums.Register.a0, -16), // 
                //LoadHalfImmediate(0x0010BC0C, MMLEnums.Register.a0, -16),
                //LoadHalfImmediate(0x0010BA84, MMLEnums.Register.a0, -16), // This one is in the internal game loop... 
                // Jk, too hard. Key pickups also read/write here, so move those instead
                //LoadHalfImmediate(0x00108F94, MMLEnums.Register.a0, jakkoRefractorTerminalVirtualOffset),

                // If the yellow refractor has been picked up, then the game decides not to load a bunch of assets
                // such as the keys and the yellow refractor itself. 
                // Nop out this check so that it loads the assets regardless
                // We can conditionally remove the yellow refractor assets in the fast game loop
                Nop(0x0010C77C),
            ];
        }

        private static OpCode LoadHalfImmediate(uint startAddress, MMLEnums.Register register, short shortVal)
        {
            // addiu rt, zero, imm16
            uint instruction =
                0x24000000u |
                ((uint)register << 16) |
                (uint)(ushort)shortVal;   // keep only low 16 bits (two's complement)

            return new OpCode(startAddress, instruction);
        }

        private static OpCode Nop(uint startAddress)
        {
            return new OpCode(startAddress, 0x00000000); // nop
        }
    }
}
