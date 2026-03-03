from __future__ import annotations
from BaseClasses import Location
from enum import IntEnum
from typing import TYPE_CHECKING, NamedTuple

if TYPE_CHECKING:
    from .world import MMLWorld

class MMLLocationCategory(IntEnum):
    CONTAINER = 0
    QUEST = 1
    COMBAT = 2

class MMLLocation(Location):
    game = "Mega Man Legends"

class MMLLocationData(NamedTuple):
    id: str
    category: MMLLocationCategory
    isMissable: bool

locationDataDict = {
  ##"Ocean tower, Right chest"                                      : MMLLocationData(1,   MMLLocationCategory.CONTAINER,  True),
  ##"Ocean tower, Left chest"                                       : MMLLocationData(2,   MMLLocationCategory.CONTAINER,  True),
  ##"Apple market, Electric goods box"                              : MMLLocationData(3,   MMLLocationCategory.CONTAINER, False),
  ##"Apple market, Book store box"                                  : MMLLocationData(4,   MMLLocationCategory.CONTAINER, False),
  ##"Apple market, Junk store box"                                  : MMLLocationData(5,   MMLLocationCategory.CONTAINER, False),
  ##"Apple market, North pail"                                      : MMLLocationData(6,   MMLLocationCategory.CONTAINER, False),
  ##"Apple market, South pail"                                      : MMLLocationData(7,   MMLLocationCategory.CONTAINER, False),
  ##"Downtown, South east peace sign pail"                          : MMLLocationData(8,   MMLLocationCategory.CONTAINER, False),
  ##"Downtown, Center east pail"                                    : MMLLocationData(9,   MMLLocationCategory.CONTAINER, False),
  ##"Downtown, Don't kick us pail"                                  : MMLLocationData(10,  MMLLocationCategory.CONTAINER, False),
  ##"Downtown, Center pail"                                         : MMLLocationData(11,  MMLLocationCategory.CONTAINER, False),
  ##"Downtown, Library pail"                                        : MMLLocationData(12,  MMLLocationCategory.CONTAINER, False),
  ##"Uptown, Hospital right pail"                                   : MMLLocationData(13,  MMLLocationCategory.CONTAINER, False),
  ##"Uptown, Hospital left pail"                                    : MMLLocationData(14,  MMLLocationCategory.CONTAINER, False),
  ##"Uptown, Ocean corner pail"                                     : MMLLocationData(15,  MMLLocationCategory.CONTAINER, False),
  ##"Wily's Boat, Right box"                                        : MMLLocationData(16,  MMLLocationCategory.CONTAINER, False),
  ##"Wily's Boat, Left box"                                         : MMLLocationData(17,  MMLLocationCategory.CONTAINER, False),
  ##"Wily's Boat, Pail"                                             : MMLLocationData(18,  MMLLocationCategory.CONTAINER, False),
  ##"Yass plains, Plateau house box"                                : MMLLocationData(19,  MMLLocationCategory.CONTAINER, False),
  ##"Yass plains, Plateau house pail"                               : MMLLocationData(20,  MMLLocationCategory.CONTAINER, False),
  ##"Yass plains, Behind hideout pail"                              : MMLLocationData(21,  MMLLocationCategory.CONTAINER, False),
  ##"Yass plains, Across hideout pail"                              : MMLLocationData(22,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Junk store man chest"                       : MMLLocationData(23,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Junk store man hole"                        : MMLLocationData(24,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Main gate entrance chest"                   : MMLLocationData(25,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, 2 box ledge chest"                          : MMLLocationData(26,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Miroc room ledge chest"                     : MMLLocationData(27,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Cross room chest"                           : MMLLocationData(28,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Miroc room left hole"                       : MMLLocationData(29,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Miroc room right hole"                      : MMLLocationData(30,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Arukoitan battle north chest"               : MMLLocationData(31,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Arukoitan battle south chest"               : MMLLocationData(32,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Obstacle room cliff east hole"              : MMLLocationData(33,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Obstacle room cliff west hole"              : MMLLocationData(34,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Shekuten pillar room chest"                 : MMLLocationData(35,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Shekuten pillar room hole"                  : MMLLocationData(36,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Kuruguru obstacle hole"                     : MMLLocationData(37,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Gold Gorubesshu chest"                      : MMLLocationData(38,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillar room south chest"          : MMLLocationData(39,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillar room north chest"          : MMLLocationData(40,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, 3 chest room middle chest"                  : MMLLocationData(41,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillars room south hole"          : MMLLocationData(42,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillars room west hole"           : MMLLocationData(43,  MMLLocationCategory.CONTAINER, False),
    "Underground ruins, Drillable pillars room north hole"          : MMLLocationData(44,  MMLLocationCategory.CONTAINER, False),
  ##"Underground ruins, Fireball Orudakoitan chest"                 : MMLLocationData(45,  MMLLocationCategory.CONTAINER, False),
  ##"Underground ruins, Horokko ledge chest"                        : MMLLocationData(46,  MMLLocationCategory.CONTAINER, False),
  ##"Underground ruins, Clozer exit chest"                          : MMLLocationData(47,  MMLLocationCategory.CONTAINER, False),
  ##"Underground ruins, Clozer exit chest"                          : MMLLocationData(48,  MMLLocationCategory.CONTAINER, False),
  ##"Underground ruins, Drillable wall room middle cliff chest"     : MMLLocationData(49,  MMLLocationCategory.CONTAINER, False),
  ##"Underground ruins, Drillable wall room west cliff chest"       : MMLLocationData(50,  MMLLocationCategory.CONTAINER, False),
  ##"Underground ruins, Drillable wall room east cliff chest"       : MMLLocationData(51,  MMLLocationCategory.CONTAINER, False),
  ##"Underground ruins, Trapped box hole"                           : MMLLocationData(52,  MMLLocationCategory.CONTAINER, False),
  ##"Underground ruins, Drillable wall room east cliff hole"        : MMLLocationData(53,  MMLLocationCategory.CONTAINER, False),
  ##"Cardon Forest Sub-Gate, Sharukurusu floor hole"                : MMLLocationData(54,  MMLLocationCategory.CONTAINER, False),
  ##"Cardon Forest Sub-Gate, Cliff hole"                            : MMLLocationData(55,  MMLLocationCategory.CONTAINER, False),
  ##"Cardon Forest Sub-Gate, Cliff chest"                           : MMLLocationData(56,  MMLLocationCategory.CONTAINER, False),
  ##"Cardon Forest Sub-Gate, Bottom conveyor hole"                  : MMLLocationData(57,  MMLLocationCategory.CONTAINER, False),
  ##"Cardon Forest Sub-Gate, Middle conveyor hole"                  : MMLLocationData(58,  MMLLocationCategory.CONTAINER, False),
  ##"Cardon Forest Sub-Gate, Middle switch chest"                   : MMLLocationData(59,  MMLLocationCategory.CONTAINER, False),
   #"Cardon Forest Sub-Gate, Sharukurusu starter key 1"             : MMLLocationData(60,  MMLLocationCategory.CONTAINER, False),
   #"Cardon Forest Sub-Gate, Conveyor key 2"                        : MMLLocationData(61,  MMLLocationCategory.CONTAINER, False),
   #"Cardon Forest Sub-Gate, Conveyor key get 3"                    : MMLLocationData(62,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, Entrance right hole"                       : MMLLocationData(63,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, Entrance left hole"                        : MMLLocationData(64,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, Entrance chest"                            : MMLLocationData(65,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, East corridor hole"                        : MMLLocationData(66,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, West corridor hole"                        : MMLLocationData(67,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, West corridor chest"                       : MMLLocationData(68,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, Sharukurusu east chest"                    : MMLLocationData(69,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, Sharukurusu middle chest"                  : MMLLocationData(70,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, Sharukurusu west chest"                    : MMLLocationData(71,  MMLLocationCategory.CONTAINER, False),
  ##"Lake Jyun Sub-Gate, Sharukurusu west hole"                     : MMLLocationData(72,  MMLLocationCategory.CONTAINER, False),
  ##"Closer Woods Sub-Gate, Sharukurusu E room left hole"           : MMLLocationData(73,  MMLLocationCategory.CONTAINER, False),
  ##"Closer Woods Sub-Gate, Sharukurusu E room right hole"          : MMLLocationData(74,  MMLLocationCategory.CONTAINER, False),
  ##"Clozer woods Sub-Gate, Miroc+Gorubesshu west cliff chest"      : MMLLocationData(75,  MMLLocationCategory.CONTAINER, False),
  ##"Clozer woods Sub-Gate, Miroc+Gorubesshu east cliff chest"      : MMLLocationData(76,  MMLLocationCategory.CONTAINER, False),
  ##"Clozer woods Sub-Gate, Miroc+Gorubesshu southeast pillar hole" : MMLLocationData(77,  MMLLocationCategory.CONTAINER, False),
  ##"Clozer woods Sub-Gate, Miroc+Gorubesshu northeast pillar hole" : MMLLocationData(78,  MMLLocationCategory.CONTAINER, False),
  ##"Clozer woods Sub-Gate, Miroc+Gorubesshu northwest pillar hole" : MMLLocationData(79,  MMLLocationCategory.CONTAINER, False),
  ##"Clozer woods Sub-Gate, Miroc+Gorubesshu southwest pillar hole" : MMLLocationData(80,  MMLLocationCategory.CONTAINER, False),
  ##"Clozer Woods Sub-Gate, Generator room upper chest"             : MMLLocationData(81,  MMLLocationCategory.CONTAINER, False),
  ##"Clozer Woods Sub-Gate, Gorubesshu corridor east chest"         : MMLLocationData(82,  MMLLocationCategory.CONTAINER, False),
  ##"Clozer Woods Sub-Gate, Generator room lower chest"             : MMLLocationData(83,  MMLLocationCategory.CONTAINER, False),
    "Main Gate, Maze Chest"                                         : MMLLocationData(84,  MMLLocationCategory.CONTAINER, False),
    "Main Gate, Maze entrance hole"                                 : MMLLocationData(85,  MMLLocationCategory.CONTAINER, False),
    "Main Gate, Maze Karumuna Bash hole"                            : MMLLocationData(86,  MMLLocationCategory.CONTAINER, False),
    "Main Gate, Maze Reaverbot hole"                                : MMLLocationData(87,  MMLLocationCategory.CONTAINER, False),
    "Main Gate, Two Gorubesshu room chest"                          : MMLLocationData(88,  MMLLocationCategory.CONTAINER, False),
    "Main Gate, Entrance hole"                                      : MMLLocationData(89,  MMLLocationCategory.CONTAINER, False),
    "Main Gate, Boss corridor chest"                                : MMLLocationData(90,  MMLLocationCategory.CONTAINER, False),
    "Old City Sub-City, Chest"                                      : MMLLocationData(91,  MMLLocationCategory.CONTAINER, False),
    "Downtown Sub-City, Chest"                                      : MMLLocationData(92,  MMLLocationCategory.CONTAINER, False),
    "Uptown Sub-City, Chest"                                        : MMLLocationData(93,  MMLLocationCategory.CONTAINER, False),
  ##"Flutter, Study chest"                                          : MMLLocationData(94,  MMLLocationCategory.CONTAINER, False),
  ##"Damaged Hanmuru Doll defeated"                                 : MMLLocationData(95,  MMLLocationCategory.COMBAT,    False),
  ##"Ferdinand defeated"                                            : MMLLocationData(96,  MMLLocationCategory.COMBAT,    False),
  ##"Bon Bonne defeated"                                            : MMLLocationData(97,  MMLLocationCategory.COMBAT,    False),
  ##"Marlwolf defeated"                                             : MMLLocationData(98,  MMLLocationCategory.COMBAT,    False),
  ##"Balkon Gerät defeated"                                         : MMLLocationData(99,  MMLLocationCategory.COMBAT,    False),
  ##"Garudoriten defeated"                                          : MMLLocationData(100, MMLLocationCategory.COMBAT,    False),
  ##"Karumuna Bash Trio defeated"                                   : MMLLocationData(101, MMLLocationCategory.COMBAT,    False),
    "Focke-Wulf defeated"                                           : MMLLocationData(102, MMLLocationCategory.COMBAT,    False),
  ##"Theodore Bruno defeated"                                       : MMLLocationData(103, MMLLocationCategory.COMBAT,    False),
  ##"Rescue the shop owner's husband"                               : MMLLocationData(104, MMLLocationCategory.QUEST,     False),
  ##"Race Technical Course Rank A"                                  : MMLLocationData(105, MMLLocationCategory.QUEST,     False),
  ##"Beast Hunter Rank A"                                           : MMLLocationData(106, MMLLocationCategory.QUEST,     False),
  ##"Race Straight Course Rank A"                                   : MMLLocationData(107, MMLLocationCategory.QUEST,     False),
  ##"Balloon Fantasy Rank A"                                        : MMLLocationData(108, MMLLocationCategory.QUEST,     False),
  ##"Race Left Curve Course Rank A"                                 : MMLLocationData(109, MMLLocationCategory.QUEST,     False),
  ##"Save the missing woman"                                        : MMLLocationData(110, MMLLocationCategory.QUEST,     False),
  ##"Cure Ira's illness"                                            : MMLLocationData(111, MMLLocationCategory.QUEST,     False),
   #"Tell painter she needs red"                                    : MMLLocationData(112, MMLLocationCategory.QUEST,     False),
   #"Get lipstick"                                                  : MMLLocationData(113, MMLLocationCategory.QUEST,     False),
  ##"Museum donation, Old Bone"                                     : MMLLocationData(114, MMLLocationCategory.QUEST,     False),
  ##"Museum donation, Old Heater"                                   : MMLLocationData(115, MMLLocationCategory.QUEST,     False),
  ##"Museum donation, Old Doll"                                     : MMLLocationData(116, MMLLocationCategory.QUEST,     False),
  ##"Museum donation, Antique Bell"                                 : MMLLocationData(117, MMLLocationCategory.QUEST,     False),
  ##"Museum donation, Giant Horn"                                   : MMLLocationData(118, MMLLocationCategory.QUEST,     False),
  ##"Museum donation, Shiny Object"                                 : MMLLocationData(119, MMLLocationCategory.QUEST,     False),
  ##"Museum donation, Old Shield"                                   : MMLLocationData(120, MMLLocationCategory.QUEST,     False),
  ##"Museum donation, Shiny Red Stone"                              : MMLLocationData(121, MMLLocationCategory.QUEST,     False),
  ##"Complete the Museum exhibit"                                   : MMLLocationData(122, MMLLocationCategory.QUEST,     False),
  ##"Take dangerous object from museum visitor"                     : MMLLocationData(123, MMLLocationCategory.QUEST,     False),
  ##"Gift Flower to Roll"                                           : MMLLocationData(124, MMLLocationCategory.QUEST,     False),
  ##"Gift Music Box to Roll"                                        : MMLLocationData(125, MMLLocationCategory.QUEST,     False),
  ##"Gift Ring to Roll"                                             : MMLLocationData(126, MMLLocationCategory.QUEST,     False)
}

LOCATION_NAME_TO_ID         = {locationName: locationDataDict[locationName].id         for locationName in locationDataDict.keys()}
LOCATION_NAME_TO_CATEGORY   = {locationName: locationDataDict[locationName].category   for locationName in locationDataDict.keys()}
LOCATION_NAME_TO_ISMISSABLE = {locationName: locationDataDict[locationName].isMissable for locationName in locationDataDict.keys()}

def get_location_names_with_ids(location_names: list[str]) -> dict[str, int | None]:
    return {location_name: LOCATION_NAME_TO_ID[location_name] for location_name in location_names}

def create_all_locations(world: MMLWorld) -> None:
    create_regular_locations(world)
    create_events(world)
    return None

def create_regular_locations(world: MMLWorld) -> None:
    universe = world.get_region("Universe")
    universe.add_locations(LOCATION_NAME_TO_ID, MMLLocation)
    return None

def create_events(world: MMLWorld) -> None:
    return None