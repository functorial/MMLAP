from __future__ import annotations
from BaseClasses import Item, ItemClassification
from typing import TYPE_CHECKING, NamedTuple

if TYPE_CHECKING:
    from .world import MMLWorld

class MMLItem(Item):
    game = "Mega Man Legends"

class MMLItemData(NamedTuple):
    id: int
    itemClassification: ItemClassification

itemDataDict = {
    "Nothing"                           : MMLItemData(0x00FF, ItemClassification.filler                                 ),
    "Power Raiser"                      : MMLItemData(0x020D, ItemClassification.filler                                 ),
    "Buster Max"                        : MMLItemData(0x0210, ItemClassification.useful                                 ), # strong buster part -> useful
    "Power Stream"                      : MMLItemData(0x0211, ItemClassification.useful                                 ), # strong buster part -> useful
    "Blaster Unit R"                    : MMLItemData(0x0212, ItemClassification.useful                                 ), # strong buster part -> useful
    "Buster Unit Omega"                 : MMLItemData(0x0213, ItemClassification.useful                                 ), # strong buster part -> useful
    "Rapid Striker"                     : MMLItemData(0x0217, ItemClassification.filler                                 ),
    "Omni-Unit"                         : MMLItemData(0x0219, ItemClassification.filler                                 ),
    "Triple Access"                     : MMLItemData(0x021D, ItemClassification.filler                                 ),
    "Buster Unit"                       : MMLItemData(0x021E, ItemClassification.filler                                 ),
    "Rapid Fire"                        : MMLItemData(0x021F, ItemClassification.filler                                 ),
  # "Cardon Forest Sub-Gate Key 1"      : MMLItemData(0x022E, ItemClassification.progression                            ), # progresses main story -> progression
  # "Cardon Forest Sub-Gate Key 2"      : MMLItemData(0x022F, ItemClassification.progression                            ), # progresses main story -> progression
  # "Cardon Forest Sub-Gate Key 3"      : MMLItemData(0x0230, ItemClassification.progression                            ), # progresses main story -> progression
    "Lake Jyun Sub-Gate Starter Key 1"  : MMLItemData(0x0231, ItemClassification.progression                            ), # progresses main story -> progression
    "Lake Jyun Sub-Gate Starter Key 2"  : MMLItemData(0x0232, ItemClassification.progression                            ), # progresses main story -> progression
    "Lake Jyun Sub-Gate Starter Key 3"  : MMLItemData(0x0233, ItemClassification.progression                            ), # progresses main story -> progression
    "Clozer Woods Sub-Gate Key 1"       : MMLItemData(0x0234, ItemClassification.progression                            ), # progresses main story -> progression
    "Clozer Woods Sub-Gate Key 2"       : MMLItemData(0x0235, ItemClassification.progression                            ), # progresses main story -> progression
    "Clozer Woods Sub-Gate Key 3"       : MMLItemData(0x0236, ItemClassification.progression                            ), # progresses main story -> progression
    "Watcher' Key"                      : MMLItemData(0x0237, ItemClassification.progression                            ), # progresses main story -> progression
    "Sleeper' Key"                      : MMLItemData(0x0238, ItemClassification.progression                            ), # progresses main story -> progression
    "Dreamer' Key"                      : MMLItemData(0x0239, ItemClassification.progression                            ), # progresses main story -> progression
    "Bag"                               : MMLItemData(0x0245, ItemClassification.filler                                 ), # SIDEQUESTS NOT IMPLEMENTED YET
    "Saw"                               : MMLItemData(0x0248, ItemClassification.filler                                 ), # SIDEQUESTS NOT IMPLEMENTED YET
    "Music Box"                         : MMLItemData(0x024A, ItemClassification.progression                            ), # give to roll -> progression
    "Old Bone"                          : MMLItemData(0x024B, ItemClassification.progression                            ), # progresses museum -> progression
  # "Old Heater"                        : MMLItemData(0x024C, ItemClassification.progression                            ), # progresses museum -> progression
    "Old Doll"                          : MMLItemData(0x024D, ItemClassification.progression                            ), # progresses museum -> progression
    "Antique Bell"                      : MMLItemData(0x024E, ItemClassification.progression                            ), # progresses museum -> progression
    "Giant Horn"                        : MMLItemData(0x024F, ItemClassification.progression                            ), # progresses museum -> progression
    "Shiny Object"                      : MMLItemData(0x0250, ItemClassification.progression                            ), # progresses museum -> progression
    "Old Shield"                        : MMLItemData(0x0251, ItemClassification.progression                            ), # progresses museum -> progression
    "Shiny Red Stone"                   : MMLItemData(0x0252, ItemClassification.progression                            ), # progresses museum -> progression
    "Ring"                              : MMLItemData(0x0256, ItemClassification.progression                            ), # give to roll -> progression
    "Mine Parts Kit"                    : MMLItemData(0x0258, ItemClassification.filler                                 ),
    "Cannon Kit"                        : MMLItemData(0x0259, ItemClassification.filler                                 ),
    "Grenade Kit"                       : MMLItemData(0x025A, ItemClassification.filler                                 ),
    "Blumebear Parts"                   : MMLItemData(0x025B, ItemClassification.useful                                 ), # strong special weapon -> useful
    "Mystic Orb"                        : MMLItemData(0x025C, ItemClassification.filler                                 ),
    "Broken Motor"                      : MMLItemData(0x025E, ItemClassification.filler                                 ),
    "Broken Propeller"                  : MMLItemData(0x025F, ItemClassification.filler                                 ),
    "Broken Cleaner"                    : MMLItemData(0x0260, ItemClassification.filler                                 ),
    "Bomb Schematic"                    : MMLItemData(0x0261, ItemClassification.progression                            ), # breaks walls -> progression
    "Blunted Drill"                     : MMLItemData(0x0262, ItemClassification.progression                            ), # breaks walls -> progression
    "Guidance Unit"                     : MMLItemData(0x0263, ItemClassification.useful                                 ), # strong special weapon -> useful
    "Zetsabre"                          : MMLItemData(0x0264, ItemClassification.filler                                 ),
    "Pen Light"                         : MMLItemData(0x0265, ItemClassification.filler                                 ),
    "Old Launcher"                      : MMLItemData(0x0266, ItemClassification.filler                                 ),
    "Ancient Book"                      : MMLItemData(0x0267, ItemClassification.filler                                 ),
    "Weapon Plans"                      : MMLItemData(0x026A, ItemClassification.useful                                 ),
    "Prism Crystal"                     : MMLItemData(0x026B, ItemClassification.useful                                 ),
    "Spring Set"                        : MMLItemData(0x026C, ItemClassification.useful | ItemClassification.progression), # powerup / convenience special items -> useful
    "Safety Helmet"                     : MMLItemData(0x026D, ItemClassification.useful                                 ), # powerup / convenience special items -> useful
    "Rollerboard"                       : MMLItemData(0x026E, ItemClassification.useful | ItemClassification.progression), # powerup / convenience special items -> useful
    "Old Hoverjets"                     : MMLItemData(0x026F, ItemClassification.useful | ItemClassification.progression), # powerup / convenience special items -> useful
    "Joint Plug"                        : MMLItemData(0x0270, ItemClassification.useful                                 ), # powerup / convenience special items -> useful
    "Main Core Shard"                   : MMLItemData(0x0272, ItemClassification.filler                                 ),
    "Sun-light"                         : MMLItemData(0x0273, ItemClassification.filler                                 ),
    "Rapidfire Barrel"                  : MMLItemData(0x0274, ItemClassification.filler                                 ),
    "Gatling Part"                      : MMLItemData(0x0277, ItemClassification.filler                                 ),
    "Flower Pearl"                      : MMLItemData(0x0278, ItemClassification.filler                                 ),
    "Autofire Barrel"                   : MMLItemData(0x0279, ItemClassification.filler                                 ),
    "Generator Part"                    : MMLItemData(0x027A, ItemClassification.filler                                 ),
    "Target Sensor"                     : MMLItemData(0x027B, ItemClassification.filler                                 ),
    "Tele-lens"                         : MMLItemData(0x027C, ItemClassification.filler                                 ),
    "10 Zenny"                          : MMLItemData(0x8001, ItemClassification.filler                                 ),
    "20 Zenny"                          : MMLItemData(0x8002, ItemClassification.filler                                 ),
    "30 Zenny"                          : MMLItemData(0x8003, ItemClassification.filler                                 ),
    "50 Zenny"                          : MMLItemData(0x8005, ItemClassification.filler                                 ),
    "100 Zenny"                         : MMLItemData(0x800A, ItemClassification.filler                                 ),
    "200 Zenny"                         : MMLItemData(0x8014, ItemClassification.filler                                 ),
    "220 Zenny"                         : MMLItemData(0x8016, ItemClassification.filler                                 ),
    "300 Zenny"                         : MMLItemData(0x801E, ItemClassification.filler                                 ),
    "450 Zenny"                         : MMLItemData(0x802D, ItemClassification.filler                                 ),
    "560 Zenny"                         : MMLItemData(0x8038, ItemClassification.filler                                 ),
    "660 Zenny"                         : MMLItemData(0x8042, ItemClassification.filler                                 ),
    "780 Zenny"                         : MMLItemData(0x804E, ItemClassification.filler                                 ),
    "820 Zenny"                         : MMLItemData(0x8052, ItemClassification.filler                                 ),
    "920 Zenny"                         : MMLItemData(0x805C, ItemClassification.filler                                 ),
    "1180 Zenny"                        : MMLItemData(0x8076, ItemClassification.filler                                 ),
    "1200 Zenny"                        : MMLItemData(0x8078, ItemClassification.filler                                 ),
    "1240 Zenny"                        : MMLItemData(0x807C, ItemClassification.filler                                 ),
    "1510 Zenny"                        : MMLItemData(0x8097, ItemClassification.filler                                 ),
    "1620 Zenny"                        : MMLItemData(0x80A2, ItemClassification.filler                                 ),
    "1780 Zenny"                        : MMLItemData(0x80B2, ItemClassification.filler                                 ),
    "1840 Zenny"                        : MMLItemData(0x80B8, ItemClassification.filler                                 ),
    "1960 Zenny"                        : MMLItemData(0x80C4, ItemClassification.filler                                 ),
    "2170 Zenny"                        : MMLItemData(0x80D9, ItemClassification.filler                                 ),
    "2280 Zenny"                        : MMLItemData(0x80E4, ItemClassification.filler                                 ),
    "2300 Zenny"                        : MMLItemData(0x80E6, ItemClassification.filler                                 ),
    "2600 Zenny"                        : MMLItemData(0x8104, ItemClassification.filler                                 ),
    "2840 Zenny"                        : MMLItemData(0x811C, ItemClassification.filler                                 ),
    "4520 Zenny"                        : MMLItemData(0x81C4, ItemClassification.filler                                 ),
    "5130 Zenny"                        : MMLItemData(0x8201, ItemClassification.filler                                 ),
    "5600 Zenny"                        : MMLItemData(0x8230, ItemClassification.filler                                 ),
    "9240 Zenny"                        : MMLItemData(0x839C, ItemClassification.useful                                 ), # Large amount of zenny -> useful
    "10000 Zenny"                       : MMLItemData(0x83E8, ItemClassification.useful                                 )  # Large amount of zenny -> useful
}

