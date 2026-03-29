using MMLAP.Models;
using System.Security.AccessControl;

namespace MMLAP
{
    public class Codes
    {
        public static readonly OpCode[] EnableCardonSubGateInside =
        {
            AddiuV1(0x0010031C, 0x03),
            Nop(0x00100320)
        };

        public static readonly OpCode[] EnableJyunSubGateInside =
        {
            AddiuV1(0x001003F4, 0x06),
            Nop(0x001003F8)
        };

        public static readonly OpCode[] EnableClozerSubGateInside =
        {
            AddiuV1(0x00100570, 0x07),
            Nop(0x00100574)
        };

        public static readonly OpCode[] EnableCardonSubGateOutside =
        {
            AddiuV1(0x00100E88, 0x03),
            Nop(0x00100E8C)
        };
        public static readonly OpCode[] EnableClozerSubGateOutside =
        {
            AddiuV1(0x00100530, 0x03),
            Nop(0x00100534)
        };


        private static OpCode AddiuV1(uint startAddress, byte byteVal)
        {
            uint instruction = 0x24030000 | (uint)byteVal; // addiu v1, zero, byteVal
            return new OpCode(startAddress, instruction);
        }
        
        private static OpCode Nop(uint startAddress)
        {
            return new OpCode(startAddress, 0x00000000); // nop
        }
    }
}
