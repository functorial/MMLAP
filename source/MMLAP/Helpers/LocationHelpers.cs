using Archipelago.Core.Models;
using Archipelago.MultiClient.Net.Models;
using MMLAP.Models;
using System.Collections.Generic;
using static MMLAP.Models.MMLEnums;
namespace MMLAP.Helpers
{
    public class LocationHelpers
    {
        public static Dictionary<ushort, LevelData> GetLevelDataDict()
        {
            Dictionary<ushort, LevelData> levelDataDict = new()
            {
                { 0x0000, new LevelData("Ocean Tower",                    "Intro cutscene",                   0x00, 0x00) },
                { 0x0001, new LevelData("Ocean Tower",                    "Room 1 (Entrance)",                0x00, 0x01) },
                { 0x0002, new LevelData("Ocean Tower",                    "Room 2",                           0x00, 0x02) },
                { 0x0003, new LevelData("Ocean Tower",                    "Room 3 (Boss)",                    0x00, 0x03) },
                { 0x0004, new LevelData("Ocean Tower",                    "Outdoor Cutscene",                 0x00, 0x04) },
                { 0x0005, new LevelData("Ocean Tower",                    "Back To Title Screen",             0x00, 0x05) },
                { 0x0006, new LevelData("Ocean Tower",                    "Empty?",                           0x00, 0x06) },
                { 0x0100, new LevelData("Opening Text Cutscene",          "Scrolling Text",                   0x01, 0x00) },
                { 0x0200, new LevelData("Crash Cutscene",                 "Cutscene",                         0x02, 0x00) },
                { 0x0300, new LevelData("Cardon Forest (Flutter Broken)", "Near ruin entrance",               0x03, 0x00) },
                { 0x0301, new LevelData("Cardon Forest (Flutter Broken)", "Crash Site",                       0x03, 0x01) },
                { 0x0302, new LevelData("Cardon Forest (Flutter Broken)", "City Entrance",                    0x03, 0x02) },
                { 0x0303, new LevelData("Cardon Forest (Flutter Broken)", "Unknown (floating model)",         0x03, 0x03) },
                { 0x0304, new LevelData("Cardon Forest (Flutter Broken)", "Unknown (floating model)",         0x03, 0x04) },
                { 0x0305, new LevelData("Cardon Forest (Flutter Broken)", "Barrell's Room",                   0x03, 0x05) },
                { 0x0306, new LevelData("Cardon Forest (Flutter Broken)", "Roll's Room (no Roll)",            0x03, 0x06) },
                { 0x0307, new LevelData("Cardon Forest (Flutter Broken)", "Flutter",                          0x03, 0x07) },
                { 0x0308, new LevelData("Cardon Forest (Flutter Broken)", "Mega Man's Room",                  0x03, 0x08) },
                { 0x0400, new LevelData("Apple Market",                   "Apple Market",                     0x04, 0x00) },
                { 0x0401, new LevelData("Apple Market",                   "Junk Shop",                        0x04, 0x01) },
                { 0x0402, new LevelData("Apple Market",                   "Electronics Shop",                 0x04, 0x02) },
                { 0x0403, new LevelData("Apple Market",                   "Hip Bone",                         0x04, 0x03) },
                { 0x0404, new LevelData("Apple Market",                   "Tailor Chinos",                    0x04, 0x04) },
                { 0x0405, new LevelData("Apple Market",                   "Record Shop",                      0x04, 0x05) },
                { 0x0500, new LevelData("Downtown",                       "Downtown",                         0x05, 0x00) },
                { 0x0501, new LevelData("Downtown",                       "(combat)",                         0x05, 0x01) },
                { 0x0502, new LevelData("Downtown",                       "Library",                          0x05, 0x02) },
                { 0x0503, new LevelData("Downtown",                       "Tron's Cockpit",                   0x05, 0x03) },
                { 0x0504, new LevelData("Downtown",                       "Stripe Burger",                    0x05, 0x04) },
                { 0x0600, new LevelData("City Hall",                      "Outdoors",                         0x06, 0x00) },
                { 0x0601, new LevelData("City Hall",                      "Amelia's Office",                  0x06, 0x01) },
                { 0x0602, new LevelData("City Hall",                      "Outdoors (no music, combat)",      0x06, 0x02) },
                { 0x0603, new LevelData("City Hall",                      "Teisel's Room",                    0x06, 0x03) },
                { 0x0604, new LevelData("City Hall",                      "Amelia's Office (wrecked)",        0x06, 0x04) },
                { 0x0700, new LevelData("Gesselschaft Interior",          "Empty?",                           0x07, 0x00) },
                { 0x0701, new LevelData("Gesselschaft Interior",          "Engine Room",                      0x07, 0x01) },
                { 0x0702, new LevelData("Gesselschaft Interior",          "Hallway",                          0x07, 0x02) },
                { 0x0703, new LevelData("Gesselschaft Interior",          "Kitchen",                          0x07, 0x03) },
                { 0x0704, new LevelData("Gesselschaft Interior",          "HQ",                               0x07, 0x04) },
                { 0x0705, new LevelData("Gesselschaft Interior",          "First Meeting Room Cutscene",      0x07, 0x05) },
                { 0x0706, new LevelData("Gesselschaft Interior",          "Meeting Room",                     0x07, 0x06) },
                { 0x0707, new LevelData("Gesselschaft Interior",          "Empty?",                           0x07, 0x07) },
                { 0x0800, new LevelData("Uptown",                         "Uptown",                           0x08, 0x00) },
                { 0x0801, new LevelData("Uptown",                         "Hospital",                         0x08, 0x01) },
                { 0x0802, new LevelData("Uptown",                         "TV Station",                       0x08, 0x02) },
                { 0x0803, new LevelData("Uptown",                         "Beast Hunter game",                0x08, 0x03) },
                { 0x0804, new LevelData("Uptown",                         "Balloon Fantasy Game",             0x08, 0x04) },
                { 0x0805, new LevelData("Uptown",                         "Ira's Room",                       0x08, 0x05) },
                { 0x0900, new LevelData("Underground Ruins",              "Room 1",                           0x09, 0x00) },
                { 0x0901, new LevelData("Underground Ruins",              "Room 2",                           0x09, 0x01) },
                { 0x0902, new LevelData("Underground Ruins",              "Room 3",                           0x09, 0x02) },
                { 0x0903, new LevelData("Underground Ruins",              "Room 4",                           0x09, 0x03) },
                { 0x0904, new LevelData("Underground Ruins",              "Room 5",                           0x09, 0x04) },
                { 0x0905, new LevelData("Underground Ruins",              "Room 6",                           0x09, 0x05) },
                { 0x0906, new LevelData("Underground Ruins",              "Room 7",                           0x09, 0x06) },
                { 0x0907, new LevelData("Underground Ruins",              "Orudakoitan Bridge",               0x09, 0x07) },
                { 0x0908, new LevelData("Underground Ruins",              "Drillable Walls Room",             0x09, 0x08) },
                { 0x0A00, new LevelData("Clozer Woods (Tiesel)",          "Tiesel Area",                      0x0A, 0x00) },
                { 0x0B00, new LevelData("Lake Jyun",                      "Inside Bonne Robot",               0x0B, 0x00) },
                { 0x0B01, new LevelData("Lake Jyun",                      "???",                              0x0B, 0x01) },
                { 0x0B02, new LevelData("Lake Jyun",                      "On the Lake",                      0x0B, 0x02) },
                { 0x0B03, new LevelData("Lake Jyun",                      "Side River",                       0x0B, 0x03) },
                { 0x0B04, new LevelData("Lake Jyun",                      "Inside Bonne Robot",               0x0B, 0x04) },
                { 0x0B05, new LevelData("Lake Jyun",                      "Empty?",                           0x0B, 0x05) },
                { 0x0B06, new LevelData("Lake Jyun",                      "Door Opening Mechanism",           0x0B, 0x06) },
                { 0x0B07, new LevelData("Lake Jyun",                      "Subgate With Open Door",           0x0B, 0x07) },
                { 0x0C00, new LevelData("Outside Cardon Forest Subgate",  "Outside",                          0x0C, 0x00) },
                { 0x0C01, new LevelData("Outside Cardon Forest Subgate",  "Door Opening Mechanism",           0x0C, 0x01) },
                { 0x0C02, new LevelData("Outside Cardon Forest Subgate",  "Subgate With Open Door",           0x0C, 0x02) },
                { 0x0C03, new LevelData("Outside Cardon Forest Subgate",  "Small Piece of Ground",            0x0C, 0x03) },
                { 0x0D00, new LevelData("Wily's Boat",                    "Walkway",                          0x0D, 0x00) },
                { 0x0D01, new LevelData("Wily's Boat",                    "Inside",                           0x0D, 0x01) },
                { 0x0D02, new LevelData("Wily's Boat",                    "Boat Area",                        0x0D, 0x02) },
                { 0x0D03, new LevelData("Wily's Boat",                    "Inside The Boat",                  0x0D, 0x03) },
                { 0x0D04, new LevelData("Wily's Boat",                    "Walkway (no box or woman)",        0x0D, 0x04) },
                { 0x0D05, new LevelData("Wily's Boat",                    "Walkway (no box, woman, or boat)", 0x0D, 0x05) },
                { 0x0E00, new LevelData("Cardon Forest Sub-gate",         "Refractor Room",                   0x0E, 0x00) },
                { 0x0E01, new LevelData("Cardon Forest Sub-gate",         "Cliff Room",                       0x0E, 0x01) },
                { 0x0E02, new LevelData("Cardon Forest Sub-gate",         "Conveyor Belts",                   0x0E, 0x02) },
                { 0x0F00, new LevelData("City Hall (Indoors)",            "Police Station",                   0x0F, 0x00) },
                { 0x0F01, new LevelData("City Hall (Indoors)",            "Inspector's Office",               0x0F, 0x01) },
                { 0x0F02, new LevelData("City Hall (Indoors)",            "City Hall 1st Floor",              0x0F, 0x02) },
                { 0x0F03, new LevelData("City Hall (Indoors)",            "Bank",                             0x0F, 0x03) },
                { 0x1000, new LevelData("Yass Plains",                    "Yass Plains Outdoors",             0x10, 0x00) },
                { 0x1001, new LevelData("Yass Plains",                    "Hideout Stage 1",                  0x10, 0x01) },
                { 0x1002, new LevelData("Yass Plains",                    "Hideout Stage 2",                  0x10, 0x02) },
                { 0x1003, new LevelData("Yass Plains",                    "Hideout Stage 3",                  0x10, 0x03) },
                { 0x1004, new LevelData("Yass Plains",                    "Empty House",                      0x10, 0x04) },
                { 0x1005, new LevelData("Yass Plains",                    "Junk Shop House",                  0x10, 0x05) },
                { 0x1100, new LevelData("Clozer Woods With Bridge",       "Clozer Woods With Bridge",         0x11, 0x00) },
                { 0x1200, new LevelData("Outside Main Gate",              "Outside Main Gate",                0x12, 0x00) },
                { 0x1300, new LevelData("Clozer Woods Sub-Gate",          "Entrance",                         0x13, 0x00) },
                { 0x1301, new LevelData("Clozer Woods Sub-Gate",          "Entrance Elevator Room",           0x13, 0x01) },
                { 0x1302, new LevelData("Clozer Woods Sub-Gate",          "Control Room",                     0x13, 0x02) },
                { 0x1303, new LevelData("Clozer Woods Sub-Gate",          "Sharukurusu Ambush",               0x13, 0x03) },
                { 0x1304, new LevelData("Clozer Woods Sub-Gate",          "Side Elevator Room",               0x13, 0x04) },
                { 0x1305, new LevelData("Clozer Woods Sub-Gate",          "Pillar Room",                      0x13, 0x05) },
                { 0x1306, new LevelData("Clozer Woods Sub-Gate",          "Breakable Ceiling",                0x13, 0x06) },
                { 0x1307, new LevelData("Clozer Woods Sub-Gate",          "Boss Room",                        0x13, 0x07) },
                { 0x1308, new LevelData("Clozer Woods Sub-Gate",          "Generator Room",                   0x13, 0x08) },
                { 0x1309, new LevelData("Clozer Woods Sub-Gate",          "Gorubesshu Corridor",              0x13, 0x09) },
                { 0x130A, new LevelData("Clozer Woods Sub-Gate",          "Flutter Barrell's Room",           0x13, 0x0A) },
                { 0x130B, new LevelData("Clozer Woods Sub-Gate",          "Flutter Roll's Room",              0x13, 0x0B) },
                { 0x130C, new LevelData("Clozer Woods Sub-Gate",          "Flutter Lobby",                    0x13, 0x0C) },
                { 0x130D, new LevelData("Clozer Woods Sub-Gate",          "Flutter Mega Man's Room",          0x13, 0x0D) },
                { 0x1400, new LevelData("Lake Jyun Sub-Gate",             "Entrance",                         0x14, 0x00) },
                { 0x1401, new LevelData("Lake Jyun Sub-Gate",             "Corridor Room",                    0x14, 0x01) },
                { 0x1402, new LevelData("Lake Jyun Sub-Gate",             "Sharukurusu Room",                 0x14, 0x02) },
                { 0x1403, new LevelData("Lake Jyun Sub-Gate",             "Firushudot Hall",                  0x14, 0x03) },
                //Missing one?
                { 0x1404, new LevelData("Lake Jyun Sub-Gate",             "Refractor Room",                   0x14, 0x04) },
                { 0x1500, new LevelData("Bonne Ending Boat",              "Boat Interior",                    0x15, 0x00) },
                { 0x1501, new LevelData("Bonne Ending Boat",              "Ocean",                            0x15, 0x01) },
                { 0x1600, new LevelData("Flutter To Subgate Cutscene",    "Cutscene",                         0x16, 0x00) },
                { 0x1601, new LevelData("Flutter To Subgate Cutscene",    "Flutter Bridge",                   0x16, 0x01) },
                { 0x1602, new LevelData("Flutter To Subgate Cutscene",    "Return Cutscene",                  0x16, 0x02) },
                { 0x1700, new LevelData("Gesselschaft Battle",            "Small Bridge",                     0x17, 0x00) },
                { 0x1701, new LevelData("Gesselschaft Battle",            "Flutter Bridge",                   0x17, 0x01) },
                { 0x1702, new LevelData("Gesselschaft Battle",            "Gesselschaft HQ",                  0x17, 0x02) },
                { 0x1703, new LevelData("Gesselschaft Battle",            "Scrolling Clouds",                 0x17, 0x03) },
                { 0x1800, new LevelData("Flutter Takeoff",                "Cardon Forest with no Flutter",    0x18, 0x00) },
                { 0x1801, new LevelData("Flutter Takeoff",                "Flutter Engine Room",              0x18, 0x01) },
                { 0x1802, new LevelData("Flutter Takeoff",                "Glowing Refractor",                0x18, 0x02) },
                { 0x1900, new LevelData("Old City",                       "Old City (no dogs, combat)",       0x19, 0x00) },
                { 0x1901, new LevelData("Old City",                       "Bonne's Warehouse",                0x19, 0x01) },
                { 0x1902, new LevelData("Old City",                       "Power Plant",                      0x19, 0x02) },
                { 0x1903, new LevelData("Old City",                       "Old City (dogs, no weapons)",      0x19, 0x03) },
                { 0x1A00, new LevelData("Main Gate",                      "Second Area (console)",            0x1A, 0x00) },
                { 0x1A01, new LevelData("Main Gate",                      "Third Area (locked doors)",        0x1A, 0x01) },
                { 0x1A02, new LevelData("Main Gate",                      "Final Area",                       0x1A, 0x02) },
                { 0x1A03, new LevelData("Main Gate",                      "First Area 1",                     0x1A, 0x03) },
                { 0x1A04, new LevelData("Main Gate",                      "First Area 2",                     0x1A, 0x04) },
                { 0x1A05, new LevelData("Main Gate",                      "First Area 3",                     0x1A, 0x05) },
                { 0x1A06, new LevelData("Main Gate",                      "First Area 4",                     0x1A, 0x06) },
                { 0x1A07, new LevelData("Main Gate",                      "First Area 5",                     0x1A, 0x07) },
                { 0x1A08, new LevelData("Main Gate",                      "First Area 6",                     0x1A, 0x08) },
                { 0x1A09, new LevelData("Main Gate",                      "First Area 7 (Entrance)",          0x1A, 0x09) },
                { 0x1A0A, new LevelData("Main Gate",                      "Tiny Kattelox",                    0x1A, 0x0A) },
                { 0x1A0B, new LevelData("Main Gate",                      "Juno Wall",                        0x1A, 0x0B) },
                { 0x1A0C, new LevelData("Main Gate",                      "Amelia's Office (simplified)",     0x1A, 0x0C) },
                { 0x1A0D, new LevelData("Main Gate",                      "Downtown (simplified)",            0x1A, 0x0D) },
                { 0x1A0E, new LevelData("Main Gate",                      "Empty?",                           0x1A, 0x0E) },
                { 0x1A0F, new LevelData("Main Gate",                      "Juno's Room Without Door",         0x1A, 0x0F) },
                { 0x1A10, new LevelData("Main Gate",                      "Unknown (textures?)",              0x1A, 0x10) },
                { 0x1B00, new LevelData("Cardon Forest (Flutter Fixed)",  "Near ruin entrance",               0x1B, 0x00) },
                { 0x1B01, new LevelData("Cardon Forest (Flutter Fixed)",  "Crash Site",                       0x1B, 0x01) },
                { 0x1B02, new LevelData("Cardon Forest (Flutter Fixed)",  "City Entrance",                    0x1B, 0x02) },
                { 0x1B03, new LevelData("Cardon Forest (Flutter Fixed)",  "Unknown (floating model)",         0x1B, 0x03) },
                { 0x1B04, new LevelData("Cardon Forest (Flutter Fixed)",  "Unknown (floating model)",         0x1B, 0x04) },
                { 0x1B05, new LevelData("Cardon Forest (Flutter Fixed)",  "Barrell's Room",                    0x1B, 0x05) },
                { 0x1B06, new LevelData("Cardon Forest (Flutter Fixed)",  "Roll's Room",                      0x1B, 0x06) },
                { 0x1B07, new LevelData("Cardon Forest (Flutter Fixed)",  "Flutter Lobby",                    0x1B, 0x07) },
                { 0x1B08, new LevelData("Cardon Forest (Flutter Fixed)",  "Mega Man's Room",                  0x1B, 0x08) },
                { 0x1C00, new LevelData("Museum",                         "First Floor",                      0x1C, 0x00) },
                { 0x1C01, new LevelData("Museum",                         "Second Floor",                     0x1C, 0x01) },
                { 0x1D00, new LevelData("Sub-Cities",                     "Watcher Sub-City",                 0x1D, 0x00) },
                { 0x1D01, new LevelData("Sub-Cities",                     "Sleeper Sub-City",                 0x1D, 0x01) },
                { 0x1D02, new LevelData("Sub-Cities",                     "Dreamer Sub-City",                 0x1D, 0x02) },
                { 0x1D03, new LevelData("Sub-Cities",                     "Watcher Chest Room",               0x1D, 0x03) },
                { 0x1D04, new LevelData("Sub-Cities",                     "Sleeper Chest Room",               0x1D, 0x04) },
                { 0x1D05, new LevelData("Sub-Cities",                     "Dreamer Chest Room",               0x1D, 0x05) },
                { 0x1E00, new LevelData("Ending",                         "Flutter Launch Area (different)",  0x1E, 0x00) },
                { 0x1E01, new LevelData("Ending",                         "Flutter Bridge",                   0x1E, 0x01) },
                { 0x1E02, new LevelData("Ending",                         "Cliff and Ocean",                  0x1E, 0x02) },
                { 0x1E03, new LevelData("Ending",                         "Empty?",                           0x1E, 0x03) },
                { 0x1E04, new LevelData("Ending",                         "Empty?",                           0x1E, 0x04) },
                { 0x1E05, new LevelData("Ending",                         "Tiny Kattelox",                    0x1E, 0x05) }
            };
            return levelDataDict;
        }
        
