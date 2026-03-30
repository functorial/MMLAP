using MMLAP.Models;
using System;

namespace MMLAP
{
    public class Codes
    {
        public static OpCode[] EnableDoorsInsideCardonSubGate(byte currentProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadByteImmediate(0x0010031C, MMLEnums.Register.v1, Math.Max((byte)0x03, currentProgressionCounter)),
                Nop(0x00100320)
            ];
        }

        public static OpCode[] EnableDoorsInsideJyunSubGate(byte currentProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadByteImmediate(0x001003F4, MMLEnums.Register.v1, Math.Max((byte)0x06, currentProgressionCounter)),
                Nop(0x001003F8)
            ];
        }

        public static OpCode[] EnableDoorsInsideClozerSubGate(byte currentProgressionCounter)
        {
            // May be written slowly as it is checked during in-game loop
            return
            [
                LoadByteImmediate(0x00100570, MMLEnums.Register.v1, Math.Max((byte)0x07, currentProgressionCounter)),
                Nop(0x00100574)
            ];
        }

        public static OpCode[] EnableDoorsOutsideCardonSubGate(byte currentProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x00100E00, MMLEnums.Register.a1, Math.Max((byte)0x04, currentProgressionCounter)),
                Nop(0x00100E04)
            ];
        }

        public static OpCode[] EnableDoorsOutsideClozerSubGate(byte currentProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x001004FC, MMLEnums.Register.a1, Math.Max((byte)0x08, currentProgressionCounter)),
                Nop(0x00100500)
            ];
        }

        public static OpCode[] EnableDoorsCardonForest(byte currentProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return [
                LoadByteImmediate(0x001007E0, MMLEnums.Register.a1, Math.Max((byte)0x01, currentProgressionCounter))
            ];
        }

        public static OpCode[] EnableDoorsAppleMarket(byte currentProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return [
                LoadByteImmediate(0x00100474, MMLEnums.Register.a1, Math.Max((byte)0x01, currentProgressionCounter)),
            ];
        }

        public static OpCode[] EnableDoorsDowntown(byte currentProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x00106B50, MMLEnums.Register.a1, Math.Max((byte)0x02, currentProgressionCounter)),
                LoadByteImmediate(0x00106B68, MMLEnums.Register.v0, Math.Max((byte)0x02, currentProgressionCounter)), // Opens doors during Tron / dog scene
            ];
        }

        public static OpCode[] EnableDoorsCityHall(byte currentProgressionCounter)
        {
            // Needs to be written fast during loading screen
            return
            [
                LoadByteImmediate(0x001006E4, MMLEnums.Register.a1, Math.Max((byte)0x01, currentProgressionCounter))
            ];
        }

        public static OpCode[] EnableBoatFixWilysBoat(byte currentProgressionCounter)
        {
            return
            [
                // Loads people into the zone (they were evacuated)
                LoadByteImmediate(0x001003A4, MMLEnums.Register.v1, Math.Max((byte)0x04, currentProgressionCounter)), 
                Nop(0x001003A8),
                // Enable "Call Roll" option when talking to worker which usually checks 0xBE37B[1]
                LoadByteImmediate(0x0001DA70, MMLEnums.Register.v0, 0x02)  
            ];
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
