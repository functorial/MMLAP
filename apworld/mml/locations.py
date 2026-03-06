from __future__ import annotations
from .regions import get_regionDataDict
from . import items
from BaseClasses import Location, ItemClassification
from enum import IntEnum
from typing import TYPE_CHECKING, NamedTuple

if TYPE_CHECKING:
    from .world import GameWorld

class LocationCategory(IntEnum):
    CONTAINER = 0
    QUEST = 1
    COMBAT = 2

class GameLocation(Location):
    game = "Mega Man Legends"

class LocationData(NamedTuple):
    id: str
    category: LocationCategory
    isMissable: bool

locationDataDict = {
    "Ocean tower, Right chest"                                      : LocationData(1,   LocationCategory.CONTAINER,  True),
    "Ocean tower, Left chest"                                       : LocationData(2,   LocationCategory.CONTAINER,  True),
    "Apple market, Electric goods box"                              : LocationData(3,   LocationCategory.CONTAINER, False),
   #"Apple market, Book store box"                                  : LocationData(4,   LocationCategory.CONTAINER, False),
    "Apple market, Junk store box"                                  : LocationData(5,   LocationCategory.CONTAINER, False),
    "Apple market, North pail"                                      : LocationData(6,   LocationCategory.CONTAINER, False),
    "Apple market, South pail"                                      : LocationData(7,   LocationCategory.CONTAINER, False),
    "Downtown, South east peace sign pail"                          : LocationData(8,   LocationCategory.CONTAINER, False),
    "Downtown, Center east pail"                                    : LocationData(9,   LocationCategory.CONTAINER, False),
    "Downtown, Don't kick us pail"                                  : LocationData(10,  LocationCategory.CONTAINER, False),
    "Downtown, Center pail"                                         : LocationData(11,  LocationCategory.CONTAINER, False),
    "Downtown, Library pail"                                        : LocationData(12,  LocationCategory.CONTAINER, False),
    "Uptown, Hospital right pail"                                   : LocationData(13,  LocationCategory.CONTAINER, False),
   #"Uptown, Hospital left pail"                                    : LocationData(14,  LocationCategory.CONTAINER, False),
    "Uptown, Ocean corner pail"                                     : LocationData(15,  LocationCategory.CONTAINER, False),
    "Wily's Boat, Right box"                                        : LocationData(16,  LocationCategory.CONTAINER, False),
   #"Wily's Boat, Left box"                                         : LocationData(17,  LocationCategory.CONTAINER, False),
    "Wily's Boat, Pail"                                             : LocationData(18,  LocationCategory.CONTAINER, False),
    "Yass plains, Plateau house box"                                : LocationData(19,  LocationCategory.CONTAINER, False),
   #"Yass plains, Plateau house pail"                               : LocationData(20,  LocationCategory.CONTAINER, False),
    "Yass plains, Behind hideout pail"                              : LocationData(21,  LocationCategory.CONTAINER, False),
   #"Yass plains, Across hideout pail"                              : LocationData(22,  LocationCategory.CONTAINER, False),
    "Underground ruins, Junk store man chest"                       : LocationData(23,  LocationCategory.CONTAINER, False),
    "Underground ruins, Junk store man hole"                        : LocationData(24,  LocationCategory.CONTAINER, False),
    "Underground ruins, Main gate entrance chest"                   : LocationData(25,  LocationCategory.CONTAINER, False),
    "Underground ruins, 2 box ledge chest"                          : LocationData(26,  LocationCategory.CONTAINER, False),
    "Underground ruins, Miroc room ledge chest"                     : LocationData(27,  LocationCategory.CONTAINER, False),
    "Underground ruins, Cross room chest"                           : LocationData(28,  LocationCategory.CONTAINER, False),
    "Underground ruins, Miroc room left hole"                       : LocationData(29,  LocationCategory.CONTAINER, False),
    "Underground ruins, Miroc room right hole"                      : LocationData(30,  LocationCategory.CONTAINER, False),
    "Underground ruins, Arukoitan battle north chest"               : LocationData(31,  LocationCategory.CONTAINER, False),
    "Underground ruins, Arukoitan battle south chest"               : LocationData(32,  LocationCategory.CONTAINER, False),
    "Underground ruins, Obstacle room cliff east hole"              : LocationData(33,  LocationCategory.CONTAINER, False),
    "Underground ruins, Obstacle room cliff west hole"              : LocationData(34,  LocationCategory.CONTAINER, False),
    "Underground ruins, Shekuten pillar room chest"                 : LocationData(35,  LocationCategory.CONTAINER, False),
    "Underground ruins, Shekuten pillar room hole"                  : LocationData(36,  LocationCategory.CONTAINER, False),
    "Underground ruins, Kuruguru obstacle hole"                     : LocationData(37,  LocationCategory.CONTAINER, False),
    "Underground ruins, Gold Gorubesshu chest"                      : LocationData(38,  LocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillar room south chest"          : LocationData(39,  LocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillar room north chest"          : LocationData(40,  LocationCategory.CONTAINER, False),
    "Underground ruins, 3 chest room middle chest"                  : LocationData(41,  LocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillars room south hole"          : LocationData(42,  LocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillars room west hole"           : LocationData(43,  LocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillars room north hole"          : LocationData(44,  LocationCategory.CONTAINER, False),
    "Underground ruins, Fireball Orudakoitan chest"                 : LocationData(45,  LocationCategory.CONTAINER, False),
    "Underground ruins, Box ledge chest"                            : LocationData(46,  LocationCategory.CONTAINER, False),
    "Underground ruins, Horokko ledge chest"                        : LocationData(47,  LocationCategory.CONTAINER, False),
    "Underground ruins, Clozer exit chest"                          : LocationData(48,  LocationCategory.CONTAINER, False),
    "Underground ruins, Drillable wall room middle cliff chest"     : LocationData(49,  LocationCategory.CONTAINER, False),
    "Underground ruins, Drillable wall room west cliff chest"       : LocationData(50,  LocationCategory.CONTAINER, False),
    "Underground ruins, Drillable wall room east cliff chest"       : LocationData(51,  LocationCategory.CONTAINER, False),
    "Underground ruins, Trapped box hole"                           : LocationData(52,  LocationCategory.CONTAINER, False),
    "Underground ruins, Drillable wall room east cliff hole"        : LocationData(53,  LocationCategory.CONTAINER, False),
    "Cardon Forest Sub-Gate, Sharukurusu floor hole"                : LocationData(54,  LocationCategory.CONTAINER, False),
    "Cardon Forest Sub-Gate, Cliff hole"                            : LocationData(55,  LocationCategory.CONTAINER, False),
    "Cardon Forest Sub-Gate, Cliff chest"                           : LocationData(56,  LocationCategory.CONTAINER, False),
    "Cardon Forest Sub-Gate, Bottom conveyor hole"                  : LocationData(57,  LocationCategory.CONTAINER, False),
    "Cardon Forest Sub-Gate, Middle conveyor hole"                  : LocationData(58,  LocationCategory.CONTAINER, False),
    "Cardon Forest Sub-Gate, Middle switch chest"                   : LocationData(59,  LocationCategory.CONTAINER, False),
   #"Cardon Forest Sub-Gate, Sharukurusu starter key 1"             : LocationData(60,  LocationCategory.CONTAINER, False),
   #"Cardon Forest Sub-Gate, Conveyor key 2"                        : LocationData(61,  LocationCategory.CONTAINER, False),
   #"Cardon Forest Sub-Gate, Conveyor key get 3"                    : LocationData(62,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, Entrance right hole"                       : LocationData(63,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, Entrance left hole"                        : LocationData(64,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, Entrance chest"                            : LocationData(65,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, East corridor hole"                        : LocationData(66,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, West corridor hole"                        : LocationData(67,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, West corridor chest"                       : LocationData(68,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, Sharukurusu east chest"                    : LocationData(69,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, Sharukurusu middle chest"                  : LocationData(70,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, Sharukurusu west chest"                    : LocationData(71,  LocationCategory.CONTAINER, False),
    "Lake Jyun Sub-Gate, Sharukurusu west hole"                     : LocationData(72,  LocationCategory.CONTAINER, False),
    "Closer Woods Sub-Gate, Sharukurusu E room left hole"           : LocationData(73,  LocationCategory.CONTAINER, False),
    "Closer Woods Sub-Gate, Sharukurusu E room right hole"          : LocationData(74,  LocationCategory.CONTAINER, False),
    "Clozer woods Sub-Gate, Miroc+Gorubesshu west cliff chest"      : LocationData(75,  LocationCategory.CONTAINER, False),
    "Clozer woods Sub-Gate, Miroc+Gorubesshu east cliff chest"      : LocationData(76,  LocationCategory.CONTAINER, False),
    "Clozer woods Sub-Gate, Miroc+Gorubesshu southeast pillar hole" : LocationData(77,  LocationCategory.CONTAINER, False),
    "Clozer woods Sub-Gate, Miroc+Gorubesshu northeast pillar hole" : LocationData(78,  LocationCategory.CONTAINER, False),
    "Clozer woods Sub-Gate, Miroc+Gorubesshu northwest pillar hole" : LocationData(79,  LocationCategory.CONTAINER, False),
    "Clozer woods Sub-Gate, Miroc+Gorubesshu southwest pillar hole" : LocationData(80,  LocationCategory.CONTAINER, False),
    "Clozer Woods Sub-Gate, Generator room upper chest"             : LocationData(81,  LocationCategory.CONTAINER, False),
    "Clozer Woods Sub-Gate, Gorubesshu corridor east chest"         : LocationData(82,  LocationCategory.CONTAINER, False),
    "Clozer Woods Sub-Gate, Generator room lower chest"             : LocationData(83,  LocationCategory.CONTAINER, False),
    "Main Gate, Maze Chest"                                         : LocationData(84,  LocationCategory.CONTAINER, False),
    "Main Gate, Maze entrance hole"                                 : LocationData(85,  LocationCategory.CONTAINER, False),
    "Main Gate, Maze Karumuna Bash hole"                            : LocationData(86,  LocationCategory.CONTAINER, False),
    "Main Gate, Maze Reaverbot hole"                                : LocationData(87,  LocationCategory.CONTAINER, False),
    "Main Gate, Two Gorubesshu room chest"                          : LocationData(88,  LocationCategory.CONTAINER, False),
    "Main Gate, Entrance hole"                                      : LocationData(89,  LocationCategory.CONTAINER, False),
    "Main Gate, Boss corridor chest"                                : LocationData(90,  LocationCategory.CONTAINER, False),
    "Old City Sub-City, Chest"                                      : LocationData(91,  LocationCategory.CONTAINER, False),
    "Downtown Sub-City, Chest"                                      : LocationData(92,  LocationCategory.CONTAINER, False),
    "Uptown Sub-City, Chest"                                        : LocationData(93,  LocationCategory.CONTAINER, False),
    "Flutter, Study chest"                                          : LocationData(94,  LocationCategory.CONTAINER, False),
    "Escape the Ocean Tower"                                        : LocationData(95,  LocationCategory.COMBAT,    False),
    "Ferdinand defeated"                                            : LocationData(96,  LocationCategory.COMBAT,    False),
    "Bon Bonne defeated"                                            : LocationData(97,  LocationCategory.COMBAT,    False),
    "Marlwolf defeated"                                             : LocationData(98,  LocationCategory.COMBAT,    False),
    "Balkon Gerät defeated"                                         : LocationData(99,  LocationCategory.COMBAT,    False),
    "Garudoriten defeated"                                          : LocationData(100, LocationCategory.COMBAT,    False),
    "Karumuna Bash Trio defeated"                                   : LocationData(101, LocationCategory.COMBAT,    False),
    "Focke-Wulf defeated"                                           : LocationData(102, LocationCategory.COMBAT,    False),
    "Theodore Bruno defeated"                                       : LocationData(103, LocationCategory.COMBAT,    False),
   #"Rescue the shop owner's husband"                               : LocationData(104, LocationCategory.QUEST,     False),
    "Race Technical Course Rank A"                                  : LocationData(105, LocationCategory.QUEST,     False),
    "Beast Hunter Rank A"                                           : LocationData(106, LocationCategory.QUEST,     False),
    "Race Straight Course Rank A"                                   : LocationData(107, LocationCategory.QUEST,     False),
    "Balloon Fantasy Rank A"                                        : LocationData(108, LocationCategory.QUEST,     False),
    "Race Left Curve Course Rank A"                                 : LocationData(109, LocationCategory.QUEST,     False),
    "Save the missing woman"                                        : LocationData(110, LocationCategory.QUEST,     False),
    "Cure Ira's illness"                                            : LocationData(111, LocationCategory.QUEST,     False),
   #"Tell painter she needs red"                                    : LocationData(112, LocationCategory.QUEST,     False),
   #"Get lipstick"                                                  : LocationData(113, LocationCategory.QUEST,     False),
    "Museum donation, Old Bone"                                     : LocationData(114, LocationCategory.QUEST,     False),
    "Museum donation, Old Heater"                                   : LocationData(115, LocationCategory.QUEST,     False),
    "Museum donation, Old Doll"                                     : LocationData(116, LocationCategory.QUEST,     False),
    "Museum donation, Antique Bell"                                 : LocationData(117, LocationCategory.QUEST,     False),
    "Museum donation, Giant Horn"                                   : LocationData(118, LocationCategory.QUEST,     False),
    "Museum donation, Shiny Object"                                 : LocationData(119, LocationCategory.QUEST,     False),
    "Museum donation, Old Shield"                                   : LocationData(120, LocationCategory.QUEST,     False),
    "Museum donation, Shiny Red Stone"                              : LocationData(121, LocationCategory.QUEST,     False),
    "Complete the Museum exhibit"                                   : LocationData(122, LocationCategory.QUEST,     False),
    "Take dangerous object from museum visitor"                     : LocationData(123, LocationCategory.QUEST,     False),
    "Gift Flower to Roll"                                           : LocationData(124, LocationCategory.QUEST,     False),
    "Gift Music Box to Roll"                                        : LocationData(125, LocationCategory.QUEST,     False),
    "Gift Ring to Roll"                                             : LocationData(126, LocationCategory.QUEST,     False),
    "Juno Defeated"                                                 : LocationData(999, LocationCategory.COMBAT,    False),
}

LOCATION_NAME_TO_ID         = {locationName: locationDataDict[locationName].id         for locationName in locationDataDict.keys()}
LOCATION_NAME_TO_CATEGORY   = {locationName: locationDataDict[locationName].category   for locationName in locationDataDict.keys()}
LOCATION_NAME_TO_ISMISSABLE = {locationName: locationDataDict[locationName].isMissable for locationName in locationDataDict.keys()}

def get_location_names_with_ids(location_names: list[str]) -> dict[str, int | None]:
    return {location_name: LOCATION_NAME_TO_ID[location_name] for location_name in location_names}

def create_all_locations(world: GameWorld) -> None:
    create_regular_locations(world)
    create_events(world)
    return None

def create_regular_locations(world: GameWorld) -> None:
    region_data_dict = get_regionDataDict(world)
    for region_name in region_data_dict.keys():
        region_data = region_data_dict[region_name]
        region = world.get_region(region_name)
        location_names_with_ids = get_location_names_with_ids(region_data.locationNameList)
        region.add_locations(location_names_with_ids, GameLocation)
    return None

def create_events(world: GameWorld) -> None:
    juno_region = world.get_region("Main Gate - Juno Area (Boss)")
    juno_region.add_event("Juno Defeated", "Victory", location_type=GameLocation,
                       item_type=items.GameItem, rule=lambda state: True)  # Add logic for beating Juno with access.
    world.multiworld.completion_condition[world.player] = lambda state: state.has("Victory", world.player)

def lock_missables_to_filler(world) -> None:
    # Assign a random filler item to each missable location
    filler_item_names = [
        name for name, cls in items.ITEM_NAME_TO_CATEGORY.items()
        if cls == ItemClassification.filler
    ]
    world.locked_missable_filler_names = []
    missable_locations = [loc for loc in world.multiworld.get_locations(world.player) if locationDataDict[loc.name].isMissable]
    for loc in missable_locations:
        name = world.random.choice(filler_item_names) 
        loc.place_locked_item(world.create_item(name))
        world.locked_missable_filler_names.append(name)