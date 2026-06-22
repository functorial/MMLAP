from . import items, locations, regions, rules
from . import options as mml_options  
from collections.abc import Mapping
from typing import Any
from worlds.AutoWorld import World # Imports of base Archipelago modules must be absolute.

class GameWorld(World):
    """
    Mega Man Legends is a 1997 action-adventure game released by Capcom. It is the first game in the Mega Man Legends
    sub-series of Mega Man games from Capcom. Explore dungeons, fight pirates and save kattlelox island from its
    imminent demise.
    """

    game = "Mega Man Legends"

    options_dataclass = mml_options.GameOptions
    options: mml_options.GameOptions
    topology_present: bool = True

    location_name_to_id = locations.LOCATION_NAME_TO_ID
    item_name_to_id = items.ITEM_NAME_TO_ID
    item_name_groups = items.ITEM_NAME_GROUPS

    # TODO: Remember to update this!
    ap_world_version = "0.3.0"

    starting_special_weapon: int;

    origin_region_name = "Ocean Tower - Room 1"

    def generate_early(self) -> None:
        self.starting_special_weapon = 11 if not self.options.randomizeStartingSpecialWeapon.value else self.random.randint(0, 13)

    def create_regions(self) -> None:
        regions.create_and_connect_regions(self)
        locations.create_all_locations(self)
        locations.lock_missables_to_filler(self)

    def set_rules(self) -> None:
        rules.set_all_rules(self)

    def create_items(self) -> None:
        items.create_all_items(self)

    def create_item(self, name: str) -> items.GameItem:
        return items.create_item_with_correct_classification(self, name)

    def get_filler_item_name(self) -> str:
        return items.get_random_filler_item_name(self)

    def fill_slot_data(self) -> Mapping[str, Any]:
        # Archipelago.Core expects a different format than self.options.as_dict
        return {
            "apworldVersion": self.ap_world_version,
            "options": {
                "goal": self.options.goal.value,
                "randomizeStartingSpecialWeapon": self.options.randomizeStartingSpecialWeapon.value,
            },
            # Returns a bit offset to be added to 0xBE410
            # 0 = Normal Arm, 2 = Machine Buster, 3 = Powered Buster, 4 = Drill Arm, 5 = Grenade Arm, 
            # 6 = Spread Buster, 7 = Vacuum Arm, 8 = Active Buster, 9 = Blade Arm, 10 = Grand Grenade, 
            # 11 = Splash Mine (default), 12 = Shield Arm, 13 = Shining Laser
            "startingSpecialWeapon": self.starting_special_weapon,
        }
