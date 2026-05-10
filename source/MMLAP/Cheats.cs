using Archipelago.Core.Util;
using MMLAP.Helpers;
using MMLAP.Models;
using ReactiveUI;
using System;

namespace MMLAP
{
    public class Cheats
    {
        // NOTE: Any address of the form 0x0001FXXX which is overwritten should be restored after leaving the area where an overwrite is needed.
        public static readonly OpCode Restore1F830 = new(0x0001F830, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1F8E0 = new(0x0001F8E0, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FA08 = new(0x0001FA08, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FA9C = new(0x0001FA9C, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FB28 = new(0x0001FB28, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FB58 = new(0x0001FB58, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FB90 = new(0x0001FB90, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FBCC = new(0x0001FBCC, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FCA8 = new(0x0001FCA8, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FD40 = new(0x0001FD40, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FD5C = new(0x0001FD5C, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FD94 = new(0x0001FD94, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FDB0 = new(0x0001FDB0, 0x80631B62); // lb v1, 0x1B62(v1)
        public static readonly OpCode Restore1FDE8 = new(0x0001FDE8, 0x80631B62); // lb v1, 0x1B62(v1)

        public static OpCode[] RestoreFixBoatCallRoll()
        {
            // This is what is overwritten by EnableFixBoatCallRoll
            return [
                new OpCode(Addresses.FixBoatCallRollUtil.Address, 0x10400006),  // beq v0, zero, 0x80055478
            ];
        }

        public static void Restore1FXXXWrites(LevelData currentLevelData)
        {
            string levelName = currentLevelData.AreaName + ": " + currentLevelData.RoomName;

            // Restore branch statement noped in EnableFixBoatCallRoll code which is used elsewhere
            if (
                currentLevelData.AreaName != "Wily's Boat: Outside Boat Shop" &&
                Memory.ReadUInt(Addresses.FixBoatCallRollUtil.Address) == 0x00000000 &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasCalledRollToFixBoat)
            )
            {
                MemoryHelpers.WriteCode(RestoreFixBoatCallRoll());
            }

            if (
                levelName != "Lake Jyun: On the Lake" &&
                levelName != "Lake Jyun: Side River" &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat) &&
                Memory.ReadUInt(0x0001FBCC) == 0x00000000
            )
            {
                //Memory.Write(0x0001FBC8, 0x3C03800C);
                MemoryHelpers.WriteCode(Restore1FBCC);
            }

            if (currentLevelData.AreaName != "Gesellschaft Interior")
            {
                MemoryHelpers.WriteCode(Restore1FB28);
            }

            if (currentLevelData.AreaName != "Uptown")
            {
                MemoryHelpers.WriteCode(Restore1FB58);
            }

            if (currentLevelData.AreaName != "Clozer Woods")
            {
                MemoryHelpers.WriteCode(Restore1FB90);
            }

            if (currentLevelData.AreaName != "Wily's Boat")
            {
                MemoryHelpers.WriteCode(Restore1FCA8);
            }

            if (currentLevelData.AreaName != "Yass Plains")
            {
                MemoryHelpers.WriteCode(Restore1FD40);
                MemoryHelpers.WriteCode(Restore1FD5C);
            }

            if (currentLevelData.AreaName != "Clozer Woods With Bridge")
            {
                MemoryHelpers.WriteCode(Restore1FD94);
                MemoryHelpers.WriteCode(Restore1FDB0);
            }

            if (currentLevelData.AreaName != "Cardon Forest (Flutter Fixed)" && currentLevelData.AreaName != "Flutter To Sub-Gate Cutscene")
            {
                MemoryHelpers.WriteCode(Restore1FDE8);
            }

            if (currentLevelData.AreaName != "Cardon Forest (Flutter Broken)")
            {
                MemoryHelpers.WriteCode(Restore1F830);
            }

            if (currentLevelData.AreaName != "Apple Market")
            {
                MemoryHelpers.WriteCode(Restore1F8E0);
            }

            if (currentLevelData.AreaName != "City Hall")
            {
                MemoryHelpers.WriteCode(Restore1FA9C);
            }

            if (currentLevelData.AreaName != "Downtown")
            {
                MemoryHelpers.WriteCode(Restore1FA08);
            }
        }

        public static OpCode[] FastForwardOutsideCardonSubgate(byte currentProgressionCounter, bool hasDefeatedFerdinand, bool hasCompletedCardonTankEvent)
        {
            // Needs to be written fast during loading screen
            byte fastForwardState = !hasDefeatedFerdinand ? (byte)0x00 :
                                    !hasCompletedCardonTankEvent ? (byte)0x03 :
                                    Math.Max((byte)0x04, currentProgressionCounter);
            return [
                // Unlock Door
                LoadHalfImmediate(0x00100E04, MMLEnums.Register.a1, 0x04),
                //
                LoadHalfImmediate(0x00100CE0, MMLEnums.Register.v0, fastForwardState),
                LoadHalfImmediate(0x00100E8C, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardCardonForestSubgate(bool hasTakenYellowRefractor)
        {
            // May be written slowly as it is checked during in-game loop
            byte fastForwardState = !hasTakenYellowRefractor ? (byte)0x03 : (byte)0x04;
            return [
                LoadHalfImmediate(0x00100320, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x0010E7FC, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x0010E8B8, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] DecoupleCardonForestSubGateKeys()
        {
            // See related code in 
            AddressData jakkoKeyAddressData = DataDicts.LocationDataDict[60].CheckAddressData;
            AddressData conveyorKeyAddressData = DataDicts.LocationDataDict[61].CheckAddressData;
            AddressData switchesKeyAddressData = DataDicts.LocationDataDict[62].CheckAddressData;
            short jakkoKeyBitNumberOffset = (short)((jakkoKeyAddressData.Address - 0xBE378) * 8 + 7 - (jakkoKeyAddressData.BitNumber ?? 0));
            short conveyorKeyBitNumberOffset = (short)((conveyorKeyAddressData.Address - 0xBE378) * 8 + 7 - (conveyorKeyAddressData.BitNumber ?? 1));
            short switchesKeyBitNumberOffset = (short)((switchesKeyAddressData.Address - 0xBE378) * 8 + 7 - (switchesKeyAddressData.BitNumber ?? 2));
            short jakkoRefractorTerminalVirtualOffset = (short)((Addresses.YellowRefractorTerminalVirtual.Address - 0xBE378) * 8 + 1);
            short conveyorRefractorTerminalVirtualOffset = (short)((Addresses.YellowRefractorTerminalVirtual.Address - 0xBE378) * 8);
            short switchesRefractorTerminalVirtualOffset = (short)((Addresses.YellowRefractorTerminalVirtual.Address - 0xBE378) * 8);
            return [
                // These will make the key pickups write to addresses, set to be distinct from "has key" addresses
                LoadHalfImmediate(0x00108F5C, MMLEnums.Register.a0, jakkoKeyBitNumberOffset), // replace write 0xBE41D with 0xBE377 (jakko key)
                LoadHalfImmediate(0x00108F64, MMLEnums.Register.a0, conveyorKeyBitNumberOffset), // replace write 0xBE41D with 0xBE377 (conveyor key)
                LoadHalfImmediate(0x00108F4C, MMLEnums.Register.a0, switchesKeyBitNumberOffset), // replace write 0xBE41E with 0xBE377 (switches key)

                // These execute on loading into a room, determining if the key is there, so we change to the same values above
                LoadHalfImmediate(0x0010CA24, MMLEnums.Register.a0, jakkoKeyBitNumberOffset), // load keys, switch read 0xBE41D with 0xBE377 (jakko key)
                LoadHalfImmediate(0x0010C9B0, MMLEnums.Register.a0, conveyorKeyBitNumberOffset), // load keys, switch read 0xBE41D with 0xBE377 (conveyor key)
                LoadHalfImmediate(0x0010CA44, MMLEnums.Register.a0, switchesKeyBitNumberOffset), // load keys, switch read 0xBE41E with 0xBE377 (switches key)

                // TODO: Figure out what these do. There is still an issue where Roll says the wrong line after picking up the third key
                //LoadHalfImmediate(0x0010C784, MMLEnums.Register.a0, jakkoKeyBitNumberOffset), // replace read 0xBE41D with 0xBE377
                //LoadHalfImmediate(0x0010C79C, MMLEnums.Register.a0, conveyorKeyBitNumberOffset), // replace read 0xBE41D with 0xBE377
                //LoadHalfImmediate(0x0010C7B4, MMLEnums.Register.a0, switchesKeyBitNumberOffset), // replace read 0xBE41E with 0xBE377

                // ?? These should make the sprite go away when picked up and control Roll's voice line
                //LoadHalfImmediate(0x00108F94, MMLEnums.Register.a0, yellowRefractorTerminalVirtualOffset), // key pickup, switch read 0xBE41D with 0xBE376

                // These should prevent the game writing to the terminal address based on your current key count when changing levels. Handled elsewhere
                Nop(0x0010C7DC),
                Nop(0x0010C850),
                Nop(0x0010C868),
                Nop(0x0010C870),
                Nop(0x0010C87C),

                // The refractor terminal reads whether you picked up the keys. These change the address the terminal is manipulated to another.
                // We need to populate the other address with emulated behavior of the original based on keys in inventory. 
                // This should probably be done in code injection, but I will put that part in the slow game loop because lazy
                //LoadHalfImmediate(0x00051228, MMLEnums.Register.a0, -16), // 
                //LoadHalfImmediate(0x0010BC0C, MMLEnums.Register.a0, -16),
                //LoadHalfImmediate(0x0010BA84, MMLEnums.Register.a0, -16), // This one is in the internal game loop... 
                // Jk, too hard. Key pickups also read/write here, so move those instead
                //LoadHalfImmediate(0x00108F94, MMLEnums.Register.a0, jakkoRefractorTerminalVirtualOffset),

                // If the yellow refractor has been picked up, then the game decides not to load a bunch of assets
                // such as the keys and the yellow refractor itself. 
                // Nop out this check so that it loads the assets regardless
                // We can conditionally remove the yellow refractor assets in the fast game loop
                Nop(0x0010C77C),
            ];
        }

        public static OpCode[] FastForwardLakeJyunSubgate()
        {
            // May be written slowly as it is checked during in-game loop
            byte fastForwardState = (byte)0x06;
            return [
                LoadHalfImmediate(0x001003F8, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardClozerWoodsSubgate()
        {
            byte fastForwardState = (byte)0x07;
            return [
                // Enables doors, spawns Flutter NPCs
                LoadHalfImmediate(0x00100574, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardCardonForestFlutterBroken(byte currentProgressionCounter, bool hasStartedTronDogCutscene, bool hasEarnedClassBLicense, bool hasEarnedCitizenship)
        {
            byte fastForwardState = hasEarnedClassBLicense && hasEarnedCitizenship ? Math.Max((byte)0x01, currentProgressionCounter) : (byte)0x00;
            byte startServbotCutscene = hasStartedTronDogCutscene && hasEarnedCitizenship ? (byte)0x01 : (byte)0x00;
            return [
                // Unlocks doors if >0
                LoadHalfImmediate(0x001007E0, MMLEnums.Register.a1, Math.Max((byte)0x01, fastForwardState)),
                Nop(0x00100848), // Locks doors after taking to inspector even if 0x001007E0 is altered
                Nop(0x0010084C), // Locks doors after taking to inspector even if 0x001007E0 is altered
                // ? Checks =0
                LoadHalfImmediate(0x001005EC, MMLEnums.Register.v0, fastForwardState),
                // ? Checks =0
                LoadHalfImmediate(0x001007F8, MMLEnums.Register.v0, fastForwardState),
                // In internal game loop. Loads car if =0 (also need 1F830=0)
                LoadHalfImmediate(0x001008F4, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x0001F830, MMLEnums.Register.v1, fastForwardState),
                // 0xBE37C bit checks (junk shop rescue)
                //LoadHalfImmediate(0x00100D10, MMLEnums.Register.v0, )
                // 0xBE378 bit checks
                //LoadHalfImmediate(0x00100808, MMLEnums.Register.v0, fastForwardState),
                //LoadHalfImmediate(0x00100838, MMLEnums.Register.v0, fastForwardState),
                //LoadHalfImmediate(0x00100C58, MMLEnums.Register.v0, fastForwardState),
                LoadHalfImmediate(0x00100C98, MMLEnums.Register.v0, !hasEarnedCitizenship ? (byte)0x00 : (byte)0x01),
                // For starting servbot cutscene if skipped some stuff
                hasStartedTronDogCutscene && hasEarnedCitizenship ? Nop(0x00100CA0) : new OpCode(0x00100CA0, 0x1040000D),

            ];
        }

        public static OpCode[] FastForwardCardonForestFlutterFixed(byte currentProgressionCounter, bool hasDefeatedJuno, bool hasWatchedFlutterFixFromJunoCutscene, bool hasCompletedGoal)
        {
            bool isWatchingFromJunoCutscene = hasDefeatedJuno && !hasWatchedFlutterFixFromJunoCutscene;
            byte fastForwardState = hasCompletedGoal || isWatchingFromJunoCutscene ? (byte)0x0B :
                                    Math.Min((byte)0x0A, Math.Max((byte)0x07, currentProgressionCounter));
            return [
                // Can re-enter flutter
                LoadHalfImmediate(0x00100A3C, MMLEnums.Register.a1, fastForwardState),
                // Flutter is there
                LoadHalfImmediate(0x00100AE8, MMLEnums.Register.v1, fastForwardState),
                // Remove roll scene
                LoadHalfImmediate(0x00100E84, MMLEnums.Register.v0, 0x01),
                // Ending Cutscene
                LoadHalfImmediate(0x001008D8, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x00100A88, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x0001FDE8, MMLEnums.Register.v1, fastForwardState),
                //Nop(0x0002FBCC),
                //Nop(0x000322DC),
                // Stops from juno cutscene
                //LoadHalfImmediate(0x00100BB4, MMLEnums.Register.v0, 1),
            ];
        }

        public static OpCode[] FastForwardUndergroundRuins(byte currentProgressionCounter, bool hasRescuedShopOwnersHusband)
        {
            byte fastForwardState = !hasRescuedShopOwnersHusband ? (byte)0x00 : currentProgressionCounter;
            return [
                //LoadHalfImmediate(0x00100484, MMLEnums.Register.v0, fastForwardState),
                LoadHalfImmediate(0x00100620, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardAppleMarket(byte currentProgressionCounter, bool hasRescuedShopOwnersHusband, bool hasEarnedClassBLicense, bool hasShownRollRedRefractor)
        {
            //byte fastForwardShopOwner = hasRescuedShopOwnersHusband ? (byte)0x00 : (byte)0x01;
            byte fastForwardState = !hasRescuedShopOwnersHusband || !hasEarnedClassBLicense ? (byte)0x00 : 
                                    !hasShownRollRedRefractor ? (byte)0x01 :
                                    Math.Max((byte)0x08, currentProgressionCounter); // Moving this state early to red refractor
            return [
                // 
                LoadHalfImmediate(0x0001F8E0, MMLEnums.Register.v1, fastForwardState),
                // Open door to downtown
                LoadHalfImmediate(0x00100474, MMLEnums.Register.a1, Math.Max((byte)0x01, fastForwardState)),
                // Check if less than 7
                LoadHalfImmediate(0x00100384, MMLEnums.Register.v0, fastForwardState),
                // lb a1 0x0(s0), check if equal 0
                LoadHalfImmediate(0x0010048C, MMLEnums.Register.v0, fastForwardState),
                // ?
                LoadHalfImmediate(0x0010054C, MMLEnums.Register.v1, fastForwardState),
                //// 0xBE378 bit checks, main area
                //LoadHalfImmediate(0x000198C8, MMLEnums.Register.v0, !hasRescuedShopOwnersHusband ? (byte)0x00 : (byte)0x01),
                //LoadHalfImmediate(0x0001F910, MMLEnums.Register.v0, !hasRescuedShopOwnersHusband ? (byte)0x00 : (byte)0x01),
                //LoadHalfImmediate(0x00100348, MMLEnums.Register.v0, !hasRescuedShopOwnersHusband ? (byte)0x00 : (byte)0x01),
                //LoadHalfImmediate(0x00100678, MMLEnums.Register.v0, !hasRescuedShopOwnersHusband ? (byte)0x00 : (byte)0x01),
                //LoadHalfImmediate(0x0010049C, MMLEnums.Register.v0, !hasRescuedShopOwnersHusband ? (byte)0x00 : (byte)0x01), //
                LoadHalfImmediate(0x00100688, MMLEnums.Register.v0, !hasRescuedShopOwnersHusband ? (byte)0x00 : (byte)0x01),
                // 0xBE378 bit checks, shop
                LoadHalfImmediate(0x001008B0, MMLEnums.Register.v0, !hasRescuedShopOwnersHusband ? (byte)0x00 : (byte)0x01),
                
            ];
        }

        public static OpCode[] FastForwardDowntown(byte currentProgressionCounter, bool hasEarnedClassBLicense, bool hasDefeatedBalkonGerat, bool hasTakenRedRefractor, bool hasUnlockedSubCities)
        {
            // Complex area, but important things here are:
            // 1. Tron and dog scene happens at 0, and can't skip this in vanilla for boss sequence
            // 2. Bomb quest requires class A license 
            // 3. Bank Robber requires sub-cities raised
            byte fastForwardState = !hasEarnedClassBLicense ? (byte)0x00 :
                                    //hasActivatedUnlockSubCities ? (byte)0x09 :
                                    hasDefeatedBalkonGerat || hasTakenRedRefractor ? (byte)0x06 :
                                    currentProgressionCounter;
            byte fastForwardStateSubCity = hasUnlockedSubCities ? (byte)0x09 : (byte)0x00;
            return [
                // Multiple tiers here for checking 0xC1B62: 0 (also checks 0xBE378[5, 6]), 1-5, 6-7, 8-10, 11
                // Writes to global value 0x800981E2, copied into 0xC1B7A
                LoadHalfImmediate(0x0001FA08, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x00106C24, MMLEnums.Register.v1, fastForwardState),
                // In internal game loop
                LoadHalfImmediate(0x00106C84, MMLEnums.Register.v1, fastForwardState),
                // Enable Sub-City
                LoadHalfImmediate(0x00106BB0, MMLEnums.Register.v0, fastForwardStateSubCity),
                // Open doors
                LoadHalfImmediate(0x00106B50, MMLEnums.Register.a1, 0x02),
                // Opens doors during Tron / dog scene
                LoadHalfImmediate(0x00106B68, MMLEnums.Register.v0, 0x02),
            ];
        }

        public static OpCode[] FastForwardOldCity(byte currentProgressionCounter)
        {
            return [
                LoadHalfImmediate(0x001007D0, MMLEnums.Register.v1, Math.Max((byte)0x09, currentProgressionCounter)),
            ];
        }

        public static OpCode[] FastForwardUptown(byte currentProgressionCounter, bool hasUnlockedSubCities, bool hasShownRollRedRefractor)
        {
            // Needs to be written fast during loading screen
            //byte fastForwardState = Math.Max((byte)0x06, currentProgressionCounter);
            byte fastForwardState = hasUnlockedSubCities ? (byte)0x09 :
                                    hasShownRollRedRefractor ? (byte)0x08 : // Moving this state early to red refractor
                                    (byte)0x02; 
            return [
                // Check slt 6, else check slt 8
                LoadHalfImmediate(0x0001FB58, MMLEnums.Register.v1, fastForwardState),
                // Sub city
                LoadHalfImmediate(0x00100648, MMLEnums.Register.v1, fastForwardState),
                // check slt 5
                LoadHalfImmediate(0x0010047C, MMLEnums.Register.v0, fastForwardState),
                // ? close to bit check @ bit 55
                LoadHalfImmediate(0x001005A4, MMLEnums.Register.a1, fastForwardState),
                // In game loop
                LoadHalfImmediate(0x00100648, MMLEnums.Register.v1, fastForwardState),
                // Check if fixed flutter when loading in hospital (for side quests).
                // Open it up, allowing Ira quest.
                // Missing woman quest turn in (?) still requires other bit check @ bit 563
                //LoadHalfImmediate(0x00101310, MMLEnums.Register.v0, (byte)0x01),
                //LoadHalfImmediate(0x00101320, MMLEnums.Register.v0, (byte)0x01),
                Nop(0x00101318),
                //Nop(0x00101328),
                // Missing woman quest check?
                LoadHalfImmediate(0x0010129C, MMLEnums.Register.v0, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardCityHall(byte currentProgressionCounter, bool hasEarnedClassBLicense, bool hasEarnedClassALicense, bool hasDefeatedBalkonGerat, bool hasTakenRedRefractor)
        {
            // Note: Important here is:
            // 1. Need 0x00 to talk to officer before pirates & to do Bon Bonne
            // 2. Amelia's Office counts as City Hall (werid things can happen there with cutscenes)
            // Going to try separating Amelia's Office from here to make things easier
            byte fastForwardState = !hasEarnedClassBLicense ? (byte)0x00 :
                                    !hasEarnedClassALicense ? (byte)0x01 :
                                    !hasDefeatedBalkonGerat || hasTakenRedRefractor ? (byte)0x02 :
                                    Math.Max((byte)0x06, currentProgressionCounter);
            return [
                // Four tiers: checks if = 1. else checks = 0 (and 0xBE43E?), else checks 2 <= .. <= 5, else checks >= 6
                // Each tier writes a different value to $gp + 0x98A = 0x800981EE which is written to 0xC1B7A
                LoadHalfImmediate(0x0001FA9C, MMLEnums.Register.v1, fastForwardState),
                // Check if = 0
                LoadHalfImmediate(0x001005D8, MMLEnums.Register.v1, fastForwardState),
                // lb a1 0x0(s0), unlocks doors if >0
                LoadHalfImmediate(0x001006E4, MMLEnums.Register.a1, Math.Max((byte)0x01, currentProgressionCounter)),
                // check if = 0, In a complex branch structure
                LoadHalfImmediate(0x00119FA4, MMLEnums.Register.v1, fastForwardState),
                // check if = 0, In a complex branch structure
                LoadHalfImmediate(0x00119FDC, MMLEnums.Register.v1, fastForwardState),
                // In internal game loop, checks if = 0, spawns NPCs if >0
                LoadHalfImmediate(0x00100778, MMLEnums.Register.v1, fastForwardState),
                // lb v0 0x0(s0), check if = 0
                LoadHalfImmediate(0x001006FC, MMLEnums.Register.v0, fastForwardState),
                LoadHalfImmediate(0x001007BC, MMLEnums.Register.v1, fastForwardState),
                // Loading in from Marlwolf cutscene
                LoadHalfImmediate(0x00101088, MMLEnums.Register.v0, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardCityHallIndoors(byte currentProgressionCounter, bool hasActivatedEmergencySystem)
        {
            byte fastForwardState = hasActivatedEmergencySystem ? Math.Max((byte)0x08, currentProgressionCounter) :
                                    Math.Max((byte)0x02, currentProgressionCounter);
            short jumpSpringsBit = MemoryHelpers.BitsFromBE378(DataDicts.ItemDataDict[0x026C].InventoryAddressData) ?? 0x00;
            return [
                // Checks < 8
                LoadHalfImmediate(0x0001FCE8, MMLEnums.Register.v1, fastForwardState),
                // Changes something at >1
                LoadHalfImmediate(0x0010022C, MMLEnums.Register.a1, fastForwardState),
                // In game loop, will move police officer if >1
                LoadHalfImmediate(0x001002B4, MMLEnums.Register.v1, fastForwardState),
                // Check HasTakenYellowRefractor @ talking to inspector
                //LoadHalfImmediate(0x001047A0, MMLEnums.Register.v0, 0x01), // Remove check entirely
                LoadHalfImmediate(0x001047A0, MMLEnums.Register.a0, jumpSpringsBit), // Replace with jump springs check
            ];
        }

        //public static OpCode[] FastForwardCityHallAmelia(byte currentProgressionCounter, bool hasEarnedClassBLicense, bool hasEarnedClassALicense)
        //{
        //    // Note: Important here is (1) need 0x00 to talk to officer before pirates (2) Amelia's Office counts as City Hall
        //    byte fastForwardState = !hasEarnedClassBLicense ? (byte)0x00 : 
        //                            !hasEarnedClassALicense ? (byte)0x01 :
        //                            currentProgressionCounter;
        //    return [
        //        // Four tiers: checks if = 1. else checks = 0 (and 0xBE43E?), else checks 2 <= .. <= 5, else checks >= 6
        //        // Each tier writes a different value to $gp + 0x98A = 0x800981EE which is written to 0xC1B7A
        //        LoadHalfImmediate(0x0001FA9C, MMLEnums.Register.v1, fastForwardState),
        //        // Check if = 0
        //        LoadHalfImmediate(0x001005D8, MMLEnums.Register.v1, fastForwardState),
        //        // lb a1 0x0(s0), unlocks doors if >0
        //        LoadHalfImmediate(0x001006E4, MMLEnums.Register.a1, Math.Max((byte)0x01, currentProgressionCounter)),
        //        // check if = 0, In a complex branch structure
        //        LoadHalfImmediate(0x00119FA4, MMLEnums.Register.v1, fastForwardState),
        //        // check if = 0, In a complex branch structure
        //        LoadHalfImmediate(0x00119FDC, MMLEnums.Register.v1, fastForwardState),
        //        // In internal game loop, checks if = 0, spawns NPCs if >0
        //        LoadHalfImmediate(0x00100778, MMLEnums.Register.v1, fastForwardState),
        //        // lb v0 0x0(s0), check if = 0
        //        LoadHalfImmediate(0x001006FC, MMLEnums.Register.v0, fastForwardState),
        //        LoadHalfImmediate(0x001007BC, MMLEnums.Register.v1, fastForwardState),

        //    ];
        //}

        public static OpCode[] FastForwardWilysBoat(byte currentProgressionCounter, bool boatIsFixed, bool hasDefeatedBalkonGerat)
        {
            // This could go in slow loop, but put in fast loop since NPCs spawn on progression check
            byte fastForwardState = (byte)(hasDefeatedBalkonGerat ? Math.Max((byte)0x06, currentProgressionCounter) : (boatIsFixed ? 0x05 : 0x04));
            return [
                // Loads people into the zone (they were evacuated)
                LoadHalfImmediate(0x001003A8, MMLEnums.Register.v1, fastForwardState),
                // 
                LoadHalfImmediate(0x00100374, MMLEnums.Register.a1, fastForwardState),
                // 
                LoadHalfImmediate(0x0001FCA8, MMLEnums.Register.v1, fastForwardState),
            ];
        }
        public static OpCode[] EnableFixBoatCallRoll()
        {
            // Enable "Call Roll" option when talking to worker which usually checks 0xBE37B[1]
            // delete branching that checks 0xBE37B[1] (the "has started taking yellow refractor cutscene" flag)
            // This is used by other stuff and can cause soft locks if not rewritten
            // An execution breakpoint here only hits once in this area, so it should be safe as long as it's restored later
            return [
                Nop(Addresses.FixBoatCallRollUtil.Address),
            ];
        }

        public static OpCode[] EnableRedRefractorCutscene()
        {
            return [
                LoadHalfImmediate(0x001001EC, MMLEnums.Register.v1, 0x06),
            ];
        }


        public static OpCode[] FastForwardYassPlains(byte currentProgressionCounter, bool hasEarnedClassBLicense, bool hasEarnedClassALicense, bool hasDefeatedBalkonGerat, bool hasTakenRedRefractor)
        {
            byte fastForwardState = !hasEarnedClassBLicense ? (byte)0x00 :
                                    !hasEarnedClassALicense ? (byte)0x01 :
                                    !hasDefeatedBalkonGerat || hasTakenRedRefractor ? (byte)0x02 :
                                    Math.Max((byte)0x06, currentProgressionCounter);
            return [
                // Original tests v1 = 1
                LoadHalfImmediate(0x0001FD40, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x0001FD5C, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x001006D4, MMLEnums.Register.a1, fastForwardState),
                // In internal game loop, loads tanks?
                LoadHalfImmediate(0x00100754, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardClozerWoodsWithBridge(byte currentProgressionCounter, bool hasEarnedClassBLicense, bool hasEarnedClassALicense, bool hasDefeatedBalkonGerat, bool hasTakenRedRefractor)
        {
            byte fastForwardState = !hasEarnedClassBLicense ? (byte)0x00 :
                                    !hasEarnedClassALicense ? (byte)0x01 :
                                    !hasDefeatedBalkonGerat || hasTakenRedRefractor ? (byte)0x02 :
                                    Math.Max((byte)0x06, currentProgressionCounter);
            return [
                // ?
                LoadHalfImmediate(0x0001FD94, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x0001FDB0, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x001002A0, MMLEnums.Register.a1, fastForwardState),
                // In internal game loop
                LoadHalfImmediate(0x00100328, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardClozerWoods(byte currentProgressionCounter, bool hasEarnedClassBLicense, bool hasEarnedClassALicense)
        {
            byte fastForwardState = !hasEarnedClassBLicense ? (byte)0x00 :
                                    !hasEarnedClassALicense ? (byte)0x01 :
                                    Math.Max((byte)0x02, currentProgressionCounter);
            return [
                // ?
                LoadHalfImmediate(0x0001FB90, MMLEnums.Register.v1, fastForwardState),
                // In internal game loop
                LoadHalfImmediate(0x00100534, MMLEnums.Register.v1, fastForwardState),
                // Enable sub-gate entrance door
                LoadHalfImmediate(0x00100500, MMLEnums.Register.a1, Math.Max((byte)0x08, currentProgressionCounter)),
            ];
        }

        public static OpCode[] FastForwardGesellschaft(byte currentProgressionCounter, bool hasTakenRedRefractor)
        {
            byte fastForwardState = !hasTakenRedRefractor ? (byte)0x01 : (byte)0x06;
            return [
                LoadHalfImmediate(0x0001FB28, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x00103F20, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x001002F0, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x001034A4, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x00100220, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardFlutterToSubGateCutscene(bool hasActivatedEmergencySystem, bool hasDefeatedFockeWulf)
        {
            byte fastForwardState = (byte)(hasActivatedEmergencySystem ? 0x07 : 0x07);
            byte disableFockeWulf = (byte)(hasDefeatedFockeWulf ? 0x01 : 0x00);
            return [
                // Fixes cardon -> clozer cutscene and vice versa
                LoadHalfImmediate(0x00100284, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x00100B48, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x00101A40, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x00101A50, MMLEnums.Register.v0, disableFockeWulf),
                LoadHalfImmediate(0x0001FDE8, MMLEnums.Register.v1, fastForwardState),
                LoadHalfImmediate(0x00100E44, MMLEnums.Register.v1, fastForwardState),
                //LoadHalfImmediate(0x001008DC, MMLEnums.Register.v1, fastForwardState),
                //LoadHalfImmediate(0x00100A3C, MMLEnums.Register.a1, fastForwardState),
                //LoadHalfImmediate(0x00100A88, MMLEnums.Register.v1, fastForwardState),
                //LoadHalfImmediate(0x00100AE8, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardGesellschaftBattle()
        {
            byte fastForwardState = (byte)0x07;
            return [
                LoadHalfImmediate(0x00100710, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardOutsideMainGate(byte currentProgressionCounter, bool hasUnlockedMainGate, bool hasActivatedEmergencySystem, bool hasWatchedMainGateOpenCutscene)
        {
            bool isInCutscene = hasActivatedEmergencySystem && !hasWatchedMainGateOpenCutscene;
            byte fastForwardState = isInCutscene ? (byte)0x07 : 
                                    hasUnlockedMainGate ? Math.Min((byte)0x0A, Math.Max((byte)0x08, currentProgressionCounter)) : 
                                    Math.Min((byte)0x07, currentProgressionCounter);
            return [
                // Prevents unlocking main gate cutscene black screen
                LoadHalfImmediate(0x00100420, MMLEnums.Register.v1, fastForwardState),
                // ?
                LoadHalfImmediate(0x001007E0, MMLEnums.Register.v1, fastForwardState),
            ];
        }

        public static OpCode[] FastForwardMainGate(byte currentProgressionCounter, bool hasShownRollRedRefractor)
        {
            byte fastForwardState = Math.Max(currentProgressionCounter, (byte)0x08);
            byte cardonForestState = hasShownRollRedRefractor ? (byte)0x1B : (byte)0x03;
            return [
                // Unlock standard doors
                LoadHalfImmediate(0x00100990, MMLEnums.Register.v1, fastForwardState),
                // Disables control panel
                //LoadHalfImmediate(0x0012B9B8, MMLEnums.Register.v0, 0x01),
                // Opens door in control panel room
                LoadHalfImmediate(0x00126798, MMLEnums.Register.v0, 0x01),
                // Pick which cardon forest flutter state to go to after Juno
                LoadHalfImmediate(0x00105660, MMLEnums.Register.v0, cardonForestState),
            ];
        }

        public static OpCode[] FastForwardLakeJyun(bool hasDefeatedBalkonGerat)
        {
            OpCode[] code = hasDefeatedBalkonGerat ? [
                // 0x05 Prevents black screen after phase 1. 0x06 has the boat there after boss
                LoadHalfImmediate(0x0010059C, MMLEnums.Register.v1, 0x06),
                // Prevents crash after starting phase 1
                LoadHalfImmediate(0x0001FBCC, MMLEnums.Register.v1, 0x06),
                // Lets phase 1 load
                //LoadHalfImmediate(0x0011674C, MMLEnums.Register.v1, 0x06),
                // Lets after phase 2 load
                LoadHalfImmediate(0x00101A70, MMLEnums.Register.v1, 0x01),
            ] : [
                // 0x05 Prevents black screen after phase 1. 0x06 has the boat there after boss
                LoadHalfImmediate(0x0010059C, MMLEnums.Register.v1, 0x05),
                // Prevents crash after starting phase 1
                LoadHalfImmediate(0x0001FBCC, MMLEnums.Register.v1, 0x05),
                // Lets phase 1 load
                LoadHalfImmediate(0x0011674C, MMLEnums.Register.v1, 0x05),
            ];
            return code;
        }

        private static OpCode LoadHalfImmediate(uint startAddress, MMLEnums.Register register, short shortVal)
        {
            // addiu rt, zero, imm16
            uint instruction =
                0x24000000u |
                ((uint)register << 16) |
                (uint)(ushort)shortVal;   // keep only low 16 bits (two's complement)

            return new OpCode(startAddress, instruction);
        }

        private static OpCode Nop(uint startAddress)
        {
            return new OpCode(startAddress, 0x00000000); // nop
        }
    }
}
