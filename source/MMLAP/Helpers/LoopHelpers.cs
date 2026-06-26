using Archipelago.Core;
using Archipelago.Core.Util;
using Archipelago.MultiClient.Net.Models;
using Avalonia.Rendering;
using MMLAP.Models;
using Serilog;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static MMLAP.Models.MMLEnums;

namespace MMLAP.Helpers
{
    public class LoopHelpers
    {
        public static void CheckGoalCondition()
        {
            ArchipelagoClient? apClient = App.APClient;
            if (
                App.HasSubmittedGoal ||
                apClient?.Options == null ||
                !App.LocationManager_EnableLocationsCondition() ||
                !apClient.Options.TryGetValue("goal", out var goal)
            )
            {
                return;
            }

            bool isGoalComplete = (CompletionGoal)int.Parse(goal.ToString()) switch
            {
                CompletionGoal.JUNO => MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedJuno),
                CompletionGoal.ALL_BOSSES =>
                    MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedFerdinand) &&
                    MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBonBonne) &&
                    MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedMarlwolf) &&
                    MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat) &&
                    MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedGarudoriten) &&
                    MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedKarumunaBashTrio) &&
                    MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedFockeWulf) &&
                    MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedTheodoreBruno) &&
                    MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedJuno),
                _ => false
            };

            if (isGoalComplete)
            {
                apClient.SendGoalCompletion();
            }
            return;
        }

        public static List<int> HandleOddLocationText(
            LevelData currentLevelData,
            Dictionary<long, ItemData>? scoutedLocationItemData,
            ConcurrentStack<TextData> textDataToWriteStack,
            List<long>? completedLocationIds = null
        )
        {
            List<int> processedLocationIds = [];
            switch (currentLevelData)
            {
                case var data when data.AreaName == "Apple Market":
                    // "Rescue the shop owner's husband" location text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(104, out var rescueScoutedItemData) &&
                        DataDicts.LocationDataDict.TryGetValue(104, out var rescueLocationData) &&
                        rescueLocationData.Name == "Rescue the shop owner's husband" &&
                        rescueLocationData.TextBoxStartAddress != null
                    )
                    {
                        ItemData itemDataToWrite = completedLocationIds != null && completedLocationIds.Contains(104) ? DataDicts.ItemDataDict[0x00FF] : rescueScoutedItemData;
                        // Being careful about text box overflow. Replacing new text window from man -> "You got" with a newpage, which saves a bunch of bytes.
                        // Not bothering to restore this text
                        byte[] writeTextArr = TextHelpers.EncodeYouGotItemWindow(itemDataToWrite, prefix: TextHelpers.newPage, suffix: [0x9F, 0x99, 0x00, 0xBD, 0xA9, 0x84]);
                        Memory.WriteByteArray(rescueLocationData.TextBoxStartAddress ?? 0, writeTextArr);
                        processedLocationIds.Add(104);
                    }
                    // Handle case when trying to get the lipstick
                    if (
                        MemoryHelpers.ReadAddressDataBit(Addresses.IsGatheringLipstick) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense)
                    )
                    {
                        byte[] veggieManText = TextHelpers.ConcatArrayList([
                            [0x8C, 0x40, 0x00, 0xA2, 0x00, 0x10, 0x03, 0x93, 0x00, 0x08],
                            TextHelpers.EncodeSimpleString("Red paint? Hmmm..."),
                            TextHelpers.newPage,
                            TextHelpers.EncodeSimpleString("I've heard that pirate\nattacks have caused\nsupply chain issues!"),
                            TextHelpers.endWindow,
                        ]);
                        Memory.WriteByteArray(0x00155FC2, veggieManText);
                    }
                    break;
                case var data when data.AreaName == "City Hall" && (data.RoomName == "Amelia's Office" || data.RoomName == "Amelia's Office (wrecked)"):
                    // Class B License text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(131, out var classBScoutedItemData) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense)
                    )
                    {
                        ItemData itemDataToWrite = completedLocationIds != null && completedLocationIds.Contains(131) ? DataDicts.ItemDataDict[0x00FF] : classBScoutedItemData;
                        uint textStartAddress = 0x154500;
                        uint textEndAddress = 0x154521;
                        byte[] hasEarnedClassBLicenseTextOverwrite = TextHelpers.EncodeYouGotItemWindow(itemDataToWrite, prefix: TextHelpers.newPage, guaranteedLength: textEndAddress - textStartAddress);
                        Memory.WriteByteArray(textStartAddress, hasEarnedClassBLicenseTextOverwrite);
                        processedLocationIds.Add(131);
                    }
                    // Class A License text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(132, out var classAScoutedItemData) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense)
                    )
                    {
                        ItemData itemDataToWrite = completedLocationIds != null && completedLocationIds.Contains(132) ? DataDicts.ItemDataDict[0x00FF] : classAScoutedItemData;
                        uint textStartAddress = 0x154E1D;
                        byte[] hasEarnedClassALicenseTextOverwrite = TextHelpers.EncodeYouGotItemWindow(itemDataToWrite);
                        Memory.WriteByteArray(textStartAddress, hasEarnedClassALicenseTextOverwrite);
                        processedLocationIds.Add(132);
                    }
                    break;

                case var data when data.AreaName == "City Hall" && data.RoomName == "City Hall Outdoors":
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

                case var data when data.AreaName == "Uptown" && data.RoomName == "Ira's Room":
                    // "Cure Ira's illness" location text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(111, out var iraScoutedItemData) &&
                        DataDicts.LocationDataDict.TryGetValue(111, out var iraLocationData) &&
                        iraLocationData.Name == "Cure Ira's illness" &&
                        iraLocationData.TextBoxStartAddress != null
                    )
                    {
                        ItemData itemDataToWrite = completedLocationIds != null && completedLocationIds.Contains(111) ? DataDicts.ItemDataDict[0x00FF] : iraScoutedItemData;
                        textDataToWriteStack.Push(TextHelpers.OverwriteText(iraLocationData.TextBoxStartAddress ?? 0, TextHelpers.EncodeYouGotItemWindow(itemDataToWrite)));
                        processedLocationIds.Add(111);
                    }
                    break;

                case var data when data.AreaName == "Cardon Forest (Flutter Broken)" && data.RoomName == "City Entrance":
                    // "Earning citizenship" location text handling
                    if (
                        scoutedLocationItemData != null &&
                        scoutedLocationItemData.TryGetValue(130, out var citizensCardScoutedItemData) &&
                        DataDicts.LocationDataDict.TryGetValue(130, out var citizensCardLocationData) &&
                        citizensCardLocationData.Name == "Earn citizenship in Kattelox City" &&
                        citizensCardLocationData.TextBoxStartAddress != null
                    )
                    {
                        ItemData itemDataToWrite = completedLocationIds != null && completedLocationIds.Contains(130) ? DataDicts.ItemDataDict[0x00FF] : citizensCardScoutedItemData;
                        // Being careful about text box overflow. Replacing new text window from man -> "You got" with a newpage, which saves a bunch of bytes.
                        // Not bothering to restore this text
                        byte[] writeTextArr = TextHelpers.EncodeYouGotItemWindow(itemDataToWrite, prefix: TextHelpers.newPage, suffix: TextHelpers.endWindow); // suffix: [0x9F, 0x99, 0x00, 0xBD, 0xA9, 0x89, 0x00]);
                        Memory.WriteByteArray(citizensCardLocationData.TextBoxStartAddress ?? 0, writeTextArr);
                        processedLocationIds.Add(130);
                    }
                    break;

                case var data when data.AreaName == "Cardon Forest (Flutter Fixed)" && data.RoomName == "Crash Site":
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
            return processedLocationIds;
        }

        public static List<int> UpdateTextBoxesForCompletedLocationsNonOdd(LevelData currentLevelData, ushort currentLevelID, ConcurrentStack<TextData> textDataToWriteStack, List<int> ignoreIds)
        {
            // This function proactively overwrites text boxes for already-completed locations with Nothing item
            List<int> processedCompletedLocationIds = [];

            ArchipelagoClient? apClient = App.APClient;
            if (
                apClient?.CurrentSession == null ||
                currentLevelData == null
            )
            {
                return processedCompletedLocationIds;
            }

            IReadOnlyCollection<long> allLocationsChecked = apClient.CurrentSession.Locations.AllLocationsChecked;
            if (allLocationsChecked.Count == 0)
            {
                return processedCompletedLocationIds;
            }

            foreach (var locationDataKV in DataDicts.LocationDataDict)
            {
                int locationId = locationDataKV.Key;
                LocationData locationData = locationDataKV.Value;

                // Overwrite if:
                // 1. Already completed
                // 2. In the current area+room
                // 3. Has a text box address
                if (
                    allLocationsChecked.Contains(locationId) &&
                    locationData.LevelData?.AreaName == currentLevelData.AreaName &&
                    locationData.LevelData?.RoomName == currentLevelData.RoomName
                )
                {
                    if (locationData.TextBoxStartAddress != null)
                    {
                        TextData overwrittenText = TextHelpers.OverwriteText(
                            locationData.TextBoxStartAddress.Value,
                            TextHelpers.EncodeYouGotItemWindow(new ItemData(MMLEnums.ItemCategory.Nothing, "Nothing"))
                        );
                        textDataToWriteStack.Push(overwrittenText);
                    }
                    if (locationData.ChestItemSignatureAddress != null)
                    {
                        Memory.WriteByteArray((locationData.ChestItemSignatureAddress ?? 0) + 1, [0x02, 0xFF], Enums.Endianness.Little);
                    }
                    processedCompletedLocationIds.Add(locationId);
                }
            }

            return processedCompletedLocationIds;
        }

        public static void HandleLoadingFastCodeWrites(LevelData currentLevelData, byte currentProgressionCounter)
        {
            var apClient = App.APClient;
            if (apClient == null)
            {
                return;
            }

            switch (currentLevelData)
            {
                case var data when data.AreaName == "Cardon Forest (Flutter Broken)":
                    bool hasStartedTronDogCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasStartedTronDogCutscene);
                    bool hasEarnedClassBLicense = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedCitizenship = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedCitizenship);
                    MemoryHelpers.WriteCode(Cheats.FastForwardCardonForestFlutterBroken(currentProgressionCounter, hasStartedTronDogCutscene, hasEarnedClassBLicense, hasEarnedCitizenship));
                    break;

                case var data when data.AreaName == "Cardon Forest (Flutter Fixed)":
                    bool hasDefeatedJunoFlutterFixed = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedJuno);
                    bool hasWatchedFlutterFixFromJunoCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasWatchedFlutterFixFromJunoCutscene);
                    bool hasCompletedGoal = App.HasSubmittedGoal;
                    MemoryHelpers.WriteCode(Cheats.FastForwardCardonForestFlutterFixed(currentProgressionCounter, hasDefeatedJunoFlutterFixed, hasWatchedFlutterFixFromJunoCutscene, hasCompletedGoal));
                    if (
                        MemoryHelpers.ReadAddressDataBit(Addresses.HasFinishedWatchingJunoDefeatCutscene) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.HasStartedFlutterFixFromJunoCutscene)
                    )
                    {
                        if (
                            !MemoryHelpers.ReadAddressDataBit(Addresses.CutsceneFlag) &&
                            (Memory.ReadUShort(0xC4C5C) == 0x215B)
                        )
                        {
                            PlayCutscene(0x00, 0x00);
                            System.Threading.Thread.Sleep(2000);
                        }
                    }
                    break;

                case var data when data.AreaName == "Outside Cardon Forest Sub-Gate":
                    bool hasDefeatedBonBonne = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBonBonne);
                    bool hasCompletedCardonTankEvent = MemoryHelpers.ReadAddressDataBit(Addresses.HasCompletedCardonTankEvent);
                    MemoryHelpers.WriteCode(Cheats.FastForwardOutsideCardonSubgate(currentProgressionCounter, hasDefeatedBonBonne, hasCompletedCardonTankEvent));
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
                        PlayCutscene(0x19, 0x00);
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
                    MemoryHelpers.WriteCode(Cheats.FastForwardOutsideMainGate(currentProgressionCounter, hasUnlockedMainGate, hasActivatedEmergencySystem, hasWatchedMainGateOpenCutscene, apClient.Options));
                    break;

                case var data when data.AreaName == "Apple Market":
                    bool hasRescuedShopOwnersHusbandApple = MemoryHelpers.ReadAddressDataBit(Addresses.HasRescuedShopOwnersHusband);
                    bool hasEarnedClassBLicenseApple = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseApple = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    bool hasShownRollRedRefractorApple = MemoryHelpers.ReadAddressDataBit(Addresses.HasShownRollRedRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardAppleMarket(currentProgressionCounter, hasRescuedShopOwnersHusbandApple, hasEarnedClassBLicenseApple, hasEarnedClassALicenseApple, hasShownRollRedRefractorApple));
                    break;

                case var data when data.AreaName == "Underground Ruins":
                    bool hasRescuedShopOwnersHusbandRuins = MemoryHelpers.ReadAddressDataBit(Addresses.HasRescuedShopOwnersHusband);
                    MemoryHelpers.WriteCode(Cheats.FastForwardUndergroundRuins(currentProgressionCounter, hasRescuedShopOwnersHusbandRuins));
                    break;

                case var data when data.AreaName == "Downtown":
                    bool hasEarnedClassBLicenseDowntown = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasUnlockedSubCitiesDowntown = ItemHelpers.HasReceivedItem(0x0002);
                    bool hasDefeatedBalkonGeratDowntown = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                    bool hasTakenRedRefractorDowntown = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenRedRefractor);
                    bool hasActivatedUnlockSubCitiesDowntown = MemoryHelpers.ReadAddressDataBit(Addresses.HasActivatedUnlockSubCities);
                    MemoryHelpers.WriteCode(Cheats.FastForwardDowntown(currentProgressionCounter, hasEarnedClassBLicenseDowntown, hasDefeatedBalkonGeratDowntown, hasTakenRedRefractorDowntown, hasUnlockedSubCitiesDowntown, hasActivatedUnlockSubCitiesDowntown, apClient.Options));
                    break;

                case var data when data.AreaName == "Uptown":
                    bool hasUnlockedSubCitiesUptown = ItemHelpers.HasReceivedItem(0x0002);
                    bool hasShownRollRedRefractorUptown = MemoryHelpers.ReadAddressDataBit(Addresses.HasShownRollRedRefractor);
                    bool hasActivatedUnlockSubCitiesUptown = MemoryHelpers.ReadAddressDataBit(Addresses.HasActivatedUnlockSubCities);
                    MemoryHelpers.WriteCode(Cheats.FastForwardUptown(currentProgressionCounter, hasUnlockedSubCitiesUptown, hasShownRollRedRefractorUptown, hasActivatedUnlockSubCitiesUptown, apClient.Options));
                    break;

                case var data when data.AreaName == "Old City":
                    //bool HasUnlockedSubCitiesOldCity = Memory.ReadBit(Addresses.HasUnlockedSubCities.Address, Addresses.HasUnlockedSubCities.BitNumber ?? 1);
                    MemoryHelpers.WriteCode(Cheats.FastForwardOldCity(currentProgressionCounter));
                    break;

                case var data when data.AreaName == "City Hall": // && !data.RoomName.Contains("Amelia's Office"):
                    bool hasEarnedClassBLicenseCityHall = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseCityHall = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    bool hasDefeatedBalkonGeratCityHall = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                    bool hasTakenRedRefractorCityHall = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenRedRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardCityHall(currentProgressionCounter, hasEarnedClassBLicenseCityHall, hasEarnedClassALicenseCityHall, hasDefeatedBalkonGeratCityHall, hasTakenRedRefractorCityHall));
                    break;

                //case var data when data.AreaName == "City Hall" && data.RoomName.Contains("Amelia's Office"):
                //    bool hasEarnedClassBLicenseAmelia = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                //    MemoryHelpers.WriteCode(Cheats.FastForwardCityHallAmelia(currentProgressionCounter, hasEarnedClassBLicenseAmelia));
                //    break;

                case var data when data.AreaName == "City Hall (Indoors)":
                    bool hasActivatedEmergencySystemCityHallIndoors = MemoryHelpers.ReadAddressDataBit(Addresses.HasActivatedEmergencySystem);
                    MemoryHelpers.WriteCode(Cheats.FastForwardCityHallIndoors(currentProgressionCounter, hasActivatedEmergencySystemCityHallIndoors));
                    break;

                case var data when data.AreaName == "Yass Plains":
                    bool hasEarnedClassBLicenseYass = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseYass = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    bool hasDefeatedBalkonGeratYass = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                    bool hasTakenRedRefractorYass = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenRedRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardYassPlains(currentProgressionCounter, hasEarnedClassBLicenseYass, hasEarnedClassALicenseYass, hasDefeatedBalkonGeratYass, hasTakenRedRefractorYass));
                    break;

                case var data when data.AreaName == "Clozer Woods With Bridge":
                    bool hasEarnedClassBLicenseClozerBridge = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseClozerBridge = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    bool hasDefeatedBalkonGeratClozerBridge = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                    bool hasTakenRedRefractorClozerBridge = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenRedRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardClozerWoodsWithBridge(currentProgressionCounter, hasEarnedClassBLicenseClozerBridge, hasEarnedClassALicenseClozerBridge, hasDefeatedBalkonGeratClozerBridge, hasTakenRedRefractorClozerBridge));
                    break;

                case var data when data.AreaName == "Clozer Woods":
                    bool hasEarnedClassBLicenseClozer = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseClozer = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    MemoryHelpers.WriteCode(Cheats.FastForwardClozerWoods(currentProgressionCounter, hasEarnedClassBLicenseClozer, hasEarnedClassALicenseClozer));
                    break;

                case var data when data.AreaName == "Wily's Boat":
                    if (MemoryHelpers.ReadAddressDataBit(Addresses.HasYellowRefractor))
                    {
                        bool HasFixedBoat = MemoryHelpers.ReadAddressDataBit(Addresses.HasFixedBoat);
                        bool hasDefeatedBalkonGeratWily = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                        MemoryHelpers.WriteCode(Cheats.FastForwardWilysBoat(currentProgressionCounter, HasFixedBoat, hasDefeatedBalkonGeratWily));
                        if (
                            data.RoomName == "Outside Boat Shop" &&
                            !MemoryHelpers.ReadAddressDataBit(Addresses.HasCalledRollToFixBoat)
                        )
                        {
                            MemoryHelpers.WriteCode(Cheats.EnableFixBoatCallRoll());
                        }
                    }
                    break;

                case var data when data.AreaName == "Lake Jyun":
                    bool hasFixedBoat = MemoryHelpers.ReadAddressDataBit(Addresses.HasFixedBoat);
                    bool hasWatchedBalkonGeratDefeatCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasWatchedBalkonGeratDefeatCutscene);
                    MemoryHelpers.WriteCode(Cheats.FastForwardLakeJyun(hasFixedBoat, hasWatchedBalkonGeratDefeatCutscene));
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
                    bool hasActivatedEmergencySystemFlutterCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasActivatedEmergencySystem);
                    bool hasDefeatedFockeWulfFlutterCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedFockeWulf);
                    MemoryHelpers.WriteCode(Cheats.FastForwardFlutterToSubGateCutscene(hasActivatedEmergencySystemFlutterCutscene, hasDefeatedFockeWulfFlutterCutscene));
                    break;

                case var data when data.AreaName == "Gesellschaft Battle":
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
            switch (currentLevelData)
            {
                case var data when data.AreaName == "Lake Jyun":
                    Memory.WriteByte(0xC39BC, 0x00); // This value is read at various points during boss fight and is typically 0. Gai-nee Tooren fight populates it with non-zero and doesn't clean it up ever
                    break;

                //case var data when data.AreaName == "Cardon Forest (Flutter Broken)" && data.RoomName == "City Entrance":
                //    if (
                //        MemoryHelpers.ReadAddressDataBit(Addresses.HasStartedTronDogCutscene) &&
                //        MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedCitizenship) && 
                //        !MemoryHelpers.ReadAddressDataBit(Addresses.HasWatchedServbotTakeoffCutscene) &&
                //        !MemoryHelpers.ReadAddressDataBit(Addresses.CutsceneFlag)
                //    )
                //    {
                //        PlayCutscene(0x08, 0x03);
                //        // Delete servbots
                //        Memory.WriteByte(0x9F108, 0x00);
                //        Memory.WriteByte(0x9F4C8, 0x00);
                //    }
                //    break;
                default:
                    break;
            }
        }

        public static void HandleYellowRefractorTerminal(LevelData currentLevelData)
        {
            if (currentLevelData.AreaName != "Cardon Forest Sub-Gate")
            {
                return;
            }

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
            else
            {
                bool hasPickedUpCardonKey1 = MemoryHelpers.ReadAddressDataBit(Addresses.CardonForestSubGateJakkoStarterKeyPickup);
                bool hasPickedUpCardonKey2 = MemoryHelpers.ReadAddressDataBit(Addresses.CardonForestSubGateConveyorStarterKeyPickup);
                bool hasPickedUpCardonKey3 = MemoryHelpers.ReadAddressDataBit(Addresses.CardonForestSubGateThreeSwitchStarterKeyPickup);
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
                        MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInSaw)
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

        private static void TryLockExitByName(string exitName, MMLEnums.RegionLockActionType regionLockActionType)
        {
            if (!DataDicts.ExitDataDict.TryGetValue(exitName, out var exitData))
            {
                //Log.Logger.Information($"LockExit skipped because exit '{exitName}' was not found in ExitDataDict.");
                return;
            }
            switch (regionLockActionType)
            {
                case MMLEnums.RegionLockActionType.Lock:
                    exitData.LockExit();
                    break;
                case MMLEnums.RegionLockActionType.Unlock:
                    exitData.UnlockExit();
                    break;
                default:
                    break;
            }
        }

        private static void HandleAreaExitLocksItem(LevelData currentLevelData, MMLEnums.RegionLockOption regionLockOption, MMLEnums.RegionLockActionType regionLockActionType, int itemId, Action<LevelData, MMLEnums.RegionLockActionType> regionLockAction)
        {
            switch (regionLockOption)
            {
                case MMLEnums.RegionLockOption.Vanilla:
                    // Assuming the relevant cheats which open the exits are optionized
                    break;
                case MMLEnums.RegionLockOption.Randomized:
                    bool hasReceiveditem = ItemHelpers.HasReceivedItem(itemId);
                    if (
                        (regionLockActionType == MMLEnums.RegionLockActionType.Lock && hasReceiveditem) ||
                        (regionLockActionType == MMLEnums.RegionLockActionType.Unlock && !hasReceiveditem)
                    )
                    {
                        return;
                    }
                    regionLockAction(currentLevelData, regionLockActionType);
                    break;
                case MMLEnums.RegionLockOption.Open:
                    if (regionLockActionType == MMLEnums.RegionLockActionType.Lock)
                    {
                        return;
                    }
                    regionLockAction(currentLevelData, regionLockActionType);
                    break;
                default:
                    break;
            }

        }

        private static Action<LevelData, MMLEnums.RegionLockActionType> regionLockActionCitizensCard = (currentLevelData, regionLockActionType) =>
        {
            switch (currentLevelData.AreaName)
            {
                case "Apple Market":
                    TryLockExitByName("Apple Market -> Downtown", regionLockActionType);
                    break;
                case "Yass Plains":
                    TryLockExitByName("Yass Plains -> City Hall", regionLockActionType);
                    break;
                case "Outside Main Gate":
                    TryLockExitByName("Outside Main Gate -> Old City", regionLockActionType);
                    break;
                case "Wily's Boat":
                    TryLockExitByName("Wily's Boat, Outside (Walkway) -> Uptown", regionLockActionType);
                    break;
                case "Underground Ruins":
                    TryLockExitByName("Underground Ruins, Room 3 (Sewer) -> Downtown", regionLockActionType);
                    TryLockExitByName("Underground Ruins, Room 4 -> Old City", regionLockActionType);
                    break;
                default:
                    break;
            }
        };

        private static Action<LevelData, MMLEnums.RegionLockActionType> regionLockActionClassBLicense = (currentLevelData, regionLockActionType) =>
        {
            switch (currentLevelData.AreaName)
            {
                case "Clozer Woods With Bridge":
                    TryLockExitByName("Clozer Woods With Bridge -> Underground Ruins", regionLockActionType);
                    break;
                case "Cardon Forest (Flutter Broken)":
                    TryLockExitByName("Cardon Forest South (Flutter Broken) -> Underground Ruins, Room 2", regionLockActionType);
                    break;
                case "Cardon Forest (Flutter Fixed)":
                    TryLockExitByName("Cardon Forest South (Flutter Fixed) -> Underground Ruins, Room 2", regionLockActionType);
                    break;
                //case "Old City":
                //    TryLockExitByName("Old City -> Underground Ruins", regionLockActionType);
                //    TryLockExitByName("Old City (dogs, no weapons) -> Underground Ruins, to Main Gate", regionLockActionType);
                //    break;
                case "Cardon Forest Sub-Gate":
                    TryLockExitByName("Cardon Forest Sub-Gate, Room 1 (N) -> Underground Ruins", regionLockActionType);
                    break;
                case "Lake Jyun Sub-Gate":
                    TryLockExitByName("Lake Jyun Sub-Gate, Room 4 (W) -> Underground Ruins (NW)", regionLockActionType);
                    TryLockExitByName("Lake Jyun Sub-Gate, Room 4 (E) -> Underground Ruins (NE)", regionLockActionType);
                    break;
                case "Clozer Woods Sub-Gate":
                    TryLockExitByName("Clozer Woods Sub-Gate, Room 10 -> Underground Ruins", regionLockActionType);
                    break;
                case "Main Gate":
                    TryLockExitByName("East Door Console Room -> Underground Ruins, NE Area 2", regionLockActionType);
                    break;
                case "Underground Ruins":
                    TryLockExitByName("Underground Ruins, Room 1 (Junk Store Man Area) -> Room 2", regionLockActionType);
                    break;
                default:
                    break;
            }
        };

        private static Action<LevelData, MMLEnums.RegionLockActionType> regionLockActionClassALicense = (currentLevelData, regionLockActionType) =>
        {
            switch (currentLevelData.AreaName)
            {
                case "Outside Cardon Forest Sub-Gate":
                    TryLockExitByName("Outside Cardon Forest Sub-Gate -> Cardon Forest Sub-Gate", regionLockActionType);
                    break;
                case "Lake Jyun":
                    TryLockExitByName("On the Lake -> Lake Jyun Sub-Gate", regionLockActionType);
                    break;
                case "Clozer Woods Sub-Gate":
                    TryLockExitByName("Flutter Lobby -> Clozer Woods Sub-Gate", regionLockActionType);
                    break;
                case "Underground Ruins":
                    TryLockExitByName("Underground Ruins, Room 2 -> Cardon Forest Sub-gate", regionLockActionType);
                    TryLockExitByName("Underground Ruins, Room 7 -> Lake Jyun Sub-Gate (W)", regionLockActionType);
                    TryLockExitByName("Underground Ruins, Room 7 -> Lake Jyun Sub-Gate (E)", regionLockActionType);
                    TryLockExitByName("Underground Ruins, Room 9 -> Clozer Woods Sub-Gate", regionLockActionType);
                    break;
                default:
                    break;
            }
        };

        private static Action<LevelData, MMLEnums.RegionLockActionType> regionLockActionMainGateUnlock = (currentLevelData, regionLockActionType) =>
        {
            switch (currentLevelData.AreaName)
            {
                case "Underground Ruins":
                    TryLockExitByName("Underground Ruins, Room 2 -> Main Gate", regionLockActionType);
                    break;
                //case "Outside Main Gate":
                //    TryLockExitByName("Outside Main Gate -> Main Gate (Entrance)", regionLockActionType);
                //    break;
                default:
                    break;
            }
        };

        private static Action<LevelData, MMLEnums.RegionLockActionType> regionLockActionSubCitiesUnlock = (currentLevelData, regionLockActionType) =>
        {
            switch (currentLevelData.AreaName)
            {
                case "Old City":
                    // TODO: Actually delete sub city entrance model if not received unlock sub-cities
                    TryLockExitByName("Old City (dogs, no weapons) -> Watcher Sub-City", regionLockActionType);
                    break;
                //case "Downtown":
                //    TryLockExitByName("Downtown -> Sleeper Sub-City", regionLockActionType);
                //    break;
                //case "Uptown":
                //    TryLockExitByName("Uptown -> Dreamer Sub-City", regionLockActionType);
                //    break;
                default:
                    break;
            }
        };

        public static void HandleAreaExitLocks(LevelData currentLevelData, Dictionary<string, object> options)
        {
            MMLEnums.RegionLockOption shuffleCitizensCard;
            if (options.TryGetValue("shuffleCitizensCard", out var shuffleCitizensCardOption))
            {
                shuffleCitizensCard = (MMLEnums.RegionLockOption)int.Parse(shuffleCitizensCardOption.ToString());
            }
            else
            {
                shuffleCitizensCard = MMLEnums.RegionLockOption.Vanilla;
                Log.Logger.Warning("shuffleCitizensCard option not found.");
            }
            HandleAreaExitLocksItem(currentLevelData, shuffleCitizensCard, MMLEnums.RegionLockActionType.Unlock, 0x022A, regionLockActionCitizensCard);
            HandleAreaExitLocksItem(currentLevelData, shuffleCitizensCard, MMLEnums.RegionLockActionType.Lock, 0x022A, regionLockActionCitizensCard);

            MMLEnums.RegionLockOption shuffleClassBLicense;
            if (options.TryGetValue("shuffleClassBLicense", out var shuffleClassBLicenseOption))
            {
                shuffleClassBLicense = (MMLEnums.RegionLockOption)int.Parse(shuffleClassBLicenseOption.ToString());
            }
            else
            {
                shuffleClassBLicense = MMLEnums.RegionLockOption.Vanilla;
                Log.Logger.Warning("shuffleClassBLicense option not found.");
            }
            HandleAreaExitLocksItem(currentLevelData, shuffleClassBLicense, MMLEnums.RegionLockActionType.Unlock, 0x022C, regionLockActionClassBLicense);
            HandleAreaExitLocksItem(currentLevelData, shuffleClassBLicense, MMLEnums.RegionLockActionType.Lock, 0x022C, regionLockActionClassBLicense);

            MMLEnums.RegionLockOption shuffleClassALicense;
            if (options.TryGetValue("shuffleClassALicense", out var shuffleClassALicenseOption))
            {
                shuffleClassALicense = (MMLEnums.RegionLockOption)int.Parse(shuffleClassALicenseOption.ToString());
            }
            else
            {
                shuffleClassALicense = MMLEnums.RegionLockOption.Vanilla;
                Log.Logger.Warning("shuffleClassALicense option not found.");
            }
            HandleAreaExitLocksItem(currentLevelData, shuffleClassALicense, MMLEnums.RegionLockActionType.Unlock, 0x022B, regionLockActionClassALicense);
            HandleAreaExitLocksItem(currentLevelData, shuffleClassALicense, MMLEnums.RegionLockActionType.Lock, 0x022B, regionLockActionClassALicense);

            MMLEnums.RegionLockOption shuffleMainGateUnlock;
            if (options.TryGetValue("shuffleMainGateUnlock", out var shuffleMainGateUnlockOption))
            {
                shuffleMainGateUnlock = (MMLEnums.RegionLockOption)int.Parse(shuffleMainGateUnlockOption.ToString());
            }
            else
            {
                shuffleMainGateUnlock = MMLEnums.RegionLockOption.Vanilla;
                Log.Logger.Warning("shuffleMainGateUnlock option not found.");
            }
            //HandleAreaExitLocksItem(currentLevelData, shuffleMainGateUnlock, MMLEnums.RegionLockActionType.Unlock, 0x0001, regionLockActionMainGateUnlock); // Handled in FastLoop with Cheats.FastForwardOutsideMainGate by just raising the main gate up
            HandleAreaExitLocksItem(currentLevelData, shuffleMainGateUnlock, MMLEnums.RegionLockActionType.Lock, 0x0001, regionLockActionMainGateUnlock);

            MMLEnums.RegionLockOption shuffleSubCitiesUnlock;
            if (options.TryGetValue("shuffleSubCitiesUnlock", out var shuffleSubCitiesUnlockOption))
            {
                shuffleSubCitiesUnlock = (MMLEnums.RegionLockOption)int.Parse(shuffleSubCitiesUnlockOption.ToString());
            }
            else
            {
                shuffleSubCitiesUnlock = MMLEnums.RegionLockOption.Vanilla;
                Log.Logger.Warning("shuffleSubCitiesUnlock option not found.");
            }
            //HandleAreaExitLocksItem(currentLevelData, shuffleSubCitiesUnlock, MMLEnums.RegionLockActionType.Unlock); // Handled in FastLoop with Cheats.FastForwardDowntown, Cheats.FastForwardUptown, and Cheats.FastForwardOldCity by just raising the sub-cities up
            HandleAreaExitLocksItem(currentLevelData, shuffleSubCitiesUnlock, MMLEnums.RegionLockActionType.Lock, 0x0002, regionLockActionSubCitiesUnlock);
        }

        // The "showing roll the red refractor" only works if you have the yellow refractor
        public static void HandleRedRefractorInSupportCar()
        {
            bool isInSupportCar = MemoryHelpers.ReadAddressDataBit(Addresses.SupportCarRnDFlag);
            bool hasRedRefractor = MemoryHelpers.ReadAddressDataBit(Addresses.HasRedRefractor);
            if (isInSupportCar && hasRedRefractor)
            {
                MemoryHelpers.WriteAddressDataBit(Addresses.HasYellowRefractor, true);
            }
            else
            {
                bool hasReceivedYellowRefractor = ItemHelpers.HasReceivedItem(0x0228);
                if (!hasReceivedYellowRefractor)
                {
                    bool hasYellowRefractor = MemoryHelpers.ReadAddressDataBit(Addresses.HasYellowRefractor);
                    if (hasYellowRefractor)
                    {
                        MemoryHelpers.WriteAddressDataBit(Addresses.HasYellowRefractor, false);
                    }
                }
            }
        }

        // Handling edge case where Class B License is given like 5 times during the cutscene in a loop and also if you skip the cutscene which makes it hard to prevent giving out otherwise. This also applies to the refractors
        public static void HandleCutsceneSkipItemObtains(LevelData currentLevelData)
        {
            switch (currentLevelData)
            {
                case var levelData when levelData.AreaName == "City Hall": // && (levelData.RoomName is "Amelia's Office" or "Amelia's Office (wrecked)"):
                    if (
                        !ItemHelpers.HasReceivedItem(0x022C) && // hasReceivedClassBLicense
                        DataDicts.ItemDataDict.TryGetValue(0x022C, out var itemDataCBL) &&
                        itemDataCBL?.InventoryAddressData != null
                    )
                    {
                        MemoryHelpers.WriteAddressDataBit(itemDataCBL.InventoryAddressData, false);
                    }
                    if (
                        !ItemHelpers.HasReceivedItem(0x022B) && // hasReceivedClassALicense
                        DataDicts.ItemDataDict.TryGetValue(0x022B, out var itemDataCAL) &&
                        itemDataCAL?.InventoryAddressData != null
                    )
                    {
                        MemoryHelpers.WriteAddressDataBit(itemDataCAL.InventoryAddressData, false);
                    }
                    break;
                case var levelData when levelData.AreaName == "Cardon Forest Sub-Gate" && levelData.RoomName == "Refractor Room":
                    if (
                        !ItemHelpers.HasReceivedItem(0x0228) && // Yellow Refractor
                        DataDicts.ItemDataDict.TryGetValue(0x0228, out var itemDataYF) &&
                        itemDataYF?.InventoryAddressData != null
                    )
                    {
                        MemoryHelpers.WriteAddressDataBit(itemDataYF.InventoryAddressData, false);
                    }
                    break;
                case var levelData when levelData.AreaName == "Lake Jyun Sub-Gate" && levelData.RoomName == "Refractor Room":
                    if (
                        !ItemHelpers.HasReceivedItem(0x0229) && // Red Refractor
                        DataDicts.ItemDataDict.TryGetValue(0x0229, out var itemDataRF) &&
                        itemDataRF?.InventoryAddressData != null
                    )
                    {
                        MemoryHelpers.WriteAddressDataBit(itemDataRF.InventoryAddressData, false);
                    }
                    break;
                default:
                    break;
            }
        }

        public static void PlayCutscene(byte cutsceneID, byte subID)
        {
            // Not sure exactly how this works
            // For stealing yellow refractor: SubID = (0 is start/pause, 1 is playing, 2 stop)
            // For cardon forest flutter broken cutscenes, the ID is shared and the subID does different scenes like inspector and servbot takeoff
            // 0xC4C4D/E might also be "cutscene steps" but not sure
            Memory.WriteByte(0xC4C49, cutsceneID);
            Memory.WriteByte(0xC4C4C, subID);
            // Clear all this stuff just in case
            Memory.WriteByteArray(0xC4C4D, Enumerable.Repeat((byte)0x00, 51).ToArray());
            Memory.WriteByte(Addresses.CutsceneFlag.Address, 0x01);        // Setting this plays the config queued up above
        }

        //public static void StopCutscene()
        //{
        //    Memory.WriteByte(0xC4C4C, 0x02);
        //}

        public static void SyncSyntheticLocations()
        {
            IReadOnlyCollection<long>? allLocationsChecked = App.APClient?.CurrentSession?.Locations?.AllLocationsChecked;
            if (allLocationsChecked == null)
            {
                return;
            }

            _ = MemoryHelpers.WriteAddressDataBit(Addresses.CardonForestSubGateJakkoStarterKeyPickup, allLocationsChecked.Contains(60));
            _ = MemoryHelpers.WriteAddressDataBit(Addresses.CardonForestSubGateConveyorStarterKeyPickup, allLocationsChecked.Contains(61));
            _ = MemoryHelpers.WriteAddressDataBit(Addresses.CardonForestSubGateThreeSwitchStarterKeyPickup, allLocationsChecked.Contains(62));
        }

        public static void RecheckPreviouslyCheckedContainerLocations(IReadOnlyCollection<long> allLocationsChecked)
        {
            if (allLocationsChecked.Count == 0)
            {
                return;
            }

            foreach (long locationID in allLocationsChecked)
            {
                if (DataDicts.LocationDataDict.TryGetValue((int)locationID, out LocationData? locationData))
                {
                    if (new[] { LocationCategory.Container, LocationCategory.Hole, LocationCategory.Pickup }.Contains(locationData.Category))
                    {
                        if (locationData.CheckAddressData.BitNumber != null)
                        {
                            MemoryHelpers.WriteAddressDataBit(locationData.CheckAddressData, true);
                        }
                        else
                        {
                            Log.Logger.Warning($"No check bit defined for location ID {locationID}. Please report this in the Discord thread!");
                        }
                    }
                }
                else
                {
                    Log.Logger.Warning($"Failed to receive item for location ID {locationID} after loading save. Please report this in the Discord thread!");
                }
            }
        }

        public static uint GetReceivedAPZennyTotal(IReadOnlyCollection<ItemInfo> allItemsReceived)
        {
            uint total = 0;
            foreach (ItemInfo itemInfo in allItemsReceived)
            {
                if (
                    DataDicts.ItemDataDict.TryGetValue(itemInfo.ItemId, out ItemData? itemData) &&
                    itemData.Category == ItemCategory.Zenny
                )
                {
                    total += itemData.Quantity;
                }
            }
            return total;
        }

        public static void ReceivePreviouslyReceivedItems(IReadOnlyCollection<ItemInfo> allItemsReceived)
        {
            uint? apZennyCommittedToSave = App.APZennyCommittedToSave;
            if (allItemsReceived.Count == 0)
            {
                return;
            }

            foreach (ItemInfo itemInfo in allItemsReceived)
            {
                if (DataDicts.ItemDataDict.TryGetValue(itemInfo.ItemId, out ItemData? itemData))
                {
                    // Skip giving items used in crafting/gifts/etc. if they are already used
                    // TODO: finish this. Make sure all item names match (capitalization) and are properly grouped/exhaustive
                    switch (itemData)
                    {
                        case var data when data.Category == ItemCategory.Zenny:
                            // Zenny handled below
                            continue;
                        case var data when data.Name == "Flower":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasGiftedFlower))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Music Box":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasGiftedMusicBox))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Ring":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasGiftedRing))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Saw":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInSaw))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Bag":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInBag))
                            {
                                continue;
                            }
                            break;
                        
                        case var data when data.Name == "Pick":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInPick))
                            {
                                continue;
                            }
                            break;
                        //case var data when data.Name == "Lipstick":
                        //    if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInLipstick))
                        //    {
                        //        continue;
                        //    }
                        //    break;
                        //case var data when data.Name == "Comic Book":
                        //    if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInComicBook))
                        //    {
                        //        continue;
                        //    }
                        //    break;
                        //case var data when data.Name == "Stag Beetle":
                        //    if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInStagBeetle))
                        //    {
                        //        continue;
                        //    }
                        //    break;
                        //case var data when data.Name == "Beetle":
                        //    if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInBeetle))
                        //    {
                        //        continue;
                        //    }
                        //    break;

                        case var data when data.Name == "Old Bone":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInOldBone))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Old Heater":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInOldHeater))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Old Doll":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInOldDoll))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Antique Bell":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInAntiqueBell))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Giant Horn":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInGiantHorn))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Shiny Object":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInShinyObject))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Old Shield":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInOldShield))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Shiny Red Stone":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasTurnedInShinyRedStone))
                            {
                                continue;
                            }
                            break;

                        case var data when data.Name == "Blumebear Parts":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasMachineBuster))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Cannon Kit":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasPoweredBuster))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Blunted Drill":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasDrillArm))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Grenade Kit":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasGrenadeArm))
                            {
                                continue;
                            }
                            break;
                        case var data when new[] { "Arm Supporter", "Ancient Book", "Old Launcher" }.Contains(data.Name):
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasSpreadBuster))
                            {
                                continue;
                            }
                            break;
                        case var data when new[] { "Broken Cleaner", "Broken Motor", "Broken Propeller" }.Contains(data.Name):
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasVacuumArm))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Guidance Unit":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasActiveBuster))
                            {
                                continue;
                            }
                            break;
                        case var data when new[] { "Zetasabre", "Pen Light" }.Contains(data.Name):
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasBladeArm))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Bomb Schematic":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasGrandGrenade))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Mine Parts Kit":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasSplashMine))
                            {
                                continue;
                            }
                            break;
                        case var data when new[] { "Mystic Orb", "Marlwolf Shell" }.Contains(data.Name):
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasShieldArm))
                            {
                                continue;
                            }
                            break;
                        case var data when new[] { "Prism Crystal", "X Buster", "Weapon Plans" }.Contains(data.Name):
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasShiningLaser))
                            {
                                continue;
                            }
                            break;

                        case var data when data.Name == "Safety Helmet":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasHelmet))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Spring Set":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasJumpSprings))
                            {
                                continue;
                            }
                            break;
                        case var data when new[] { "Old Hoverjets", "Rollerboard" }.Contains(data.Name):
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasJetSkates))
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Joint Plug":
                            if (MemoryHelpers.ReadAddressDataBit(Addresses.HasAdapterPlug))
                            {
                                continue;
                            }
                            break;

                        case var data when new[] { "Sun-light", "Broken Circuits", "Main Core Shard" }.Contains(data.Name):
                            if (ItemHelpers.HasBusterPart(DataDicts.ItemDataDict[0x0214])) // Omni-Unit Omega
                            {
                                continue;
                            }
                            break;
                        case var data when new[] { "Autofire Barrel", "Generator Part" }.Contains(data.Name):
                            if (ItemHelpers.HasBusterPart(DataDicts.ItemDataDict[0x0215])) // Auto Battery
                            {
                                continue;
                            }
                            break;
                        case var data when new[] { "Tele-lens", "Target Sensor" }.Contains(data.Name):
                            if (ItemHelpers.HasBusterPart(DataDicts.ItemDataDict[0x0216])) // Sniper Scope
                            {
                                continue;
                            }
                            break;
                        case var data when new[] { "Flower Pearl", "Gatling Part" }.Contains(data.Name):
                            if (ItemHelpers.HasBusterPart(DataDicts.ItemDataDict[0x0218])) // Gatling Gun
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Bomb":
                            if (ItemHelpers.HasBusterPart(DataDicts.ItemDataDict[0x021A])) // Power Blaster R
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Plastique":
                            if (ItemHelpers.HasBusterPart(DataDicts.ItemDataDict[0x021B])) // Power Blaster L
                            {
                                continue;
                            }
                            break;
                        case var data when data.Name == "Rapidfire Barrel":
                            if (ItemHelpers.HasBusterPart(DataDicts.ItemDataDict[0x021C])) // Machine Gun
                            {
                                continue;
                            }
                            break;

                        default:
                            break;
                    }

                    // Else give item
                    ItemHelpers.ReceiveGenericItem(itemData);
                }
                else
                {
                    Log.Logger.Warning($"Failed to receive item ID {itemInfo.ItemId} after loading save. Please report this in the Discord thread!");
                }
            }

            // Give the correct amount of zenny based on tracked amount in game loop
            uint receivedAPZennyTotal = GetReceivedAPZennyTotal(allItemsReceived);
            if (apZennyCommittedToSave == null)
            {
                App.APZennyCommittedToSave = receivedAPZennyTotal;
                return;
            }
            else if (receivedAPZennyTotal > apZennyCommittedToSave.Value)
            {
                uint missingZenny = receivedAPZennyTotal - apZennyCommittedToSave.Value;
                ItemHelpers.ReceiveGenericItem(new ItemData(ItemCategory.Zenny, "Zenny", missingZenny));
            }
        }

        public static void ShuffleStartingSpecialWeapon()
        {
            var apClient = App.APClient;
            var slotData = App.SlotData;
            if (
                apClient == null ||
                App.SlotData == null ||
                !MemoryHelpers.ReadAddressDataBit(Addresses.SupportCarRnDFlag) || 
                MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedCitizenshipLate) ||
                !apClient.Options.TryGetValue("shuffleStartingSpecialWeapon", out var shuffleStartingSpecialWeapon) ||
                int.Parse(shuffleStartingSpecialWeapon.ToString()) != 1 ||
                !slotData.TryGetValue("startingSpecialWeapon", out var startingSpecialWeapon)
            )
            {
                return;
            }
            MemoryHelpers.WriteAddressDataBit(Addresses.HasSplashMine, false);
            byte offset = byte.Parse(startingSpecialWeapon.ToString());
            Memory.WriteByte((ulong)(0xBE410 + (offset >> 3)), (byte)(7 - (offset % 8)));
            //Memory.WriteByte(Addresses.SpecialWeaponEquippedActual.Address, offset);
            //Memory.WriteByte(Addresses.SpecialWeaponEquippedLoadout.Address, offset);
            MemoryHelpers.WriteCode(Cheats.AlterStartingSpecialWeapon(offset));
        }
    }
}
