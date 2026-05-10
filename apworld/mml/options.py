import typing
from dataclasses import dataclass
from Options import Toggle, DefaultOnToggle, Option, Range, Choice, ItemDict, DeathLink, PerGameCommonOptions, OptionGroup

# Each option is its own class. options can be broken up into different categories:
# Toggle: options that are either on or off, like a hard mode or adding an item to the item pool.
# Range: options that have a mix/max value, like damage amplification, or a chance of something happening.
# Choice: an option where you pick a discrete answer like in a dropdown menu.

class Goal(Choice):
    """
    Juno: Defeat Juno.
    All Bosses: Defeat all bosses with a healthbar.
    """

    display_name = "Goal"

    option_juno = 0
    option_all_bosses = 1

    default = option_juno

@dataclass
class GameOptions(PerGameCommonOptions):
    goal: Goal

option_presets = {}
