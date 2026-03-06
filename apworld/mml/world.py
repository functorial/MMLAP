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

    origin_region_name = "Ocean Tower - Room 1"

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
        # If you need access to the player's chosen options on the client side, there is a helper for that.
        return self.options.as_dict("goal")