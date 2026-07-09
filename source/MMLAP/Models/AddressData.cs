namespace MMLAP.Models
{
    public class AddressData(
        uint address,
        int? bitNumber = null,
        int? byteLength = null
        )
    {
        public uint Address { get; set; } = address;
        public int? BitNumber { get; set; } = bitNumber;
        public int? ByteLength { get; set; } = byteLength;
    }
}