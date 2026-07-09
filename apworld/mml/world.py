from . import items, locations, regions, rules
from . import options as mml_options  
from collections.abc import Mapping
from typing import Any
from worlds.AutoWorld import World, WebWorld # Imports of base Archipelago modules must be absolute.

class MMLWebWorld(WebWorld):
    option_groups = mml_options.option_groups

class GameWorld(World):
    """
    Mega Man Legends is a 1997 action-adventure game released by Capcom. It is the first game in the Mega Man Legends
    sub-series of Mega Man games from Capcom. Explore dungeons, fight pirates and save kattlelox island from its
    imminent demise.
    """

    game = "Mega Man Legends"

    options_dataclass = mml_options.GameOptions
    options: mml_options.GameOptions
    web = MMLWebWorld()
    topology_present: bool = True

    location_name_to_id = locations.LOCATION_NAME_TO_ID
    item_name_to_id = items.ITEM_NAME_TO_ID
    item_name_groups = items.ITEM_NAME_GROUPS

    # TODO: Remember to update this!
    ap_world_version = "0.3.0"

    starting_special_weapon: int;

    origin_region_name = "Ocean Tower - Room 1"

    def generate_early(self) -> None:
        if not self.options.shuffleStartingSpecialWeapon.value:
            self.starting_special_weapon = 11
        else:
            weapon_map = {
                "Normal Arm": 0,
               #"Mega Buster Sidearm": 1
                "Machine Buster": 2,
                "Powered Buster": 3,
                "Drill Arm": 4,
                "Grenade Arm": 5,
                "Spread Buster": 6,
                "Vacuum Arm": 7,
                "Active Buster": 8,
                "Blade Arm": 9,
                "Grand Grenade": 10,
                "Splash Mine": 11,
                "Shield Arm": 12,
                "Shining Laser": 13,
            }
            weapon_pool = [weapon_map[key] for key in self.options.shuffleStartingSpecialWeaponOptions.value]
            self.starting_special_weapon = self.random.choice(weapon_pool) if weapon_pool else weapon_map["Splash Mine"]

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
                "shuffleStartingSpecialWeapon": self.options.shuffleStartingSpecialWeapon.value,
                "shuffleCitizensCard": self.options.shuffleCitizensCard.value,
                "shuffleClassBLicense": self.options.shuffleClassBLicense.value,
                "shuffleClassALicense": self.options.shuffleClassALicense.value,
                "shuffleMainGateUnlock": self.options.shuffleMainGateUnlock.value,
                "shuffleSubCitiesUnlock": self.options.shuffleSubCitiesUnlock.value,
            },
            # Returns a bit offset to be added to 0xBE410
            # 0 = Normal Arm, 2 = Machine Buster, 3 = Powered Buster, 4 = Drill Arm, 5 = Grenade Arm, 
            # 6 = Spread Buster, 7 = Vacuum Arm, 8 = Active Buster, 9 = Blade Arm, 10 = Grand Grenade, 
            # 11 = Splash Mine (default), 12 = Shield Arm, 13 = Shining Laser
            "startingSpecialWeapon": self.starting_special_weapon,
        }
