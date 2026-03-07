from __future__ import annotations
from BaseClasses import Item, ItemClassification
from typing import TYPE_CHECKING, NamedTuple

if TYPE_CHECKING:
    from .world import GameWorld

class GameItem(Item):
    game = "Mega Man Legends"

class ItemData(NamedTuple):
    id: int
    itemClassification: ItemClassification

itemDataDict = {
    "Nothing"                           : ItemData(0x00FF, ItemClassification.filler                                  ),
    "Power Raiser"                      : ItemData(0x020D, ItemClassification.filler                                  ),
    "Buster Max"                        : ItemData(0x0210, ItemClassification.useful                                  ), # strong buster part -> useful
    "Power Stream"                      : ItemData(0x0211, ItemClassification.useful                                  ), # strong buster part -> useful
    "Blaster Unit R"                    : ItemData(0x0212, ItemClassification.useful                                  ), # strong buster part -> useful
    "Buster Unit Omega"                 : ItemData(0x0213, ItemClassification.useful                                  ), # strong buster part -> useful
    "Rapid Striker"                     : ItemData(0x0217, ItemClassification.filler                                  ),
    "Omni-Unit"                         : ItemData(0x0219, ItemClassification.filler                                  ),
    "Triple Access"                     : ItemData(0x021D, ItemClassification.filler                                  ),
    "Buster Unit"                       : ItemData(0x021E, ItemClassification.filler                                  ),
    "Rapid Fire"                        : ItemData(0x021F, ItemClassification.filler                                  ),
  # "Cardon Forest Sub-Gate Key 1"      : ItemData(0x022E, ItemClassification.progression                             ), # progresses main story -> progression
  # "Cardon Forest Sub-Gate Key 2"      : ItemData(0x022F, ItemClassification.progression                             ), # progresses main story -> progression
  # "Cardon Forest Sub-Gate Key 3"      : ItemData(0x0230, ItemClassification.progression                             ), # progresses main story -> progression
    "Lake Jyun Sub-Gate Key 1"  : ItemData(0x0231, ItemClassification.progression                             ), # progresses main story -> progression
    "Lake Jyun Sub-Gate Key 2"  : ItemData(0x0232, ItemClassification.progression                             ), # progresses main story -> progression
    "Lake Jyun Sub-Gate Key 3"  : ItemData(0x0233, ItemClassification.progression                             ), # progresses main story -> progression
    "Clozer Woods Sub-Gate Key 1"       : ItemData(0x0234, ItemClassification.progression                             ), # progresses main story -> progression
    "Clozer Woods Sub-Gate Key 2"       : ItemData(0x0235, ItemClassification.progression                             ), # progresses main story -> progression
    "Clozer Woods Sub-Gate Key 3"       : ItemData(0x0236, ItemClassification.progression                             ), # progresses main story -> progression
    "'Watcher' Key"                     : ItemData(0x0237, ItemClassification.progression                             ), # progresses main story -> progression
    "'Sleeper' Key"                     : ItemData(0x0238, ItemClassification.progression                             ), # progresses main story -> progression
    "'Dreamer' Key"                     : ItemData(0x0239, ItemClassification.progression                             ), # progresses main story -> progression
    "Flower"                            : ItemData(0x0244, ItemClassification.progression                             ), # LOCATION NOT IMPLEMENTED YET
    "Bag"                               : ItemData(0x0245, ItemClassification.filler                                  ), # SIDEQUESTS NOT IMPLEMENTED YET
    "Saw"                               : ItemData(0x0248, ItemClassification.progression                             ), # SIDEQUESTS NOT IMPLEMENTED YET
    "Music Box"                         : ItemData(0x024A, ItemClassification.progression                             ), # give to roll -> progression
    "Old Bone"                          : ItemData(0x024B, ItemClassification.progression                             ), # progresses museum -> progression
  # "Old Heater"                        : ItemData(0x024C, ItemClassification.progression                             ), # progresses museum -> progression
    "Old Doll"                          : ItemData(0x024D, ItemClassification.progression                             ), # progresses museum -> progression
    "Antique Bell"                      : ItemData(0x024E, ItemClassification.progression                             ), # progresses museum -> progression
    "Giant Horn"                        : ItemData(0x024F, ItemClassification.progression                             ), # progresses museum -> progression
    "Shiny Object"                      : ItemData(0x0250, ItemClassification.progression                             ), # progresses museum -> progression
    "Old Shield"                        : ItemData(0x0251, ItemClassification.progression                             ), # progresses museum -> progression
    "Shiny Red Stone"                   : ItemData(0x0252, ItemClassification.progression                             ), # progresses museum -> progression
    "Stag Beetle"                       : ItemData(0x0253, ItemClassification.progression                             ), # LOCATION NOT IMPLEMENTED YET
    "Beetle"                            : ItemData(0x0254, ItemClassification.progression                             ), # LOCATION NOT IMPLEMENTED YET
    "Comic Book"                        : ItemData(0x0255, ItemClassification.progression                             ), # LOCATION NOT IMPLEMENTED YET
    "Ring"                              : ItemData(0x0256, ItemClassification.progression                             ), # give to roll -> progression
   #"Mine Parts Kit"                    : ItemData(0x0258, ItemClassification.filler                                  ),
    "Cannon Kit"                        : ItemData(0x0259, ItemClassification.progression                             ),
    "Grenade Kit"                       : ItemData(0x025A, ItemClassification.filler                                  ),
    "Blumebear Parts"                   : ItemData(0x025B, ItemClassification.useful                                  ), # strong special weapon -> useful
    "Mystic Orb"                        : ItemData(0x025C, ItemClassification.filler                                  ),
    "Broken Motor"                      : ItemData(0x025E, ItemClassification.filler                                  ),
    "Broken Propeller"                  : ItemData(0x025F, ItemClassification.filler                                  ),
    "Broken Cleaner"                    : ItemData(0x0260, ItemClassification.filler                                  ),
    "Bomb Schematic"                    : ItemData(0x0261, ItemClassification.progression                             ), # breaks walls -> progression
    "Blunted Drill"                     : ItemData(0x0262, ItemClassification.progression                             ), # breaks walls -> progression
    "Guidance Unit"                     : ItemData(0x0263, ItemClassification.useful | ItemClassification.progression ), # strong special weapon -> useful
    "Zetsabre"                          : ItemData(0x0264, ItemClassification.filler                                  ),
    "Pen Light"                         : ItemData(0x0265, ItemClassification.filler                                  ),
    "Old Launcher"                      : ItemData(0x0266, ItemClassification.progression                             ),
    "Ancient Book"                      : ItemData(0x0267, ItemClassification.progression                             ),
    "Weapon Plans"                      : ItemData(0x026A, ItemClassification.useful                                  ),
    "Prism Crystal"                     : ItemData(0x026B, ItemClassification.useful                                  ),
    "Spring Set"                        : ItemData(0x026C, ItemClassification.useful | ItemClassification.progression ), # powerup / convenience special items -> useful
    "Safety Helmet"                     : ItemData(0x026D, ItemClassification.useful                                  ), # powerup / convenience special items -> useful
    "Rollerboard"                       : ItemData(0x026E, ItemClassification.useful | ItemClassification.progression ), # powerup / convenience special items -> useful
    "Old Hoverjets"                     : ItemData(0x026F, ItemClassification.useful | ItemClassification.progression ), # powerup / convenience special items -> useful
    "Joint Plug"                        : ItemData(0x0270, ItemClassification.useful                                  ), # powerup / convenience special items -> useful
    "Main Core Shard"                   : ItemData(0x0272, ItemClassification.filler                                  ),
    "Sun-light"                         : ItemData(0x0273, ItemClassification.filler                                  ),
    "Rapidfire Barrel"                  : ItemData(0x0274, ItemClassification.filler                                  ),
    "Gatling Part"                      : ItemData(0x0277, ItemClassification.filler                                  ),
    "Flower Pearl"                      : ItemData(0x0278, ItemClassification.filler                                  ),
    "Autofire Barrel"                   : ItemData(0x0279, ItemClassification.filler                                  ),
    "Generator Part"                    : ItemData(0x027A, ItemClassification.filler                                  ),
    "Target Sensor"                     : ItemData(0x027B, ItemClassification.filler                                  ),
    "Tele-lens"                         : ItemData(0x027C, ItemClassification.filler                                  ),
    "10 Zenny"                          : ItemData(0x8001, ItemClassification.filler                                  ),
    "20 Zenny"                          : ItemData(0x8002, ItemClassification.filler                                  ),
    "30 Zenny"                          : ItemData(0x8003, ItemClassification.filler                                  ),
    "50 Zenny"                          : ItemData(0x8005, ItemClassification.filler                                  ),
    "100 Zenny"                         : ItemData(0x800A, ItemClassification.filler                                  ),
    "200 Zenny"                         : ItemData(0x8014, ItemClassification.filler                                  ),
    "220 Zenny"                         : ItemData(0x8016, ItemClassification.filler                                  ),
    "300 Zenny"                         : ItemData(0x801E, ItemClassification.filler                                  ),
    "450 Zenny"                         : ItemData(0x802D, ItemClassification.filler                                  ),
    "560 Zenny"                         : ItemData(0x8038, ItemClassification.filler                                  ),
    "660 Zenny"                         : ItemData(0x8042, ItemClassification.filler                                  ),
    "780 Zenny"                         : ItemData(0x804E, ItemClassification.filler                                  ),
    "820 Zenny"                         : ItemData(0x8052, ItemClassification.filler                                  ),
    "920 Zenny"                         : ItemData(0x805C, ItemClassification.filler                                  ),
    "1180 Zenny"                        : ItemData(0x8076, ItemClassification.filler                                  ),
    "1200 Zenny"                        : ItemData(0x8078, ItemClassification.filler                                  ),
    "1240 Zenny"                        : ItemData(0x807C, ItemClassification.filler                                  ),
    "1510 Zenny"                        : ItemData(0x8097, ItemClassification.filler                                  ),
    "1620 Zenny"                        : ItemData(0x80A2, ItemClassification.filler                                  ),
    "1780 Zenny"                        : ItemData(0x80B2, ItemClassification.filler                                  ),
    "1840 Zenny"                        : ItemData(0x80B8, ItemClassification.filler                                  ),
    "1960 Zenny"                        : ItemData(0x80C4, ItemClassification.filler                                  ),
    "2170 Zenny"                        : ItemData(0x80D9, ItemClassification.filler                                  ),
    "2280 Zenny"                        : ItemData(0x80E4, ItemClassification.filler                                  ),
    "2300 Zenny"                        : ItemData(0x80E6, ItemClassification.filler                                  ),
    "2600 Zenny"                        : ItemData(0x8104, ItemClassification.filler                                  ),
    "2840 Zenny"                        : ItemData(0x811C, ItemClassification.filler                                  ),
    "4520 Zenny"                        : ItemData(0x81C4, ItemClassification.filler                                  ),
    "5130 Zenny"                        : ItemData(0x8201, ItemClassification.filler                                  ),
    "5600 Zenny"                        : ItemData(0x8230, ItemClassification.filler                                  ),
    "9240 Zenny"                        : ItemData(0x839C, ItemClassification.useful                                  ), # Large amount of zenny -> useful
    "10000 Zenny"                       : ItemData(0x83E8, ItemClassification.useful                                  )  # Large amount of zenny -> useful
}

