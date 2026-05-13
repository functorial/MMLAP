using MMLAP.Models;

namespace MMLAP
{
    public static class Addresses
    {
        // Player status
        public static readonly AddressData CurrentLevel = new(0xC1B60, null, 2); // This was previously 0xC356E, but seems to be inaccurate in some cases (like cutscenes)
        public static readonly AddressData CurrentProgressionCounter = new(0xC1B63, null, 1);
        public static readonly AddressData CurrentZenny = new(0xC1B2C, null, 4);
        public static readonly AddressData UnequippedBusterInvStart = new(0xB5604, null, 34);
        // Game status
        // TODO: Is there an indicator for moving through a door? E.g. when "cutscene" of player running through. Useful for slow writes.
        public static readonly AddressData ScreenWipeFlag = new(0x1FF3E2, 0, null);
        public static readonly AddressData LoadingFlag = new(0x98A70, 0, null);
        public static readonly AddressData DungeonMapFlag = new(0x1B80AB, 3, null);
        public static readonly AddressData PauseMenuFlag = new(0x1B8017, 2, null);
        public static readonly AddressData CameraAlteredFlag = new(0x98008, 0, null);
        public static readonly AddressData SaveDataMenuFlag = new(0x98910, 0, null);
        public static readonly AddressData TitleScreen = new(0x98158, null, 1); // 0xA4 = Cutscenes and in-game, 0x20 = Title screen
        public static readonly AddressData TextBoxOpenFlag = new(0x98A5B, 7, null);
        public static readonly AddressData SupportCarRnDFlag = new(0x14C433, 0, null);
        public static readonly AddressData CutsceneFlag = new(0xC4C48, 0, null);
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
        public static readonly AddressData HasRescuedShopOwnersHusband = new(0xBE3D6, 4, null);
        public static readonly AddressData HasEarnedCitizenship = new(0xBE380, 1, null); //new(0xBE37C, 4, null);
        public static readonly AddressData HasStartedTronDogCutscene = new(0xBE378, 6, null);
        public static readonly AddressData HasWatchedServbotTakeoffCutscene = new(0xBE37A, 0, null);
        public static readonly AddressData HasStartedPirateAttackEvent = new(0xBE378, 1, null); // Flipped after talking to roll by the car
        public static readonly AddressData HasStartedBlumebearFight = new(0xBE379, 6, null); // Flipped after policemen cutscene
        public static readonly AddressData HasDefeatedFerdinand = new(0xBE383, 6, null);
        public static readonly AddressData HasDefeatedBonBonne = new(0xBE382, 6, null);
        public static readonly AddressData HasEarnedClassBLicense = new(0xBE3EA, 4, null);
        public static readonly AddressData HasStartedMarlwolf = new(0xBE3EA, 3, null);
        public static readonly AddressData HasDefeatedMarlwolf = new(0xBE37A, 7, null);
        public static readonly AddressData HasEarnedClassALicense = new(0xBE3EB, 5, null);
        public static readonly AddressData HasCompletedCardonTankEvent = new(0xBE37E, 7, null);
        public static readonly AddressData HasInteractedWithYellowTerminalOnce = new(0xBE380, 4, null);
        public static readonly AddressData HasTakenYellowRefractor = new(0xBE37B, 1, null);
        public static readonly AddressData HasYellowRefractor = new(0xBE41D, 7, null);
        public static readonly AddressData HasWatchedYellowRefractorCutscene = new(0xBE3EB, 6, 0);
        public static readonly AddressData HasCalledRollToFixBoat = new(0xBE37E, 5, null);
        public static readonly AddressData BoatIsFixed = new(0xBE37A, 6, null);
        public static readonly AddressData HasDefeatedBalkonGerat = new(0xBE37B, 2, null);
        public static readonly AddressData HasWatchedBalkonGeratDefeatCutscene = new(0xBE3EC, 4, null);
        public static readonly AddressData HasTakenRedRefractor = new(0xBE37C, 0, null);
        public static readonly AddressData HasDefeatedGarudoriten = new(0xBE382, 7, null);
        public static readonly AddressData HasShownRollRedRefractor = new(0xBE3B2, 6, null);
        public static readonly AddressData HasRedRefractor = new(0xBE41D, 6, null);
        public static readonly AddressData HasDefeatedKarumunaBashTrio = new(0xBE382, 4, null);
        public static readonly AddressData HasActivatedEmergencySystem = new(0xBE380, 3, null);
        public static readonly AddressData HasWatchedMainGateOpenCutscene = new(0xBE3EE, 4, null);
        public static readonly AddressData HasDefeatedFockeWulf = new(0xBE37D, 5, null);
        public static readonly AddressData HasActivatedUnlockSubCities = new(0xBE382, 1, null);
        public static readonly AddressData HasDefeatedTheodoreBruno = new(0xBE382, 5, null);
        public static readonly AddressData HasDefeatedJuno = new(0xBE385, 3, null);
        public static readonly AddressData HasFinishedWatchingJunoDefeatCutscene = new(0xBE3B7, 0, null);
        public static readonly AddressData HasStartedFlutterFixFromJunoCutscene = new(0xBE381, 2, null);
        public static readonly AddressData HasWatchedFlutterFixFromJunoCutscene = new(0xBE386, 7, null);
        // Side quest flags
        public static readonly AddressData HasSavedTheMissingWoman = new(0xBE3BE, 7, null);
        // Utility addresses for codes
        public static readonly AddressData FixBoatCallRollUtil = new(0x5545C, null, 4);
        public static readonly AddressData YellowRefractorTerminal = new(0xBE439, null, 1);
        public static readonly AddressData YellowRefractorTerminalVirtual = new(0xBE376, null, 1);
    }
}