        public static ItemData GetZennyItemData(uint units, bool isFiller=false)
        {
            string name = (10 * units).ToString() + " Zenny";
            return new(ItemCategory.Zenny, name, units, null, null, isFiller);
        }
        
        public static ItemData GetNothingItemData(bool isFiller=false)
        {
            return new(ItemCategory.Nothing, "Nothing", 1, 0xFF, null, isFiller);
        }

        public static ItemData GetAPItemData()
        {
            return new(ItemCategory.AP, "AP Item", 1, null, null, false);
        }
        
        public static Dictionary<long, ItemData> GetItemDataDict()
        {
            Dictionary<long, ItemData> itemDataDict = new()
            {
                { 0x0000, GetAPItemData() },
                { 0x00FF, GetNothingItemData() },
                { 0x0200, new ItemData(ItemCategory.Buster,  "Blaster Unit Omega",               1, 0x00, null)                              },
                { 0x0201, new ItemData(ItemCategory.Buster,  "Sniper Unit Omega",                1, 0x01, null)                              },
                { 0x0202, new ItemData(ItemCategory.Buster,  "Laser",                            1, 0x02, null)                              },
                { 0x0203, new ItemData(ItemCategory.Buster,  "Sniper Range",                     1, 0x03, null)                              },
                { 0x0204, new ItemData(ItemCategory.Buster,  "Turbo Battery",                    1, 0x04, null)                              },
                { 0x0205, new ItemData(ItemCategory.Buster,  "Power Raiser Omega",               1, 0x05, null)                              },
                { 0x0206, new ItemData(ItemCategory.Buster,  "Range Booster Omega",              1, 0x06, null)                              },
                { 0x0207, new ItemData(ItemCategory.Buster,  "Turbo Charger Omega",              1, 0x07, null)                              },
                { 0x0208, new ItemData(ItemCategory.Buster,  "Blast Unit",                       1, 0x08, null)                              },
                { 0x0209, new ItemData(ItemCategory.Buster,  "Sniper Unit",                      1, 0x09, null)                              },
                { 0x020A, new ItemData(ItemCategory.Buster,  "Power Raiser Alpha",               1, 0x0A, null)                              },
                { 0x020B, new ItemData(ItemCategory.Buster,  "Range Booster Alpha",              1, 0x0B, null)                              },
                { 0x020C, new ItemData(ItemCategory.Buster,  "Turbo Charger Alpha",              1, 0x0C, null)                              },
                { 0x020D, new ItemData(ItemCategory.Buster,  "Power Raiser",                     1, 0x0D, null)                              },
                { 0x020E, new ItemData(ItemCategory.Buster,  "Range Booster",                    1, 0x0E, null)                              },
                { 0x020F, new ItemData(ItemCategory.Buster,  "Turbo Charger",                    1, 0x0F, null)                              },
                { 0x0210, new ItemData(ItemCategory.Buster,  "Buster Max",                       1, 0x10, null)                              },
                { 0x0211, new ItemData(ItemCategory.Buster,  "Power Stream",                     1, 0x11, null)                              },
                { 0x0212, new ItemData(ItemCategory.Buster,  "Blaster Unit R",                   1, 0x12, null)                              },
                { 0x0213, new ItemData(ItemCategory.Buster,  "Buster Unit Omega",                1, 0x13, null)                              },
                { 0x0214, new ItemData(ItemCategory.Buster,  "Omni-Unit Omega",                  1, 0x14, null)                              },
                { 0x0215, new ItemData(ItemCategory.Buster,  "Auto Battery",                     1, 0x15, null)                              },
                { 0x0216, new ItemData(ItemCategory.Buster,  "Sniper Scope",                     1, 0x16, null)                              },
                { 0x0217, new ItemData(ItemCategory.Buster,  "Rapid Striker",                    1, 0x17, null)                              },
                { 0x0218, new ItemData(ItemCategory.Buster,  "Gatling Gun",                      1, 0x18, null)                              },
                { 0x0219, new ItemData(ItemCategory.Buster,  "Omni Unit",                        1, 0x19, null)                              },
                { 0x021A, new ItemData(ItemCategory.Buster,  "Power Blaster R",                  1, 0x1A, null)                              },
                { 0x021B, new ItemData(ItemCategory.Buster,  "Power Blaster L",                  1, 0x1B, null)                              },
                { 0x021C, new ItemData(ItemCategory.Buster,  "Machine Gun",                      1, 0x1C, null)                              },
                { 0x021D, new ItemData(ItemCategory.Buster,  "Triple Access",                    1, 0x1D, null)                              },
                { 0x021E, new ItemData(ItemCategory.Buster,  "Buster Unit",                      1, 0x1E, null)                              },
                { 0x021F, new ItemData(ItemCategory.Buster,  "Rapid Fire",                       1, 0x1F, null)                              },
                { 0x0220, new ItemData(ItemCategory.Special, "Helmet",                           1, 0x20, new AddressData(0xBE41C, 7, null)) },
                { 0x0221, new ItemData(ItemCategory.Special, "Jump Springs",                     1, 0x21, new AddressData(0xBE41C, 6, null)) },
                { 0x0222, new ItemData(ItemCategory.Special, "Jet Skates",                       1, 0x22, new AddressData(0xBE41C, 5, null)) },
                { 0x0223, new ItemData(ItemCategory.Special, "Life Gauge",                       1, 0x23, new AddressData(0xBE41C, 4, null)) },
                { 0x0224, new ItemData(ItemCategory.Special, "Energy Canteen",                   1, 0x24, new AddressData(0xBE41C, 3, null)) },
                { 0x0225, new ItemData(ItemCategory.Special, "Extra Pack",                       1, 0x25, new AddressData(0xBE41C, 2, null)) },
                { 0x0226, new ItemData(ItemCategory.Special, "Adapter Plug",                     1, 0x26, new AddressData(0xBE41C, 1, null)) },
                { 0x0227, new ItemData(ItemCategory.Special, "Blue Refractor",                   1, 0x27, new AddressData(0xBE41C, 0, null)) },
                { 0x0228, new ItemData(ItemCategory.Special, "Yellow Refractor",                 1, 0x28, new AddressData(0xBE41D, 7, null)) },
                { 0x0229, new ItemData(ItemCategory.Special, "Red Refractor",                    1, 0x29, new AddressData(0xBE41D, 6, null)) },
                { 0x022A, new ItemData(ItemCategory.Special, "Citizen's Card",                   1, 0x2A, new AddressData(0xBE41D, 5, null)) },
                { 0x022B, new ItemData(ItemCategory.Special, "Class A License",                  1, 0x2B, new AddressData(0xBE41D, 4, null)) },
                { 0x022C, new ItemData(ItemCategory.Special, "Class B License",                  1, 0x2C, new AddressData(0xBE41D, 3, null)) },
                { 0x022D, new ItemData(ItemCategory.Special, "Bonne Key",                        1, 0x2D, new AddressData(0xBE41D, 2, null)) },
              //{ 0x022E, new ItemData(ItemCategory.Special, "Cardon Forest Sub-Gate Key 1",     1, 0x2E, new AddressData(0xBE41D, 1, null)) },
              //{ 0x0230, new ItemData(ItemCategory.Special, "Cardon Forest Sub-Gate Key 3",     1, 0x30, new AddressData(0xBE41E, 7, null)) },
              //{ 0x022F, new ItemData(ItemCategory.Special, "Cardon Forest Sub-Gate Key 2",     1, 0x2F, new AddressData(0xBE41D, 0, null)) },
                { 0x0231, new ItemData(ItemCategory.Special, "Lake Jyun Sub-Gate Starter Key 1", 1, 0x31, new AddressData(0xBE41E, 6, null)) },
                { 0x0232, new ItemData(ItemCategory.Special, "Lake Jyun Sub-Gate Starter Key 2", 1, 0x32, new AddressData(0xBE41E, 5, null)) },
                { 0x0233, new ItemData(ItemCategory.Special, "Lake Jyun Sub-Gate Starter Key 3", 1, 0x33, new AddressData(0xBE41E, 4, null)) },
                { 0x0234, new ItemData(ItemCategory.Special, "Clozer Woods Sub-Gate Key 1",      1, 0x34, new AddressData(0xBE41E, 3, null)) },
                { 0x0235, new ItemData(ItemCategory.Special, "Clozer Woods Sub-Gate Key 2",      1, 0x35, new AddressData(0xBE41E, 2, null)) },
                { 0x0236, new ItemData(ItemCategory.Special, "Clozer Woods Sub-Gate Key 3",      1, 0x36, new AddressData(0xBE41E, 1, null)) },
                { 0x0237, new ItemData(ItemCategory.Special, "'Watcher' Key",                     1, 0x37, new AddressData(0xBE41E, 0, null)) },
                { 0x0238, new ItemData(ItemCategory.Special, "'Sleeper' Key",                     1, 0x38, new AddressData(0xBE41F, 7, null)) },
                { 0x0239, new ItemData(ItemCategory.Special, "'Dreamer' Key",                     1, 0x39, new AddressData(0xBE41F, 6, null)) },
                { 0x023A, new ItemData(ItemCategory.Special, "Flak Jacket",                      1, 0x3A, new AddressData(0xBE41F, 5, null)) },
                { 0x023B, new ItemData(ItemCategory.Special, "Kevlar Jacket",                    1, 0x3B, new AddressData(0xBE41F, 4, null)) },
                { 0x023C, new ItemData(ItemCategory.Special, "Kevlar Jacket Omega",              1, 0x3C, new AddressData(0xBE41F, 3, null)) },
                { 0x023D, new ItemData(ItemCategory.Special, "Walkie-Talkie",                    1, 0x3D, new AddressData(0xBE41F, 2, null)) },
                { 0x023E, new ItemData(ItemCategory.Special, "KEY-ITEM 1E",                      1, 0x3E, new AddressData(0xBE41F, 1, null)) },
                { 0x023F, new ItemData(ItemCategory.Special, "(empty)",                          1, 0x3F, new AddressData(0xBE41F, 0, null)) },
                { 0x0240, new ItemData(ItemCategory.Normal,  "Shield Repair",                    1, 0x40, new AddressData(0xBE420, 7, null)) },
                { 0x0241, new ItemData(ItemCategory.Normal,  "Hyper Cartridge",                  1, 0x41, new AddressData(0xBE420, 6, null)) },
                { 0x0242, new ItemData(ItemCategory.Normal,  "Chamelion Net",                    1, 0x42, new AddressData(0xBE420, 5, null)) },
                { 0x0243, new ItemData(ItemCategory.Normal,  "Defense Shield",                   1, 0x43, new AddressData(0xBE420, 4, null)) },
                { 0x0244, new ItemData(ItemCategory.Normal,  "Flower",                           1, 0x44, new AddressData(0xBE420, 3, null)) },
                { 0x0245, new ItemData(ItemCategory.Normal,  "Bag",                              1, 0x45, new AddressData(0xBE420, 2, null)) },
                { 0x0246, new ItemData(ItemCategory.Normal,  "Trunk",                            1, 0x46, new AddressData(0xBE420, 1, null)) },
                { 0x0247, new ItemData(ItemCategory.Normal,  "Pick",                             1, 0x47, new AddressData(0xBE420, 0, null)) },
                { 0x0248, new ItemData(ItemCategory.Normal,  "Saw",                              1, 0x48, new AddressData(0xBE421, 7, null)) },
                { 0x0249, new ItemData(ItemCategory.Normal,  "Lipstick",                         1, 0x49, new AddressData(0xBE421, 6, null)) },
                { 0x024A, new ItemData(ItemCategory.Normal,  "Music Box",                        1, 0x4A, new AddressData(0xBE421, 5, null)) },
                { 0x024B, new ItemData(ItemCategory.Normal,  "Old Bone",                         1, 0x4B, new AddressData(0xBE421, 4, null)) },
                { 0x024C, new ItemData(ItemCategory.Normal,  "Old Heater",                       1, 0x4C, new AddressData(0xBE421, 3, null)) },
                { 0x024D, new ItemData(ItemCategory.Normal,  "Old Doll",                         1, 0x4D, new AddressData(0xBE421, 2, null)) },
                { 0x024E, new ItemData(ItemCategory.Normal,  "Antique Bell",                     1, 0x4E, new AddressData(0xBE421, 1, null)) },
                { 0x024F, new ItemData(ItemCategory.Normal,  "Giant Horn",                       1, 0x4F, new AddressData(0xBE421, 0, null)) },
                { 0x0250, new ItemData(ItemCategory.Normal,  "Shiny Object",                     1, 0x50, new AddressData(0xBE422, 7, null)) },
                { 0x0251, new ItemData(ItemCategory.Normal,  "Old Shield",                       1, 0x51, new AddressData(0xBE422, 6, null)) },
                { 0x0252, new ItemData(ItemCategory.Normal,  "Shiny Red Stone",                  1, 0x52, new AddressData(0xBE422, 5, null)) },
                { 0x0253, new ItemData(ItemCategory.Normal,  "Stag Beetle",                      1, 0x53, new AddressData(0xBE422, 4, null)) },
                { 0x0254, new ItemData(ItemCategory.Normal,  "Beetle",                           1, 0x54, new AddressData(0xBE422, 3, null)) },
                { 0x0255, new ItemData(ItemCategory.Normal,  "Comic Book",                       1, 0x55, new AddressData(0xBE422, 2, null)) },
                { 0x0256, new ItemData(ItemCategory.Normal,  "Ring",                             1, 0x56, new AddressData(0xBE422, 1, null)) },
                { 0x0257, new ItemData(ItemCategory.Normal,  "EMPTY",                            1, 0x57, new AddressData(0xBE422, 0, null)) },
                { 0x0258, new ItemData(ItemCategory.Normal,  "Mine Parts Kit",                   1, 0x58, new AddressData(0xBE423, 7, null)) },
                { 0x0259, new ItemData(ItemCategory.Normal,  "Cannon Kit",                       1, 0x59, new AddressData(0xBE423, 6, null)) },
                { 0x025A, new ItemData(ItemCategory.Normal,  "Grenade Kit",                      1, 0x5A, new AddressData(0xBE423, 5, null)) },
                { 0x025B, new ItemData(ItemCategory.Normal,  "Blumebear Parts",                  1, 0x5B, new AddressData(0xBE423, 4, null)) },
                { 0x025C, new ItemData(ItemCategory.Normal,  "Mystic Orb",                       1, 0x5C, new AddressData(0xBE423, 3, null)) },
                { 0x025D, new ItemData(ItemCategory.Normal,  "Marlwolf Shell",                   1, 0x5D, new AddressData(0xBE423, 2, null)) },
                { 0x025E, new ItemData(ItemCategory.Normal,  "Broken Motor",                     1, 0x5E, new AddressData(0xBE423, 1, null)) },
                { 0x025F, new ItemData(ItemCategory.Normal,  "Broken Propeller",                 1, 0x5F, new AddressData(0xBE423, 0, null)) },
                { 0x0260, new ItemData(ItemCategory.Normal,  "Broken Cleaner",                   1, 0x60, new AddressData(0xBE424, 7, null)) },
                { 0x0261, new ItemData(ItemCategory.Normal,  "Bomb Schematic",                   1, 0x61, new AddressData(0xBE424, 6, null)) },
                { 0x0262, new ItemData(ItemCategory.Normal,  "Blunted Drill",                    1, 0x62, new AddressData(0xBE424, 5, null)) },
                { 0x0263, new ItemData(ItemCategory.Normal,  "Guidance Unit",                    1, 0x63, new AddressData(0xBE424, 4, null)) },
                { 0x0264, new ItemData(ItemCategory.Normal,  "Zetsabre",                         1, 0x64, new AddressData(0xBE424, 3, null)) },
                { 0x0265, new ItemData(ItemCategory.Normal,  "Pen Light",                        1, 0x65, new AddressData(0xBE424, 2, null)) },
                { 0x0266, new ItemData(ItemCategory.Normal,  "Old Launcher",                     1, 0x66, new AddressData(0xBE424, 1, null)) },
                { 0x0267, new ItemData(ItemCategory.Normal,  "Ancient Book",                     1, 0x67, new AddressData(0xBE424, 0, null)) },
                { 0x0268, new ItemData(ItemCategory.Normal,  "Arm Supporter",                    1, 0x68, new AddressData(0xBE425, 7, null)) },
                { 0x0269, new ItemData(ItemCategory.Normal,  "X Buster",                         1, 0x69, new AddressData(0xBE425, 6, null)) },
                { 0x026A, new ItemData(ItemCategory.Normal,  "Weapon Plans",                     1, 0x6A, new AddressData(0xBE425, 5, null)) },
                { 0x026B, new ItemData(ItemCategory.Normal,  "Prism Crystal",                    1, 0x6B, new AddressData(0xBE425, 4, null)) },
                { 0x026C, new ItemData(ItemCategory.Normal,  "Spring Set",                       1, 0x6C, new AddressData(0xBE425, 3, null)) },
                { 0x026D, new ItemData(ItemCategory.Normal,  "Safety Helmet",                    1, 0x6D, new AddressData(0xBE425, 2, null)) },
                { 0x026E, new ItemData(ItemCategory.Normal,  "Rollerboard",                      1, 0x6E, new AddressData(0xBE425, 1, null)) },
                { 0x026F, new ItemData(ItemCategory.Normal,  "Old Hoverjets",                    1, 0x6F, new AddressData(0xBE425, 0, null)) },
                { 0x0270, new ItemData(ItemCategory.Normal,  "Joint Plug",                       1, 0x70, new AddressData(0xBE426, 7, null)) },
                { 0x0271, new ItemData(ItemCategory.Normal,  "Broken Circuits",                  1, 0x71, new AddressData(0xBE426, 6, null)) },
                { 0x0272, new ItemData(ItemCategory.Normal,  "Main Core Shard",                  1, 0x72, new AddressData(0xBE426, 5, null)) },
                { 0x0273, new ItemData(ItemCategory.Normal,  "Sun-light",                        1, 0x73, new AddressData(0xBE426, 4, null)) },
                { 0x0274, new ItemData(ItemCategory.Normal,  "Rapidfire Barrel",                 1, 0x74, new AddressData(0xBE426, 3, null)) },
                { 0x0275, new ItemData(ItemCategory.Normal,  "Plastique",                        1, 0x75, new AddressData(0xBE426, 2, null)) },
                { 0x0276, new ItemData(ItemCategory.Normal,  "Bomb",                             1, 0x76, new AddressData(0xBE426, 1, null)) },
                { 0x0277, new ItemData(ItemCategory.Normal,  "Gatling Part",                     1, 0x77, new AddressData(0xBE426, 0, null)) },
                { 0x0278, new ItemData(ItemCategory.Normal,  "Flower Pearl",                     1, 0x78, new AddressData(0xBE427, 7, null)) },
                { 0x0279, new ItemData(ItemCategory.Normal,  "Autofire Barrel",                  1, 0x79, new AddressData(0xBE427, 6, null)) },
                { 0x027A, new ItemData(ItemCategory.Normal,  "Generator Part",                   1, 0x7A, new AddressData(0xBE427, 5, null)) },
                { 0x027B, new ItemData(ItemCategory.Normal,  "Target Sensor",                    1, 0x7B, new AddressData(0xBE427, 4, null)) },
                { 0x027C, new ItemData(ItemCategory.Normal,  "Tele-lens",                        1, 0x7C, new AddressData(0xBE427, 3, null)) },
                { 0x027D, new ItemData(ItemCategory.Normal,  "JUNK-ITEM 25",                     1, 0x7D, new AddressData(0xBE427, 2, null)) },
                { 0x027E, new ItemData(ItemCategory.Normal,  "JUNK-ITEM 26",                     1, 0x7E, new AddressData(0xBE427, 1, null)) },
                { 0x027F, new ItemData(ItemCategory.Normal,  "JUNK-ITEM 27",                     1, 0x7F, new AddressData(0xBE427, 0, null)) },
                { 0x8001, GetZennyItemData(0x001)                                                                                            }, // 10
                { 0x8002, GetZennyItemData(0x002)                                                                                            }, // 20
                { 0x8003, GetZennyItemData(0x003)                                                                                            }, // 30
                { 0x8005, GetZennyItemData(0x005)                                                                                            }, // 50
                { 0x800A, GetZennyItemData(0x00A)                                                                                            }, // 100
                { 0x8014, GetZennyItemData(0x014)                                                                                            }, // 200
                { 0x8016, GetZennyItemData(0x016)                                                                                            }, // 220
                { 0x801E, GetZennyItemData(0x01E)                                                                                            }, // 300
                { 0x802D, GetZennyItemData(0x02D)                                                                                            }, // 450
                { 0x8038, GetZennyItemData(0x038)                                                                                            }, // 560
                { 0x8042, GetZennyItemData(0x042)                                                                                            }, // 660
                { 0x804E, GetZennyItemData(0x04E)                                                                                            }, // 780
                { 0x8050, GetZennyItemData(0x050)                                                                                            }, // 800
                { 0x8052, GetZennyItemData(0x052)                                                                                            }, // 820
                { 0x805C, GetZennyItemData(0x05C)                                                                                            }, // 920
                { 0x8076, GetZennyItemData(0x076)                                                                                            }, // 1180
                { 0x8078, GetZennyItemData(0x078)                                                                                            }, // 1200
                { 0x807C, GetZennyItemData(0x07C)                                                                                            }, // 1240
                { 0x8097, GetZennyItemData(0x097)                                                                                            }, // 1510
                { 0x80A2, GetZennyItemData(0x0A2)                                                                                            }, // 1620
                { 0x80B2, GetZennyItemData(0x0B2)                                                                                            }, // 1780
                { 0x80B8, GetZennyItemData(0x0B8)                                                                                            }, // 1840
                { 0x80C4, GetZennyItemData(0x0C4)                                                                                            }, // 1960
                { 0x80D9, GetZennyItemData(0x0D9)                                                                                            }, // 2170
                { 0x80E4, GetZennyItemData(0x0E4)                                                                                            }, // 2280
                { 0x80E6, GetZennyItemData(0x0E6)                                                                                            }, // 2300
                { 0x8104, GetZennyItemData(0x104)                                                                                            }, // 2600
                { 0x811C, GetZennyItemData(0x11C)                                                                                            }, // 2840
                { 0x81C4, GetZennyItemData(0x1C4)                                                                                            }, // 4520
                { 0x8201, GetZennyItemData(0x201)                                                                                            }, // 5130
                { 0x8230, GetZennyItemData(0x230)                                                                                            }, // 5600
                { 0x839C, GetZennyItemData(0x39C)                                                                                            }, // 9240
                { 0x83E8, GetZennyItemData(0x3E8)                                                                                            }  // 10000
            };
            return itemDataDict;
        }

