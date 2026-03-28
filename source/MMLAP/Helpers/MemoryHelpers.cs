using Archipelago.Core.Util;
using MMLAP.Models;
using System.Linq;

namespace MMLAP.Helpers
{
    public class MemoryHelpers
    {
        public static bool IsInTitleScreen()
        {
            return Memory.ReadByte(Addresses.TitleScreen.Address) == 0x20;
        }

        public static bool IsOutOfTitleScreen()
        {
            bool isInGameOrCutscene = Memory.ReadByte(Addresses.TitleScreen.Address) == 0xA4;
            short currentLevelID = Memory.ReadShort(Addresses.CurrentLevel.Address, Enums.Endianness.Big);
            bool isInTitleScreenCutscene = new short[]
            {
                    0x0100,  // Scrolling text cutscene
                    0x0700,  // Gesselschaft cutscene
                    0x0701,  // Gesselschaft cutscene
                    0x0702,  // Gesselschaft cutscene
                    0x0703,  // Gesselschaft cutscene
                    0x0704,  // Gesselschaft cutscene
                    0x0705,  // Gesselschaft cutscene
                    0x0706,  // Gesselschaft cutscene
                    0x0707   // Gesselschaft cutscene
            }.Contains(currentLevelID);
            return isInGameOrCutscene && !isInTitleScreenCutscene;
        }

        public static void WriteCode(OpCode[] code)
        {
            foreach (OpCode op in code)
            {
                Memory.Write(op.StartAddress, op.Instruction);
            }
        }
    }
}
