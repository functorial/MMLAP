using Archipelago.Core;
using Archipelago.Core.AvaloniaGUI.Models;
using Archipelago.Core.AvaloniaGUI.ViewModels;
using Archipelago.Core.AvaloniaGUI.Views;
using Archipelago.Core.Helpers;
using Archipelago.Core.Models;
using Archipelago.Core.Util;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Models;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MMLAP.Helpers;
using MMLAP.Models;
using Newtonsoft.Json;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Timers;
using static MMLAP.Models.MMLEnums;

namespace MMLAP;

public partial class App : Application
{
    // TODO: Remember to set this in MMLAP.Desktop as well.
    public static string Version = "0.0.1";
    public static List<string> SupportedVersions = ["0.0.1"];

    public static MainWindowViewModel Context;
    public static ArchipelagoClient APClient { get; set; }
    public static List<ILocation> GameLocations { get; set; }
    public static Dictionary<long, ItemData> scoutedLocationItemData { get; set; }
    private static string _playerName { get; set; }
    private static bool _hasSubmittedGoal { get; set; }
    private static Timer _gameLoopTimer { get; set; }

    private static Timer _startMMLTimer { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        return;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Start();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Context
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainWindow
            {
                DataContext = Context
            };
        }
        base.OnFrameworkInitializationCompleted();
        return;
    }

    private static bool IsRunningAsAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    public void Start()
    {
        Context = new MainWindowViewModel("0.6.3 or later");
        Context.ClientVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();

        Context.ConnectClicked += Context_ConnectClicked;
        Context.CommandReceived += Context_CommandReceived;
        Context.OverlayEnabled = true;
        Context.AutoscrollEnabled = true;
        Context.ConnectButtonEnabled = true;

        _hasSubmittedGoal = false;

        Log.Logger.Information("This Archipelago Client is compatible only with the NTSC-U release of Mega Man Legends.");
        Log.Logger.Information("Trying to play with a different version will not work as intended.");
        if (!IsRunningAsAdministrator())
        {
            Log.Logger.Warning("You do not appear to be running this client as an administrator.");
            Log.Logger.Warning("This may result in errors or crashes when trying to connect to Duckstation.");
        }
        return;
    }

    private void Context_CommandReceived(object? sender, ArchipelagoCommandEventArgs a)
    {

        if (string.IsNullOrWhiteSpace(a.Command))
        {
            return;
        }
        Log.Logger.Information($"> {a.Command}");
        string command = a.Command.Trim().ToLower();
        switch (command)
        {
            case "reload":
                if (APClient != null && APClient.ItemManager != null)
                {
                    Log.Logger.Information("Clearing the game state.  Please reconnect to the server while in game to refresh received items.");
                    APClient.ItemManager.ForceReloadAllItems();
                    return;
                }
                else
                {
                    Log.Logger.Warning("Please connect the client before attempting reload.");
                }
                break;
            case "goal":
                string goalText;
                if (APClient != null && APClient.Options != null && APClient.Options.TryGetValue("goal", out var goalValueObj))
                {
                    int goalValue = goalValueObj as int? ?? 0;
                    CompletionGoal goal = (CompletionGoal)goalValue;
                    goalText = goal switch
                    {
                        CompletionGoal.Juno => "Defeat Juno",
                        _ => "Unknown",
                    };
                }
                else
                {
                    goalText = "Unknown";
                }
                Log.Logger.Information($"Your goal is: {goalText}.");
                break;
            default:
                Log.Logger.Information("Command not recognized. Did you mean one of the following?\n  [reload]: Forces all items to reload.\n  [goal]: Show current goal.");
                break;
        }
        return;
    }

    private async void Context_ConnectClicked(object? sender, ConnectClickedEventArgs e)
    {
        Context.ConnectButtonEnabled = false;
        Log.Logger.Information("Connecting..."); 

        // Refreshing subscriptions
        if (APClient != null)
        {
            APClient.Connected -= Client_Connected;
            APClient.Disconnected -= Client_Disconnected;
            APClient.MessageReceived -= Client_MessageReceived;
            if (APClient.ItemManager != null)
            {
                APClient.ItemManager.ItemReceived -= ItemManager_ItemReceived;
            }
            if (APClient.LocationManager != null)
            {
                APClient.LocationManager.CancelMonitors();
                APClient.LocationManager.EnableLocationsCondition = null;
                APClient.LocationManager.LocationCompleted -= LocationManager_LocationCompleted;
            }
            if (APClient.CurrentSession != null)
            {
                APClient.CurrentSession.Locations.CheckedLocationsUpdated -= CurrentSession_CheckedLocationsUpdated;
            }
        }

        // Connect to Duckstation
        GameClient? gameClient = null;
        try
        {
            gameClient = new GameClient("duckstation");
        }
        catch (ArgumentException ex)
        {
            Log.Logger.Warning("Duckstation not running, open Duckstation and launch the game before connecting!");
            Context.ConnectButtonEnabled = true;
            return;
        }
        try
        {
            bool connected = gameClient.Connect();
            if (!connected)
            {
                Log.Logger.Warning("Duckstation not running, open Duckstation and launch the game before connecting!");
                Context.ConnectButtonEnabled = true;
                return;
            }
        }
        catch (ArgumentException ex)
        {
            Log.Logger.Warning("Duckstation not running, open Duckstation and launch the game before connecting!");
            Context.ConnectButtonEnabled = true;
            return;
        }

        Memory.GlobalOffset = Memory.GetDuckstationOffset();

        // Initialize ArchipelagoClient
        APClient = new ArchipelagoClient(gameClient);
        APClient.Connected += Client_Connected;
        APClient.Disconnected += Client_Disconnected;
        APClient.MessageReceived += Client_MessageReceived;

        // Connect to host and log in to slot => init Options, ItemManager, LocationManager
        if (e.Host != null) {
            await APClient.Connect(e.Host, "Mega Man Legends");
        }
        if (!APClient.IsConnected)
        {
            Log.Logger.Error("Your host seems to be invalid.  Please confirm that you have entered it correctly.");
            Context.ConnectButtonEnabled = true;
            return;
        }
        _playerName = e.Slot;
        await APClient.Login(_playerName, !string.IsNullOrWhiteSpace(e.Password) ? e.Password : null);
        if (!APClient.IsLoggedIn)
        {
            Log.Logger.Error("Failed to login.  Please check your host, name, and password.");
            Context.ConnectButtonEnabled = true;
            return;
        }

        // Subscribe to item and location events
        APClient.ItemManager.ItemReceived += ItemManager_ItemReceived;
        APClient.LocationManager.EnableLocationsCondition = LocationManager_EnableLocationsCondition;
        APClient.LocationManager.LocationCompleted += LocationManager_LocationCompleted;
        APClient.CurrentSession.Locations.CheckedLocationsUpdated += CurrentSession_CheckedLocationsUpdated;

        // TODO: parse options
        GameLocations = LocationHelpers.BuildLocationList(APClient.Options);
        GameLocations = GameLocations.Where(x => x != null && !APClient.CurrentSession.Locations.AllLocationsChecked.Contains(x.Id)).ToList();

        int slot = APClient.CurrentSession.ConnectionInfo.Slot;

        // Scout location item data for future use
        long[] locationIds = GameLocations.Select(loc => (long)loc.Id).ToArray();
        ArchipelagoSession session = APClient.CurrentSession;
        Dictionary<long, ScoutedItemInfo> scoutedLocations = await session.Locations.ScoutLocationsAsync(locationIds);
        Dictionary<long, ItemData> itemDataDict = LocationHelpers.GetItemDataDict();
        scoutedLocationItemData = scoutedLocations.Keys.ToDictionary(
            locationId => locationId, locationId => scoutedLocations[locationId].Player.Slot == slot ? itemDataDict[scoutedLocations[locationId].ItemId] : itemDataDict[0]
        );

        // Check apworld version compatibility with host and log results
        Dictionary<string, object> slotData = await APClient.CurrentSession.DataStorage.GetSlotDataAsync(slot);
        if (slotData.TryGetValue("apworldVersion", out var versionValue) && versionValue != null)
        {
            if (SupportedVersions.Contains(versionValue.ToString().ToLower()))
            {
                Log.Logger.Information($"The host's AP world version is {versionValue} and the client version is {Version}.");
                Log.Logger.Information("These versions are known to be compatible.");
            }
            else
            {
                Log.Logger.Warning($"The host's AP world version is {versionValue} but the client version is {Version}.");
                Log.Logger.Warning("Please ensure these are compatible before proceeding.");
            }
        }
        else
        {
            Log.Logger.Error("Unable to retrieve apworldversion from slot data.");
        }
        Log.Logger.Information("Warnings and errors above are okay if this is your first time connecting to this multiworld server.");

        APClient.MonitorLocationsAsync(GameLocations);
        await APClient.ReceiveReady();

        Context.ConnectButtonEnabled = true;
        return;
    }

    private void LocationManager_LocationCompleted(object? sender, LocationCompletedEventArgs e)
    {
        if (APClient.LocationManager != null && APClient.CurrentSession != null)
        {
            // Use scouted location item to rewrite textbox
            Dictionary<int, LocationData> locationDataDict = LocationHelpers.GetLocationDataDict();
            LocationData locationData = locationDataDict[e.CompletedLocation.Id];
            if (locationData.TextBoxStartAddress != null)
            {
                ItemData itemData = scoutedLocationItemData[e.CompletedLocation.Id];
                Memory.WriteByteArray(locationData.TextBoxStartAddress ?? 0, TextHelpers.EncodeYouGotItemWindow(itemData)); // TODO: Is this big endian?
            }
        }
        return;
    }

    private static bool LocationManager_EnableLocationsCondition()
    {
        bool[] conditions = [
            !Memory.ReadBit(Addresses.ScreenWipeFlag.Address, Addresses.ScreenWipeFlag.BitNumber??0),
            !Memory.ReadBit(Addresses.LoadingFlag.Address, Addresses.LoadingFlag.BitNumber??0),
            //!Memory.ReadBit(Addresses.DungeonMapFlag.Address, Addresses.DungeonMapFlag.BitNumber??0),
            //!Memory.ReadBit(Addresses.PauseMenuFlag.Address, Addresses.PauseMenuFlag.BitNumber??0),
            !Memory.ReadBit(Addresses.CameraAlteredFlag.Address, Addresses.CameraAlteredFlag.BitNumber??0),
            !Memory.ReadBit(Addresses.SaveDataMenuFlag.Address, Addresses.SaveDataMenuFlag.BitNumber??0),
            Memory.ReadByte(Addresses.TitleScreen.Address) == 0xA4
        ];
        return conditions.All(value => value);
    }

    private static void ItemManager_ItemReceived(object? sender, ItemReceivedEventArgs args)
    {
        Dictionary<long, ItemData> itemDataDict = LocationHelpers.GetItemDataDict();
        if (APClient.CurrentSession != null && itemDataDict.TryGetValue(args.Item.Id, out ItemData? itemData))
        {
            ItemHelpers.ReceiveGenericItem(itemData);
            Log.Logger.Debug($"Item Received: {JsonConvert.SerializeObject(args.Item)}");
        }
        return;
    }

    private static async void StartMMLGame(object? sender, ElapsedEventArgs e)
    {
        if (APClient == null || APClient.ItemManager == null || APClient.CurrentSession == null || !LocationManager_EnableLocationsCondition())
        {
            return;
        }

        // Start gameplay loop
        _gameLoopTimer = new Timer();
        _gameLoopTimer.Elapsed += new ElapsedEventHandler(ModifyGameLoop);
        _gameLoopTimer.Interval = 500;
        _gameLoopTimer.Enabled = true;
        APClient.ReceiveReady();

        if (_startMMLTimer != null)
        {
            _startMMLTimer.Enabled = false;
        }
    }

    private static async void ModifyGameLoop(object? sender, ElapsedEventArgs e)
    {
        if (APClient == null || APClient.ItemManager == null || APClient.CurrentSession == null)
        {
            return;
        }
        try
        {
            //Log.Logger.Information($"Locations enabled: {LocationManager_EnableLocationsCondition().ToString()}");
            CheckGoalCondition();
        }
        catch (Exception ex)
        {
            Log.Logger.Warning("Encountered an error while managing the game loop.");
            Log.Logger.Warning(ex.ToString());
            Log.Logger.Warning("This is not necessarily a problem if it happens during release or collect.");
        }
        return;
    }

    private static void CheckGoalCondition()
    {
        if (
            !_hasSubmittedGoal ||
            LocationManager_EnableLocationsCondition()
        )
        {
            if (APClient != null && APClient.Options != null && APClient.Options.TryGetValue("goal", out var goalValueObj))
            {
                int goalValue = goalValueObj as int? ?? 0;
                bool isGoalComplete = (CompletionGoal)goalValue switch
                {
                    CompletionGoal.Juno => Memory.ReadBit(Addresses.GoalJunoFlag.Address, Addresses.GoalJunoFlag.BitNumber ?? 0),
                    _ => false
                };
                if (isGoalComplete)
                {
                    APClient.SendGoalCompletion();
                    _hasSubmittedGoal = true;
                }
            }
        }
        return;
    }

    private static async void Client_MessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        Log.Logger.Information(JsonConvert.SerializeObject(e.Message));
        return;
    }

    private static async void CurrentSession_CheckedLocationsUpdated(System.Collections.ObjectModel.ReadOnlyCollection<long> newCheckedLocations)
    {
        if (APClient.ItemManager == null || APClient.CurrentSession == null) return;
        if (!LocationManager_EnableLocationsCondition())
        {
            Log.Logger.Error("Check sent while not in game. Please report this in the Discord thread!");
        }
        return;
    }

    private static async void Client_Connected(object? sender, EventArgs args)
    {
        // Ensure player is in game before starting gameplay loop
        _startMMLTimer = new Timer();
        _startMMLTimer.Elapsed += new ElapsedEventHandler(StartMMLGame);
        _startMMLTimer.Interval = 5000;
        _startMMLTimer.Enabled = true;

        Log.Logger.Information("Connected to Archipelago");
        Log.Logger.Information($"Playing {APClient.CurrentSession.ConnectionInfo.Game} as {APClient.CurrentSession.Players.GetPlayerName(APClient.CurrentSession.ConnectionInfo.Slot)}");
        return;
    }

    private static async void Client_Disconnected(object? sender, EventArgs args)
    {
        Log.Logger.Information("Disconnected from Archipelago");
        // Avoid ongoing timers affecting a new game.
        if (_startMMLTimer != null)
        {
            _startMMLTimer.Enabled = false;
        }
        if (_gameLoopTimer != null)
        {
            _gameLoopTimer.Enabled = false;
        }
        _hasSubmittedGoal = false;
        return;
    }
}
