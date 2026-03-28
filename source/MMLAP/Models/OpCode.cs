namespace MMLAP.Models
{
    public class OpCode(
        ulong startAddress,
        ulong instruction
    )
    {
        public ulong StartAddress { get; set; } = startAddress;
        public ulong Instruction { get; set; } = instruction;
    }
}
