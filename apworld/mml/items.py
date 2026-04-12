from __future__ import annotations
from BaseClasses import Item, ItemClassification
from itertools import groupby
from typing import TYPE_CHECKING, NamedTuple

if TYPE_CHECKING:
    from .world import GameWorld

class GameItem(Item):
    game = "Mega Man Legends"

class ItemData(NamedTuple):
    id: int
    itemClassification: ItemClassification
    itemNameGroup: str

ITEM_DATA_DICT = {
    "Nothing"                           : ItemData(0x00FF, ItemClassification.filler,                                  None                        ),
    "Power Raiser"                      : ItemData(0x020D, ItemClassification.filler,                                  None                        ),
    "Buster Max"                        : ItemData(0x0210, ItemClassification.useful,                                  None                        ), # strong buster part -> useful
    "Power Stream"                      : ItemData(0x0211, ItemClassification.useful,                                  None                        ), # strong buster part -> useful
    "Blaster Unit R"                    : ItemData(0x0212, ItemClassification.useful,                                  None                        ), # strong buster part -> useful
    "Buster Unit Omega"                 : ItemData(0x0213, ItemClassification.useful,                                  None                        ), # strong buster part -> useful
    "Rapid Striker"                     : ItemData(0x0217, ItemClassification.filler,                                  None                        ),
    "Omni-Unit"                         : ItemData(0x0219, ItemClassification.filler,                                  None                        ),
    "Triple Access"                     : ItemData(0x021D, ItemClassification.filler,                                  None                        ),
    "Buster Unit"                       : ItemData(0x021E, ItemClassification.filler,                                  None                        ),
    "Rapid Fire"                        : ItemData(0x021F, ItemClassification.filler,                                  None                        ),
    "Yellow Refractor"                  : ItemData(0x0228, ItemClassification.progression,                             None                        ),
    "Red Refractor"                     : ItemData(0x0229, ItemClassification.progression,                             None                        ),
    "Citizen's Card"                    : ItemData(0x022A, ItemClassification.progression,                             None                        ),
    "Class A License"                   : ItemData(0x022B, ItemClassification.progression,                             None                        ),
    "Class B License"                   : ItemData(0x022C, ItemClassification.progression,                             None                        ),
    "Cardon Forest Sub-Gate Key 1"      : ItemData(0x022E, ItemClassification.progression,                             "Cardon Forest Sub-Gate Key"),
    "Cardon Forest Sub-Gate Key 2"      : ItemData(0x022F, ItemClassification.progression,                             "Cardon Forest Sub-Gate Key"),
    "Cardon Forest Sub-Gate Key 3"      : ItemData(0x0230, ItemClassification.progression,                             "Cardon Forest Sub-Gate Key"),
    "Lake Jyun Sub-Gate Key 1"          : ItemData(0x0231, ItemClassification.progression,                             "Lake Jyun Sub-Gate Key"    ),
    "Lake Jyun Sub-Gate Key 2"          : ItemData(0x0232, ItemClassification.progression,                             "Lake Jyun Sub-Gate Key"    ),
    "Lake Jyun Sub-Gate Key 3"          : ItemData(0x0233, ItemClassification.progression,                             "Lake Jyun Sub-Gate Key"    ),
    "Clozer Woods Sub-Gate Key 1"       : ItemData(0x0234, ItemClassification.progression,                             "Clozer Woods Sub-Gate Key" ),
    "Clozer Woods Sub-Gate Key 2"       : ItemData(0x0235, ItemClassification.progression,                             "Clozer Woods Sub-Gate Key" ),
    "Clozer Woods Sub-Gate Key 3"       : ItemData(0x0236, ItemClassification.progression,                             "Clozer Woods Sub-Gate Key" ),
    "'Watcher' Key"                     : ItemData(0x0237, ItemClassification.progression,                             "Sub-City Key"              ),
    "'Sleeper' Key"                     : ItemData(0x0238, ItemClassification.progression,                             "Sub-City Key"              ),
    "'Dreamer' Key"                     : ItemData(0x0239, ItemClassification.progression,                             "Sub-City Key"              ),
   #"Flower"                            : ItemData(0x0244, ItemClassification.progression,                             None                        ), # LOCATION NOT IMPLEMENTED YET
    "Bag"                               : ItemData(0x0245, ItemClassification.progression,                             None                        ), # Bag is in the item pool, associated pail is a location
    "Pick"                              : ItemData(0x0247, ItemClassification.progression,                             None,                       ), # Pick is in the item pool but there is no location from talking to worker due to issues with getting Saw early
    "Saw"                               : ItemData(0x0248, ItemClassification.progression,                             None                        ), # Saw is in the item pool, associated pail is a location
    "Music Box"                         : ItemData(0x024A, ItemClassification.progression,                             None                        ),
    "Old Bone"                          : ItemData(0x024B, ItemClassification.progression,                             "Museum"                    ),
  # "Old Heater"                        : ItemData(0x024C, ItemClassification.progression,                             "Museum"                    ),
    "Old Doll"                          : ItemData(0x024D, ItemClassification.progression,                             "Museum"                    ),
    "Antique Bell"                      : ItemData(0x024E, ItemClassification.progression,                             "Museum"                    ),
    "Giant Horn"                        : ItemData(0x024F, ItemClassification.progression,                             "Museum"                    ),
    "Shiny Object"                      : ItemData(0x0250, ItemClassification.progression,                             "Museum"                    ),
    "Old Shield"                        : ItemData(0x0251, ItemClassification.progression,                             "Museum"                    ),
    "Shiny Red Stone"                   : ItemData(0x0252, ItemClassification.progression,                             "Museum"                    ),
   #"Stag Beetle"                       : ItemData(0x0253, ItemClassification.progression,                             None                        ), # LOCATION NOT IMPLEMENTED YET
   #"Beetle"                            : ItemData(0x0254, ItemClassification.progression,                             None                        ), # LOCATION NOT IMPLEMENTED YET
   #"Comic Book"                        : ItemData(0x0255, ItemClassification.progression,                             None                        ), # LOCATION NOT IMPLEMENTED YET
    "Ring"                              : ItemData(0x0256, ItemClassification.progression,                             None                        ),
    "Mine Parts Kit"                    : ItemData(0x0258, ItemClassification.filler,                                  None                        ),
    "Cannon Kit"                        : ItemData(0x0259, ItemClassification.progression,                             None                        ),
    "Grenade Kit"                       : ItemData(0x025A, ItemClassification.filler,                                  None                        ),
    "Blumebear Parts"                   : ItemData(0x025B, ItemClassification.useful,                                  None                        ), # strong special weapon -> useful
    "Mystic Orb"                        : ItemData(0x025C, ItemClassification.filler,                                  None                        ),
    "Broken Motor"                      : ItemData(0x025E, ItemClassification.filler,                                  None                        ),
    "Broken Propeller"                  : ItemData(0x025F, ItemClassification.filler,                                  None                        ),
    "Broken Cleaner"                    : ItemData(0x0260, ItemClassification.filler,                                  None                        ),
    "Bomb Schematic"                    : ItemData(0x0261, ItemClassification.progression,                             None                        ),
    "Blunted Drill"                     : ItemData(0x0262, ItemClassification.progression,                             None                        ),
    "Guidance Unit"                     : ItemData(0x0263, ItemClassification.useful | ItemClassification.progression, None                        ), # strong special weapon -> useful
    "Zetsabre"                          : ItemData(0x0264, ItemClassification.filler,                                  None                        ),
    "Pen Light"                         : ItemData(0x0265, ItemClassification.filler,                                  None                        ),
    "Old Launcher"                      : ItemData(0x0266, ItemClassification.progression,                             None                        ),
    "Ancient Book"                      : ItemData(0x0267, ItemClassification.progression,                             None                        ),
    "Arm Supporter"                     : ItemData(0x0268, ItemClassification.progression,                             None                        ),
    "Weapon Plans"                      : ItemData(0x026A, ItemClassification.useful,                                  None                        ), # strong special weapon -> useful
    "Prism Crystal"                     : ItemData(0x026B, ItemClassification.useful,                                  None                        ), # strong special weapon -> useful
    "Spring Set"                        : ItemData(0x026C, ItemClassification.useful | ItemClassification.progression, None                        ), # strong special items -> useful
    "Safety Helmet"                     : ItemData(0x026D, ItemClassification.useful,                                  None                        ), # strong special items -> useful
    "Rollerboard"                       : ItemData(0x026E, ItemClassification.useful | ItemClassification.progression, None                        ), # strong special items -> useful
    "Old Hoverjets"                     : ItemData(0x026F, ItemClassification.useful | ItemClassification.progression, None                        ), # strong special items -> useful
    "Joint Plug"                        : ItemData(0x0270, ItemClassification.useful,                                  None                        ), # strong special items -> useful
    "Main Core Shard"                   : ItemData(0x0272, ItemClassification.filler,                                  None                        ),
    "Sun-light"                         : ItemData(0x0273, ItemClassification.filler,                                  None                        ),
    "Rapidfire Barrel"                  : ItemData(0x0274, ItemClassification.filler,                                  None                        ),
    "Gatling Part"                      : ItemData(0x0277, ItemClassification.filler,                                  None                        ),
    "Flower Pearl"                      : ItemData(0x0278, ItemClassification.filler,                                  None                        ),
    "Autofire Barrel"                   : ItemData(0x0279, ItemClassification.filler,                                  None                        ),
    "Generator Part"                    : ItemData(0x027A, ItemClassification.filler,                                  None                        ),
    "Target Sensor"                     : ItemData(0x027B, ItemClassification.filler,                                  None                        ),
    "Tele-lens"                         : ItemData(0x027C, ItemClassification.filler,                                  None                        ),
    "10 Zenny"                          : ItemData(0x8001, ItemClassification.filler,                                  None                        ),
    "20 Zenny"                          : ItemData(0x8002, ItemClassification.filler,                                  None                        ),
    "30 Zenny"                          : ItemData(0x8003, ItemClassification.filler,                                  None                        ),
    "50 Zenny"                          : ItemData(0x8005, ItemClassification.filler,                                  None                        ),
    "100 Zenny"                         : ItemData(0x800A, ItemClassification.filler,                                  None                        ),
    "200 Zenny"                         : ItemData(0x8014, ItemClassification.filler,                                  None                        ),
    "220 Zenny"                         : ItemData(0x8016, ItemClassification.filler,                                  None                        ),
    "300 Zenny"                         : ItemData(0x801E, ItemClassification.filler,                                  None                        ),
    "450 Zenny"                         : ItemData(0x802D, ItemClassification.filler,                                  None                        ),
    "560 Zenny"                         : ItemData(0x8038, ItemClassification.filler,                                  None                        ),
    "660 Zenny"                         : ItemData(0x8042, ItemClassification.filler,                                  None                        ),
    "780 Zenny"                         : ItemData(0x804E, ItemClassification.filler,                                  None                        ),
    "800 Zenny"                         : ItemData(0x8050, ItemClassification.filler,                                  None                        ),
    "820 Zenny"                         : ItemData(0x8052, ItemClassification.filler,                                  None                        ),
    "920 Zenny"                         : ItemData(0x805C, ItemClassification.filler,                                  None                        ),
    "1180 Zenny"                        : ItemData(0x8076, ItemClassification.filler,                                  None                        ),
    "1200 Zenny"                        : ItemData(0x8078, ItemClassification.filler,                                  None                        ),
    "1240 Zenny"                        : ItemData(0x807C, ItemClassification.filler,                                  None                        ),
    "1510 Zenny"                        : ItemData(0x8097, ItemClassification.filler,                                  None                        ),
    "1620 Zenny"                        : ItemData(0x80A2, ItemClassification.filler,                                  None                        ),
    "1780 Zenny"                        : ItemData(0x80B2, ItemClassification.filler,                                  None                        ),
    "1840 Zenny"                        : ItemData(0x80B8, ItemClassification.filler,                                  None                        ),
    "1960 Zenny"                        : ItemData(0x80C4, ItemClassification.filler,                                  None                        ),
    "2170 Zenny"                        : ItemData(0x80D9, ItemClassification.filler,                                  None                        ),
    "2280 Zenny"                        : ItemData(0x80E4, ItemClassification.filler,                                  None                        ),
    "2300 Zenny"                        : ItemData(0x80E6, ItemClassification.filler,                                  None                        ),
    "2600 Zenny"                        : ItemData(0x8104, ItemClassification.filler,                                  None                        ),
    "2840 Zenny"                        : ItemData(0x811C, ItemClassification.filler,                                  None                        ),
    "4520 Zenny"                        : ItemData(0x81C4, ItemClassification.filler,                                  None                        ),
    "5130 Zenny"                        : ItemData(0x8201, ItemClassification.filler,                                  None                        ),
    "5600 Zenny"                        : ItemData(0x8230, ItemClassification.filler,                                  None                        ),
    "9240 Zenny"                        : ItemData(0x839C, ItemClassification.useful,                                  None                        ), # Large amount of zenny -> useful
    "10000 Zenny"                       : ItemData(0x83E8, ItemClassification.useful,                                  None                        ), # Large amount of zenny -> useful
}

