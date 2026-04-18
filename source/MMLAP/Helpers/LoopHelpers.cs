using Archipelago.Core.Util;
using MMLAP.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Serilog;
using System.Linq;

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
                        byte[] writeTextArr = TextHelpers.EncodeYouGotItemWindow(citizensCardScoutedItemData, prefix: TextHelpers.newPage, suffix: [0x9F, 0x99, 0x00, 0xBD, 0xA9, 0x84]);
                        Memory.WriteByteArray(citizensCardLocationData.TextBoxStartAddress ?? 0, writeTextArr);
                    }
                    break;

                default:
                    break;
            }
        }

        public static void HandleFastForwardCodeWrites(LevelData currentLevelData, byte currentProgressionCounter)
        {
            string areaName = currentLevelData.AreaName;

            switch (areaName)
            {
                case "Cardon Forest (Flutter Broken)":
                    MemoryHelpers.WriteCode(Cheats.FastForwardCardonForestFlutterBroken(currentProgressionCounter));
                    break;

                case "Cardon Forest (Flutter Fixed)":
                    MemoryHelpers.WriteCode(Cheats.FastForwardCardonForestFlutterFixed(currentProgressionCounter));
                    break;

                case "Outside Cardon Forest Sub-Gate":
                    bool hasDefeatedFerdinand = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedFerdinand);
                    bool hasCompletedCardonTankEvent = MemoryHelpers.ReadAddressDataBit(Addresses.HasCompletedCardonTankEvent);
                    MemoryHelpers.WriteCode(Cheats.FastForwardOutsideCardonSubgate(currentProgressionCounter, hasDefeatedFerdinand, hasCompletedCardonTankEvent));
                    break;

                case "Cardon Forest Sub-Gate":
                    bool hasTakenYellowRefractor = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenYellowRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardCardonSubgate(hasTakenYellowRefractor));
                    MemoryHelpers.WriteCode(Cheats.DecoupleCardonForestSubGateKeys());

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

                case "Lake Jyun Sub-Gate":
                    MemoryHelpers.WriteCode(Cheats.FastForwardLakeJyunSubgate());
                    break;

                case "Clozer Woods Sub-Gate":
                    MemoryHelpers.WriteCode(Cheats.FastForwardClozerWoodsSubgate());
                    break;

                case "Outside Main Gate":
                    bool hasUnlockedMainGate = ItemHelpers.HasReceivedItem(0x0001);
                    bool hasActivatedEmergencySystem = MemoryHelpers.ReadAddressDataBit(Addresses.HasActivatedEmergencySystem);
                    bool hasWatchedMainGateOpenCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.HasWatchedMainGateOpenCutscene);
                    MemoryHelpers.WriteCode(Cheats.FastForwardOutsideMainGate(currentProgressionCounter, hasUnlockedMainGate, hasActivatedEmergencySystem, hasWatchedMainGateOpenCutscene));
                    break;

                case "Apple Market":
                    MemoryHelpers.WriteCode(Cheats.FastForwardAppleMarket(currentProgressionCounter));
                    break;

                case "Downtown":
                    bool hasUnlockedSubCitiesDowntown = ItemHelpers.HasReceivedItem(0x0002);
                    MemoryHelpers.WriteCode(Cheats.FastForwardDowntown(currentProgressionCounter, hasUnlockedSubCitiesDowntown));
                    break;

                case "Uptown":
                    bool hasUnlockedSubCitiesUptown = ItemHelpers.HasReceivedItem(0x0002);
                    MemoryHelpers.WriteCode(Cheats.FastForwardUptown(currentProgressionCounter, hasUnlockedSubCitiesUptown));
                    break;

                case "Old City":
                    //bool HasUnlockedSubCitiesOldCity = Memory.ReadBit(Addresses.HasUnlockedSubCities.Address, Addresses.HasUnlockedSubCities.BitNumber ?? 1);
                    MemoryHelpers.WriteCode(Cheats.FastForwardOldCity(currentProgressionCounter));
                    break;

                case "City Hall":
                    MemoryHelpers.WriteCode(Cheats.FastForwardCityHall(currentProgressionCounter));
                    break;

                case "Yass Plains":
                    bool hasEarnedClassBLicenseYass = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseYass = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    MemoryHelpers.WriteCode(Cheats.FastForwardYassPlains(currentProgressionCounter, hasEarnedClassBLicenseYass, hasEarnedClassALicenseYass));
                    break;

                case "Clozer Woods With Bridge":
                    bool hasEarnedClassBLicenseClozerBridge = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseClozerBridge = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    MemoryHelpers.WriteCode(Cheats.FastForwardClozerWoodsWithBridge(currentProgressionCounter, hasEarnedClassBLicenseClozerBridge, hasEarnedClassALicenseClozerBridge));
                    break;

                case "Clozer Woods":
                    bool hasEarnedClassBLicenseClozer = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
                    bool hasEarnedClassALicenseClozer = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
                    MemoryHelpers.WriteCode(Cheats.FastForwardClozerWoods(currentProgressionCounter, hasEarnedClassBLicenseClozer, hasEarnedClassALicenseClozer));
                    break;

                case "Wily's Boat":
                    if (MemoryHelpers.ReadAddressDataBit(Addresses.HasYellowRefractor))
                    {
                        bool boatIsFixed = MemoryHelpers.ReadAddressDataBit(Addresses.BoatIsFixed);
                        bool hasDefeatedBalkonGeratWily = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                        MemoryHelpers.WriteCode(Cheats.FastForwardWilysBoat(currentProgressionCounter, boatIsFixed, hasDefeatedBalkonGeratWily));
                    }
                    break;

                case "Lake Jyun":
                    bool hasDefeatedBalkonGeratLake = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
                    MemoryHelpers.WriteCode(Cheats.FastForwardLakeJyun(hasDefeatedBalkonGeratLake));
                    break;

                case "Flutter Takeoff":
                    MemoryHelpers.WriteCode(Cheats.EnableRedRefractorCutscene());
                    break;

                case "Gesselschaft Interior":
                    // This is for during Bonne cutscenes
                    // Player is sent to Amelia if value is low, Wily's if 0x06
                    bool hasTakenRedRefractorGesselschaft = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenRedRefractor);
                    MemoryHelpers.WriteCode(Cheats.FastForwardGesselschaft(currentProgressionCounter, hasTakenRedRefractorGesselschaft));
                    break;

                case "Flutter To Sub-Gate Cutscene":
                    Log.Logger.Information("Applying Flutter to Sub-Gate Cutscene fast-forward.");
                    MemoryHelpers.WriteCode(Cheats.FastForwardFlutterToSubGateCutscene());
                    break;

                case "Main Gate":
                    MemoryHelpers.WriteCode(Cheats.FastForwardMainGate(currentProgressionCounter));
                    break;

                default:
                    break;
            }

            string levelName = currentLevelData.AreaName + ": " + currentLevelData.RoomName;
            switch (levelName)
            {
                case "Wily's Boat: Outside Boat Shop":
                    if (
                        MemoryHelpers.ReadAddressDataBit(Addresses.HasYellowRefractor) &&
                        !MemoryHelpers.ReadAddressDataBit(Addresses.HasCalledRollToFixBoat)
                    )
                    {
                        MemoryHelpers.WriteCode(Cheats.EnableFixBoatCallRoll());
                    }
                    break;

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
                    break

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
            switch (currentLevelData.AreaName)
            {
                case "Apple Market":
                    // Lock door to Downtown if haven't received Citizen's Card
                    if (
                        !ItemHelpers.HasReceivedItem(0x022A) &&
                        DataDicts.ExitDataDict.TryGetValue("Apple Market -> Downtown", out var exitDataAppleToDowntown)
                    )
                    {
                        _ = exitDataAppleToDowntown.LockExit();
                    }
                    break;

                case "Yass Plains":
                    // Lock door to City Hall if haven't received Citizen's Card
                    if (
                        !ItemHelpers.HasReceivedItem(0x022A) &&
                        DataDicts.ExitDataDict.TryGetValue("Clozer Woods With Bridge -> Underground Ruins", out var exitDataYassToCity)
                    )
                    {
                        _ = exitDataYassToCity.LockExit();
                    }
                    break;

                case "Outside Main Gate":
                    // Lock door to Old City if haven't received Citizen's Card
                    if (
                        !ItemHelpers.HasReceivedItem(0x022A) &&
                        DataDicts.ExitDataDict.TryGetValue("Outside Main Gate -> Old City", out var exitDataMainGateToOldCity)
                    )
                    {
                        _ = exitDataMainGateToOldCity.LockExit();
                    }
                    break;

                case "Clozer Woods With Bridge":
                    // Lock door to ruins if haven't received Class B License
                    if (
                        !ItemHelpers.HasReceivedItem(0x022C) &&
                        DataDicts.ExitDataDict.TryGetValue("\"Yass Plains -> City Hall\"", out var exitDataClozerToRuins)
                    )
                    {
                        _ = exitDataClozerToRuins.LockExit();
                    }
                    break;

                case "Old City":
                    // TODO: Actually delete sub city entrance model if not received unlock sub-cities
                    if (
                        !ItemHelpers.HasReceivedItem(0x0002) &&
                        DataDicts.ExitDataDict.TryGetValue("Old City (dogs, no weapons) -> Watcher Sub-City", out var watcherSubCityExit)
                    )
                    {
                        _ = watcherSubCityExit.LockExit();
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
