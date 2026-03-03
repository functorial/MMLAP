from __future__ import annotations
from .locations import MMLLocationData, locationDataDict
from BaseClasses import Entrance, Region
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .world import MMLWorld

# TODO: Current setup only has one big region. Create logical regions
# e.g. a region you can only get to with jump springs.
# It will make sense to eventually have regions be actual rooms in the game
# because it will make it possible to do entrance keys a la OoT.

def create_and_connect_regions(world: MMLWorld) -> None:
    create_all_regions(world)
    connect_regions(world)

def create_region_generic(world: MMLWorld, region_name:str, location_table:list[MMLLocationData]) -> Region:
    new_region = Region(region_name, world.player, world.multiworld)
    return new_region

# Going to make this more granular than currently required for future-proofing
location_tables = {
    "Ocean Tower - Room 1": [

    ], 
    "Ocean Tower - Room 2": [
        locationDataDict["Ocean tower, Right chest"],
        locationDataDict["Ocean tower, Left chest"]
    ],
    "Ocean Tower - Room 3": [
        locationDataDict["Damaged Hanmuru Doll defeated"]
    ],
    "Cardon Forest": [
        
    ],
    "Flutter - Common Room": [

    ],
    "Flutter - Barrell's Room": [
        locationDataDict["Flutter, Study chest"]
    ],
    "Flutter - Mega Man's Room": [

    ],
    "Flutter - Roll's Room": [

    ],
    "Support Car / R&D Room": [

    ],
    "Support Car / R&D Room (Gift Flower)": [
        locationDataDict["Gift Flower to Roll"]
    ],
    "Support Car / R&D Room (Gift Music Box)": [
        locationDataDict["Gift Music Box to Roll"]
    ],
    "Support Car / R&D Room (Gift Ring)": [
        locationDataDict["Gift Ring to Roll"]
    ],
    "Apple Market": [
        locationDataDict["Apple market, Electric goods box"],
        locationDataDict["Apple market, Book store box"],
        locationDataDict["Apple market, Junk store box"],
        locationDataDict["Apple market, North pail"],
        locationDataDict["Apple market, South pail"]
    ],
    "Apple Market - Junk Shop": [
        locationDataDict["Rescue the shop owner's husband"]
    ],
    "Apple Market - Electronics Shop": [

    ],
    "Apple Market - Hip Bone": [

    ],
    "Apple Market - Tailor Chinos": [

    ],
    "Apple Market - Record Shop": [

    ],
    "Downtown - Outside": [
        locationDataDict["Downtown, South east peace sign pail"],
        locationDataDict["Downtown, Center east pail"]
    ],
    "Downtown - Outside (Boss fight)": [
        locationDataDict["Ferdinand defeated"]
    ],
    "Downtown - Outside (Blumebear pail)": [
        # Only lootable after city hall is saved
        locationDataDict["Downtown, Don't kick us pail"]
    ],
    "Downtown - Outside (Lost Bag)": [
        # Only lootable after sufficiently far in police quest
        # Requires completing Cardon Forest Sub-Gate, Lake Jyun Sub-Gate, Jump Springs, and Police station is not destroyed
        locationDataDict["Downtown, Center pail"]         
    ],
    "Downtown - Outside (Discarded Saw)": [
        # Only lootable after sufficiently far in hideout quest
        # Requires completing Cardon Forest Sub-Gate, getting Pick -> give to Jim, and making some main/side quest progression to continue
        locationDataDict["Downtown, Library pail"]
    ],
    "Downtown - Library": [

    ],
    "City Hall - Outside": [

    ],
    "City Hall - Outside (Boss Fight)": [
        locationDataDict["Bon Bonne defeated"]
    ],
    "City Hall - City Hall 1st Floor": [

    ],
    "City Hall - Amelia's Office": [

    ],
    "City Hall - Police Station": [

    ],
    "City Hall - Inspector's Office": [

    ],
    "City Hall - Bank": [

    ],
    "Uptown": [
        locationDataDict["Uptown, Hospital right pail"],
        locationDataDict["Uptown, Hospital left pail"],
        locationDataDict["Uptown, Ocean corner pail"]
    ],
    "Uptown - Hospital": [

    ],
    "Uptown - Hospital (Missing woman turn-in)": [
        locationDataDict["Save the missing woman"]
    ],
    "Uptown - Hospital (Ira's Room)": [
        locationDataDict["Cure Ira's illness"]
    ],
    "Uptown - TV Station": [

    ],
    "Uptown - TV Station (Beast Hunter Game)": [
        locationDataDict["Beast Hunter Rank A"]
    ],
    "Uptown - TV Station (Balloon Fantasy Game)": [
        locationDataDict["Balloon Fantasy Rank A"]
    ],
    "Uptown - TV Station (Racing Game)": [
        locationDataDict["Race Technical Course Rank A"],
        locationDataDict["Race Straight Course Rank A"],
        locationDataDict["Race Left Curve Course Rank A"]
    ],
    "Wily's Boat - Walkway": [
        locationDataDict["Wily's Boat, Right box"],
        locationDataDict["Wily's Boat, Left box"]
    ],
    "Wily's Boat - Inside": [

    ],
    "Wily's Boat - Mechanic Area": [
        locationDataDict["Wily's Boat, Pail"]
    ],
    "Wily's Boat - Dock": [

    ],
    "Museum - Floor 1": [

    ],
    "Museum - Floor 2 - Old Bone": [
        locationDataDict["Museum donation, Old Bone"]
    ],
    "Museum - Floor 2 - Old Heater": [
        # Old heater currently isn't in the item pool
        locationDataDict["Museum donation, Old Heater"]
    ],
    "Museum - Floor 2 - Old Doll": [
        locationDataDict["Museum donation, Old Doll"]
    ],
    "Museum - Floor 2 - Antique Bell": [
        locationDataDict["Museum donation, Antique Bell"]
    ],
    "Museum - Floor 2 - Giant Horn": [
        locationDataDict["Museum donation, Giant Horn"]
    ],
    "Museum - Floor 2 - Shiny Object": [
        locationDataDict["Museum donation, Shiny Object"]
    ],
    "Museum - Floor 2 - Old Shield": [
        locationDataDict["Museum donation, Old Shield"]
    ],
    "Museum - Floor 2 - Shiny Red Stone": [
        locationDataDict["Museum donation, Shiny Red Stone"]
    ],
    "Museum - Floor 2 - Exhibit Complete": [
        locationDataDict["Complete the Museum exhibit"],
        locationDataDict["Take dangerous object from museum visitor"]
    ],
    "Old City": [

    ],
    "Old City (Inside Warehouse Gate)": [
        # Connects with sub-city, and UR (Old City Exit)

    ],
    "Old City (Boss Fight)": [
        locationDataDict["Theodore Bruno defeated"]
    ],
    "Old City - Power Plant": [

    ],
    "Outside Main Gate": [

    ],
    "Yass Plains - Outside": [
        locationDataDict["Yass plains, Plateau house box"],
        locationDataDict["Yass plains, Plateau house pail"],
        locationDataDict["Yass plains, Behind hideout pail"],
        locationDataDict["Yass plains, Across hideout pail"]
    ],
    "Yass Plains - Hideout Stage 1": [

    ],
    "Yass Plains - Hideout Stage 2": [

    ],
    "Yass Plains - Hideout Stage 3": [

    ],
    "Yass Plains - Empty House": [

    ],
    "Yass Plains - Junk Shop House": [

    ],
    "Clozer Woods - Bridge Area": [
        # Connects to underground ruins drillable wall room right

    ],
    "Clozer Woods - Boss Fight": [
        locationDataDict["Marlwolf defeated"]
    ],
    "Outside Cardon Forest Subgate": [

    ],
    "Cardon Forest SubGate - Refractor Room": [
        
    ],
    "Cardon Forest SubGate - Cliff Room": [
        locationDataDict["Cardon Forest Sub-Gate, Sharukurusu floor hole"],
        locationDataDict["Cardon Forest Sub-Gate, Cliff hole"],
        locationDataDict["Cardon Forest Sub-Gate, Cliff chest"]
    ],
    "Cardon Forest SubGate - Conveyor Belts": [
        locationDataDict["Cardon Forest Sub-Gate, Bottom conveyor hole"],
        locationDataDict["Cardon Forest Sub-Gate, Middle conveyor hole"],
        locationDataDict["Cardon Forest Sub-Gate, Middle switch chest"]
    ],
    "Lake Jyun - Boss Fight": [
        locationDataDict["Balkon Gerät defeated"]
    ],
    "Lake Jyun - Outside Sub-Gate": [

    ],
    "Lake Jyun Sub-Gate - Entrance": [
        locationDataDict["Lake Jyun Sub-Gate, Entrance right hole"],
        locationDataDict["Lake Jyun Sub-Gate, Entrance left hole"],
        locationDataDict["Lake Jyun Sub-Gate, Entrance chest"]
    ],
    "Lake Jyun Sub-Gate - Corridor Room": [
        # Requires jump springs to get to from Entrance
        locationDataDict["Lake Jyun Sub-Gate, East corridor hole"],
        locationDataDict["Lake Jyun Sub-Gate, West corridor hole"],
        locationDataDict["Lake Jyun Sub-Gate, West corridor chest"]
    ],
    "Lake Jyun Sub-Gate - Sharukurusu Room (Upper East)": [
        # Does not require jump springs from Corridor East entrance
        locationDataDict["Lake Jyun Sub-Gate, Sharukurusu east chest"]
    ],
    "Lake Jyun Sub-Gate - Sharukurusu Room (Upper West)": [
        # Does not require jump springs from Corridor West entrance
        locationDataDict["Lake Jyun Sub-Gate, Sharukurusu west chest"],
        locationDataDict["Lake Jyun Sub-Gate, Sharukurusu west hole"]
    ],
    "Lake Jyun Sub-Gate - Sharukurusu Room (Pyramid)": [
        # Requires jump springs
        locationDataDict["Lake Jyun Sub-Gate, Sharukurusu middle chest"]
    ],
    "Lake Jyun Sub-Gate - Firushudot Hall": [
        
    ],
    "Lake Jyun Sub-Gate - Boss Room (Inactive)": [
        
    ],
    "Lake Jyun Sub-Gate - Refractor Room": [
        
    ],
    "Lake Jyun Sub-Gate - Boss Room (Active)": [
        locationDataDict["Garudoriten defeated"]
    ],
    "Clozer Woods Sub-Gate - Entrance": [
        
    ],
    "Clozer Woods Sub-Gate - Entrance Elevator Room": [
        
    ],
    "Clozer Woods Sub-Gate - Control Room": [
        
    ],
    "Clozer Woods Sub-Gate - Sharukurusu Ambush": [
        # Connects with Side Elevator Room (Upper), Entrance Elevator, and Pillar West Cliff
        locationDataDict["Closer Woods Sub-Gate, Sharukurusu E room left hole"],
        locationDataDict["Closer Woods Sub-Gate, Sharukurusu E room right hole"]
    ],
    "Clozer Woods Sub-Gate - Side Elevator Room (Upper)": [
        # Starts in the "Down" position -- Can drop down
    ],
    "Clozer Woods Sub-Gate - Side Elevator Room (Lower)": [
        # Can't go up without fixing generator
    ],
    "Clozer Woods Sub-Gate - Pillar Room (West Cliff)": [
        # Can go from Sharukurusu Ambush no req or Pillar Room lower + jump springs
        locationDataDict["Clozer woods Sub-Gate, Miroc+Gorubesshu west cliff chest"]
    ],
    "Clozer Woods Sub-Gate - Pillar Room (Lower Level)": [
        # Connects with Gorubesshu Corridor, Side Elevator (Lower), and Breakable Ceiling (Lower)
        locationDataDict["Clozer woods Sub-Gate, Miroc+Gorubesshu southeast pillar hole"],
        locationDataDict["Clozer woods Sub-Gate, Miroc+Gorubesshu northeast pillar hole"],
        locationDataDict["Clozer woods Sub-Gate, Miroc+Gorubesshu northwest pillar hole"],
        locationDataDict["Clozer woods Sub-Gate, Miroc+Gorubesshu southwest pillar hole"]
    ],
    "Clozer Woods Sub-Gate - Pillar Room (East Cliff)": [
        # Requires jump springs from lower level
        locationDataDict["Clozer woods Sub-Gate, Miroc+Gorubesshu east cliff chest"]
    ],
    "Clozer Woods Sub-Gate - Breakable Ceiling (Lower)": [
        # Need Explosive + jump springs to get upper
    ],
    "Clozer Woods Sub-Gate - Breakable Ceiling (Upper)": [
        # Need Explosive to get lower
        # Connects to Boss Room
    ],
    "Clozer Woods Sub-Gate - Gorubesshu Corridor": [
        # Connects Pillar Lower, UR (Drillable Walls Room, Left-Middle, Lower), Generator (Lower)
        locationDataDict["Clozer Woods Sub-Gate, Gorubesshu corridor east chest"]
    ],
    "Clozer Woods Sub-Gate - Generator Room (Lower)": [
        # Connects to Gorubesshu Corridor
        locationDataDict["Clozer Woods Sub-Gate, Generator room lower chest"]
    ],
    "Clozer Woods Sub-Gate - Generator Room (Upper)": [
        # Can drop down, Connects to Lower & Boss room
        locationDataDict["Clozer Woods Sub-Gate, Generator room upper chest"]
    ],
    "Clozer Woods Sub-Gate - Boss Room": [
        locationDataDict["Karumuna Bash Trio defeated"]
    ],
    "Focke-Wulf Boss Area": [
        # Connects from flutter after getting red refractor. Back to flutter on boss defeat
        locationDataDict["Focke-Wulf defeated"]
    ],
    "Underground Ruins - Orudakoitan Bridge Area (Ledges)": [
        # Connects back to bridge
        locationDataDict["Underground ruins, Horokko ledge chest"],
        locationDataDict["Underground ruins, Clozer exit chest"]
    ],
    "Underground Ruins - Orudakoitan Bridge Area (Bridge)": [
        # Connects Drillable Right, Ledges with jump springs
        locationDataDict["Underground ruins, Fireball Orudakoitan chest" ]
    ],
    "Underground Ruins - Drillable Wall Area (Right)": [
        # Connects RML w/ Drill, Clozer Woods
        locationDataDict["Underground ruins, Clozer exit chest"],
        locationDataDict["Underground ruins, Trapped box hole"]
    ],
    "Underground Ruins - Drillable Wall Area (Right-Middle, Upper)": [
        # Requires jump springs
        locationDataDict["Underground ruins, Drillable wall room east cliff chest"],
        locationDataDict["Underground ruins, Drillable wall room east cliff hole"]
    ],
    "Underground Ruins - Drillable Wall Area (Right-Middle, Lower)": [
        # Connects LML w/ Drill, Right w/ Drill, Upper
    ],
    "Underground Ruins - Drillable Wall Area (Left-Middle, Upper)": [
        # Requires jump springs
        locationDataDict["Underground ruins, Drillable wall room middle cliff chest"]
    ],
    "Underground Ruins - Drillable Wall Area (Left-Middle, Lower)": [
        # Connects LL w/ Drill, RML w/ Drill, and LMU, and Clozer sub-gate
    ],
    "Underground Ruins - Drillable Wall Area (Left, Upper)": [
        # Requires jump springs
        locationDataDict["Underground ruins, Drillable wall room west cliff chest"]
    ],
    "Underground Ruins - Drillable Wall Area (Left, Lower)": [
        # Connects to LML w/ Drill, LU, and Spinning Tower Trap Area (Sharukurusu Corridor)
    ],
    "Underground Ruins - Spinning Tower Trap Area (Sharukurusu Corridor)": [
        # Connects Drillable Wall Area (Left, Lower), (ledge room)
        locationDataDict["Underground ruins, Obstacle room cliff east hole"],
        locationDataDict["Underground ruins, Obstacle room cliff west hole"]
    ],
    "Underground Ruins - Spinning Tower Trap Area (Ledge Room)": [
        # Connects to (Sharukurusu Corridor) w/ jump springs, and (Arukoitan battle)
        locationDataDict["Underground ruins, Arukoitan battle south chest"]
    ],
    "Underground Ruins - Spinning Tower Trap Area (Arukoitan Battle + Hanmuru Doll)": [
        # Connects to (Ledge Room) w/e jump springs, (Cardon Forest Surface Exit), (Shekuten Lower)
        locationDataDict["Underground ruins, Arukoitan battle north chest"]
    ],
    "Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Surface Exit)": [
        # Connects to Cardon Forest, (Hanmuru Doll), (Sub-Gate Exit) w/ jump springs, (Pillar Room West Ledge) w/ jump springs
    ],
    "Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Sub-Gate Exit)": [
        # Connects to Cardon Forest Sub-Gate, (Cardon Forest Surface Exit)
        locationDataDict["Underground ruins, Miroc room ledge chest"],
        locationDataDict["Underground ruins, Cross room chest"],
        locationDataDict["Underground ruins, Miroc room left hole"],
        locationDataDict["Underground ruins, Miroc room right hole"]
    ],
    "Underground Ruins - Cardon Forest Sub-Gate Area (Pillar Room West Ledge)": [
        # Connects to (Cardon Forest Surface Exit), (Junk Man Rescue Exit)
        locationDataDict["Underground ruins, 2 box ledge chest"],
    ],
    "Underground Ruins - Cardon Forest Sub-Gate Area (Junk Man Rescue Exit)": [
        # Connects (Pillar Room West Ledge) w/ jump springs, (Main Gate exit) w/ drill arm, (Junk man rescue spot)
    ],
    "Underground Ruins - Cardon Forest Sub-Gate Area (Main Gate Exit)": [
        # Connects to (Junk Man Rescue Exit) w/ drill, main gate ?? 
        locationDataDict["Underground ruins, Main gate entrance chest"]
    ],
    "Underground Ruins - Junk Man Rescue Area (Junk Man Rescue Spot)": [
        # Connects to (Junk Man Rescue Exit), (Sewer Ledge) w/ jump springs
        locationDataDict["Underground ruins, Junk store man chest"],
        locationDataDict["Underground ruins, Junk store man hole"]
    ],
    "Underground Ruins - Junk Man Rescue Area (Sewer Ledge)": [
        # Connects to (Junk Man Rescue Spot), City Sewer
    ],
    "Underground Ruins - City Sewer": [
        # Connects to (Sewer Ledge), Downtown
    ],
    "Underground Ruins - Shekuten + Kuruguru Area (Shekuten Lower)": [
        # Connects (Arukoitan Battle), (Kuruguru Upper) w/ jump springs, (Pillar Room West Ledge)
        locationDataDict["Underground ruins, Shekuten pillar room hole"]
    ],
    "Underground Ruins - Shekuten + Kuruguru Area (Kuruguru Upper)": [
        # Connects (Shekuten Lower), (Exit to Kuruguru)
        locationDataDict["Underground ruins, Shekuten pillar room chest"],
        locationDataDict["Underground ruins, Kuruguru obstacle hole"]
    ],
    "Underground Ruins - Lake Jyun Sub-Gate Area (Exit to Kuruguru)": [
        # Connects (Kuruguru Upper), (Gorubeshu Side Room) w/ drill, (Gorubeshu Walls) w/ drill
        locationDataDict["Underground ruins, Drillable pillar room south chest"]
    ],
    "Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Side Room)": [
        # Connects (Exit to Kuruguru) w/ drill, (Gorubeshu Trap Chests) w/ drill
        locationDataDict["Underground ruins, Gold Gorubesshu chest"]
    ],
    "Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Trap Chests)": [
        # Connects (Gorubeshu Side Room) w/ drill, (Lake Jyun sub-gate west exit) w/ drill
        locationDataDict["Underground ruins, 3 chest room middle chest"]
    ],
    "Underground Ruins - Lake Jyun Sub-Gate Area (Lake Jyun Sub-Gate West Exit)": [
        # Connects (Gorubeshu Trap Chests) w/ drill, Lake Jyun sub-gate
        locationDataDict["Underground ruins, Drillable pillar room north chest"]
    ],
    "Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Walls)": [
        # Connects (East Exit) w/ drill, (Exit to Kuruguru) w/ drill
        locationDataDict["Underground ruins, Drillable pillars room south hole"],
        locationDataDict["Underground ruins, Drillable pillars room west hole"],
        locationDataDict["Underground ruins, Drillable pillars room north hole"]
    ],
    "Underground Ruins - Lake Jyun Sub-Gate Area (Lake Jyun Sub-Gate East Exit)": [
        # Connects (Gorubeshu Walls) w/ drill, Lake Jyun sub-gate
    ],
    "Underground Ruins - Main Gate to Old City Connection": [

    ],
    "Main Gate - (Entrance)": [
        locationDataDict["Main Gate, Entrance hole"],
        locationDataDict["Main Gate, Two Gorubesshu room chest"],
    ],
    "Main Gate - (Command Terminal)": [
        # Connects to (Juno Area) with the sub-city keys
        locationDataDict["Main Gate, Maze Chest"],
        locationDataDict["Main Gate, Maze entrance hole"],
        locationDataDict["Main Gate, Maze Karumuna Bash hole"],
        locationDataDict["Main Gate, Maze Reaverbot hole"]
    ],
    "Main Gate - (Juno Area)": [
        # Technically this connects to Cardon Forest after killing Juno.
        locationDataDict["Main Gate, Boss corridor chest"]
    ],
    "Old City Sub-City - City": [
        # Connects with Old City (Inside Warehouse Gate)
    ],
    "Old City Sub-City - Chest": [
        locationDataDict["Old City Sub-City, Chest"]
    ],
    "Downtown Sub-City - City": [

    ],
    "Downtown Sub-City - Chest": [
        locationDataDict["Downtown Sub-City, Chest"]
    ],
    "Uptown Sub-City - City": [

    ],
    "Uptown Sub-City - Chest": [
        locationDataDict["Uptown Sub-City, Chest"]
    ]
}

def create_all_regions(world: MMLWorld) -> list:
    #create_region = lambda region_name, location_table: create_region_generic(world, region_name, location_table)
    universe = Region("Universe", world.player, world.multiworld)
    regions = [universe]
    world.multiworld.regions += regions

def connect_regions(world: MMLWorld) -> None:
    universe = world.get_region("Universe")
    pass