ITEM_NAME_TO_ID       = {itemName: itemDataDict[itemName].id                 for itemName in itemDataDict.keys()}
ITEM_NAME_TO_CATEGORY = {itemName: itemDataDict[itemName].itemClassification for itemName in itemDataDict.keys()}

def get_random_filler_item_name(world: MMLWorld) -> str:
    return "100 Zenny"

def create_item_with_correct_classification(world: MMLWorld, name: str) -> MMLItem:
    classification = ITEM_NAME_TO_CATEGORY[name]
    return MMLItem(name, classification, ITEM_NAME_TO_ID[name], world.player)

def create_all_items(world: MMLWorld) -> None:
    itemPool: list[MMLItem] = []
    for mmlItemData in itemDataDict:
        match mmlItemData.name:
            case "Nothing":
                for _ in range(5):
                    itemPool.append(world.create_item(mmlItemData.name))
            case "10 Zenny":
                for _ in range(2):
                    itemPool.append(world.create_item(mmlItemData.name))
            case "20 Zenny":
                for _ in range(2):
                    itemPool.append(world.create_item(mmlItemData.name))
            case "920 Zenny":
                for _ in range(2):
                    itemPool.append(world.create_item(mmlItemData.name))
            case "Buster Max":
                pass
            case _:
                itemPool.append(world.create_item(mmlItemData.name))

    number_of_items = len(itemPool)
    number_of_unfilled_locations = len(world.multiworld.get_unfilled_locations(world.player))
    needed_number_of_filler_items = number_of_unfilled_locations - number_of_items
    itemPool += [world.create_filler() for _ in range(needed_number_of_filler_items)]
    world.multiworld.itempool += itemPool
    return None
