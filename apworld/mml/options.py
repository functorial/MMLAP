import typing
from dataclasses import dataclass
from Options import Toggle, DefaultOnToggle, Option, Range, Choice, OptionSet, ItemDict, DeathLink, PerGameCommonOptions, OptionGroup

# Each option is its own class. options can be broken up into different categories:
# Toggle: options that are either on or off, like a hard mode or adding an item to the item pool.
# Range: options that have a mix/max value, like damage amplification, or a chance of something happening.
# Choice: an option where you pick a discrete answer like in a dropdown menu.

class Goal(Choice):
    """
    Juno: Defeat Juno.
    All Bosses: Defeat all bosses with a health bar.
    """

    display_name = "Goal"

    option_juno = 0
    option_all_bosses = 1

    default = option_juno

class ShuffleCitizensCard(Choice):
    """
    The Citizen's Card controls access to Kattelox city.
    Vanilla: The Citizen's Card is obtained normally.
    Randomized: Shuffles the Citizen's Card and location into the pool.
    Open: The Citizen's Card does nothing and the doors to the city are always open.
    """

    display_name = "Shuffle Citizens Card"

    option_vanilla = 0
    option_randomized = 1
    option_open = 2

    default = option_vanilla

class ShuffleClassBLicense(Choice):
    """
    The Class B License controls access to the Underground Ruins.
    Vanilla: The Class B License is obtained normally.
    Randomized: Shuffles the Class B License and location into the pool.
    Open: The Class B License does nothing and the doors to the Underground Ruins are always open.
    """

    display_name = "Shuffle Class B License"

    option_vanilla = 0
    option_randomized = 1
    option_open = 2

    default = option_vanilla

class ShuffleClassALicense(Choice):
    """
    The Class A License controls access to the three Sub-Gates.
    Vanilla: The Class A License is obtained normally.
    Randomized: Shuffles the Class A License and location into the pool.
    Open: The Class A License does nothing and the doors to the Sub-Gates are always open.
    """

    display_name = "Shuffle Class A License"

    option_vanilla = 0
    option_randomized = 1
    option_open = 2

    default = option_vanilla

class ShuffleMainGateUnlock(Choice):
    """
    Vanilla: Access to the Main Gate is obtained normally.
    Randomized: Shuffles a Main Gate Unlock item and location into the pool.
    Open: The doors to the Main Gate are always open.
    """

    display_name = "Shuffle Main Gate Unlock"

    option_vanilla = 0
    option_randomized = 1
    option_open = 2

    default = option_vanilla

class ShuffleSubCitiesUnlock(Choice):
    """
    Vanilla: Access to the Sub-Cities is obtained normally.
    Randomized: Shuffles a Sub-Cities Unlock item and location into the pool.
    Open: The doors to the Sub-Cities are always open.
    """

    display_name = "Shuffle Sub-Cities Unlock"

    option_vanilla = 0
    option_randomized = 1
    option_open = 2

    default = option_vanilla

class ShuffleStartingSpecialWeapon(Toggle):
    """
    Shuffles the Mine Parts Kit into the item pool and randomizes the first special weapon received.
    """

    display_name = "Shuffle Starting Special Weapon"

    default = False

class ShuffleStartingSpecialWeaponOptions(OptionSet):
    """
    If 'Shuffle Starting Special Weapon' is enabled, this option will determine the special weapon pool to shuffle from.
    If none are selected, the logic will default to the Splash Arm.
    """

    display_name = "Shuffle Starting Special Weapon Options"

    valid_weapons = {
        "Normal Arm",
       #"Mega Buster Sidearm",
        "Machine Buster",
        "Powered Buster",
        "Drill Arm",
        "Grenade Arm",
        "Spread Buster",
        "Vacuum Arm",
        "Active Buster",
        "Blade Arm",
        "Grand Grenade",
        "Splash Mine",
        "Shield Arm",
        "Shining Laser",
    }

    default = valid_weapons

class ShuffleBusterMax(Toggle):
    """
    Shuffles the Buster Max buster part into the item pool.
    """
    
    display_name = "Shuffle Buster Max"

    default = False


@dataclass
class GameOptions(PerGameCommonOptions):
    goal: Goal
    shuffleCitizensCard: ShuffleCitizensCard
    shuffleClassBLicense: ShuffleClassBLicense
    shuffleClassALicense: ShuffleClassALicense
    shuffleMainGateUnlock: ShuffleMainGateUnlock
    shuffleSubCitiesUnlock: ShuffleSubCitiesUnlock
    shuffleStartingSpecialWeapon: ShuffleStartingSpecialWeapon
    shuffleStartingSpecialWeaponOptions: ShuffleStartingSpecialWeaponOptions
    shuffleBusterMax: ShuffleBusterMax

option_presets = {}

option_groups = [
    OptionGroup(
        name = "Region Locks",
        options = [
            ShuffleCitizensCard,
            ShuffleClassBLicense,
            ShuffleClassALicense,
            ShuffleMainGateUnlock,
            ShuffleSubCitiesUnlock,
        ],
        start_collapsed = False,
    ),
    OptionGroup(
        name = "Weapon Options",
        options = [
            ShuffleStartingSpecialWeapon,
            ShuffleStartingSpecialWeaponOptions,
            ShuffleBusterMax,
        ],
        start_collapsed = False,
    ),
]