ITEM_NAME_TO_ID       = {itemName: ITEM_DATA_DICT[itemName].id                 for itemName in ITEM_DATA_DICT.keys()}
ITEM_NAME_TO_CATEGORY = {itemName: ITEM_DATA_DICT[itemName].itemClassification for itemName in ITEM_DATA_DICT.keys()}
ITEM_NAME_TO_GROUP    = {itemName: ITEM_DATA_DICT[itemName].itemNameGroup      for itemName in ITEM_DATA_DICT.keys()}

# https://www.geeksforgeeks.org/python/python-grouping-dictionary-keys-by-value/
ITEM_NAME_GROUPS = {
    itemNameGroup: [itemName for (itemName, _) in itemNameIterable]
    for itemNameGroup, itemNameIterable in groupby(
        sorted(
            [(itemName, itemNameGroup) for itemName, itemNameGroup in ITEM_NAME_TO_GROUP.items() if itemNameGroup is not None],
            key=lambda x: x[1]
        ),
        key=lambda x: x[1]
    )
}

def get_random_filler_item_name(world: GameWorld) -> str:
    return "100 Zenny"

def create_item_with_correct_classification(world: GameWorld, name: str) -> GameItem:
    classification = ITEM_NAME_TO_CATEGORY[name]
    return GameItem(name, classification, ITEM_NAME_TO_ID[name], world.player)

