using Archipelago.Core.Util;

namespace MMLAP.Models
{
    public class ExitData(    
        string sourceName,
        ushort sourceLevelID,
        string targetName,
        byte targetAreaID,
        uint targetAreaAddress,
        byte targetRoomID,
        uint targetRoomAddress,
        byte[] targetCoordinatesValue,
        uint targetCoordinatesAddress,
        bool isDoor
    )
    {
        public string SourceName {get; set;} = sourceName;
        public ushort SourceLevelID {get; set;} = sourceLevelID;
        public string TargetName {get; set;} = targetName;
        public byte TargetAreaID {get; set;} = targetAreaID;
        public uint TargetAreaAddress {get; set;} = targetAreaAddress;
        public byte TargetRoomID {get; set;} = targetRoomID;
        public uint TargetRoomAddress {get; set;} = targetRoomAddress;
        public byte[] TargetCoordinatesValue {get; set;} = targetCoordinatesValue; // 8 bytes: XX XX ZZ ZZ YY YY AA AA
        public uint TargetCoordinatesAddress {get; set;} = targetCoordinatesAddress;
        public bool IsDoor {get; set;} = isDoor;
        
        public bool LockExit()
        {
            // TODO: Change for non-standard exits e.g. sub-cities, elevators
            if (!IsDoor)
            {
                return false;
            }
            uint sourceZCoordinateAddress = TargetCoordinatesAddress - 5; // xx xx zz zz yy yy aa aa *XX XX ZZ ZZ YY YY AA AA
            byte sourceZCoordinateValue = Memory.ReadByte(sourceZCoordinateAddress);
            Memory.WriteByte(sourceZCoordinateAddress, (byte)(sourceZCoordinateValue + 0x80));
            return true;
        }
    }
}
