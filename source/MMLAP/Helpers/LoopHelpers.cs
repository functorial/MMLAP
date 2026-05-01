using Archipelago.Core.Util;
using MMLAP.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Serilog;
using System.Linq;
using System.Collections;
using Avalonia.Rendering;

namespace MMLAP.Helpers
{
    public class LoopHelpers
    {
        public static void HandleOddLocationText(
            LevelData currentLevelData,
            Dictionary<long, ItemData>? scoutedLocationItemData,
            ConcurrentStack<TextData> textDataToWriteStack
        )
        {
            string areaName = currentLevelData.AreaName;
            string roomName = currentLevelData.RoomName;
            string levelName = areaName + ": " + roomName;
            switch (levelName)
            {
                case "Apple Market: Junk Shop":
                    // "Rescue the shop owner's husband" location text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(104, out var rescueScoutedItemData) &&
                        DataDicts.LocationDataDict.TryGetValue(104, out var rescueLocationData) &&
                        rescueLocationData.Name == "Rescue the shop owner's husband" &&
                        rescueLocationData.TextBoxStartAddress != null
                    )
                    {
                        // Being careful about text box overflow. Replacing new text window from man -> "You got" with a newpage, which saves a bunch of bytes.
                        // Not bothering to restore this text
                        byte[] writeTextArr = TextHelpers.EncodeYouGotItemWindow(rescueScoutedItemData, prefix: TextHelpers.newPage, suffix: [0x9F, 0x99, 0x00, 0xBD, 0xA9, 0x84]);
                        Memory.WriteByteArray(rescueLocationData.TextBoxStartAddress ?? 0, writeTextArr);
                    }
                    break;

                case "City Hall: Amelia's Office":
                    // Class B License text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(131, out var classBScoutedItemData) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense)
                    )
                    {
                        uint textStartAddress = 0x154500;
                        uint textEndAddress = 0x154521;
                        byte[] hasEarnedClassBLicenseTextOverwrite = TextHelpers.EncodeYouGotItemWindow(classBScoutedItemData, prefix: TextHelpers.newPage, guaranteedLength: textEndAddress - textStartAddress);
                        Memory.WriteByteArray(textStartAddress, hasEarnedClassBLicenseTextOverwrite);
                    }
                    // Class A License text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(132, out var classAScoutedItemData) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense)
                    )
                    {
                        uint textStartAddress = 0x154E1D;
                        byte[] hasEarnedClassALicenseTextOverwrite = TextHelpers.EncodeYouGotItemWindow(classAScoutedItemData);
                        Memory.WriteByteArray(textStartAddress, hasEarnedClassALicenseTextOverwrite);
                    }
                    break;

                case "City Hall: City Hall Outdoors":
                    // Handle worker dialogue for Pick
                    // Currently talking to this guy is not a location, but the Pick item is randomized in the pool
                    List<byte[]> substrs =
                        [
                            TextHelpers.EncodeSimpleString("Huh? A pick?"),
                            TextHelpers.newPage,
                            TextHelpers.EncodeSimpleString("Never heard of it.\n:)"),
                            TextHelpers.newPage,
                            TextHelpers.EncodeSimpleString("Try looking elsewhere!"),
                            TextHelpers.endWindow
                        ];
                    byte[] workerTextChange = TextHelpers.ConcatArrayList(substrs);
                    Memory.WriteByteArray(Addresses.WorkerGetPickTextStart.Address, workerTextChange);
                    break;

