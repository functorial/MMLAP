namespace MMLAP.Models
{
    public class TextData(
        ulong startAddress,
        byte[] textByteArr,
        ushort? sourceLevelId = null
    )
    {
        public ulong StartAddress { get; set; } = startAddress;
        public byte[] TextByteArr { get; set; } = textByteArr;
        public ushort? SourceLevelId { get; set; } = sourceLevelId;
    }
}
