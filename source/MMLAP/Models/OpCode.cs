namespace MMLAP.Models
{
    public class OpCode(
        uint startAddress,
        uint instruction
    )
    {
        public uint StartAddress { get; set; } = startAddress;
        public uint Instruction { get; set; } = instruction;
    }
}