def create_all_items(world: GameWorld) -> None:
    itemPool: list[GameItem] = []
    
    # Get filler items from locations.lock_missables_to_filler(world)
    locked_counts: dict[str, int] = {}
    for locked_item_name in getattr(world, "locked_missable_filler_names", []):
        locked_counts[locked_item_name] = locked_counts.get(locked_item_name, 0) + 1

    # Assign quantities to all items
    for item_name in ITEM_DATA_DICT.keys():
        add_count = 1
        match item_name:
            case "10 Zenny":
                add_count = 2
            case "20 Zenny":
                add_count = 2
            case "920 Zenny":
                add_count = 2
            case "Nothing":
                add_count = 0 # 4
            case "Buster Max":
                add_count = 0
            case _:
                add_count = 1

        # Subtract from quantity those that were already placed into missable locations
        item_name_locked_count = locked_counts.get(item_name, 0)
        if ITEM_NAME_TO_CATEGORY.get(item_name) != ItemClassification.filler:
            item_name_locked_count = 0
        quantity_to_add = add_count - item_name_locked_count
        if quantity_to_add < 0:
            quantity_to_add = 0
        for _ in range(quantity_to_add):
            itemPool.append(world.create_item(item_name))

    number_of_items = len(itemPool)
    number_of_unfilled_locations = len(world.multiworld.get_unfilled_locations(world.player))
    needed_number_of_filler_items = number_of_unfilled_locations - number_of_items
    itemPool += [world.create_filler() for _ in range(needed_number_of_filler_items)]
    world.multiworld.itempool += itemPool
    return None