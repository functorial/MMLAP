using MMLAP;
using MMLAP.Models;
using System;

namespace MMLAP
{
    public class Codes
    {
        public static OpCode[] EnableDoorsInsideCardonSubGate(byte minProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadByteImmediate(0x0010031C, MMLEnums.Register.v1, Math.Max((byte)0x03, minProgressionCounter)),
                Nop(0x00100320)
            ];
        }

        public static OpCode[] EnableDoorsInsideJyunSubGate(byte minProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadByteImmediate(0x001003F4, MMLEnums.Register.v1, Math.Max((byte)0x06, minProgressionCounter)),
                Nop(0x001003F8)
            ];
        }

        public static OpCode[] EnableDoorsInsideClozerSubGate(byte minProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadByteImmediate(0x00100570, MMLEnums.Register.v1, Math.Max((byte)0x07, minProgressionCounter)),
                Nop(0x00100574)
            ];
        }

        public static OpCode[] EnableDoorsOutsideCardonSubGate(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x00100E00, MMLEnums.Register.a1, Math.Max((byte)0x04, minProgressionCounter)),
                Nop(0x00100E04)
            ];
        }

        public static OpCode[] EnableDoorsOutsideClozerSubGate(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x001004FC, MMLEnums.Register.a1, Math.Max((byte)0x08, minProgressionCounter)),
                Nop(0x00100500)
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

        public static OpCode[] EnableDoorsDowntown(byte minProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x00106B50, MMLEnums.Register.a1, Math.Max((byte)0x02, minProgressionCounter)),
                LoadByteImmediate(0x00106B68, MMLEnums.Register.v0, Math.Max((byte)0x02, minProgressionCounter)), // Opens doors during Tron / dog scene
            ];
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
                LoadByteImmediate(0x001003A4, MMLEnums.Register.v1, Math.Max(fastForwardState, minProgressionCounter)), 
                Nop(0x001003A8),
                // 
                LoadByteImmediate(0x00100370, MMLEnums.Register.a1, Math.Max(fastForwardState, minProgressionCounter)),
                Nop(0x00100374),
                // 
                LoadByteImmediate(0x0001FCA4, MMLEnums.Register.v1, Math.Max(fastForwardState, minProgressionCounter)),
                Nop(0x0001FCA8)
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
                LoadByteImmediate(0x001001E8, MMLEnums.Register.v1, 0x06),
                Nop(0x001001EC)
            ];
        }

        public static OpCode[] FastForwardCardonForestFlutterFixed(byte minProgressionCounter)
        {
            // Fast load
            return [
                // Can re-enter flutter
                LoadByteImmediate(0x00100A38, MMLEnums.Register.a1, Math.Max((byte)0x07, minProgressionCounter)),
                Nop(0x00100A3C),
                // Flutter is there
                LoadByteImmediate(0x00100AE4, MMLEnums.Register.v1, Math.Max((byte)0x07, minProgressionCounter)),
                Nop(0x00100AE8)
            ];
        }

        public static OpCode[] FastForwardOldCity(byte minProgressionCounter)
        {
            // Slow
            return [
                    LoadByteImmediate(0x001007CC, MMLEnums.Register.v1, Math.Max((byte)0x02, minProgressionCounter)),
                    Nop(0x001007D0)
            ];
        }

        public static OpCode[] FastForwardUptown(byte minProgressionCounter)
        {
            // Slow
            return [
                    LoadByteImmediate(0x00100644, MMLEnums.Register.v1, Math.Max((byte)0x02, minProgressionCounter)),
                    Nop(0x00100648)
            ];
        }

        public static OpCode[] FastForwardLakeJyun(bool hasDefeatedBalkonGerat)
        {
            //byte fastForwardState = (byte)(hasDefeatedBalkonGerat ? 0x06 : 0x05);
            OpCode[] code = hasDefeatedBalkonGerat ? [
                // 0x05 Prevents black screen after phase 1. 0x06 has the boat there after boss
                LoadByteImmediate(0x00100598, MMLEnums.Register.v1, 0x06),
                Nop(0x0010059C),
                // Prevents crash after starting phase 1
                LoadByteImmediate(0x0001FBC8, MMLEnums.Register.v1, 0x06),
                Nop(0x0001FBCC),
                // Lets phase 1 load
                //LoadByteImmediate(0x00116748, MMLEnums.Register.v1, 0x06),
                //Nop(0x0011674C),
                // Lets after phase 2 load
                LoadByteImmediate(0x00101A70, MMLEnums.Register.v1, 0x01)
            ] :
            [
                // 0x05 Prevents black screen after phase 1. 0x06 has the boat there after boss
                LoadByteImmediate(0x00100598, MMLEnums.Register.v1, 0x05),
                Nop(0x0010059C),
                // Prevents crash after starting phase 1
                LoadByteImmediate(0x0001FBC8, MMLEnums.Register.v1, 0x05),
                Nop(0x0001FBCC),
                // Lets phase 1 load
                LoadByteImmediate(0x00116748, MMLEnums.Register.v1, 0x05),
                Nop(0x0011674C)
            ];
            return code;
        }

        private static OpCode LoadByteImmediate(uint startAddress, MMLEnums.Register register, byte byteVal)
        {
            // registerNumber 0= zero, 1 = at, 2-3 = v0-v1, 4-7 = a0-a3, 8-15 = t0-t7, 16-23 = s0-s7, 24-25 = t8-t9,
            // 26-27 = k0-k1, 28 = gp, 29 = sp, 30 = fp, 31 = ra
            uint instruction = 0x24000000 | ((uint)register * 0x10000) | (uint)byteVal ; // addiu v1, zero, byteVal
            return new OpCode(startAddress, instruction);
        }
        
        private static OpCode Nop(uint startAddress)
        {
            return new OpCode(startAddress, 0x00000000); // nop
        }
    }
}
