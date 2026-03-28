using MMLAP.Models;
using System.Security.AccessControl;

namespace MMLAP
{
    public class Codes
    {
        public static readonly OpCode[] EnableInsideCardonSubGate =
        {
            AddiuV1(0x0010031C, 0x03), // addiu v1, zero, 3
            Nop(0x0100320) // nop
        };

        private static OpCode AddiuV1(ulong startAddress, byte byteVal)
        {
            ulong instruction = 0x24030000 | (ulong)byteVal; // addiu v1, zero, byteVal
            return new OpCode(startAddress, instruction);
        }
        
        private static OpCode Nop(ulong startAddress)
        {
            return new OpCode(startAddress, 0x00000000); // nop
        }
    }
}
