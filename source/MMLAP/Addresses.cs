using MMLAP.Models;

namespace MMLAP
{
    public static class Addresses
    {
        // Player status
        public static readonly AddressData CurrentLevel = new(0xC356E, null, 2);
        public static readonly AddressData CurrentProgressionCounter = new(0xC1B63, null, 1);
        public static readonly AddressData CurrentZenny = new(0xC1B2C, null, 4);
        public static readonly AddressData UnequippedBusterInvStart = new(0xB5604, null, 34);
        // Goals
        public static readonly AddressData GoalJunoFlag = new(0xBE385, 3, null);
        // Game status
        public static readonly AddressData ScreenWipeFlag = new(0x1FF3E2, 0, null);
        public static readonly AddressData LoadingFlag = new(0x98A70, 0, null);
        public static readonly AddressData DungeonMapFlag = new(0x1B80AB, 3, null);
        public static readonly AddressData PauseMenuFlag = new(0x1B8017, 2, null);
        public static readonly AddressData CameraAlteredFlag = new(0x98008, 0, null);
        public static readonly AddressData SaveDataMenuFlag = new(0x98910, 0, null);
        public static readonly AddressData TitleScreen = new(0x98158, null, 1); // 0xA4 = Cutscenes and in-game, 0x20 = Title screen
        public static readonly AddressData TextBoxOpenFlag = new(0x98A5B, 7, null);
        public static readonly AddressData SupportCarRnDFlag = new(0x14C433, 1, null);
        // Hideout quest
        public static readonly AddressData WorkerGetPickTextStart = new(0x154A78, null, 0x48); // Huh? A pick? ...
        public static readonly AddressData SawWorkerDialogueIsReady = new(0xBE3BC, 0, null);
        public static readonly AddressData SawPailIsReady = new(0xBE3BD, 7, null);
        public static readonly AddressData TurnedInSaw = new(0xBE3BB, 5, null);
        // Inspector quest
        public static readonly AddressData StartBombQuest = new(0xBE3B8, 3, null); // bombs go off = 0xBE3B8 2 and 1, BE3D4 1, turn in to inspector 0xBE3B8 0
        public static readonly AddressData StartBagQuest = new(0xBE3B9, 6, null); //lobby man = 0xBE3B9 5, electric = 0xBE3B9 4, baker = 0xBE3B9 2, librarian = 0xBE3B9 3, vending = 0xBE3B9 1, boy1 = 0xBE3B9 0, boy2 = 0xBE3BA 7, talk to inspector with bag 0xBE3BA 6
        public static readonly AddressData BagPailIsReady = new(0xBE3BA, 7, null);
        // Main story flags
        public static readonly AddressData SubCitiesSurfaced = new(0xBE382, 1, null);
        public static readonly AddressData BoatIsFixed = new(0xBE37A, 6, null);
        public static readonly AddressData ShownRollRedRefractor = new(0xBE3B2, 5, null);
        public static readonly AddressData HasYellowRefractor = new(0xBE41D, 7, null); // TODO: Make these locations / item data
        public static readonly AddressData HasRedRefractor = new(0xBE41D, 6, null); // TODO: Make these locations / item data
        public static readonly AddressData HasDefeatedBalkonGerat = new(0xBE37B, 2, null);
        public static readonly AddressData HasCalledRollToFixBoat = new(0xBE37E, 5, null);
        // Utility addresses for codes
        public static readonly AddressData FixBoatCallRollUtil = new(0x5545C, null, 4);
    }
}