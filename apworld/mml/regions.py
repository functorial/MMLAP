from __future__ import annotations
from BaseClasses import Region, CollectionState
from typing import TYPE_CHECKING, Callable, NamedTuple, Dict

if TYPE_CHECKING:
    from .world import GameWorld

class ExitData(NamedTuple):
    defaultTargetRegionName: str
    rule: Callable[[CollectionState], bool] = lambda state: True

class GameRegionData(NamedTuple):
    locationNameList: list[str]
    exitDataList: list[ExitData]

def run_unit_tests(world:GameWorld):
    region_data_dict = get_regionDataDict(world)
    # Test 1: Print one-way entrances
    one_way_entrances = []
    for source_region_name in region_data_dict.keys():
        for exitData in region_data_dict[source_region_name].exitDataList:
            target_region_name = exitData.defaultTargetRegionName
            target_region_exits = region_data_dict[target_region_name].exitDataList
            if source_region_name not in [target_target.defaultTargetRegionName for target_target in target_region_exits]:
                one_way_entrances.append(f"{source_region_name} -> {target_region_name}")
    print("One-way entrances:")
    for e in one_way_entrances:
        print(f"\t{e}")

def create_and_connect_regions(world: GameWorld) -> None:
    regionDataDict = get_regionDataDict(world)
    run_unit_tests(world)
    # Creating all regions
    regions = []
    for region_name in regionDataDict.keys():
        regions.append(Region(region_name, world.player, world.multiworld))
    world.multiworld.regions += regions

    # Attaching exits to regions
    # Entrance randomization logic would go here using something like
    # source_region.create_exit("..."); target_region.create_er_target("...");
    # instead of using source_region.connect(target_region)
    for source_region_name in regionDataDict.keys():
        source_region = world.get_region(source_region_name)
        source_region_data = regionDataDict[source_region_name]
        for target_region_data in source_region_data.exitDataList:
            target_region_name = target_region_data.defaultTargetRegionName
            target_region = world.get_region(target_region_name)
            source_region.connect(
                connecting_region = target_region,
                name = f"{source_region_name} -> {target_region_name}",
                rule = target_region_data.rule
            )

