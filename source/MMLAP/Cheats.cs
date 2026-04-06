using MMLAP;
using MMLAP.Helpers;
using MMLAP.Models;
using System;

namespace MMLAP
{
    public class Cheats
    {
        public static OpCode[] EnableDoorsInsideCardonSubgate(byte minProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadByteImmediate(0x00100320, MMLEnums.Register.v1, Math.Max((byte)0x03, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsInsideJyunSubgate(byte minProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadByteImmediate(0x001003F8, MMLEnums.Register.v1, Math.Max((byte)0x06, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsInsideClozerSubgate(byte minProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadByteImmediate(0x00100574, MMLEnums.Register.v1, Math.Max((byte)0x07, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsOutsideCardonSubgate(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x00100E04, MMLEnums.Register.a1, Math.Max((byte)0x04, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsOutsideClozerSubgate(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x00100500, MMLEnums.Register.a1, Math.Max((byte)0x08, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsCardonForestFlutterBroken(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return [
                LoadByteImmediate(0x001007E0, MMLEnums.Register.a1, Math.Max((byte)0x01, minProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsAppleMarket(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return [
                LoadByteImmediate(0x00100474, MMLEnums.Register.a1, Math.Max((byte)0x01, minProgressionCounter)),
            ];
        }

        public static OpCode[] FastForwardDowntown(byte minProgressionCounter, bool subCitiesAreSurfaced)
        {
            // Needs to be written fast during loading screen
            OpCode[] code = subCitiesAreSurfaced ?
            [
                // Enable Sub-City
                LoadByteImmediate(0x00106BB0, MMLEnums.Register.v0, 0x09),
                // Open doors
                LoadByteImmediate(0x00106B50, MMLEnums.Register.a1, Math.Max((byte)0x09, minProgressionCounter)),
                // Opens doors during Tron / dog scene
                LoadByteImmediate(0x00106B68, MMLEnums.Register.v0, Math.Max((byte)0x09, minProgressionCounter)),
            ] :
            [
                // Open doors
                LoadByteImmediate(0x00106B50, MMLEnums.Register.a1, Math.Max((byte)0x02, minProgressionCounter)),
                // Opens doors during Tron / dog scene
                LoadByteImmediate(0x00106B68, MMLEnums.Register.v0, Math.Max((byte)0x02, minProgressionCounter)), 

            ];
            return code;
        }

        public static OpCode[] FastForwardOldCity(byte minProgressionCounter)
        {
            return 
            [
                LoadByteImmediate(0x001007D0, MMLEnums.Register.v1, Math.Max((byte)0x09, minProgressionCounter))
            ];
        }

        public static OpCode[] FastForwardUptown(byte minProgressionCounter, bool subCitiesAreSurfaced)
        {
            OpCode[] code = subCitiesAreSurfaced ?
            [
                LoadByteImmediate(0x0001FB58, MMLEnums.Register.v1, Math.Max((byte)0x09, minProgressionCounter)),
                LoadByteImmediate(0x00100648, MMLEnums.Register.v1, Math.Max((byte)0x09, minProgressionCounter))
            ] :
            [
                LoadByteImmediate(0x00100648, MMLEnums.Register.v1, Math.Max((byte)0x02, minProgressionCounter))
            ];
            return code;
        }

        public static OpCode[] EnableDoorsCityHall(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x001006E4, MMLEnums.Register.a1, Math.Max((byte)0x01, minProgressionCounter))
            ];
        }

        public static OpCode[] FastForwardWilysBoat(byte minProgressionCounter, bool boatIsFixed, bool hasDefeatedBalkonGerat)
        {
            // This could go in slow loop, but put in fast loop since NPCs spawn on progression check
            byte fastForwardState = (byte)(hasDefeatedBalkonGerat ? 0x06 : (boatIsFixed ? 0x05 : 0x04));
            return
            [
                // Loads people into the zone (they were evacuated)
                LoadByteImmediate(0x001003A8, MMLEnums.Register.v1, Math.Max(fastForwardState, minProgressionCounter)),
                // 
                LoadByteImmediate(0x00100374, MMLEnums.Register.a1, Math.Max(fastForwardState, minProgressionCounter)),
                // 
                LoadByteImmediate(0x0001FCA8, MMLEnums.Register.v1, Math.Max(fastForwardState, minProgressionCounter))
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
                LoadByteImmediate(0x001001EC, MMLEnums.Register.v1, 0x06)
            ];
        }

        public static OpCode[] FastForwardCardonForestFlutterFixed(byte minProgressionCounter)
        {
            // Fast load
            return [
                // Can re-enter flutter
                LoadByteImmediate(0x00100A3C, MMLEnums.Register.a1, Math.Max((byte)0x07, minProgressionCounter)),
                // Flutter is there
                LoadByteImmediate(0x00100AE8, MMLEnums.Register.v1, Math.Max((byte)0x07, minProgressionCounter))
            ];
        }

        public static OpCode[] FastForwardLakeJyun(bool hasDefeatedBalkonGerat)
        {
            OpCode[] code = hasDefeatedBalkonGerat ? 
            [
                // 0x05 Prevents black screen after phase 1. 0x06 has the boat there after boss
                LoadByteImmediate(0x0010059C, MMLEnums.Register.v1, 0x06),
                // Prevents crash after starting phase 1
                LoadByteImmediate(0x0001FBCC, MMLEnums.Register.v1, 0x06),
                // Lets phase 1 load
                //LoadByteImmediate(0x0011674C, MMLEnums.Register.v1, 0x06),
                // Lets after phase 2 load
                LoadByteImmediate(0x00101A70, MMLEnums.Register.v1, 0x01)
            ] :
            [
                // 0x05 Prevents black screen after phase 1. 0x06 has the boat there after boss
                LoadByteImmediate(0x0010059C, MMLEnums.Register.v1, 0x05),
                // Prevents crash after starting phase 1
                LoadByteImmediate(0x0001FBCC, MMLEnums.Register.v1, 0x05),
                // Lets phase 1 load
                LoadByteImmediate(0x0011674C, MMLEnums.Register.v1, 0x05)
            ];
            return code;
        }

        public static OpCode[] DecoupleCardonForestSubGateKeys()
        {
            AddressData jakkoKeyAddressData = DataDicts.LocationDataDict[60].CheckAddressData;
            AddressData conveyorKeyAddressData = DataDicts.LocationDataDict[61].CheckAddressData;
            AddressData switchesKeyAddressData = DataDicts.LocationDataDict[62].CheckAddressData;
            short jakkoKeyBitNumberOffset    = (short)((jakkoKeyAddressData.Address    - 0xBE378) * 8 + 1 - (jakkoKeyAddressData.BitNumber ?? 0));
            short conveyorKeyBitNumberOffset = (short)((conveyorKeyAddressData.Address - 0xBE378) * 8 + 1 - (conveyorKeyAddressData.BitNumber ?? 1));
            short switchesKeyBitNumberOffset = (short)((switchesKeyAddressData.Address - 0xBE378) * 8 + 1 - (switchesKeyAddressData.BitNumber ?? 2));
            return [
                LoadHalfImmediate(0x00108F5C, MMLEnums.Register.a0, jakkoKeyBitNumberOffset), // replace write 0xBE41D with 0xBE376 (jakko key)
                LoadHalfImmediate(0x00108F64, MMLEnums.Register.a0, conveyorKeyBitNumberOffset), // replace write 0xBE41D with 0xBE376 (conveyor key)
                LoadHalfImmediate(0x00108F4C, MMLEnums.Register.a0, switchesKeyBitNumberOffset), // replace write 0xBE41E with 0xBE377 (switches key)

                // TODO: Figure out if these are correct reads. There is still an issue where Roll says the wrong line after picking up the third key
              //LoadHalfImmediate(0x0010C784, MMLEnums.Register.a0, jakkoKeyBitNumberOffset), // replace read 0xBE41D with 0xBE376
              //LoadHalfImmediate(0x0010C79C, MMLEnums.Register.a0, conveyorKeyBitNumberOffset), // replace read 0xBE41D with 0xBE376
              //LoadHalfImmediate(0x0010C7B4, MMLEnums.Register.a0, switchesKeyBitNumberOffset), // replace read 0xBE41E with 0xBE377

                LoadHalfImmediate(0x0010CA24, MMLEnums.Register.a0, jakkoKeyBitNumberOffset), // load keys, switch read 0xBE41D with 0xBE376 (jakko key) (confirmed)
                LoadHalfImmediate(0x0010C9B0, MMLEnums.Register.a0, conveyorKeyBitNumberOffset), // load keys, switch read 0xBE41D with 0xBE376 (conveyor key) (confirmed)
                LoadHalfImmediate(0x0010CA44, MMLEnums.Register.a0, switchesKeyBitNumberOffset), // load keys, switch read 0xBE41E with 0xBE377 (switches key) (confirmed)

            ];
        }

        private static OpCode LoadByteImmediate(uint startAddress, MMLEnums.Register register, byte byteVal)
        {
            // registerNumber 0= zero, 1 = at, 2-3 = v0-v1, 4-7 = a0-a3, 8-15 = t0-t7, 16-23 = s0-s7, 24-25 = t8-t9,
            // 26-27 = k0-k1, 28 = gp, 29 = sp, 30 = fp, 31 = ra
            uint instruction = 0x24000000 | ((uint)register * 0x10000) | (uint)byteVal; // addiu v1, zero, byteVal
            return new OpCode(startAddress, instruction);
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