ITEM_NAME_TO_ID       = {itemName: itemDataDict[itemName].id                 for itemName in itemDataDict.keys()}
ITEM_NAME_TO_CATEGORY = {itemName: itemDataDict[itemName].itemClassification for itemName in itemDataDict.keys()}

def get_random_filler_item_name(world: GameWorld) -> str:
    return "100 Zenny"

def create_item_with_correct_classification(world: GameWorld, name: str) -> GameItem:
    classification = ITEM_NAME_TO_CATEGORY[name]
    return GameItem(name, classification, ITEM_NAME_TO_ID[name], world.player)

def create_all_items(world: GameWorld) -> None:
    itemPool: list[GameItem] = []
    
    # Get filler items 
    locked_counts: dict[str, int] = {}
    for name in getattr(world, "locked_missable_filler_names", []):
        locked_counts[name] = locked_counts.get(name, 0) + 1

    # Assign quantities to items
    for itemName in itemDataDict.keys():
        add_count = 1
        match itemName:
            case "10 Zenny":
                # Reduced count of some filler items to temporarily add Flower, Comic Book, Beetle, and Stag Beetle
                # TODO: Revert when locations are randomized.
                #for _ in range(2):
                #    itemPool.append(world.create_item(itemName))
                    add_count = 0
            case "20 Zenny":
                #for _ in range(2):
                #    itemPool.append(world.create_item(itemName))
                    add_count = 0
            case "920 Zenny":
                for _ in range(2):
                    itemPool.append(world.create_item(itemName))
                    add_count = 2
            case "Nothing":
               #for _ in range(5):
               #    itemPool.append(world.create_item(itemName))
                add_count = 0
            case "Buster Max":
                add_count = 0
            case _:
                add_count = 1

        # Handle quantities for locked (missable) location items 
        locked_for_name = locked_counts.get(itemName, 0)
        to_add = add_count - locked_for_name
        if to_add < 0:
            to_add = 0
        locked_counts[itemName] = max(0, locked_for_name - add_count)
        for _ in range(to_add):
            itemPool.append(world.create_item(itemName))

    number_of_items = len(itemPool)
    number_of_unfilled_locations = len(world.multiworld.get_unfilled_locations(world.player))
    needed_number_of_filler_items = number_of_unfilled_locations - number_of_items
    itemPool += [world.create_filler() for _ in range(needed_number_of_filler_items)]
    world.multiworld.itempool += itemPool
    return None