                case "Uptown: Ira's Room":
                    // "Cure Ira's illness" location text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(111, out var iraScoutedItemData) &&
                        DataDicts.LocationDataDict.TryGetValue(111, out var iraLocationData) &&
                        iraLocationData.Name == "Cure Ira's illness" &&
                        iraLocationData.TextBoxStartAddress != null
                    )
                    {
                        textDataToWriteStack.Push(TextHelpers.OverwriteText(iraLocationData.TextBoxStartAddress ?? 0, TextHelpers.EncodeYouGotItemWindow(iraScoutedItemData)));
                    }
                    break;

                case "Cardon Forest (Flutter Broken): City Entrance":
                    // "Earning citizenship" location text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(130, out var citizensCardScoutedItemData) &&
                        DataDicts.LocationDataDict.TryGetValue(130, out var citizensCardLocationData) &&
                        citizensCardLocationData.Name == "Earn citizenship in Kattelox City" &&
                        citizensCardLocationData.TextBoxStartAddress != null
                    )
                    {
                        // Being careful about text box overflow. Replacing new text window from man -> "You got" with a newpage, which saves a bunch of bytes.
                        // Not bothering to restore this text
                        byte[] writeTextArr = TextHelpers.EncodeYouGotItemWindow(citizensCardScoutedItemData, prefix: TextHelpers.newPage, suffix: TextHelpers.endWindow); // suffix: [0x9F, 0x99, 0x00, 0xBD, 0xA9, 0x89, 0x00]);
                        Memory.WriteByteArray(citizensCardLocationData.TextBoxStartAddress ?? 0, writeTextArr);
                    }
                    break;

                case "Cardon Forest (Flutter Fixed): Crash Site":
                    // Prevent starting ending cutscene at Roll if Goal isn't completed
                    List<byte[]> rollInitiateEndingCutsceneTextArrs = [
                        TextHelpers.EncodeSimpleString("Have you completed your\n"),
                        TextHelpers.AddTextColor(TextHelpers.EncodeSimpleString("Archipelago Goal"), TextHelpers.textColorRed),
                        TextHelpers.EncodeSimpleString(" yet,\nMegaMan?"),
                        TextHelpers.endWindow,
                    ];
                    byte[] rollInitiateEndingCutsceneText = TextHelpers.ConcatArrayList(rollInitiateEndingCutsceneTextArrs);
                    Memory.WriteByteArray(0x00154737, rollInitiateEndingCutsceneText);
                    break;

                default:
                    break;
            }
        }

        public static void HandleLoadingFastCodeWrites(LevelData currentLevelData, byte currentProgressionCounter)
        {
            string areaName = currentLevelData.AreaName;

            switch (currentLevelData)
            {
                case var data when data.AreaName == "Cardon Forest (Flutter Broken)":
                    bool hasEarnedClassBLicense = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedCitizenship = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedCitizenship);
                    MemoryHelpers.WriteCode(Cheats.FastForwardCardonForestFlutterBroken(currentProgressionCounter, hasEarnedClassBLicense, hasEarnedCitizenship));
                    break;

                case var data when data.AreaName == "Cardon Forest (Flutter Fixed)":
                    bool hasDefeatedJunoFlutterFixed = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedJuno);
                    bool hasWatchedFlutterFixFromJunoCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasWatchedFlutterFixFromJunoCutscene);
                    bool hasCompletedGoal = App.HasSubmittedGoal;
                    MemoryHelpers.WriteCode(Cheats.FastForwardCardonForestFlutterFixed(currentProgressionCounter, hasDefeatedJunoFlutterFixed, hasWatchedFlutterFixFromJunoCutscene, hasCompletedGoal));
                    break;

                case var data when data.AreaName == "Outside Cardon Forest Sub-Gate":
                    bool hasDefeatedFerdinand = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedFerdinand);
                    bool hasCompletedCardonTankEvent = MemoryHelpers.ReadAddressDataBit(Addresses.HasCompletedCardonTankEvent);
                    MemoryHelpers.WriteCode(Cheats.FastForwardOutsideCardonSubgate(currentProgressionCounter, hasDefeatedFerdinand, hasCompletedCardonTankEvent));
                    break;

                case var data when data.AreaName == "Cardon Forest Sub-Gate":
                    bool hasTakenYellowRefractor = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenYellowRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardCardonForestSubgate(hasTakenYellowRefractor));
                    MemoryHelpers.WriteCode(Cheats.DecoupleCardonForestSubGateKeys());

                    // Prevent black screen on cutscene due to meddling with other stuff
                    if (
                        MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenYellowRefractor) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.HasWatchedYellowRefractorCutscene) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.CutsceneFlag)
                    )
                    {
                        PlayCutscene(0x19);
                    }

                    // Manually unload the assets showing the yellow refractor if it has already been picked up
                    // The DecoupleCardonForestSubGateKeys requires explicitly not checking hasTakenYellowRefractor and loading stuff anyway
                    if (hasTakenYellowRefractor)
                    {
                        Memory.WriteByte(0xBF4D8, 0x00); // Shield 
                        Memory.WriteByte(0xBF988, 0x00); // Refractor transparent glow
                        Memory.WriteByte(0xBF9D8, 0x00); // Refractor 
                        Memory.WriteByte(0xA3B40, 0x00); // Sparkles 
                        Memory.WriteByte(0xA3B88, 0x00); // Sparkles 
                        Memory.WriteByte(0xA3BD0, 0x00); // Sparkles
                        Memory.WriteByte(0xA3C18, 0x00); // Sparkles 
                        Memory.WriteByte(0xA3C60, 0x00); // Sparkles 
                        Memory.WriteByte(0xA3CA8, 0x00); // Sparkles 
                        Memory.WriteByte(0xA3CF0, 0x00); // Sparkles 
                    }
                    break;

                case var data when data.AreaName == "Lake Jyun Sub-Gate":
                    MemoryHelpers.WriteCode(Cheats.FastForwardLakeJyunSubgate());
                    break;

                case var data when data.AreaName == "Clozer Woods Sub-Gate":
                    MemoryHelpers.WriteCode(Cheats.FastForwardClozerWoodsSubgate());
                    break;

                case var data when data.AreaName == "Outside Main Gate":
                    bool hasUnlockedMainGate = ItemHelpers.HasReceivedItem(0x0001);
                    bool hasActivatedEmergencySystem = MemoryHelpers.ReadAddressDataBit(Addresses.HasActivatedEmergencySystem);
                    bool hasWatchedMainGateOpenCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasWatchedMainGateOpenCutscene);
                    MemoryHelpers.WriteCode(Cheats.FastForwardOutsideMainGate(currentProgressionCounter, hasUnlockedMainGate, hasActivatedEmergencySystem, hasWatchedMainGateOpenCutscene));
                    break;

                case var data when data.AreaName == "Apple Market":
                    bool hasRescuedShopOwnersHusbandApple = MemoryHelpers.ReadAddressDataBit(Addresses.HasRescuedShopOwnersHusband);
                    bool hasEarnedClassBLicenseApple = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasShownRollRedRefractorApple = MemoryHelpers.ReadAddressDataBit(Addresses.HasShownRollRedRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardAppleMarket(currentProgressionCounter, hasRescuedShopOwnersHusbandApple, hasEarnedClassBLicenseApple, hasShownRollRedRefractorApple));
                    break;

                case var data when data.AreaName == "Underground Ruins":
                    bool hasRescuedShopOwnersHusbandRuins = MemoryHelpers.ReadAddressDataBit(Addresses.HasRescuedShopOwnersHusband);
                    MemoryHelpers.WriteCode(Cheats.FastForwardUndergroundRuins(currentProgressionCounter, hasRescuedShopOwnersHusbandRuins));
                    break;

                case var data when data.AreaName == "Downtown":
                    bool hasEarnedClassBLicenseDowntown = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasUnlockedSubCitiesDowntown = ItemHelpers.HasReceivedItem(0x0002);
                    MemoryHelpers.WriteCode(Cheats.FastForwardDowntown(currentProgressionCounter, hasEarnedClassBLicenseDowntown, hasUnlockedSubCitiesDowntown));
                    break;

                case var data when data.AreaName == "Uptown":
                    bool hasUnlockedSubCitiesUptown = ItemHelpers.HasReceivedItem(0x0002);
                    MemoryHelpers.WriteCode(Cheats.FastForwardUptown(currentProgressionCounter, hasUnlockedSubCitiesUptown));
                    break;

                case var data when data.AreaName == "Old City":
                    //bool HasUnlockedSubCitiesOldCity = Memory.ReadBit(Addresses.HasUnlockedSubCities.Address, Addresses.HasUnlockedSubCities.BitNumber ?? 1);
                    MemoryHelpers.WriteCode(Cheats.FastForwardOldCity(currentProgressionCounter));
                    break;

                case var data when data.AreaName == "City Hall": // && !data.RoomName.Contains("Amelia's Office"):
                    bool hasEarnedClassBLicenseCityHall = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseCityHall = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    bool hasDefeatedBalkonGeratCityHall = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                    MemoryHelpers.WriteCode(Cheats.FastForwardCityHall(currentProgressionCounter, hasEarnedClassBLicenseCityHall, hasEarnedClassALicenseCityHall, hasDefeatedBalkonGeratCityHall));
                    break;

                //case var data when data.AreaName == "City Hall" && data.RoomName.Contains("Amelia's Office"):
                //    bool hasEarnedClassBLicenseAmelia = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                //    MemoryHelpers.WriteCode(Cheats.FastForwardCityHallAmelia(currentProgressionCounter, hasEarnedClassBLicenseAmelia));
                //    break;

                case var data when data.AreaName == "City Hall (Indoors)":
                    bool hasActivatedEmergencySystemcityHallIndoors = MemoryHelpers.ReadAddressDataBit(Addresses.HasActivatedEmergencySystem);
                    MemoryHelpers.WriteCode(Cheats.FastForwardCityHallIndoors(currentProgressionCounter, hasActivatedEmergencySystemcityHallIndoors));
                    break;

                case var data when data.AreaName == "Yass Plains":
                    bool hasEarnedClassBLicenseYass = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseYass = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    bool hasDefeatedBalkonGeratYass = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                    MemoryHelpers.WriteCode(Cheats.FastForwardYassPlains(currentProgressionCounter, hasEarnedClassBLicenseYass, hasEarnedClassALicenseYass, hasDefeatedBalkonGeratYass));
                    break;

                case var data when data.AreaName == "Clozer Woods With Bridge":
                    bool hasEarnedClassBLicenseClozerBridge = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseClozerBridge = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    bool hasDefeatedBalkonGeratClozerBridge = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                    MemoryHelpers.WriteCode(Cheats.FastForwardClozerWoodsWithBridge(currentProgressionCounter, hasEarnedClassBLicenseClozerBridge, hasEarnedClassALicenseClozerBridge, hasDefeatedBalkonGeratClozerBridge));
                    break;

                case var data when data.AreaName == "Clozer Woods":
                    bool hasEarnedClassBLicenseClozer = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseClozer = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    MemoryHelpers.WriteCode(Cheats.FastForwardClozerWoods(currentProgressionCounter, hasEarnedClassBLicenseClozer, hasEarnedClassALicenseClozer));
                    break;

                case var data when data.AreaName == "Wily's Boat" && data.RoomName != "Outside Boat Shop":
                    if (MemoryHelpers.ReadAddressDataBit(Addresses.HasYellowRefractor))
                    {
                        bool boatIsFixed = MemoryHelpers.ReadAddressDataBit(Addresses.BoatIsFixed);
                        bool hasDefeatedBalkonGeratWily = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                        MemoryHelpers.WriteCode(Cheats.FastForwardWilysBoat(currentProgressionCounter, boatIsFixed, hasDefeatedBalkonGeratWily));
                    }
                    break;

                case var data when data.AreaName == "Wily's Boat" && data.RoomName == "Outside Boat Shop":
                    if (
                        MemoryHelpers.ReadAddressDataBit(Addresses.HasYellowRefractor) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.HasCalledRollToFixBoat)
                    )
                    {
                        MemoryHelpers.WriteCode(Cheats.EnableFixBoatCallRoll());
                    }
                    break;

                case var data when data.AreaName == "Lake Jyun":
                    bool hasDefeatedBalkonGeratLake = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                    MemoryHelpers.WriteCode(Cheats.FastForwardLakeJyun(hasDefeatedBalkonGeratLake));
                    break;

                case var data when data.AreaName == "Flutter Takeoff":
                    MemoryHelpers.WriteCode(Cheats.EnableRedRefractorCutscene());
                    break;

                case var data when data.AreaName == "Gesellschaft Interior":
                    // This is for during Bonne cutscenes
                    // Player is sent to Amelia if value is low, Wily's if 0x06
                    bool hasTakenRedRefractorGesellschaft = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenRedRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardGesellschaft(currentProgressionCounter, hasTakenRedRefractorGesellschaft));
                    break;

                case var data when data.AreaName == "Flutter To Sub-Gate Cutscene":
                    Log.Logger.Information("Applying Flutter to Sub-Gate Cutscene fast-forward.");
                    bool hasActivatedEmergencySystemFlutterCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasActivatedEmergencySystem);
                    bool hasDefeatedFockeWulfFlutterCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedFockeWulf);
                    MemoryHelpers.WriteCode(Cheats.FastForwardFlutterToSubGateCutscene(hasActivatedEmergencySystemFlutterCutscene, hasDefeatedFockeWulfFlutterCutscene));
                    break;

                case var data when data.AreaName == "Gesellschaft Battle":
                    Log.Logger.Information("Applying Gesellshaft Battle fast-forward");
                    MemoryHelpers.WriteCode(Cheats.FastForwardGesellschaftBattle());
                    break;

                case var data when data.AreaName == "Main Gate":
                    bool hasShownRollRedRefractorMainGate = MemoryHelpers.ReadAddressDataBit(Addresses.HasShownRollRedRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardMainGate(currentProgressionCounter, hasShownRollRedRefractorMainGate));
                    break;

                default:
                    break;
            }
        }

        public static void HandleSlowCodeWrites(LevelData currentLevelData, byte currentProgressionCounter)
        {
            string areaName = currentLevelData.AreaName;

            switch (areaName)
            {
                default:
                    break;
            }
        }

        public static void HandleYellowRefractorTerminal()
        {
            // Basically we want this byte to reflect key in inventory when interacting with terminal and key sprites picked up elsewhere
            // This byte is used for multiple things in the subgate which makes it a big pain in the ass to do with MIPS edits. But this seems to win the race condition in the fast game loop, so whatever
            bool terminalInteractionBitSet = Memory.ReadBit(Addresses.YellowRefractorTerminal.Address, 7);
            bool hasTakenYellowRefractor = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenYellowRefractor);
            bool shouldUseTerminalInteractionPath = terminalInteractionBitSet && !hasTakenYellowRefractor;

            if (shouldUseTerminalInteractionPath) // This bit is flipped when interacting with terminal and also, for a split second(?), when picking up keys. Small sleep seems to get around distinction.
            {
                System.Threading.Thread.Sleep(1);
            }

            byte yellowRefractorTerminalVal = Memory.ReadByte(Addresses.YellowRefractorTerminal.Address);
            int terminalMask = shouldUseTerminalInteractionPath ? 0x8F : 0x0F;
            int focusedKeyCount = 0;

            if (
                shouldUseTerminalInteractionPath &&
                DataDicts.ItemDataDict.TryGetValue(0x022E, out var cardonKey1Data) &&
                DataDicts.ItemDataDict.TryGetValue(0x022F, out var cardonKey2Data) &&
                DataDicts.ItemDataDict.TryGetValue(0x0230, out var cardonKey3Data) &&
                cardonKey1Data.InventoryAddressData != null &&
                cardonKey2Data.InventoryAddressData != null &&
                cardonKey3Data.InventoryAddressData != null
            )
            {
                bool hasCardonKey1 = MemoryHelpers.ReadAddressDataBit(cardonKey1Data.InventoryAddressData);
                bool hasCardonKey2 = MemoryHelpers.ReadAddressDataBit(cardonKey2Data.InventoryAddressData);
                bool hasCardonKey3 = MemoryHelpers.ReadAddressDataBit(cardonKey3Data.InventoryAddressData);
                focusedKeyCount = new[] { hasCardonKey1, hasCardonKey2, hasCardonKey3 }.Count(t => t);
            }
            else if (
                DataDicts.LocationDataDict.TryGetValue(60, out var hasPickedUpCardonKey1LocationData) &&
                DataDicts.LocationDataDict.TryGetValue(61, out var hasPickedUpCardonKey2LocationData) &&
                DataDicts.LocationDataDict.TryGetValue(62, out var hasPickedUpCardonKey3LocationData)
            )
            {
                bool hasPickedUpCardonKey1 = MemoryHelpers.ReadAddressDataBit(hasPickedUpCardonKey1LocationData.CheckAddressData);
                bool hasPickedUpCardonKey2 = MemoryHelpers.ReadAddressDataBit(hasPickedUpCardonKey2LocationData.CheckAddressData);
                bool hasPickedUpCardonKey3 = MemoryHelpers.ReadAddressDataBit(hasPickedUpCardonKey3LocationData.CheckAddressData);
                focusedKeyCount = new[] { hasPickedUpCardonKey1, hasPickedUpCardonKey2, hasPickedUpCardonKey3 }.Count(t => t);
            }

            int yellowRefractorTerminalValOverwrite = focusedKeyCount switch
            {
                1 => (yellowRefractorTerminalVal & terminalMask) | 0x40,
                2 => (yellowRefractorTerminalVal & terminalMask) | 0x60,
                3 => (yellowRefractorTerminalVal & terminalMask) | 0x70,
                _ => (yellowRefractorTerminalVal & terminalMask) | 0x00,
            };
            Memory.WriteByte(Addresses.YellowRefractorTerminal.Address, (byte)yellowRefractorTerminalValOverwrite);

            bool hasInteractedWithYellowTerminalOnce = MemoryHelpers.ReadAddressDataBit(Addresses.HasInteractedWithYellowTerminalOnce);
            if (!hasInteractedWithYellowTerminalOnce)
            {
                MemoryHelpers.WriteAddressDataBit(Addresses.HasInteractedWithYellowTerminalOnce, true);
            }
        }

        public static void HandleOddPails(LevelData currentLevelData)
        {
            string levelName = currentLevelData.AreaName + ": " + currentLevelData.RoomName;

            switch (levelName)
            {
                case "Downtown: Downtown":
                    // Handle library pail in case player can't trigger worker dialogue because they already have the Saw
                    if (
                        MemoryHelpers.ReadAddressDataBit(Addresses.SawWorkerDialogueIsReady) ||
                        MemoryHelpers.ReadAddressDataBit(Addresses.TurnedInSaw)
                    )
                    {
                        MemoryHelpers.WriteAddressDataBit(Addresses.SawPailIsReady, true);
                    }
                    // Handle center pail in case player can't enable dialogue chain because they already have the Bag
                    if (Memory.ReadBit(0xBE3BA, 6))
                    {
                        MemoryHelpers.WriteAddressDataBit(Addresses.BagPailIsReady, true);
                    }
                    break;

                default:
                    break;
            }
        }

        public static void HandleFlutterFixedBrokenDistinction(LevelData currentLevelData)
        {
            switch (currentLevelData.AreaName)
            {
                case "Apple Market":
                    WriteFlutterFixedBrokenExitTarget("Apple Market -> Cardon Forest");
                    break;

                case "Outside Cardon Forest Sub-Gate":
                    WriteFlutterFixedBrokenExitTarget("Outside Cardon Forest Sub-Gate -> Cardon Forest");
                    break;

                case "Underground Ruins":
                    WriteFlutterFixedBrokenExitTarget("Underground Ruins, Room 1 (Junk Store Man Area) -> Cardon Forest");
                    WriteFlutterFixedBrokenExitTarget("Underground Ruins, Room 2 -> Cardon Forest");
                    break;

                default:
                    break;
            }
        }

        private static void WriteFlutterFixedBrokenExitTarget(string exitName)
        {
            if (!DataDicts.ExitDataDict.TryGetValue(exitName, out ExitData? exitData))
            {
                return;
            }

            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasShownRollRedRefractor))
            {
                Memory.WriteByte(exitData.TargetAreaAddress, 0x1B);
            }
            else
            {
                Memory.WriteByte(exitData.TargetAreaAddress, 0x03);
            }
        }

        public static void HandleAreaExitLocks(LevelData currentLevelData)
        {
            HandleCitizensCardExitLocks(currentLevelData);
            HandleClassBLicenseExitLocks(currentLevelData);
            HandleClassALicenseExitLocks(currentLevelData);
            HandleMainGateExitLocks(currentLevelData);
            HandleSubCityUnlockExitLocks(currentLevelData);
        }

        private static void TryLockExitWithDebug(string exitName, ExitData exitData)
        {
            bool didLock = exitData.LockExit();
            if (!didLock)
            {
                Log.Logger.Information($"LockExit failed for '{exitName}'. Source='{exitData.SourceName}', Target='{exitData.TargetName}', IsDoor={exitData.IsDoor}.");
            }
            else
            {
                Log.Logger.Information($"LockExit succeeded for '{exitName}'. Source='{exitData.SourceName}', Target='{exitData.TargetName}', IsDoor={exitData.IsDoor}.");
            }

        }

        private static void TryLockExitByName(string exitName)
        {
            if (!DataDicts.ExitDataDict.TryGetValue(exitName, out var exitData))
            {
                Log.Logger.Information($"LockExit skipped because exit '{exitName}' was not found in ExitDataDict.");
                return;
            }

            TryLockExitWithDebug(exitName, exitData);
        }

        public static void HandleCitizensCardExitLocks(LevelData currentLevelData)
        {
            if (ItemHelpers.HasReceivedItem(0x022A))
            {
                return;
            }

            switch (currentLevelData.AreaName)
            {
                case "Apple Market":
                    TryLockExitByName("Apple Market -> Downtown");
                    break;

                case "Yass Plains":
                    TryLockExitByName("Clozer Woods With Bridge -> Underground Ruins");
                    break;

                case "Outside Main Gate":
                    TryLockExitByName("Outside Main Gate -> Old City");
                    break;

                case "Wily's Boat":
                    TryLockExitByName("Wily's Boat, Outside (Walkway) -> Uptown");
                    break;

                case "Underground ruins":
                    TryLockExitByName("Underground Ruins, Room 3 (Sewer) -> Downtown");
                    TryLockExitByName("Underground Ruins, Room 4  -> Old City");
                    break;

                default:
                    break;
            }
        }

        public static void HandleClassBLicenseExitLocks(LevelData currentLevelData)
        {
            if (ItemHelpers.HasReceivedItem(0x022C))
            {
                return;
            }

            switch (currentLevelData.AreaName)
            {
                case "Clozer Woods With Bridge":
                    TryLockExitByName("Yass Plains -> City Hall");
                    break;

                case "Cardon Forest (Flutter Broken)":
                    TryLockExitByName("Cardon Forest South (Flutter Broken) -> Underground Ruins, Room 2");
                    //if (DataDicts.ExitDataDict.TryGetValue("Cardon Forest South (Flutter Broken) -> Underground Ruins, Room 2", out var exitDataCardonForestBrokenToRuins2))
                    //{
                    //    _ = exitDataCardonForestBrokenToRuins2.LockExit();
                    //}
                    break;

                case "Cardon Forest (Flutter Fixed)":
                    TryLockExitByName("Cardon Forest South (Flutter Broken) -> Underground Ruins, Room 2");
                    //TryLockExitByName("Cardon Forest South (Flutter Fixed) -> Underground Ruins, Room 2");
                    break;

                //case "Old City":
                //    TryLockExitByName("Old City -> Underground Ruins");
                //    TryLockExitByName("Old City (dogs, no weapons) -> Underground Ruins, to Main Gate");
                //    break;

                case "Cardon Forest Sub-Gate":
                    TryLockExitByName("Cardon Forest Sub-Gate, Room 1 (N) -> Underground Ruins");
                    break;

                case "Lake Jyun Sub-Gate":
                    TryLockExitByName("Lake Jyun Sub-Gate, Room 4 (W) -> Underground Ruins (NW)");
                    TryLockExitByName("Lake Jyun Sub-Gate, Room 4 (E) -> Underground Ruins (NE)");
                    break;

                case "Clozer Woods Sub-Gate":
                    TryLockExitByName("Clozer Woods Sub-Gate, Room 10 -> Underground Ruins");
                    break;

                case "Main Gate":
                    TryLockExitByName("East Door Console Room -> Underground Ruins, NE Area 2");
                    break;

                case "Underground Ruins":
                    TryLockExitByName("Underground Ruins, Room 1 (Junk Store Man Area) -> Room 2");
                    break;

                default:
                    break;
            }
        }

        public static void HandleClassALicenseExitLocks(LevelData currentLevelData)
        {
            if (ItemHelpers.HasReceivedItem(0x022B))
            {
                return;
            }
            switch (currentLevelData.AreaName)
            {
                case "Outside Cardon Forest Sub-Gate":
                    TryLockExitByName("Outside Cardon Forest Sub-Gate -> Cardon Forest Sub-Gate");
                    break;

                case "Lake Jyun":
                    TryLockExitByName("On the Lake -> Lake Jyun Sub-Gate");
                    break;

                case "Clozer Woods Sub-Gate":
                    TryLockExitByName("Flutter Lobby -> Clozer Woods Sub-Gate");
                    break;

                case "Underground Ruins":
                    TryLockExitByName("Underground Ruins, Room 2 -> Cardon Forest Sub-gate");
                    TryLockExitByName("Underground Ruins, Room 7 -> Lake Jyun Sub-Gate (W)");
                    TryLockExitByName("Underground Ruins, Room 7 -> Lake Jyun Sub-Gate (E)");
                    TryLockExitByName("Underground Ruins, Room 9 -> Clozer Woods Sub-Gate");
                    break;

                default:
                    break;
            }
        }

        // The outside main gate to main gate is handled in the fast foward cheat. Little room from Bruno is ignored as well.
        public static void HandleMainGateExitLocks(LevelData currentLevelData)
        {
            if (ItemHelpers.HasReceivedItem(0x0001))
            {
                return;
            }
            switch (currentLevelData.AreaName)
            {
                case "Underground Ruins":
                    TryLockExitByName("Underground Ruins, Room 2 -> Main Gate");
                    break;
                default:
                    break;
            }
        }

        // The Downtown and Uptown sub-city locks are done in the fast forward cheats by just preventing the sub-city to raise up
        public static void HandleSubCityUnlockExitLocks(LevelData currentLevelData)
        {
            if (ItemHelpers.HasReceivedItem(0x0002))
            {
                return;
            }

            switch (currentLevelData.AreaName)
            {
                case "Old City":
                    // TODO: Actually delete sub city entrance model if not received unlock sub-cities
                    TryLockExitByName("Old City (dogs, no weapons) -> Watcher Sub-City");
                    break;

                default:
                    break;
            }
        }

        public static void PlayCutscene(byte cutsceneID)
        {
            Memory.WriteByte(0xC4C4C, 0x00);        // Clear play status(?) (0 is start/pause, 1 is playing, 2 stop)
            Memory.WriteByte(0xC4C4D, 0x00);        // Load cutscene step(?) (no standard format here)
            Memory.WriteByte(0xC4C49, cutsceneID);  // ID is unique
            Memory.WriteByte(Addresses.CutsceneFlag.Address, 0x01);        // Setting this plays the config queued up above
        }

        public static void StopCutscene()
        {
            Memory.WriteByte(0xC4C4C, 0x02);
        }
    }
}