def get_regionDataDict(world: GameWorld) -> Dict[str, GameRegionData]:
    def has_item(item_name: str) -> Callable[[CollectionState], bool]:
        return lambda state: state.has(item_name, world.player)
    
    def has_any(bool_func_list: list[Callable[[CollectionState], bool]]) -> Callable[[CollectionState], bool]:
        return lambda state: any([f(state) for f in bool_func_list])
    
    def has_all(bool_func_list: list[Callable[[CollectionState], bool]]=[]) -> Callable[[CollectionState], bool]:
        return lambda state: all([f(state) for f in bool_func_list])
    
    def has_all_items(item_names:list[str]=[]) -> Callable[[CollectionState], bool]:
        return has_all([has_item(item_name) for item_name in item_names])
    
    def has_jump_springs() -> Callable[[CollectionState], bool]:
        return has_item("Jump Springs")

    def has_jet_skates() -> Callable[[CollectionState], bool]:
        return has_all_items(["Rollerboard", "Old Hoverjets"])
    
    def has_citizens_card() -> Callable[[CollectionState], bool]:
        return has_item("Citizen's Card")
    
    def has_class_b_license() -> Callable[[CollectionState], bool]:
        return has_item("Class B License")
    
    def has_class_a_license() -> Callable[[CollectionState], bool]:
        return has_item("Class A License")
    
    def has_unlocked_main_gate() -> Callable[[CollectionState], bool]:
        return has_item("Unlock Main Gate")
    
    def has_unlocked_sub_cities() -> Callable[[CollectionState], bool]:
        return has_item("Unlock Sub-Cities")
    
    def has_drill_arm() -> Callable[[CollectionState], bool]:
        return has_all([can_fix_support_car(), has_item("Blunted Drill")])
    
    def has_grand_grenade() -> Callable[[CollectionState], bool]:
        return has_all([can_fix_support_car(), has_item("Bomb Schematic")])
    
    def can_destroy_cracked_walls() -> Callable[[CollectionState], bool]:
        return has_any([has_drill_arm(), has_grand_grenade()])

    def has_explosive_wep() -> Callable[[CollectionState], bool]:
        has_powered_buster = has_all([can_fix_support_car(), has_item("Cannon Kit")])
        has_grand_grenade = has_all([can_fix_support_car(), has_item("Bomb Schematic")])
        has_active_buster = has_all([can_fix_support_car(), has_item("Guidance Unit")])
        has_spread_buster = has_all([can_fix_support_car(), has_all_items(["Ancient Book", "Old Launcher", "Arm Supporter"])])
        return has_any([has_powered_buster, has_grand_grenade, has_active_buster, has_spread_buster])

    def has_clubhouse_items() -> Callable[[CollectionState], bool]:
        # The reward for this quest is not randomized, but the items for it are.
        items = [
            "Pick",
            "Saw",
            # These items are currently vanilla.
            #"Stag Beetle",
            #"Beetle",
            #"Comic Book",
        ]
        return has_all([can_steal_yellow_refractor(), has_all_items(items)])

    def has_museum_items() -> Callable[[CollectionState], bool]:
        items = [
           #"Lipstick",
            "Old Bone",
           #"Old Heater",
            "Old Doll",
            "Antique Bell",
            "Giant Horn",
            "Shiny Object",
            "Old Shield",
            "Shiny Red Stone",
        ]
        # Use has_clubhouse_items() instead of checking for Old Heater, since it isn't randomized.
        return has_all([has_all_items(items), has_clubhouse_items()])
    
    def has_cardon_forest_keys() -> Callable[[CollectionState], bool]:
        items = [
            "Cardon Forest Sub-Gate Key 1",
            "Cardon Forest Sub-Gate Key 2",
            "Cardon Forest Sub-Gate Key 3",
        ]
        return has_all_items(items)

    def has_lake_jyun_keys() -> Callable[[CollectionState], bool]:
        items = [
            "Lake Jyun Sub-Gate Key 1",
            "Lake Jyun Sub-Gate Key 2",
            "Lake Jyun Sub-Gate Key 3",
        ]
        return has_all_items(items)

    def has_clozer_woods_keys() -> Callable[[CollectionState], bool]:
        items = [
            "Clozer Woods Sub-Gate Key 1",
            "Clozer Woods Sub-Gate Key 2",
            "Clozer Woods Sub-Gate Key 3",
        ]
        return has_all_items(items)

    def has_sub_city_keys() -> Callable[[CollectionState], bool]:
        items = [
            "'Watcher' Key",
            "'Sleeper' Key",
            "'Dreamer' Key",
        ]
        return has_all_items(items)
    
    def can_fix_support_car() -> Callable[[CollectionState], bool]:
        return has_citizens_card()
    
    def can_defeat_bon_bonne() -> Callable[[CollectionState], bool]:
        return has_citizens_card()
    
    def can_defeat_marlwolf() -> Callable [[CollectionState], bool]:
        return has_citizens_card()

    def can_steal_yellow_refractor() -> Callable[[CollectionState], bool]:
        return has_all([
            has_cardon_forest_keys(), 
            has_class_a_license(),
        ])
    
    def can_steal_red_refractor() -> Callable[[CollectionState], bool]:
        return has_all([
            has_lake_jyun_keys(),
            has_class_a_license(),
            has_any([
                # Fix the boat and go through as usual
                has_all([
                    has_citizens_card(),
                    has_item("Yellow Refractor"),
                    has_jump_springs(),
                ]),
                # Go through underground ruins
                has_all([
                    has_jump_springs(),
                    can_destroy_cracked_walls(),
                ]),
            ]),
        ])
    
    def can_fix_boat() -> Callable [[CollectionState], bool]:
        return has_all([has_item("Yellow Refractor"), has_citizens_card()])
    
    def can_fix_flutter() -> Callable [[CollectionState], bool]:
        return has_all([
            has_item("Red Refractor"), 
            can_fix_support_car(),
        ])
    
    def can_open_main_gate() -> Callable[[CollectionState], bool]:
        return has_all([
            has_clozer_woods_keys(),
            has_class_a_license(),
            has_any([
                # Fix flutter and go through there
                can_fix_flutter(),
                # Go through underground ruins
                has_all([
                    has_class_b_license(),
                    has_drill_arm(),
                ]),
            ]),
        ])

    # Current Assumptions:
    # - Yellow Refractor = No requirement b/c cardon keys aren't randomized
    # - Balkon Gerat defeated = No req b/c +2 range upgrade in shop isn't randomized
    # - Red Refractor = Lake Jyun Requirements = Jump springs & Lake Jyun keys
    # - Sub City = Lake Jyun and Clozer Requirements = Jump springs & Lake Jyun keys & Clozer keys (& Explosive ?? Not sure)
    # - Clozer sub-gate is not accessible from ruins besides (Gorubesshu Corridor), the other doors dont work
    # - You can always get to Roll to create stuff from where you obtain stuff
    # - You can always farm up as much zenny as you need
    # - You can always buy shop items
    # - You can always repair the damaged buildings
    # - You can always "complete a quest" to progress things like hideout, repairing buildings, etc.
    # - A bunch of logic not included because it is not randomized in (e.g. items not randomized like Citizen Card, door unlocks, etc.)
    #
    # Known soft-locks:
    # - Can get stuck in Old City warehouse before the Bruno fight is enabled.
    regionDataDict = {
        "Ocean Tower - Room 1": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Ocean Tower - Room 2"),
                ] 
            ),
        "Ocean Tower - Room 2": 
            GameRegionData(
                [
                    "Ocean tower, Right chest",
                    "Ocean tower, Left chest",
                ],
                [
                    ExitData("Ocean Tower - Room 1"),
                    ExitData("Ocean Tower - Room 3"),
                ]
            ),
        "Ocean Tower - Room 3": 
            GameRegionData(
                [
                    "Escape the Ocean Tower",
                ],
                [
                    ExitData("Ocean Tower - Room 2"),    # NOTE: This doesn't actually connect in game, but I'm forcing it in logic
                    ExitData("Cardon Forest"),
                ]
            ),
        "Cardon Forest": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Ocean Tower - Room 3"),     # NOTE: This doesn't actually connect in game, but I'm forcing it in logic
                    ExitData("Apple Market"),
                    ExitData("Underground Ruins - Junk Man Rescue Area (Junk Man Rescue Spot)"),
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Surface Exit)", has_class_b_license()),
                    ExitData("Outside Cardon Forest Sub-Gate"),
                    ExitData("Flutter - Common Room", can_fix_flutter()),
                    ExitData("Support Car / R&D Room", can_fix_support_car()),
                    ExitData("Cardon Forest (Get Citizen's Card)"),
                ]
            ),
        "Cardon Forest (Get Citizen's Card)": 
            GameRegionData(
                [
                    "Earn citizenship in Kattelox City",
                ],
                [
                    ExitData("Cardon Forest"),

                ]
            ),
        "Flutter - Common Room": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Cardon Forest"),
                    ExitData("Flutter - Barrell's Room"),
                    ExitData("Flutter - Mega Man's Room"),
                    ExitData("Flutter - Roll's Room"),
                    ExitData("Clozer Woods Sub-Gate - Entrance"),
                ]
            ),
        "Flutter - Barrell's Room": 
            GameRegionData(
                [
                    "Flutter, Study chest",
                ],
                [
                    ExitData("Flutter - Common Room"),
                ]
            ),
        "Flutter - Mega Man's Room": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Flutter - Common Room"),
                ]
            ),
        "Flutter - Roll's Room": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Flutter - Common Room"),
                    ExitData("Support Car / R&D Room", can_fix_support_car()), # Require support car fixed?
                ]
            ),
        "Support Car / R&D Room": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Support Car / R&D Room (Gift Flower)", can_fix_support_car()), #, has_item("Flower")), - Flower is currently vanilla.
                    ExitData("Support Car / R&D Room (Gift Music Box)", has_all([can_fix_support_car(), has_item("Music Box")])),
                    ExitData("Support Car / R&D Room (Gift Ring)", has_all([can_fix_support_car(), has_item("Ring")])),
                    ExitData("Flutter - Roll's Room", can_steal_red_refractor()),
                    ExitData("Cardon Forest"),
                    ExitData("Downtown - Outside", has_citizens_card()),
                    ExitData("City Hall - Outside", has_citizens_card()),
                    ExitData("Uptown", has_citizens_card()),
                    ExitData("Old City", has_citizens_card()),
                    ExitData("Outside Cardon Forest Sub-Gate"),
                ]
            ),
        "Support Car / R&D Room (Gift Flower)": 
            GameRegionData(
                [
                    "Gift Flower to Roll",
                ],
                [
                    ExitData("Support Car / R&D Room"),
                ]
            ),
        "Support Car / R&D Room (Gift Music Box)": 
            GameRegionData(
                [
                    "Gift Music Box to Roll",
                ],
                [
                    ExitData("Support Car / R&D Room"),
                ]
            ),
        "Support Car / R&D Room (Gift Ring)": 
            GameRegionData(
                [
                    "Gift Ring to Roll",
                ],
                [
                    ExitData("Support Car / R&D Room"),
                ]
            ),
        "Apple Market": 
            GameRegionData(
                [
                    "Apple market, Electric goods box",
                   #"Apple market, Book store box",
                    "Apple market, Junk store box",
                    "Apple market, North pail",
                    "Apple market, South pail",
                ],
                [
                    ExitData("Cardon Forest"),
                    ExitData("Apple Market - Junk Shop"),
                    ExitData("Apple Market - Electronics Shop"),
                    ExitData("Apple Market - Hip Bone"),
                    ExitData("Apple Market - Tailor Chinos"),
                    ExitData("Apple Market - Record Shop"),
                    ExitData("Downtown - Outside", has_citizens_card()),

                ]
            ),
        "Apple Market - Junk Shop": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Apple Market"),
                    ExitData("Apple Market - Junk Shop (Turn-in Rescue)"),
                    ExitData("Apple Market - Junk Shop (Shop Enabled)"),
                ]
            ),
        "Apple Market - Junk Shop (Turn-in Rescue)": 
            GameRegionData(
                [
                   "Rescue the shop owner's husband",
                ],
                [
                    ExitData("Apple Market - Junk Shop"),
                ]
            ),
        "Apple Market - Junk Shop (Shop Enabled)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Apple Market - Junk Shop"),
                ]
            ),
        "Apple Market - Electronics Shop": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Apple Market"),
                ]
            ),
        "Apple Market - Hip Bone": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Apple Market"),
                ]
            ),
        "Apple Market - Tailor Chinos": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Apple Market"),
                ]
            ),
        "Apple Market - Record Shop": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Apple Market"),
                ]
            ),
        "Downtown - Outside": 
            GameRegionData(
                [
                    "Downtown, South east peace sign pail",
                    "Downtown, Center east pail",
                ],
                [
                    ExitData("Apple Market"),
                    ExitData("Downtown - Outside (Boss fight)"),
                    ExitData("Downtown - Outside (Blumebear pail)"), # Only lootable after city hall is saved
                    ExitData("Downtown - Outside (Lost Bag)", has_all([has_jump_springs(), can_steal_red_refractor()])), # Assume jump springs for bomb quest instead of can take yellow refractor 
                    ExitData("Downtown - Outside (Discarded Saw)", has_all([can_steal_yellow_refractor(), has_item("Pick")])),
                    ExitData("Downtown - Library"),
                    ExitData("City Hall - Outside"),
                    ExitData("Uptown"),
                    ExitData("Old City"),
                    ExitData("Underground Ruins - City Sewer"),
                    ExitData("Downtown Sub-City - City", has_unlocked_sub_cities()),
                    ExitData("Support Car / R&D Room", can_fix_support_car()),
                ]
            ),
        "Downtown - Outside (Boss fight)": 
            GameRegionData(
                [
                    "Ferdinand defeated",
                ],
                [
                    ExitData("Downtown - Outside"),
                ]
            ),
        "Downtown - Outside (Blumebear pail)": 
            GameRegionData(
                [
                    "Downtown, Don't kick us pail",
                ],
                [
                    ExitData("Downtown - Outside"),
                ]
            ),
        "Downtown - Outside (Lost Bag)": 
            GameRegionData(
                [
                    "Downtown, Center pail",
                ],
                [
                    ExitData("Downtown - Outside"),
                ]
            ),
        "Downtown - Outside (Discarded Saw)": 
            GameRegionData(
                [
                    "Downtown, Library pail",
                ],
                [
                    ExitData("Downtown - Outside"),
                ]
            ),
        "Downtown - Library": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Downtown - Outside"),
                ]
            ),
        "City Hall - Outside": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Downtown - Outside"),
                    ExitData("City Hall - Outside (Boss Fight)"),
                    ExitData("City Hall - City Hall 1st Floor"),
                    ExitData("City Hall - Police Station"),
                    ExitData("City Hall - Bank"),
                    ExitData("Yass Plains - Outside"),
                    ExitData("Support Car / R&D Room", can_fix_support_car()),
                ]
            ),
        "City Hall - Outside (Boss Fight)": 
            GameRegionData(
                [
                    "Bon Bonne defeated",
                ],
                [
                    ExitData("City Hall - Amelia's Office (Get Class B License)"),
                ]
            ),
        "City Hall - City Hall 1st Floor": 
            GameRegionData(
                [

                ],
                [
                    ExitData("City Hall - Outside"),
                    ExitData("City Hall - Amelia's Office"),
                ]
            ),
        "City Hall - Amelia's Office": 
            GameRegionData(
                [

                ],
                [
                    ExitData("City Hall - City Hall 1st Floor"),
                ]
            ),
        "City Hall - Amelia's Office (Get Class B License)": 
            GameRegionData(
                [
                    "Earn the Class B License",
                ],
                [
                    ExitData("City Hall - Amelia's Office"),
                ]
            ),
        "City Hall - Amelia's Office (Get Class A License)": 
            GameRegionData(
                [
                    "Earn the Class A License",
                ],
                [
                    ExitData("City Hall - Amelia's Office"),
                ]
            ),
        "City Hall - Police Station": 
            GameRegionData(
                [

                ],
                [
                    ExitData("City Hall - Outside"),
                    ExitData("City Hall - Inspector's Office"),
                ]
            ),
        "City Hall - Inspector's Office": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("City Hall - Police Station"),
                    ExitData("City Hall - Inspector's Office (Turn in Bag)", has_all([can_steal_yellow_refractor(), can_steal_red_refractor(), has_item("Bag")])),
                ]
            ),
        "City Hall - Inspector's Office (Turn in Bag)": 
            GameRegionData(
                [
                    "Turn in missing bag",
                ],
                [
                    ExitData("City Hall - Inspector's Office"),
                ]
            ),
        "City Hall - Bank": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("City Hall - Outside"),
                ]
            ),
        "Uptown": 
            GameRegionData(
                [
                    "Uptown, Hospital right pail",
                    "Uptown, Ocean corner pail",
                ],
                [
                    ExitData("Downtown - Outside"),
                    ExitData("Uptown - Hospital"),
                    ExitData("Museum - Floor 1"), # TODO: has lipstick
                    ExitData("Wily's Boat - Walkway"),
                    ExitData("Uptown - TV Station"),
                    ExitData("Uptown Sub-City - City", has_unlocked_sub_cities()),
                    ExitData("Support Car / R&D Room", can_fix_support_car()),
                    ExitData("Uptown - (Hospital left pail)", can_steal_yellow_refractor()),
                ]
            ),
        "Uptown - (Hospital left pail)":
            GameRegionData(
                [
                   "Uptown, Hospital left pail",
                ],
                [
                    ExitData("Uptown"),
                ]
            ),
        "Uptown - Hospital": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Uptown"),
                    ExitData("Uptown - Hospital (Ira's Room)", can_defeat_marlwolf()), #can_steal_red_refractor()),  # Moving this to earlier in the story
                    ExitData("Uptown - Hospital (Missing woman turn-in)", can_fix_flutter()) #can_open_main_gate()), # Moving this to earlier in the story
                ]
            ),
        "Uptown - Hospital (Ira's Room)": 
            GameRegionData(
                [
                    "Cure Ira's illness",
                ],
                [
                    ExitData("Uptown - Hospital"),
                ]
            ),
        "Uptown - Hospital (Missing woman turn-in)": 
            GameRegionData(
                [
                    "Save the missing woman",
                ],
                [
                    ExitData("Uptown - Hospital"),
                ]
            ),
        "Uptown - TV Station": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Uptown"),
                    ExitData("Uptown - TV Station (Beast Hunter Game)"),
                    ExitData("Uptown - TV Station (Balloon Fantasy Game)"),
                    ExitData("Uptown - TV Station (Racing Game)", has_jet_skates()),
                ]
            ),
        "Uptown - TV Station (Beast Hunter Game)": 
            GameRegionData(
                [
                    "Beast Hunter Rank A",
                ],
                [
                    ExitData("Uptown - TV Station"),
                ]
            ),
        "Uptown - TV Station (Balloon Fantasy Game)": 
            GameRegionData(
                [
                    "Balloon Fantasy Rank A",
                ],
                [
                    ExitData("Uptown - TV Station"),
                ]
            ),
        "Uptown - TV Station (Racing Game)": 
            GameRegionData(
                [
                    "Race Technical Course Rank A",
                    "Race Straight Course Rank A",
                    "Race Left Curve Course Rank A",
                ],
                [
                    ExitData("Uptown - TV Station"),
                ]
            ),
        "Wily's Boat - Walkway": 
            GameRegionData(
                [
                    "Wily's Boat, Right box",
                   #"Wily's Boat, Left box",
                ],
                [
                    ExitData("Uptown", has_citizens_card()),
                    ExitData("Wily's Boat - Inside"),
                ]
            ),
        "Wily's Boat - Inside": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Wily's Boat - Walkway"),
                    ExitData("Wily's Boat - Mechanic Area"),
                    ExitData("Wily's Boat - Dock"),

                ]
            ),
        "Wily's Boat - Mechanic Area": 
            GameRegionData(
                [
                    "Wily's Boat, Pail",
                ],
                [
                    ExitData("Wily's Boat - Inside"),
                ]
            ),
        "Wily's Boat - Dock": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Wily's Boat - Inside"),
                    ExitData("Lake Jyun - Boss Fight", has_item("Yellow Refractor")),
                    ExitData("Lake Jyun - Outside Sub-Gate", has_item("Yellow Refractor")),
                ]
            ),
        "Museum - Floor 1": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Uptown"),
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Museum - Floor 2": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Museum - Floor 1"),
                    ExitData("Museum - Floor 2 (Old Bone)", has_item("Old Bone")),
                    # TODO: I think this logic isn't quite right and you don't need the full quest done.
                    ExitData("Museum - Floor 2 (Old Heater)", has_clubhouse_items()),
                    ExitData("Museum - Floor 2 (Old Doll)", has_item("Old Doll")),
                    ExitData("Museum - Floor 2 (Antique Bell)", has_item("Antique Bell")),
                    ExitData("Museum - Floor 2 (Giant Horn)", has_item("Giant Horn")),
                    ExitData("Museum - Floor 2 (Shiny Object)", has_item("Shiny Object")),
                    ExitData("Museum - Floor 2 (Old Shield)", has_item("Old Shield")),
                    ExitData("Museum - Floor 2 (Shiny Red Stone)", has_item("Shiny Red Stone")),
                    ExitData("Museum - Floor 2 - (Exhibit Complete)", has_museum_items()),
                ]
            ),
        "Museum - Floor 2 (Old Bone)": 
            GameRegionData(
                [
                    "Museum donation, Old Bone",
                ],
                [
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Museum - Floor 2 (Old Heater)": 
            GameRegionData(
                [
                    # Old heater currently isn't in the item pool
                    "Museum donation, Old Heater",
                ],
                [
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Museum - Floor 2 (Old Doll)": 
            GameRegionData(
                [
                    "Museum donation, Old Doll",
                ],
                [
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Museum - Floor 2 (Antique Bell)": 
            GameRegionData(
                [
                    "Museum donation, Antique Bell",
                ],
                [
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Museum - Floor 2 (Giant Horn)": 
            GameRegionData(
                [
                    "Museum donation, Giant Horn",
                ],
                [
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Museum - Floor 2 (Shiny Object)": 
            GameRegionData(
                [
                    "Museum donation, Shiny Object",
                ],
                [
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Museum - Floor 2 (Old Shield)": 
            GameRegionData(
                [
                    "Museum donation, Old Shield",
                ],
                [
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Museum - Floor 2 (Shiny Red Stone)": 
            GameRegionData(
                [
                    "Museum donation, Shiny Red Stone",
                ],
                [
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Museum - Floor 2 - (Exhibit Complete)": 
            GameRegionData(
                [
                   #"Complete the Museum exhibit",
                    "Take dangerous object from museum visitor",
                ],
                [
                    ExitData("Museum - Floor 2"),
                ]
            ),
        "Old City": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Downtown - Outside"),
                    ExitData("Outside Main Gate"),
                    ExitData("Old City - Power Plant"),
                    ExitData("Old City (Inside Warehouse Gate)", has_unlocked_main_gate()), # After killing Bruno
                    ExitData("Support Car / R&D Room", can_fix_support_car()),
                ]
            ),
        "Old City (Inside Warehouse Gate)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Underground Ruins - Main Gate to Old City Connection"),
                    ExitData("Old City (Boss Fight)"),
                    ExitData("Old City"),
                    ExitData("Old City Sub-City - City", has_unlocked_sub_cities()),
                ]
            ),
        "Old City (Boss Fight)": 
            GameRegionData(
                [
                    "Theodore Bruno defeated",
                ],
                [
                    ExitData("Old City (Inside Warehouse Gate)"),
                ]
            ),
        "Old City - Power Plant": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Old City"),
                ]
            ),
        "Outside Main Gate": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Old City", has_citizens_card()),
                    ExitData("Main Gate - (Entrance)", has_unlocked_main_gate()),
                ]
            ),
        "Yass Plains - Outside": 
            GameRegionData(
                [
                    "Yass plains, Plateau house box",
                   #"Yass plains, Plateau house pail",
                    "Yass plains, Behind hideout pail",
                   #"Yass plains, Across hideout pail",
                ],
                [
                    ExitData("City Hall - Outside", has_citizens_card()),
                    ExitData("Clozer Woods - Bridge Area"),
                    ExitData("Yass Plains - Hideout Stage 1", can_steal_yellow_refractor()),
                    ExitData("Yass Plains - Empty House"),
                    ExitData("Yass Plains - Junk Shop House"),
                ]
            ),
        "Yass Plains - Hideout Stage 1": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Yass Plains - Outside"),
                    ExitData("Yass Plains - Hideout Stage 2"),
                    ExitData("Yass Plains - Hideout Stage 3"),
                ]
            ),
        "Yass Plains - Hideout Stage 2": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Yass Plains - Hideout Stage 1"),
                ]
            ),
        "Yass Plains - Hideout Stage 3": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Yass Plains - Hideout Stage 1"),
                ]
            ),
        "Yass Plains - Empty House": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Yass Plains - Outside"),
                ]
            ),
        "Yass Plains - Junk Shop House": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Yass Plains - Outside"),
                ]
            ),
        "Clozer Woods - Bridge Area": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Yass Plains - Outside"),
                    ExitData("Clozer Woods - Boss Fight", can_defeat_bon_bonne()),
                    ExitData("Underground Ruins - Drillable Wall Area (Right)", has_class_b_license()),
                ]
            ),
        "Clozer Woods - Boss Fight": 
            GameRegionData(
                [
                    "Marlwolf defeated",
                ],
                [
                    ExitData("City Hall - Amelia's Office (Get Class A License)"),
                ]
            ),
        "Outside Cardon Forest Sub-Gate":
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Cardon Forest"),
                    ExitData("Cardon Forest Sub-Gate - Refractor Room (Lower)", has_class_a_license()),
                    ExitData("Support Car / R&D Room", can_fix_support_car()),
                ]
            ),
        "Cardon Forest Sub-Gate - Refractor Room (Upper)": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Sub-Gate Exit)", has_class_b_license()),
                    ExitData("Cardon Forest Sub-Gate - Refractor Room (Lower)"),
                ]
            ),
        "Cardon Forest Sub-Gate - Refractor Room (Lower)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Outside Cardon Forest Sub-Gate"),
                    ExitData("Cardon Forest Sub-Gate - Refractor Room (Upper)", has_jump_springs()),
                    ExitData("Cardon Forest Sub-Gate - Cliff Room (Lower)"),
                    ExitData("Cardon Forest Sub-Gate - Refractor Room (Lower, Get Refractor)", has_cardon_forest_keys()),
                ]
            ),
        "Cardon Forest Sub-Gate - Refractor Room (Lower, Get Refractor)": 
            GameRegionData(
                [
                    "Take the yellow refractor",
                ],
                [
                    ExitData("Cardon Forest Sub-Gate - Refractor Room (Lower)"),
                ]
            ),
        "Cardon Forest Sub-Gate - Cliff Room (Lower)": 
            GameRegionData(
                [
                    "Cardon Forest Sub-Gate, Sharukurusu floor hole",
                    "Cardon Forest Sub-Gate, Jakko nest starter key pickup",
                ],
                [
                    ExitData("Cardon Forest Sub-Gate - Refractor Room (Lower)"),
                    ExitData("Cardon Forest Sub-Gate - Conveyor Belts"),
                ]
            ),
        "Cardon Forest Sub-Gate - Conveyor Belts": 
            GameRegionData(
                [
                    "Cardon Forest Sub-Gate, Bottom conveyor hole",
                    "Cardon Forest Sub-Gate, Middle conveyor hole",
                    "Cardon Forest Sub-Gate, Middle switch chest",
                    "Cardon Forest Sub-Gate, Conveyor chest starter key pickup",
                ],
                [
                    ExitData("Cardon Forest Sub-Gate - Cliff Room (Lower)"),
                    ExitData("Cardon Forest Sub-Gate - Cliff Room (Upper)"),
                ]
            ),
        "Cardon Forest Sub-Gate - Cliff Room (Upper)":
            GameRegionData(
                [
                    "Cardon Forest Sub-Gate, Cliff hole",
                    "Cardon Forest Sub-Gate, Cliff chest",
                ],
                [
                    ExitData("Cardon Forest Sub-Gate - Cliff Room (Lower)"),
                    ExitData("Cardon Forest Sub-Gate - Conveyor Belts"),
                    ExitData("Cardon Forest Sub-Gate - Switch Room"),
                ]
            ),
        "Cardon Forest Sub-Gate - Switch Room":
            GameRegionData(
                [
                    "Cardon Forest Sub-Gate, Three switch starter key pickup",
                ],
                [
                    ExitData("Cardon Forest Sub-Gate - Cliff Room (Upper)"),
                    ExitData("Cardon Forest Sub-Gate - Cliff Room (Lower)"),
                ]
            ),
        "Lake Jyun - Boss Fight": 
            GameRegionData(
                [
                    # Requires +2 Range upgrade
                    "Balkon Gerat defeated",
                ],
                [
                    ExitData("Lake Jyun - Outside Sub-Gate"), #has_item("Range Booster Alpha"))
                ]
            ),
        "Lake Jyun - Outside Sub-Gate": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Wily's Boat - Dock", can_fix_boat()), # Assuming you can get to Wily's
                    ExitData("Lake Jyun Sub-Gate - Entrance", has_class_a_license()),
                ]
            ),
        "Lake Jyun Sub-Gate - Entrance": 
            GameRegionData(
                [
                    "Lake Jyun Sub-Gate, Entrance right hole",
                    "Lake Jyun Sub-Gate, Entrance left hole",
                    "Lake Jyun Sub-Gate, Entrance chest",
                ],
                [
                    ExitData("Lake Jyun - Outside Sub-Gate"),
                    ExitData("Lake Jyun Sub-Gate - Corridor Room", has_jump_springs()),
                ]
            ),
        "Lake Jyun Sub-Gate - Corridor Room": 
            GameRegionData(
                [
                    "Lake Jyun Sub-Gate, East corridor hole",
                    "Lake Jyun Sub-Gate, West corridor hole",
                    "Lake Jyun Sub-Gate, West corridor chest",
                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Entrance"),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Upper East)"),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Upper West)"),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Upper South)"),
                ]
            ),
        "Lake Jyun Sub-Gate - Sharukurusu Room (Upper South)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Corridor Room"),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Lower)"),
                ]
            ),
        "Lake Jyun Sub-Gate - Sharukurusu Room (Upper East)": 
            GameRegionData(
                [
                    "Lake Jyun Sub-Gate, Sharukurusu east chest",
                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Corridor Room"),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Lower)"),
                ]
            ),
        "Lake Jyun Sub-Gate - Sharukurusu Room (Upper West)": 
            GameRegionData(
                [
                    # Does not require jump springs from Corridor West entrance
                    "Lake Jyun Sub-Gate, Sharukurusu west chest",
                    "Lake Jyun Sub-Gate, Sharukurusu west hole",
                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Corridor Room"),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Lower)"),
                ]
            ),
        "Lake Jyun Sub-Gate - Sharukurusu Room (Pyramid)": 
            GameRegionData(
                [
                    "Lake Jyun Sub-Gate, Sharukurusu middle chest",
                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Lower)"),
                ]
            ),
        "Lake Jyun Sub-Gate - Sharukurusu Room (Lower)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Pyramid)", has_jump_springs()),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Upper East)", has_jump_springs()),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Upper West)", has_jump_springs()),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Upper North)", has_jump_springs()),
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Upper South)", has_jump_springs()),
                ]
            ),
        "Lake Jyun Sub-Gate - Sharukurusu Room (Upper North)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Lower)"),
                    ExitData("Lake Jyun Sub-Gate - Firushudot Hall"),
                ]
            ),
        "Lake Jyun Sub-Gate - Firushudot Hall": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Sharukurusu Room (Upper North)"),
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Lake Jyun Sub-Gate West Exit)", has_class_b_license()),
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Lake Jyun Sub-Gate East Exit)", has_class_b_license()),
                    ExitData("Lake Jyun Sub-Gate - Boss Room (Inactive)"),
                ]
            ),
        "Lake Jyun Sub-Gate - Boss Room (Inactive)": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Firushudot Hall"),
                    ExitData("Lake Jyun Sub-Gate - Refractor Room"),
                ]
            ),
        "Lake Jyun Sub-Gate - Refractor Room": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Boss Room (Inactive)"),
                    ExitData("Lake Jyun Sub-Gate - Boss Room (Active)", has_lake_jyun_keys()),
                    ExitData("Lake Jyun Sub-Gate - Refractor Room (Get Refractor)", has_lake_jyun_keys()),
                ]
            ),
        "Lake Jyun Sub-Gate - Refractor Room (Get Refractor)": 
            GameRegionData(
                [
                    "Take the red refractor",
                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Refractor Room"),
                ]
            ),
        "Lake Jyun Sub-Gate - Boss Room (Active)": 
            GameRegionData(
                [
                    "Garudoriten defeated",
                ],
                [
                    ExitData("Lake Jyun Sub-Gate - Boss Room (Inactive)"),
                ]
            ),
        "Clozer Woods Sub-Gate - Entrance": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Flutter - Common Room", can_fix_flutter()),
                    #ExitData("Clozer Woods - Boss Fight"), # Removing but this is possible by going through ruins -> clozer Sub-Gate -> out the entrance
                    ExitData("Clozer Woods Sub-Gate - Entrance Elevator Room"),
                    ExitData("Focke-Wulf Boss Area", has_all([can_fix_flutter(), can_open_main_gate()])),
                ]
            ),
        "Clozer Woods Sub-Gate - Entrance Elevator Room": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Entrance"),
                    ExitData("Clozer Woods Sub-Gate - Control Room"),
                    ExitData("Clozer Woods Sub-Gate - Sharukurusu Ambush"),
                ]
            ),
        "Clozer Woods Sub-Gate - Control Room": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Entrance Elevator Room"),
                    ExitData("Clozer Woods Sub-Gate - Control Room (Activate The Emergency System)", has_clozer_woods_keys()),
                ]
            ),
        "Clozer Woods Sub-Gate - Control Room (Activate The Emergency System)": 
            GameRegionData(
                [
                    "Activate the emergency system",
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Control Room"),
                ]
            ),
        "Clozer Woods Sub-Gate - Sharukurusu Ambush": 
            GameRegionData(
                [
                    "Clozer Woods Sub-Gate, Sharukurusu E room left hole",
                    "Clozer Woods Sub-Gate, Sharukurusu E room right hole",
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Entrance Elevator Room"),
                    ExitData("Clozer Woods Sub-Gate - Side Elevator Room (Upper)"),
                    ExitData("Clozer Woods Sub-Gate - Pillar Room (West Cliff)"),
                ]
            ),
        "Clozer Woods Sub-Gate - Side Elevator Room (Upper)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Sharukurusu Ambush"),
                    ExitData("Clozer Woods Sub-Gate - Side Elevator Room (Lower)"),
                ]
            ),
        "Clozer Woods Sub-Gate - Side Elevator Room (Lower)": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Side Elevator Room (Upper)"), # Need to hit generator, but don't need any items to do it
                    ExitData("Clozer Woods Sub-Gate - Pillar Room (Lower Level)"),
                ]
            ),
        "Clozer Woods Sub-Gate - Pillar Room (West Cliff)": 
            GameRegionData(
                [
                    "Clozer woods Sub-Gate, Miroc+Gorubesshu west cliff chest",
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Sharukurusu Ambush"),
                    ExitData("Clozer Woods Sub-Gate - Pillar Room (Lower Level)", has_jump_springs()),
                ]
            ),
        "Clozer Woods Sub-Gate - Pillar Room (Lower Level)": 
            GameRegionData(
                [
                    "Clozer woods Sub-Gate, Miroc+Gorubesshu southeast pillar hole",
                    "Clozer woods Sub-Gate, Miroc+Gorubesshu northeast pillar hole",
                    "Clozer woods Sub-Gate, Miroc+Gorubesshu northwest pillar hole",
                    "Clozer woods Sub-Gate, Miroc+Gorubesshu southwest pillar hole",
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Pillar Room (West Cliff)", has_jump_springs()),
                    ExitData("Clozer Woods Sub-Gate - Pillar Room (East Cliff)", has_jump_springs()),
                    ExitData("Clozer Woods Sub-Gate - Gorubesshu Corridor"),
                    ExitData("Clozer Woods Sub-Gate - Side Elevator Room (Lower)"),
                    ExitData("Clozer Woods Sub-Gate - Breakable Ceiling (Lower)"),
                ]
            ),
        "Clozer Woods Sub-Gate - Pillar Room (East Cliff)": 
            GameRegionData(
                [
                    "Clozer woods Sub-Gate, Miroc+Gorubesshu east cliff chest",
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Pillar Room (Lower Level)"),
                ]
            ),
        "Clozer Woods Sub-Gate - Breakable Ceiling (Lower)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Pillar Room (Lower Level)"),
                    ExitData("Clozer Woods Sub-Gate - Breakable Ceiling (Upper)", has_all([
                        has_explosive_wep(),
                        has_jump_springs(),
                    ])),
                ]
            ),
        "Clozer Woods Sub-Gate - Breakable Ceiling (Upper)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Breakable Ceiling (Lower)", has_explosive_wep()),
                    ExitData("Clozer Woods Sub-Gate - Boss Room"),
                ]
            ),
        "Clozer Woods Sub-Gate - Gorubesshu Corridor": 
            GameRegionData(
                [
                    "Clozer Woods Sub-Gate, Gorubesshu corridor east chest"
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Pillar Room (Lower Level)"), # Doors broken if not far enough
                    ExitData("Clozer Woods Sub-Gate - Generator Room (Lower)"), # Doors broken if not far enough
                    ExitData("Underground Ruins - Drillable Wall Area (Left-Middle, Lower)", has_class_b_license()),
                ]
            ),
        "Clozer Woods Sub-Gate - Generator Room (Lower)": 
            GameRegionData(
                [
                    "Clozer Woods Sub-Gate, Generator room lower chest",
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Gorubesshu Corridor"),
                ]
            ),
        "Clozer Woods Sub-Gate - Generator Room (Upper)": 
            GameRegionData(
                [
                    # Can drop down, Connects to Lower & Boss room
                    "Clozer Woods Sub-Gate, Generator room upper chest",
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Generator Room (Lower)"),
                    ExitData("Clozer Woods Sub-Gate - Boss Room"),
                ]
            ),
        "Clozer Woods Sub-Gate - Boss Room": 
            GameRegionData(
                [
                    "Karumuna Bash Trio defeated",
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Breakable Ceiling (Upper)"),
                    ExitData("Clozer Woods Sub-Gate - Generator Room (Upper)"),
                ]
            ),
        "Focke-Wulf Boss Area": 
            GameRegionData(
                [
                    # Connects from flutter after getting red refractor. Back to flutter on boss defeat
                    "Focke-Wulf defeated",
                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Entrance"),
                ]
            ),
        "Underground Ruins - Orudakoitan Bridge Area (Ledges)": 
            GameRegionData(
                [
                    "Underground ruins, Horokko ledge chest",
                    "Underground ruins, Box ledge chest",
                ],
                [
                    ExitData("Underground Ruins - Orudakoitan Bridge Area (Bridge)"),
                ]
            ),
        "Underground Ruins - Orudakoitan Bridge Area (Bridge)": 
            GameRegionData(
                [
                    "Underground ruins, Fireball Orudakoitan chest",
                ],
                [
                    ExitData("Underground Ruins - Orudakoitan Bridge Area (Ledges)", has_jump_springs()),
                    ExitData("Underground Ruins - Drillable Wall Area (Right)"),
                ]
            ),
        "Underground Ruins - Drillable Wall Area (Right)": 
            GameRegionData(
                [
                    "Underground ruins, Clozer exit chest",
                    "Underground ruins, Trapped box hole",
                ],
                [
                    ExitData("Underground Ruins - Drillable Wall Area (Right-Middle, Lower)", has_drill_arm()),
                    ExitData("Underground Ruins - Orudakoitan Bridge Area (Bridge)"),
                    ExitData("Clozer Woods - Bridge Area"),
                ]
            ),
        "Underground Ruins - Drillable Wall Area (Right-Middle, Lower)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Underground Ruins - Drillable Wall Area (Right)", has_drill_arm()),
                    ExitData("Underground Ruins - Drillable Wall Area (Left-Middle, Lower)", has_drill_arm()),
                    ExitData("Underground Ruins - Drillable Wall Area (Right-Middle, Upper)", has_jump_springs()),
                ]
            ),
        "Underground Ruins - Drillable Wall Area (Right-Middle, Upper)": 
            GameRegionData(
                [
                    "Underground ruins, Drillable wall room east cliff chest",
                    "Underground ruins, Drillable wall room east cliff hole",
                ],
                [
                    ExitData("Underground Ruins - Drillable Wall Area (Right-Middle, Lower)"),
                ]
            ),
        "Underground Ruins - Drillable Wall Area (Left-Middle, Lower)": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Clozer Woods Sub-Gate - Gorubesshu Corridor", has_class_a_license()),
                    ExitData("Underground Ruins - Drillable Wall Area (Left-Middle, Upper)", has_jump_springs()),
                    ExitData("Underground Ruins - Drillable Wall Area (Right-Middle, Lower)", has_drill_arm()),
                    ExitData("Underground Ruins - Drillable Wall Area (Left, Lower)", has_drill_arm()),
                ]
            ),
        "Underground Ruins - Drillable Wall Area (Left-Middle, Upper)": 
            GameRegionData(
                [
                    "Underground ruins, Drillable wall room middle cliff chest",
                ],
                [
                    ExitData("Underground Ruins - Drillable Wall Area (Left-Middle, Lower)"),
                ]
            ),
        "Underground Ruins - Drillable Wall Area (Left, Lower)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Underground Ruins - Spinning Tower Trap Area (Sharukurusu Corridor)"),
                    ExitData("Underground Ruins - Drillable Wall Area (Left-Middle, Lower)", has_drill_arm()),
                    ExitData("Underground Ruins - Drillable Wall Area (Left, Upper)", has_jump_springs()),
                ]
            ),
        "Underground Ruins - Drillable Wall Area (Left, Upper)": 
            GameRegionData(
                [
                    "Underground ruins, Drillable wall room west cliff chest",
                ],
                [
                    ExitData("Underground Ruins - Drillable Wall Area (Left, Lower)"),
                ]
            ),
        "Underground Ruins - Spinning Tower Trap Area (Sharukurusu Corridor)": 
            GameRegionData(
                [
                    "Underground ruins, Obstacle room cliff east hole",
                    "Underground ruins, Obstacle room cliff west hole",
                ],
                [
                    ExitData("Underground Ruins - Drillable Wall Area (Left, Lower)"),
                    ExitData("Underground Ruins - Spinning Tower Trap Area (Ledge Room)"),
                ]
            ),
        "Underground Ruins - Spinning Tower Trap Area (Ledge Room)": 
            GameRegionData(
                [
                    "Underground ruins, Arukoitan battle south chest",
                ],
                [
                    ExitData("Underground Ruins - Spinning Tower Trap Area (Sharukurusu Corridor)", has_jump_springs()),
                    ExitData("Underground Ruins - Spinning Tower Trap Area (Arukoitan Battle + Hanmuru Doll)"),
                ]
            ),
        "Underground Ruins - Spinning Tower Trap Area (Arukoitan Battle + Hanmuru Doll)": 
            GameRegionData(
                [
                    "Underground ruins, Arukoitan battle north chest",
                ],
                [
                    ExitData("Underground Ruins - Spinning Tower Trap Area (Ledge Room)", has_jump_springs()),
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Surface Exit)"),
                    ExitData("Underground Ruins - Shekuten + Kuruguru Area (Shekuten Lower)"),
                ]
            ),
        "Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Surface Exit)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Cardon Forest"),
                    ExitData("Underground Ruins - Spinning Tower Trap Area (Arukoitan Battle + Hanmuru Doll)"),
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Sub-Gate Exit)", has_jump_springs()),
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Pillar Room West Ledge)", has_jump_springs()),
                ]
            ),
        "Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Sub-Gate Exit)": 
            GameRegionData(
                [
                    "Underground ruins, Miroc room ledge chest",
                    "Underground ruins, Cross room chest",
                    "Underground ruins, Miroc room left hole",
                    "Underground ruins, Miroc room right hole",
                ],
                [
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Surface Exit)"),
                    ExitData("Cardon Forest Sub-Gate - Refractor Room (Upper)", has_class_a_license()),
                ]
            ),
        "Underground Ruins - Cardon Forest Sub-Gate Area (Pillar Room West Ledge)": 
            GameRegionData(
                [
                    "Underground ruins, 2 box ledge chest",
                ],
                [
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Cardon Forest Surface Exit)"),
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Junk Man Rescue Exit)"),
                    ExitData("Underground Ruins - Shekuten + Kuruguru Area (Shekuten Lower)"),
                ]
            ),
        "Underground Ruins - Cardon Forest Sub-Gate Area (Junk Man Rescue Exit)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Pillar Room West Ledge)", has_jump_springs()),
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Main Gate Exit)", has_drill_arm()),
                    ExitData("Underground Ruins - Junk Man Rescue Area (Junk Man Rescue Spot)"),
                ]
            ),
        "Underground Ruins - Cardon Forest Sub-Gate Area (Main Gate Exit)": 
            GameRegionData(
                [
                    "Underground ruins, Main gate entrance chest",
                ],
                [
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Junk Man Rescue Exit)", has_drill_arm()),
                    ExitData("Main Gate - (Maze)", has_unlocked_main_gate()),
                ]
            ),
        "Underground Ruins - Junk Man Rescue Area (Junk Man Rescue Spot)": 
            GameRegionData(
                [
                    "Underground ruins, Junk store man chest",
                    "Underground ruins, Junk store man hole",
                ],
                [
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Junk Man Rescue Exit)", has_class_b_license()),
                    ExitData("Underground Ruins - Junk Man Rescue Area (Sewer Ledge)", has_jump_springs()),
                    ExitData("Cardon Forest"),
                ]
            ),
        "Underground Ruins - Junk Man Rescue Area (Sewer Ledge)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Underground Ruins - Junk Man Rescue Area (Junk Man Rescue Spot)"),
                    ExitData("Underground Ruins - City Sewer"),
                ]
            ),
        "Underground Ruins - City Sewer": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Underground Ruins - Junk Man Rescue Area (Sewer Ledge)"),
                    ExitData("Downtown - Outside", has_citizens_card()),
                ]
            ),
        "Underground Ruins - Shekuten + Kuruguru Area (Shekuten Lower)": 
            GameRegionData(
                [
                    "Underground ruins, Shekuten platform room hole",
                ],
                [
                    ExitData("Underground Ruins - Spinning Tower Trap Area (Arukoitan Battle + Hanmuru Doll)"),
                    ExitData("Underground Ruins - Shekuten + Kuruguru Area (Kuruguru Upper)", has_jump_springs()),
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Pillar Room West Ledge)"),
                ]
            ),
        "Underground Ruins - Shekuten + Kuruguru Area (Kuruguru Upper)": 
            GameRegionData(
                [
                    "Underground ruins, Shekuten platform room chest",
                    "Underground ruins, Kuruguru obstacle hole",
                ],
                [
                    ExitData("Underground Ruins - Shekuten + Kuruguru Area (Shekuten Lower)"),
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Exit to Kuruguru)"),
                ]
            ),
        "Underground Ruins - Lake Jyun Sub-Gate Area (Exit to Kuruguru)": 
            GameRegionData(
                [
                    "Underground ruins, Drillable pillar room south chest",
                ],
                [
                    ExitData("Underground Ruins - Shekuten + Kuruguru Area (Kuruguru Upper)"),
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Side Room)", can_destroy_cracked_walls()),
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Walls)", can_destroy_cracked_walls()),
                ]
            ),
        "Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Side Room)": 
            GameRegionData(
                [
                    "Underground ruins, Gold Gorubesshu chest",
                ],
                [
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Exit to Kuruguru)", can_destroy_cracked_walls()),
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Trap Chests)", can_destroy_cracked_walls()),
                ]
            ),
        "Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Trap Chests)": 
            GameRegionData(
                [
                    "Underground ruins, 3 chest room middle chest",
                ],
                [
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Side Room)", can_destroy_cracked_walls()),
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Lake Jyun Sub-Gate West Exit)", can_destroy_cracked_walls()),
                ]
            ),
        "Underground Ruins - Lake Jyun Sub-Gate Area (Lake Jyun Sub-Gate West Exit)": 
            GameRegionData(
                [
                    "Underground ruins, Drillable pillar room north chest",
                ],
                [
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Trap Chests)", can_destroy_cracked_walls()),
                    ExitData("Lake Jyun Sub-Gate - Firushudot Hall", has_class_a_license()),
                ]
            ),
        "Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Walls)": 
            GameRegionData(
                [
                    "Underground ruins, Drillable pillars room south hole",
                    "Underground ruins, Drillable pillars room west hole",
                    "Underground ruins, Drillable pillars room north hole",
                ],
                [
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Lake Jyun Sub-Gate East Exit)", can_destroy_cracked_walls()),
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Exit to Kuruguru)", can_destroy_cracked_walls()),
                ]
            ),
        "Underground Ruins - Lake Jyun Sub-Gate Area (Lake Jyun Sub-Gate East Exit)": 
            GameRegionData(
                [
                    # Connects (Gorubeshu Walls) w/ drill, Lake Jyun sub-gate
                ],
                [
                    ExitData("Underground Ruins - Lake Jyun Sub-Gate Area (Gorubeshu Walls)", can_destroy_cracked_walls()),
                    ExitData("Lake Jyun Sub-Gate - Firushudot Hall", has_class_a_license()),
                ]
            ),
        "Underground Ruins - Main Gate to Old City Connection": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Main Gate - (Maze)", has_unlocked_main_gate()),
                    ExitData("Old City (Inside Warehouse Gate)", has_citizens_card()),
                ]
            ),
        "Main Gate - (Entrance)": 
            GameRegionData(
                [
                    "Main Gate, Entrance hole",
                    "Main Gate, Two Gorubesshu room chest",
                ],
                [
                    ExitData("Main Gate - (Command Terminal)"),
                    ExitData("Main Gate - Juno Area (Sub-City Key Doors)"),
                    ExitData("Outside Main Gate")
                ]
            ),
        "Main Gate - (Command Terminal)": 
            GameRegionData(
                [
                    "Activate unlock sub-cities",
                ],
                [
                    ExitData("Main Gate - (Entrance)"),
                    ExitData("Main Gate - (Maze)"),
                ]
            ),
        "Main Gate - (Maze)": 
            GameRegionData(
                [
                    "Main Gate, Maze Chest",
                    "Main Gate, Maze entrance hole",
                    "Main Gate, Maze Karumuna Bash hole",
                    "Main Gate, Maze Reaverbot hole",
                ],
                [
                    ExitData("Main Gate - (Command Terminal)"),
                    ExitData("Underground Ruins - Main Gate to Old City Connection"),
                    ExitData("Underground Ruins - Cardon Forest Sub-Gate Area (Main Gate Exit)", has_class_b_license()),
                ]
            ),
        "Main Gate - Juno Area (Sub-City Key Doors)": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Main Gate - (Entrance)"),
                    ExitData("Main Gate - Juno Area (Boss corridor)", has_sub_city_keys()),
                ]
            ),
        "Main Gate - Juno Area (Boss corridor)": 
            GameRegionData(
                [
                    "Main Gate, Boss corridor chest",
                ],
                [
                    ExitData("Main Gate - Juno Area (Sub-City Key Doors)", has_sub_city_keys()),
                    ExitData("Main Gate - Juno Area (Boss)"),
                ]
            ),
        "Main Gate - Juno Area (Boss)": 
            GameRegionData(
                [
                    "Juno defeated",
                ],
                [
                    ExitData("Cardon Forest"), # This one-time exit disappears after defeating Juno, but leaving it here for now
                ]
            ),
        "Old City Sub-City - City": 
            GameRegionData(
                [
                    
                ],
                [
                    ExitData("Old City (Inside Warehouse Gate)"),
                    ExitData("Old City Sub-City - Chest"),
                ]
            ),
        "Old City Sub-City - Chest": 
            GameRegionData(
                [
                    "Old City Sub-City, Chest",
                ],
                [
                    ExitData("Old City Sub-City - City"),
                ]
            ),
        "Downtown Sub-City - City": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Downtown - Outside"),
                    ExitData("Downtown Sub-City - Chest"),
                ]
            ),
        "Downtown Sub-City - Chest": 
            GameRegionData(
                [
                    "Downtown Sub-City, Chest",
                ],
                [
                    ExitData("Downtown Sub-City - City"),
                ]
            ),
        "Uptown Sub-City - City": 
            GameRegionData(
                [

                ],
                [
                    ExitData("Uptown"),
                    ExitData("Uptown Sub-City - Chest"),
                ]
            ),
        "Uptown Sub-City - Chest": 
            GameRegionData(
                [
                    "Uptown Sub-City, Chest",
                ],
                [
                    ExitData("Uptown Sub-City - City"),
                ]
            )
    }
    return regionDataDict