        public static Dictionary<int, LocationData> GetLocationDataDict()
        {
            Dictionary<ushort, LevelData> levelDataDict = GetLevelDataDict();
            Dictionary<long, ItemData> itemDataDict = GetItemDataDict();
            Dictionary<int, LocationData> locationDataDict = new()
            {
                { 1,   new LocationData(1,   "Ocean tower, Right chest",                                      LocationCategory.Container, levelDataDict[0x0002], itemDataDict[0x020D], new AddressData(0xBE400, 7, null), null, true,  0x9F114, 0x1539E8) },
                { 2,   new LocationData(2,   "Ocean tower, Left chest",                                       LocationCategory.Container, levelDataDict[0x0002], itemDataDict[0x8038], new AddressData(0xBE400, 6, null), null, true,  0x9F4D4, 0x8C759)  },
                { 3,   new LocationData(3,   "Apple market, Electric goods box",                              LocationCategory.Container, levelDataDict[0x0400], itemDataDict[0x0260], new AddressData(0xBE3F9, 6, null), null, false, 0xBF264, 0x8C4B2)  },
                { 4,   new LocationData(4,   "Apple market, Book store box",                                  LocationCategory.Container, levelDataDict[0x0400], itemDataDict[0x00FF], new AddressData(0xBE3F9, 7, null), null, false, 0xBF214, 0x8C46A)  },
                { 5,   new LocationData(5,   "Apple market, Junk store box",                                  LocationCategory.Container, levelDataDict[0x0400], itemDataDict[0x800A], new AddressData(0xBE3F8, 0, null), null, false, 0xBF1C4, 0x8C502)  },
                { 6,   new LocationData(6,   "Apple market, North pail",                                      LocationCategory.Container, levelDataDict[0x0400], itemDataDict[0x8002], new AddressData(0xBE3F9, 5, null), null, false, 0xBF2B4, 0x8C3DE)  },
                { 7,   new LocationData(7,   "Apple market, South pail",                                      LocationCategory.Container, levelDataDict[0x0400], itemDataDict[0x8005], new AddressData(0xBE3F9, 2, null), null, false, 0xBF304, 0x8C332)  },
                { 8,   new LocationData(8,   "Downtown, South east peace sign pail",                          LocationCategory.Container, levelDataDict[0x0500], itemDataDict[0x8001], new AddressData(0xBE3F8, 6, null), null, false, 0xBF354, 0x8C3DE)  },
                { 9,   new LocationData(9,   "Downtown, Center east pail",                                    LocationCategory.Container, levelDataDict[0x0500], itemDataDict[0x025E], new AddressData(0xBE3F8, 2, null), null, false, 0xBF494, 0x8C382)  },
                { 10,  new LocationData(10,  "Downtown, Don't kick us pail",                                  LocationCategory.Container, levelDataDict[0x0500], itemDataDict[0x025B], new AddressData(0xBE3F8, 5, null), null, false, 0xBF3A4, 0x8C332)  },
                { 11,  new LocationData(11,  "Downtown, Center pail",                                         LocationCategory.Container, levelDataDict[0x0500], itemDataDict[0x0245], new AddressData(0xBE3F8, 4, null), null, false, 0xBF3F4, 0x8C332)  },
                { 12,  new LocationData(12,  "Downtown, Library pail",                                        LocationCategory.Container, levelDataDict[0x0500], itemDataDict[0x0248], new AddressData(0xBE3F8, 3, null), null, false, 0xBF444, 0x8C332)  },
                { 13,  new LocationData(13,  "Uptown, Hospital right pail",                                   LocationCategory.Container, levelDataDict[0x0800], itemDataDict[0x025F], new AddressData(0xBE3F9, 1, null), null, false, 0xBF214, 0x8C382)  },
                { 14,  new LocationData(14,  "Uptown, Hospital left pail",                                    LocationCategory.Container, levelDataDict[0x0800], itemDataDict[0x00FF], new AddressData(0xBE3F9, 0, null), null, false, 0xBF264, 0x8C332)  },
                { 15,  new LocationData(15,  "Uptown, Ocean corner pail",                                     LocationCategory.Container, levelDataDict[0x0800], itemDataDict[0x8003], new AddressData(0xBE3FA, 6, null), null, false, 0xBF2B4, 0x8C3DE)  },
                { 16,  new LocationData(16,  "Wily's Boat, Right box",                                        LocationCategory.Container, levelDataDict[0x0D00], itemDataDict[0x8002], new AddressData(0xBE3FA, 1, null), null, false, 0xBF174, 0x8C46A)  },
                { 17,  new LocationData(17,  "Wily's Boat, Left box",                                         LocationCategory.Container, levelDataDict[0x0D00], itemDataDict[0x00FF], new AddressData(0xBE3FA, 2, null), null, false, 0xBF124, 0x8C46A)  },
                { 18,  new LocationData(18,  "Wily's Boat, Pail",                                             LocationCategory.Container, levelDataDict[0x0D00], itemDataDict[0x8001], new AddressData(0xBE3FA, 0, null), null, false, 0xBF1C4, 0x8C3DE)  },
                { 19,  new LocationData(19,  "Yass plains, Plateau house box",                                LocationCategory.Container, levelDataDict[0x1000], itemDataDict[0x026D], new AddressData(0xBE3FB, 6, null), null, false, 0xBF174, 0x8C4B2)  },
                { 20,  new LocationData(20,  "Yass plains, Plateau house pail",                               LocationCategory.Container, levelDataDict[0x1000], itemDataDict[0x00FF], new AddressData(0xBE3FB, 3, null), null, false, 0xBF214, 0x8C332)  },
                { 21,  new LocationData(21,  "Yass plains, Behind hideout pail",                              LocationCategory.Container, levelDataDict[0x1000], itemDataDict[0x8014], new AddressData(0xBE3FB, 4, null), null, false, 0xBF1C4, 0x8C3DE)  },
                { 22,  new LocationData(22,  "Yass plains, Across hideout pail",                              LocationCategory.Container, levelDataDict[0x1000], itemDataDict[0x00FF], new AddressData(0xBE3FB, 5, null), null, false, 0xBF264, 0x8C332)  },
                { 23,  new LocationData(23,  "Underground ruins, Junk store man chest",                       LocationCategory.Container, levelDataDict[0x0900], itemDataDict[0x021F], new AddressData(0xBE400, 5, null), null, false, 0x9F114, 0x1533A5) },
                { 24,  new LocationData(24,  "Underground ruins, Junk store man hole",                        LocationCategory.Hole,      levelDataDict[0x0900], itemDataDict[0x8104], new AddressData(0xBE3DF, 6, null), null, false, null,    0x15386F) },
                { 25,  new LocationData(25,  "Underground ruins, Main gate entrance chest",                   LocationCategory.Container, levelDataDict[0x0901], itemDataDict[0x027A], new AddressData(0xBE402, 6, null), null, false, 0x9F894, 0x8C735)  },
                { 26,  new LocationData(26,  "Underground ruins, 2 box ledge chest",                          LocationCategory.Container, levelDataDict[0x0901], itemDataDict[0x021E], new AddressData(0xBE400, 4, null), null, false, 0x9F114, 0x8C735)  },
                { 27,  new LocationData(27,  "Underground ruins, Miroc room ledge chest",                     LocationCategory.Container, levelDataDict[0x0901], itemDataDict[0x8076], new AddressData(0xBE404, 6, null), null, false, 0x9FC54, 0x8C759)  },
                { 28,  new LocationData(28,  "Underground ruins, Cross room chest",                           LocationCategory.Container, levelDataDict[0x0901], itemDataDict[0x026E], new AddressData(0xBE400, 1, null), null, false, 0x9F4D4, 0x8C735)  },
                { 29,  new LocationData(29,  "Underground ruins, Miroc room left hole",                       LocationCategory.Hole,      levelDataDict[0x0901], itemDataDict[0x839C], new AddressData(0xBE3DE, 3, null), null, false, null,    0x1535B7) },
                { 30,  new LocationData(30,  "Underground ruins, Miroc room right hole",                      LocationCategory.Hole,      levelDataDict[0x0901], itemDataDict[0x8052], new AddressData(0xBE3DF, 4, null), null, false, null,    0x1539AB) },
                { 31,  new LocationData(31,  "Underground ruins, Arukoitan battle north chest",               LocationCategory.Container, levelDataDict[0x0904], itemDataDict[0x80E4], new AddressData(0xBE403, 0, null), null, false, 0x9F114, 0x8C759)  },
                { 32,  new LocationData(32,  "Underground ruins, Arukoitan battle south chest",               LocationCategory.Container, levelDataDict[0x0904], itemDataDict[0x811C], new AddressData(0xBE404, 7, null), null, false, 0x9F4D4, 0x8C759)  },
                { 33,  new LocationData(33,  "Underground ruins, Obstacle room cliff east hole",              LocationCategory.Hole,      levelDataDict[0x0904], itemDataDict[0x804E], new AddressData(0xBE3DF, 2, null), null, false, null,    0x153AB8) },
                { 34,  new LocationData(34,  "Underground ruins, Obstacle room cliff west hole",              LocationCategory.Hole,      levelDataDict[0x0904], itemDataDict[0x81C4], new AddressData(0xBE3DE, 0, null), null, false, null,    0x1536C4) },
                { 35,  new LocationData(35,  "Underground ruins, Shekuten pillar room chest",                 LocationCategory.Container, levelDataDict[0x0905], itemDataDict[0x80A2], new AddressData(0xBE404, 5, null), null, false, 0x9F114, 0x8C759)  },
                { 36,  new LocationData(36,  "Underground ruins, Shekuten pillar room hole",                  LocationCategory.Hole,      levelDataDict[0x0905], itemDataDict[0x0251], new AddressData(0xBE422, 6, null), null, false, null,    0x153800) },
                { 37,  new LocationData(37,  "Underground ruins, Kuruguru obstacle hole",                     LocationCategory.Hole,      levelDataDict[0x0905], itemDataDict[0x0250], new AddressData(0xBE3DE, 2, null), null, false, null,    0x153655) },
                { 38,  new LocationData(38,  "Underground ruins, Gold Gorubesshu chest",                      LocationCategory.Container, levelDataDict[0x0906], itemDataDict[0x0266], new AddressData(0xBE401, 6, null), null, false, 0x9F114, 0x8C735)  },
                { 39,  new LocationData(39,  "Underground ruins, Drillable pillar room south chest",          LocationCategory.Container, levelDataDict[0x0906], itemDataDict[0x0265], new AddressData(0xBE402, 5, null), null, false, 0x9F4D4, 0x8C735)  },
                { 40,  new LocationData(40,  "Underground ruins, Drillable pillar room north chest",          LocationCategory.Container, levelDataDict[0x0906], itemDataDict[0x0217], new AddressData(0xBE402, 4, null), null, false, 0x9F894, 0x8C735)  },
                { 41,  new LocationData(41,  "Underground ruins, 3 chest room middle chest",                  LocationCategory.Container, levelDataDict[0x0906], itemDataDict[0x8230], new AddressData(0xBE401, 5, null), null, false, 0x9FC54, 0x8C759)  },
                { 42,  new LocationData(42,  "Underground ruins, Drillable pillars room south hole",          LocationCategory.Hole,      levelDataDict[0x0906], itemDataDict[0x805C], new AddressData(0xBE3DF, 5, null), null, false, null,    0x15390D) },
                { 43,  new LocationData(43,  "Underground ruins, Drillable pillars room west hole",           LocationCategory.Hole,      levelDataDict[0x0906], itemDataDict[0x8042], new AddressData(0xBE3DF, 0, null), null, false, null,    0x153BF4) },
                { 44,  new LocationData(44,  "Underground ruins, Drillable pillars room north hole",          LocationCategory.Hole,      levelDataDict[0x0906], itemDataDict[0x0272], new AddressData(0xBE3DE, 4, null), null, false, null,    0x153548) },
                { 45,  new LocationData(45,  "Underground ruins, Fireball Orudakoitan chest",                 LocationCategory.Container, levelDataDict[0x0907], itemDataDict[0x0259], new AddressData(0xBE403, 4, null), null, false, 0x9F114, 0x8C735)  },
                { 46,  new LocationData(46,  "Underground ruins, Box ledge chest",                            LocationCategory.Container, levelDataDict[0x0907], itemDataDict[0x80C4], new AddressData(0xBE403, 3, null), null, false, null,    0x8C759)  },
                { 47,  new LocationData(47,  "Underground ruins, Horokko ledge chest",                        LocationCategory.Container, levelDataDict[0x0907], itemDataDict[0x021D], new AddressData(0xBE403, 1, null), null, false, null,    0x8C735)  },
                { 48,  new LocationData(48,  "Underground ruins, Clozer exit chest",                          LocationCategory.Container, levelDataDict[0x0908], itemDataDict[0x80B2], new AddressData(0xBE403, 3, null), null, false, 0x9FC54, 0x8C735)  },
                { 49,  new LocationData(49,  "Underground ruins, Drillable wall room middle cliff chest",     LocationCategory.Container, levelDataDict[0x0908], itemDataDict[0x0267], new AddressData(0xBE401, 2, null), null, false, 0x9F4D4, 0x8C735)  },
                { 50,  new LocationData(50,  "Underground ruins, Drillable wall room west cliff chest",       LocationCategory.Container, levelDataDict[0x0908], itemDataDict[0x0274], new AddressData(0xBE400, 0, null), null, false, 0x9F114, 0x8C735)  },
                { 51,  new LocationData(51,  "Underground ruins, Drillable wall room east cliff chest",       LocationCategory.Container, levelDataDict[0x0908], itemDataDict[0x0212], new AddressData(0xBE401, 1, null), null, false, 0x9F894, 0x8C735)  },
                { 52,  new LocationData(52,  "Underground ruins, Trapped box hole",                           LocationCategory.Hole,      levelDataDict[0x0908], itemDataDict[0x8097], new AddressData(0xBE3DE, 0, null), null, false, null,    0x153762) },
                { 53,  new LocationData(53,  "Underground ruins, Drillable wall room east cliff hole",        LocationCategory.Hole,      levelDataDict[0x0908], itemDataDict[0x026A], new AddressData(0xBE3DF, 3, null), null, false, null,    0x153A49) },
                { 54,  new LocationData(54,  "Cardon Forest Sub-Gate, Sharukurusu floor hole",                LocationCategory.Hole,      levelDataDict[0x0E01], itemDataDict[0x80E6], new AddressData(0xBE3DC, 5, null), null, false, null,    0x153C41) },
                { 55,  new LocationData(55,  "Cardon Forest Sub-Gate, Cliff hole",                            LocationCategory.Hole,      levelDataDict[0x0E01], itemDataDict[0x024B], new AddressData(0xBE3DC, 7, null), null, false, null,    0x153B63) },
                { 56,  new LocationData(56,  "Cardon Forest Sub-Gate, Cliff chest",                           LocationCategory.Container, levelDataDict[0x0E01], itemDataDict[0x026C], new AddressData(0xBE400, 2, null), null, false, 0x9F114, 0x8C735)  },
                { 57,  new LocationData(57,  "Cardon Forest Sub-Gate, Bottom conveyor hole",                  LocationCategory.Hole,      levelDataDict[0x0E02], itemDataDict[0x801E], new AddressData(0xBE3DC, 4, null), null, false, null,    0x153CDF) },
                { 58,  new LocationData(58,  "Cardon Forest Sub-Gate, Middle conveyor hole",                  LocationCategory.Hole,      levelDataDict[0x0E02], itemDataDict[0x024D], new AddressData(0xBE3DC, 4, null), null, false, null,    0x153BD2) },
                { 59,  new LocationData(59,  "Cardon Forest Sub-Gate, Middle switch chest",                   LocationCategory.Container, levelDataDict[0x0E02], itemDataDict[0x025A], new AddressData(0xBE400, 3, null), null, false, 0x9F114, 0x8C735)  },
              //{ 60,  new LocationData(60,  "Cardon Forest Sub-Gate, Sharukurusu starter key get",           LocationCategory.Hole,      levelDataDict[0x0E01], itemDataDict[0x0230], new AddressData(0xBE439, 6, null), null, false, null,    null)     },
              //{ 61,  new LocationData(61,  "Cardon Forest Sub-Gate, Conveyor key get",                      LocationCategory.Hole,      levelDataDict[0x0E02], itemDataDict[0x022F], new AddressData(0xBE439, 5, null), null, false, null,    null)     },
              //{ 62,  new LocationData(62,  "Cardon Forest Sub-Gate, Conveyor key get",                      LocationCategory.Hole,      levelDataDict[0x0E00], itemDataDict[0x022E], new AddressData(0xBE439, 4, null), null, false, null,    null)     },
                { 63,  new LocationData(63,  "Lake Jyun Sub-Gate, Entrance right hole",                       LocationCategory.Hole,      levelDataDict[0x1400], itemDataDict[0x807C], new AddressData(0xBE3DC, 1, null), null, false, null,    0x153A7E) },
                { 64,  new LocationData(64,  "Lake Jyun Sub-Gate, Entrance left hole",                        LocationCategory.Hole,      levelDataDict[0x1400], itemDataDict[0x8016], new AddressData(0xBE3DD, 7, null), null, false, null,    0x153BBA) },
                { 65,  new LocationData(65,  "Lake Jyun Sub-Gate, Entrance chest",                            LocationCategory.Container, levelDataDict[0x1400], itemDataDict[0x026F], new AddressData(0xBE401, 7, null), null, false, 0x9F114, 0x8C735)  },
                { 66,  new LocationData(66,  "Lake Jyun Sub-Gate, East corridor hole",                        LocationCategory.Hole,      levelDataDict[0x1401], itemDataDict[0x0256], new AddressData(0xBE3DC, 2, null), null, false, null,    0x153A0F) },
                { 67,  new LocationData(67,  "Lake Jyun Sub-Gate, West corridor hole",                        LocationCategory.Hole,      levelDataDict[0x1401], itemDataDict[0x0277], new AddressData(0xBE3DC, 3, null), null, false, null,    0x1539A0) },
                { 68,  new LocationData(68,  "Lake Jyun Sub-Gate, West corridor chest",                       LocationCategory.Container, levelDataDict[0x1401], itemDataDict[0x0231], new AddressData(0xBE402, 3, null), null, false, 0x9F114, 0x8C735)  },
                { 69,  new LocationData(69,  "Lake Jyun Sub-Gate, Sharukurusu east chest",                    LocationCategory.Container, levelDataDict[0x1402], itemDataDict[0x0233], new AddressData(0xBE402, 1, null), null, false, 0x9F894, 0x8C735)  },
                { 70,  new LocationData(70,  "Lake Jyun Sub-Gate, Sharukurusu middle chest",                  LocationCategory.Container, levelDataDict[0x1402], itemDataDict[0x0270], new AddressData(0xBE402, 0, null), null, false, 0x9F114, 0x8C735)  },
                { 71,  new LocationData(71,  "Lake Jyun Sub-Gate, Sharukurusu west chest",                    LocationCategory.Container, levelDataDict[0x1402], itemDataDict[0x0232], new AddressData(0xBE402, 2, null), null, false, 0x9F4D4, 0x8C735)  },
                { 72,  new LocationData(72,  "Lake Jyun Sub-Gate, Sharukurusu west hole",                     LocationCategory.Hole,      levelDataDict[0x1402], itemDataDict[0x8201], new AddressData(0xBE3DC, 0, null), null, false, null,    0x153B1C) },
                { 73,  new LocationData(73,  "Closer Woods Sub-Gate, Sharukurusu E room left hole",           LocationCategory.Hole,      levelDataDict[0x1303], itemDataDict[0x027C], new AddressData(0xBE3DD, 6, null), null, false, null,    0x153ED6) },
                { 74,  new LocationData(74,  "Closer Woods Sub-Gate, Sharukurusu E room right hole",          LocationCategory.Hole,      levelDataDict[0x1303], itemDataDict[0x802D], new AddressData(0xBE3DD, 1, null), null, false, null,    0x15418E) },
                { 75,  new LocationData(75,  "Clozer woods Sub-Gate, Miroc+Gorubesshu west cliff chest",      LocationCategory.Container, levelDataDict[0x1305], itemDataDict[0x0263], new AddressData(0xBE401, 4, null), null, false, 0x9F114, 0x8C735)  },
                { 76,  new LocationData(76,  "Clozer woods Sub-Gate, Miroc+Gorubesshu east cliff chest",      LocationCategory.Container, levelDataDict[0x1305], itemDataDict[0x0234], new AddressData(0xBE403, 7, null), null, false, 0x9F4D4, 0x8C735)  },
                { 77,  new LocationData(77,  "Clozer woods Sub-Gate, Miroc+Gorubesshu southeast pillar hole", LocationCategory.Hole,      levelDataDict[0x1305], itemDataDict[0x80B8], new AddressData(0xBE3DD, 4, null), null, false, null,    0x153FB4) },
                { 78,  new LocationData(78,  "Clozer woods Sub-Gate, Miroc+Gorubesshu northeast pillar hole", LocationCategory.Hole,      levelDataDict[0x1305], itemDataDict[0x8078], new AddressData(0xBE3DD, 2, null), null, false, null,    0x1540F0) },
                { 79,  new LocationData(79,  "Clozer woods Sub-Gate, Miroc+Gorubesshu northwest pillar hole", LocationCategory.Hole,      levelDataDict[0x1305], itemDataDict[0x024E], new AddressData(0xBE3DD, 5, null), null, false, null,    0x153F45) },
                { 80,  new LocationData(80,  "Clozer woods Sub-Gate, Miroc+Gorubesshu southwest pillar hole", LocationCategory.Hole,      levelDataDict[0x1305], itemDataDict[0x805C], new AddressData(0xBE3DD, 3, null), null, false, null,    0x154052) },
                { 81,  new LocationData(81,  "Clozer Woods Sub-Gate, Generator room upper chest",             LocationCategory.Container, levelDataDict[0x1308], itemDataDict[0x0236], new AddressData(0xBE403, 5, null), null, false, 0x9F4D4, 0x8C735)  },
                { 82,  new LocationData(82,  "Clozer Woods Sub-Gate, Gorubesshu corridor east chest",         LocationCategory.Container, levelDataDict[0x1309], itemDataDict[0x027B], new AddressData(0xBE401, 3, null), null, false, 0x9F114, 0x8C735)  },
                { 83,  new LocationData(83,  "Clozer Woods Sub-Gate, Generator room lower chest",             LocationCategory.Container, levelDataDict[0x1308], itemDataDict[0x0235], new AddressData(0xBE403, 6, null), null, false, 0x9F114, 0x8C735)  },
                { 84,  new LocationData(84,  "Main Gate, Maze Chest",                                         LocationCategory.Container, levelDataDict[0x1A00], itemDataDict[0x0262], new AddressData(0xBE402, 7, null), null, false, 0x9F114, 0x8C735)  },
                { 85,  new LocationData(85,  "Main Gate, Maze entrance hole",                                 LocationCategory.Hole,      levelDataDict[0x1A00], itemDataDict[0x80D9], new AddressData(0xBE3DE, 6, null), null, false, null,    0x153D8F) },
                { 86,  new LocationData(86,  "Main Gate, Maze Karumuna Bash hole",                            LocationCategory.Hole,      levelDataDict[0x1A00], itemDataDict[0x0252], new AddressData(0xBE3DD, 0, null), null, false, null,    0x153C82) },
                { 87,  new LocationData(87,  "Main Gate, Maze Reaverbot hole",                                LocationCategory.Hole,      levelDataDict[0x1A00], itemDataDict[0x0279], new AddressData(0xBE3DE, 5, null), null, false, null,    0x153E2D) },
                { 88,  new LocationData(88,  "Main Gate, Two Gorubesshu room chest",                          LocationCategory.Container, levelDataDict[0x1A06], itemDataDict[0x0213], new AddressData(0xBE401, 0, null), null, false, 0x9F114, 0x8C735)  },
                { 89,  new LocationData(89,  "Main Gate, Entrance hole",                                      LocationCategory.Hole,      levelDataDict[0x1A07], itemDataDict[0x83E8], new AddressData(0xBE3DE, 7, null), null, false, null,    0x153CF1) },
                { 90,  new LocationData(90,  "Main Gate, Boss corridor chest",                                LocationCategory.Hole,      levelDataDict[0x1A01], itemDataDict[0x0211], new AddressData(0xBE404, 1, null), null, false, null,    0x8C735)  },
                { 91,  new LocationData(91,  "Old City Sub-City, Chest",                                      LocationCategory.Container, levelDataDict[0x1D03], itemDataDict[0x0237], new AddressData(0xBE404, 4, null), null, false, 0x9F114, 0x8C735)  },
                { 92,  new LocationData(92,  "Downtown Sub-City, Chest",                                      LocationCategory.Container, levelDataDict[0x1D04], itemDataDict[0x0238], new AddressData(0xBE404, 3, null), null, false, 0x9F114, 0x8C735)  },
                { 93,  new LocationData(93,  "Uptown Sub-City, Chest",                                        LocationCategory.Container, levelDataDict[0x1D05], itemDataDict[0x0239], new AddressData(0xBE404, 0, null), null, false, 0x9F114, 0x8C735)  },
                { 94,  new LocationData(94,  "Flutter, Study chest",                                          LocationCategory.Hole,      levelDataDict[0x1B05], itemDataDict[0x0261], new AddressData(0xBE3E1, 5, null), null, false, null,    0x15333F) },
                { 95,  new LocationData(95,  "Escape the Ocean Tower",                                        LocationCategory.Combat,    null,                  itemDataDict[0x00FF], new AddressData(0xBE3E8, 4, null), null, false, null,    null)     },
                { 96,  new LocationData(96,  "Ferdinand defeated",                                            LocationCategory.Combat,    null,                  itemDataDict[0x00FF], new AddressData(0xBE383, 6, null), null, false, null,    null)     },
                { 97,  new LocationData(97,  "Bon Bonne defeated",                                            LocationCategory.Combat,    null,                  itemDataDict[0x00FF], new AddressData(0xBE382, 6, null), null, false, null,    null)     },
                { 98,  new LocationData(98,  "Marlwolf defeated",                                             LocationCategory.Combat,    null,                  itemDataDict[0x00FF], new AddressData(0xBE37A, 7, null), null, false, null,    null)     },
                { 99,  new LocationData(99,  "Balkon Gerät defeated",                                         LocationCategory.Combat,    null,                  itemDataDict[0x00FF], new AddressData(0xBE37B, 2, null), null, false, null,    null)     },
                { 100,  new LocationData(100, "Garudoriten defeated",                                         LocationCategory.Combat,    null,                  itemDataDict[0x00FF], new AddressData(0xBE382, 7, null), null, false, null,    null)     },
                { 101,  new LocationData(101, "Karumuna Bash Trio defeated",                                  LocationCategory.Combat,    null,                  itemDataDict[0x00FF], new AddressData(0xBE382, 4, null), null, false, null,    null)     },
                { 102, new LocationData(102, "Focke-Wulf defeated",                                           LocationCategory.Combat,    null,                  itemDataDict[0x00FF], new AddressData(0xBE37D, 5, null), null, false, null,    null)     },
                { 103, new LocationData(103, "Theodore Bruno defeated",                                       LocationCategory.Combat,    null,                  itemDataDict[0x00FF], new AddressData(0xBE382, 5, null), null, false, null,    null)     },
                { 104, new LocationData(104, "Rescue the shop owner's husband",                               LocationCategory.Quest,     levelDataDict[0x0401], itemDataDict[0x0258], new AddressData(0xBE3D6, 4, null), null, false, null,    0x155C8C) },
                { 105, new LocationData(105, "Race Technical Course Rank A",                                  LocationCategory.Quest,     levelDataDict[0x0500], itemDataDict[0x024F], new AddressData(0xBE3B8, 4, null), null, false, null,    0x153BC3) },
                { 106, new LocationData(106, "Beast Hunter Rank A",                                           LocationCategory.Quest,     levelDataDict[0x0803], itemDataDict[0x0264], new AddressData(0xBE3DA, 3, null), null, false, null,    0x1535B3) },
                { 107, new LocationData(107, "Race Straight Course Rank A",                                   LocationCategory.Quest,     levelDataDict[0x0500], itemDataDict[0x024A], new AddressData(0xBE3B8, 6, null), null, false, null,    0x153BC3) },
                { 108, new LocationData(108, "Balloon Fantasy Rank A",                                        LocationCategory.Quest,     levelDataDict[0x0804], itemDataDict[0x025C], new AddressData(0xBE3DA, 2, null), null, false, null,    0x153CAB) },
                { 109, new LocationData(109, "Race Left Curve Course Rank A",                                 LocationCategory.Quest,     levelDataDict[0x0500], itemDataDict[0x0219], new AddressData(0xBE3B8, 5, null), null, false, null,    0x153BC3) },
                { 110, new LocationData(110, "Save the missing woman",                                        LocationCategory.Quest,     levelDataDict[0x0801], itemDataDict[0x0273], new AddressData(0xBE3BE, 7, null), null, false, null,    0x15332D) },
                { 111, new LocationData(111, "Cure Ira's illness",                                            LocationCategory.Quest,     levelDataDict[0x0805], itemDataDict[0x0278], new AddressData(0xBE427, 7, null), null, false, null,    0x154657) },
              //{ 112, new LocationData(112, "Tell painter she needs red"
              //{ 113, new LocationData(112, "Get lipstick"
                { 114, new LocationData(114, "Museum donation, Old Bone",                                     LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x00FF], new AddressData(0xBE37F, 7, null), null, false, null,    null)     },
                { 115, new LocationData(115, "Museum donation, Old Heater",                                   LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x00FF], new AddressData(0xBE37F, 6, null), null, false, null,    null)     },
                { 116, new LocationData(116, "Museum donation, Old Doll",                                     LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x00FF], new AddressData(0xBE37F, 5, null), null, false, null,    null)     },
                { 117, new LocationData(117, "Museum donation, Antique Bell",                                 LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x00FF], new AddressData(0xBE37F, 4, null), null, false, null,    null)     },
                { 118, new LocationData(118, "Museum donation, Giant Horn",                                   LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x00FF], new AddressData(0xBE37F, 3, null), null, false, null,    null)     },
                { 119, new LocationData(119, "Museum donation, Shiny Object",                                 LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x00FF], new AddressData(0xBE37F, 2, null), null, false, null,    null)     },
                { 120, new LocationData(120, "Museum donation, Old Shield",                                   LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x00FF], new AddressData(0xBE37F, 1, null), null, false, null,    null)     },
                { 121, new LocationData(121, "Museum donation, Shiny Red Stone",                              LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x00FF], new AddressData(0xBE37F, 0, null), null, false, null,    null)     },
                { 122, new LocationData(122, "Complete the Museum exhibit",                                   LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x00FF], new AddressData(0xBE37F, null, 1), null, false, null,    null)     },
                { 123, new LocationData(123, "Take dangerous object from museum visitor",                     LocationCategory.Quest,     levelDataDict[0x1C01], itemDataDict[0x026B], new AddressData(0xBE37D, 0, null), null, false, null,    0x155387) },
                { 124, new LocationData(124, "Gift Flower to Roll",                                           LocationCategory.Quest,     null,                  itemDataDict[0x00FF], new AddressData(0xBE3B4, 7, null), null, false, null,    null)     },
                { 125, new LocationData(125, "Gift Music Box to Roll",                                        LocationCategory.Quest,     null,                  itemDataDict[0x00FF], new AddressData(0xBE3B4, 6, null), null, false, null,    null)     },
                { 126, new LocationData(126, "Gift Ring to Roll",                                             LocationCategory.Quest,     null,                  itemDataDict[0x00FF], new AddressData(0xBE3B4, 5, null), null, false, null,    null)     }
            };
            return locationDataDict;
        }
        
        public static List<ILocation> BuildLocationList(Dictionary<string, object> options)
        {
            Dictionary<int, LocationData> locationDataDict = GetLocationDataDict();
            List<ILocation> locationList = [];
            foreach (LocationData locationData in locationDataDict.Values)
            {
                // TODO: Make locations depend on options
                if (locationData.CheckAddressData.BitNumber is not null) {
                    Location location = new()
                    {
                        Id = locationData.Id,
                        Name = locationData.Name,
                        Address = locationData.CheckAddressData.Address,
                        CheckType = LocationCheckType.Bit,
                        AddressBit = locationData.CheckAddressData.BitNumber ?? -1,
                        Category = locationData.Category.ToString()
                    };
                    locationList.Add(location);
                }
            }
            return locationList;
        }
    }
}
