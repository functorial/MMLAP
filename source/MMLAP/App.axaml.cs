using Archipelago.Core;
using Archipelago.Core.AvaloniaGUI.Models;
using Archipelago.Core.AvaloniaGUI.ViewModels;
using Archipelago.Core.AvaloniaGUI.Views;
using Archipelago.Core.Helpers;
using Archipelago.Core.Models;
using Archipelago.Core.Util;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DynamicData;
using MMLAP.Helpers;
using MMLAP.Models;
using Newtonsoft.Json;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Security.Principal;
using System.Timers;
using static MMLAP.Models.MMLEnums;

namespace MMLAP;

public partial class App : Application
{
    // TODO: Remember to set this in MMLAP.Desktop as well.
    public static readonly string Version = "0.3.0";
    public static readonly List<string> SupportedVersions = ["0.3.0"];
    public static MainWindowViewModel? Context;
    public static ArchipelagoClient? APClient { get; set; }
    private static Dictionary<long, ItemData>? ScoutedLocationItemData { get; set; }
    private static List<ILocation>? GameLocations { get; set; }
    private static string? PlayerName { get; set; }
    public static bool HasSubmittedGoal { get; set; } = false;
    private static int IsInGameSyncInitialized = 0;
    private static Timer? SlowGameLoopTimer { get; set; }
    private static int IsSlowLoopRunning = 0;
    private static Timer? FastGameLoopTimer { get; set; }
    private static int IsFastLoopRunning = 0;
    private static Timer? StartMMLTimer { get; set; }
    private static ConcurrentStack<TextData> TextDataToWriteStack { get; set; } = new();
    private static ushort? PreviousLevelID { get; set; }
    private static byte CurrentProgressionCounter { get; set; } = 0x0;
    private static bool IsManagingLevelChange { get; set; } = false;
    private static bool IsPreviouslyInTitleScreen { get; set; } = false;
    private static bool IsReceivingItemsAfterLoad { get; set; } = false;
    //private static int YellowRefractorTerminalVal { get; set; } = 0x00;
    private static readonly object _lockObject = new object();

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

    /**
     * Returns a Dictionary of details from the most recent connection.
     *
     * File path is ./connection.json.
     *
     * @return A Dictionary with the most recent connection, with keys of host and slot.
     */
    public static Dictionary<String, String> GetLastConnectionDetails()
    {
        string connectionDetails = File.ReadAllText(@"./connection.json");
        return System.Text.Json.JsonSerializer.Deserialize<Dictionary<String, String>>(connectionDetails);
    }

    /**
     * Saves a details from the most recent connection to ./connection.json.
     *
     * @param A Dictionary with the most recent connection, with keys of host and slot.
     */
    public static void SaveLastConnectionDetails(Dictionary<String, String> lastConnectionDetails)
    {
        string json = System.Text.Json.JsonSerializer.Serialize(lastConnectionDetails);
        File.WriteAllText(@"./connection.json", json);
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

        Dictionary<String, String> lastConnectionDetails = new Dictionary<string, string>();
        lastConnectionDetails["slot"] = "";
        lastConnectionDetails["host"] = "";
        // Don't save password.
        try
        {
            lastConnectionDetails = GetLastConnectionDetails();
            if (!lastConnectionDetails.ContainsKey("slot"))
            {
                lastConnectionDetails["slot"] = "";
            }
            if (!lastConnectionDetails.ContainsKey("host"))
            {
                lastConnectionDetails["host"] = "";
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Verbose($"Could not load connection details file.\r\n{ex.ToString()}");
        }
        Context.Host = lastConnectionDetails["host"];
        Context.Slot = lastConnectionDetails["slot"];

        HasSubmittedGoal = false;

        Log.Logger.Information("This Archipelago Client is compatible only with the NTSC-U release of Mega Man Legends.");
        Log.Logger.Information("Trying to play with a different version will not work as intended.");
        if (!IsRunningAsAdministrator())
        {
            Log.Logger.Warning("You do not appear to be running this client as an administrator.");
            Log.Logger.Warning("This may result in errors or crashes when trying to connect to Duckstation.");
        }
        Log.Logger.Information("Please report any issues in the Discord thread. Thank you!");
        return;
    }

    private void Context_CommandReceived(object? sender, ArchipelagoCommandEventArgs a)
    {

        if (string.IsNullOrWhiteSpace(a.Command))
        {
            return;
        }
        string command = a.Command.Trim().ToLower();
        switch (command)
        {
            case "!help":
                Log.Logger.Information($"> {a.Command}");
                Log.Logger.Information("Available commands:");
                Log.Logger.Information("!help - Show this help message.");
                Log.Logger.Information("!reload - Force reload all items.  Use this if you think you may have missed received items.  Please reconnect to the server while in game to refresh received items.");
                Log.Logger.Information("!goal - Check your current goal.");
                break;
            case "!reload":
                Log.Logger.Information($"> {a.Command}");
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
            case "!goal":
                Log.Logger.Information($"> {a.Command}");
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
                APClient?.SendMessage(a.Command);
                break;
        }
        return;
    }

    private async void Context_ConnectClicked(object? sender, ConnectClickedEventArgs e)
    {
        Context.ConnectButtonEnabled = false;
        System.Threading.Interlocked.Exchange(ref IsInGameSyncInitialized, 0);
        Log.Logger.Information("Connecting...");

        // Refreshing subscriptions
        ScoutedLocationItemData = null;
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
        await APClient.Connect((e.Host ?? "localhost:38281").Trim(), "Mega Man Legends");
        if (!APClient.IsConnected)
        {
            Log.Logger.Error("Your host seems to be invalid.  Please confirm that you have entered it correctly.");
            Context.ConnectButtonEnabled = true;
            return;
        }
        PlayerName = e.Slot ?? "".Trim();
        await APClient.Login(PlayerName, !string.IsNullOrWhiteSpace(e.Password) ? e.Password : null);
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
        List<ILocation> allGameLocations = DataDicts.BuildLocationList(APClient.Options).Where(x => x != null).ToList();
        GameLocations = allGameLocations.Where(x => !APClient.CurrentSession.Locations.AllLocationsChecked.Contains(x.Id)).ToList(); // Only unchecked locations?

        int slot = APClient.CurrentSession.ConnectionInfo.Slot;

        // Scout location item data for future use
        long[] locationIds = allGameLocations.Select(loc => (long)loc.Id).ToArray();
        ArchipelagoSession session = APClient.CurrentSession;
        Dictionary<long, ScoutedItemInfo> scoutedLocations = await session.Locations.ScoutLocationsAsync(locationIds);
        ScoutedLocationItemData = scoutedLocations.Keys.ToDictionary(
            locationId => locationId, 
            locationId => scoutedLocations[locationId].Player.Slot == slot ? 
                DataDicts.ItemDataDict[scoutedLocations[locationId].ItemId] : 
                DataDicts.ItemDataDict[0]
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

        //APClient.MonitorLocationsAsync(GameLocations);
        //await APClient.ReceiveReady();

        Context.ConnectButtonEnabled = true;
        return;
    }

    private static async void Client_Connected(object? sender, EventArgs args)
    {
        if (APClient != null)
        {
            // Ensure player is in game before starting gameplay loop
            StartMMLTimer = new Timer();
            StartMMLTimer.Elapsed += new ElapsedEventHandler(StartMMLGame);
            StartMMLTimer.Interval = 500;
            StartMMLTimer.Enabled = true;

            System.Threading.Thread.Sleep(250);

            // Start gameplay loop
            SlowGameLoopTimer = new Timer();
            SlowGameLoopTimer.Elapsed += new ElapsedEventHandler(SlowGameLoop);
            SlowGameLoopTimer.Interval = 500;
            SlowGameLoopTimer.Enabled = true;

            FastGameLoopTimer = new Timer();
            FastGameLoopTimer.Elapsed += new ElapsedEventHandler(FastGameLoop);
            FastGameLoopTimer.Interval = 1;
            FastGameLoopTimer.Enabled = true;

            Log.Logger.Information("Connected to Archipelago");
            Log.Logger.Information($"Playing {APClient.CurrentSession.ConnectionInfo.Game} as {APClient.CurrentSession.Players.GetPlayerName(APClient.CurrentSession.ConnectionInfo.Slot)}");

            Dictionary<String, String> lastConnectionDetails = new();
            lastConnectionDetails["slot"] = Context.Slot;
            lastConnectionDetails["host"] = Context.Host;
            try
            {
                SaveLastConnectionDetails(lastConnectionDetails);
            }
            catch (Exception ex)
            {
                Log.Logger.Debug($"Failed to write connection details\r\n{ex.ToString()}");
            }
            // Repopulate hint list.  There is likely a better way to do this using the Get network protocol
            // with keys=[$"hints_{team}_{slot}"].
            APClient?.SendMessage("!hint");
            UpdateItemLog();
        }
        return;
    }

    private static async void Client_Disconnected(object? sender, EventArgs args)
    {
        Log.Logger.Information("Disconnected from Archipelago");
        // Avoid ongoing timers affecting a new game.
        StartMMLTimer?.Enabled = false;
        SlowGameLoopTimer?.Enabled = false;
        FastGameLoopTimer?.Enabled = false;
        HasSubmittedGoal = false;
        ScoutedLocationItemData = null;
        System.Threading.Interlocked.Exchange(ref IsInGameSyncInitialized, 0);
        return;
    }

    private static async void StartMMLGame(object? sender, ElapsedEventArgs e)
    {
        if (
            APClient != null &&
            APClient.ItemManager != null &&
            APClient.CurrentSession != null &&
            GameLocations != null &&
            LocationManager_EnableLocationsCondition() &&
            System.Threading.Interlocked.CompareExchange(ref IsInGameSyncInitialized, 1, 0) == 0 // Only start the game loop once per connection
        )
        {
            StartMMLTimer?.Enabled = false;
            APClient.MonitorLocationsAsync(GameLocations);
            await APClient.ReceiveReady();
            Log.Logger.Debug("In-game confirmed. Location monitoring and item receive are now enabled.");
        }
        return;
    }

    /**
     * Adds a hint message to the Hints tab.
     *
     * @param message The message with the hint.
     */
    private static void LogHint(LogMessage message)
    {
        var newMessage = message.Parts.Select(x => x.Text);
        List<TextSpan> spans = new List<TextSpan>();

        LogListItem? updatedHint = null;

        foreach (var hint in Context.HintList)
        {
            IEnumerable<string> hintText = hint.TextSpans.Select(y => y.Text);
            if (newMessage.Count() != hintText.Count())
            {
                continue;
            }
            bool isMatch = true;
            for (int i = 0; i < hintText.Count(); i++)
            {
                if (newMessage.ElementAt(i) != hintText.ElementAt(i) && newMessage.ElementAt(i).Trim() != "(found)")
                {
                    isMatch = false;
                    break;
                }
                else if (newMessage.ElementAt(i).Trim() == "(found)")
                {
                    updatedHint = hint;
                    break;
                }
            }
            if (updatedHint != null)
            {
                break;
            }
            if (isMatch)
            {
                return; // Hint already in list
            }
        }
        foreach (var part in message.Parts)
        {
            RxApp.MainThreadScheduler.Schedule(() =>
            {
                spans.Add(new TextSpan() { Text = part.Text, TextColor = new SolidColorBrush(Avalonia.Media.Color.FromRgb(part.Color.R, part.Color.G, part.Color.B)) });
            });
        }
        lock (_lockObject)
        {
            RxApp.MainThreadScheduler.Schedule(() =>
            {
                if (updatedHint == null)
                {
                    Context.HintList.Add(new LogListItem(spans));
                }
                else
                {
                    Context.HintList.Replace(updatedHint, new LogListItem(spans));
                }
            });
        }
    }

    /**
     * Updates the Received Items tab.  Unlike the Hints tab, rewrites the list each time.
     */
    private static void UpdateItemLog()
    {
        Dictionary<string, int> itemCount = new Dictionary<string, int>();
        List<LogListItem> messagesToLog = new List<LogListItem>();
        if (APClient != null && APClient.CurrentSession != null && APClient.CurrentSession.Items != null)
        {
            foreach (ItemInfo item in APClient.CurrentSession.Items.AllItemsReceived)
            {
                string itemName = item.ItemName;
                if (itemCount.ContainsKey(itemName))
                {
                    itemCount[itemName] = itemCount[itemName] + 1;
                }
                else
                {
                    itemCount[itemName] = 1;
                }
            }
            RxApp.MainThreadScheduler.Schedule(() =>
            {
                List<string> sortedItems = itemCount.Keys.ToList();
                sortedItems.Sort();
                foreach (string item in sortedItems)
                {
                    messagesToLog.Add(new LogListItem(
                        new List<TextSpan>() {
                            new TextSpan() { Text = $"{item}: ", TextColor = new SolidColorBrush(Avalonia.Media.Color.FromRgb(200, 255, 200)) },
                            new TextSpan() { Text = $"{itemCount[item]}", TextColor = new SolidColorBrush(Avalonia.Media.Color.FromRgb(200, 255, 200)) }
                        }
                    ));
                }
            });
        }
        lock (_lockObject)
        {
            RxApp.MainThreadScheduler.Schedule(() =>
            {
                Context.ItemList.Clear();
                foreach (LogListItem messageToLog in messagesToLog)
                {
                    Context.ItemList.Add(messageToLog);
                }
            });
        }
    }

    private static async void FastGameLoop(object? sender, ElapsedEventArgs e)
    {
        if (System.Threading.Interlocked.CompareExchange(ref IsFastLoopRunning, 1, 0) != 0)
        {
            return; // Previous call is still running, skip this tick
        }
        try
        {
            if (
                APClient != null &&
                APClient.ItemManager != null &&
                APClient.CurrentSession != null
            )
            {
                ushort currentLevelID = Memory.ReadUShort(Addresses.CurrentLevel.Address, Enums.Endianness.Big);
                if (
                    DataDicts.LevelDataDict.TryGetValue(currentLevelID, out LevelData? currentLevelData)
                )
                {

                    // Task: This is for hijacking game code which is loaded and executed only once during loading screens
                    // Pause these loop actions if not in loading screen
                    if (
                        MemoryHelpers.ReadAddressDataBit(Addresses.LoadingFlag)
                    )
                    {
                        // Write fast forward area cheats which do things like unlock doors and prevent black screens
                        LoopHelpers.HandleLoadingFastCodeWrites(currentLevelData, CurrentProgressionCounter);
                        // Handle text and possible overflows from locations that give items during text windows Mine Parts Kit from the rescue shop owner's husband lcoation
                        LoopHelpers.HandleOddLocationText(currentLevelData, ScoutedLocationItemData, TextDataToWriteStack);
                    }

                    // Run these after loading
                    // These will happen before AND after slow game loop, so be mindful
                    // and only put stuff here that needs to be fast and doesn't interact with slow game loop
                    // (e.g. overwriting things happening in the internal game loop)
                    else
                    {
                        switch (currentLevelData.AreaName)
                        {
                            case "Cardon Forest Sub-Gate":
                                // Part of the decoupling of cardon sub-gate keys with the yellow refractor terminal
                                LoopHelpers.HandleYellowRefractorTerminal();
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Warning("Encountered an error while managing the game loop.");
            Log.Logger.Warning(ex.ToString());
            Log.Logger.Warning("This is not necessarily a problem if it happens during release or collect.");
        }
        finally
        {
            System.Threading.Interlocked.Exchange(ref IsFastLoopRunning, 0);
        }
        return;
    }

    private static async void SlowGameLoop(object? sender, ElapsedEventArgs e)
    {
        if (System.Threading.Interlocked.CompareExchange(ref IsSlowLoopRunning, 1, 0) != 0)
        {
            return; // Previous call is still running, skip this tick
        }
        try
        {
            if (
                APClient != null &&
                APClient.ItemManager != null &&
                APClient.CurrentSession != null
            )
            {
                // Pause these loop actions if in title menu or save menu
                if (
                    MemoryHelpers.IsOutOfTitleScreen() &&
                    !MemoryHelpers.ReadAddressDataBit(Addresses.SaveDataMenuFlag)
                )
                {
                    // Task 1: Read useful memory
                    CheckGoalCondition();
                    CurrentProgressionCounter = Memory.ReadByte(Addresses.CurrentProgressionCounter.Address);

                    // Task 2: Do things when changing rooms
                    // Task 2.a: Check if level has changed, and if so, set flag to manage level change
                    ushort currentLevelID = Memory.ReadUShort(Addresses.CurrentLevel.Address, Enums.Endianness.Big);

                    if (currentLevelID != PreviousLevelID)
                    {
                        APClient.CurrentSession.DataStorage[Scope.Slot, "MML_ROOM"] = currentLevelID;
                        IsManagingLevelChange = true;
                    }

                    PreviousLevelID = currentLevelID;

                    if (DataDicts.LevelDataDict.TryGetValue(currentLevelID, out LevelData? currentLevelData))
                    {
                        System.Threading.Thread.Sleep(50);

                        // Task 2.b: Based on current level, do things like overwrite text, write code that isnt needed during loading, and locking doors
                        if (
                            IsManagingLevelChange &&
                            !MemoryHelpers.ReadAddressDataBit(Addresses.LoadingFlag) &&
                            !MemoryHelpers.ReadAddressDataBit(Addresses.ScreenWipeFlag) &&
                            !MemoryHelpers.ReadAddressDataBit(Addresses.CameraAlteredFlag)
                        )
                        {
                            // Do slow code writes, typically ones that cause problems in fast write
                            LoopHelpers.HandleSlowCodeWrites(currentLevelData, CurrentProgressionCounter);
                            // Lock doors based on items received
                            LoopHelpers.HandleFlutterFixedBrokenDistinction(currentLevelData);
                            LoopHelpers.HandleAreaExitLocks(currentLevelData);
                            LoopHelpers.HandleOddPails(currentLevelData);
                            IsManagingLevelChange = false;
                        }
                        // Task 3: Do things regardless of level change
                        // Task 3.a: Restore overwritten memory
                        if (
                            !MemoryHelpers.ReadAddressDataBit(Addresses.LoadingFlag) &&
                            !MemoryHelpers.ReadAddressDataBit(Addresses.ScreenWipeFlag) &&
                            !MemoryHelpers.ReadAddressDataBit(Addresses.CameraAlteredFlag)
                        )
                        {
                            // If we have overwritten text for a scouted location, check if the textbox is closed, and if so, restore the original text
                            if (!MemoryHelpers.ReadAddressDataBit(Addresses.TextBoxOpenFlag))
                            {
                                while (TextDataToWriteStack.TryPop(out var overwrittenTextData))
                                {
                                    Memory.WriteByteArray(overwrittenTextData.StartAddress, overwrittenTextData.TextByteArr);
                                }
                            }

                            // Restore non-area-specific code writes (i.e. functions that may be used in multiple areas)
                            Cheats.Restore1FXXXWrites(currentLevelData);
                        }
                    }
                }

                // Task 4: Handle receiving items after loading a save (leaving title screen)
                // Logic:
                // - Receive all non-zenny items after moving from title screen to in-game
                // - Open all containers/holes that were previously opened
                // Potential issues:
                // - Currently only works on bit checks, which is fine for now since we only have bit checks
                // - Only re-check container locations, so players can still get vanilla items from side-quest checks. But didn't want to risk breaking quest progression by writing to quest check addresses.
                bool isCurrentlyInTitleScreen = MemoryHelpers.IsInTitleScreen();
                if (
                    IsPreviouslyInTitleScreen &&
                    !isCurrentlyInTitleScreen
                )
                {
                    IsReceivingItemsAfterLoad = true;
                }
                IsPreviouslyInTitleScreen = isCurrentlyInTitleScreen;

                if (
                    IsReceivingItemsAfterLoad &&
                    LocationManager_EnableLocationsCondition()
                )
                {
                    IReadOnlyCollection<long> allLocationsChecked = APClient.CurrentSession.Locations.AllLocationsChecked;
                    if (allLocationsChecked.Count > 0)
                    {
                        Log.Logger.Information("Re-checking previously checked container locations...");
                        foreach (long locationID in allLocationsChecked)
                        {
                            if (DataDicts.LocationDataDict.TryGetValue((int)locationID, out LocationData? locationData))
                            {
                                if (new[] { LocationCategory.Container, LocationCategory.Hole, LocationCategory.Pickup }.Contains(locationData.Category))
                                {
                                    if (locationData.CheckAddressData.BitNumber != null)
                                    {
                                        MemoryHelpers.WriteAddressDataBit(locationData.CheckAddressData, true);
                                    }
                                    else
                                    {
                                        Log.Logger.Warning($"No check bit defined for location ID {locationID}. Please report this in the Discord thread!");
                                    }
                                }
                            }
                            else
                            {
                                Log.Logger.Warning($"Failed to receive item for location ID {locationID} after loading save. Please report this in the Discord thread!");
                            }
                        }
                    }
                    IReadOnlyCollection<ItemInfo> allItemsReceived = APClient.CurrentSession.Items.AllItemsReceived;
                    if (allItemsReceived.Count > 0)
                    {
                        Log.Logger.Information("Receiving non-zenny items from previously received items...");
                        foreach (ItemInfo itemInfo in allItemsReceived)
                        {
                            if (DataDicts.ItemDataDict.TryGetValue(itemInfo.ItemId, out ItemData? itemData))
                            {
                                if (itemData.Category != ItemCategory.Zenny)
                                {
                                    ItemHelpers.ReceiveGenericItem(itemData);
                                }
                            }
                            else
                            {
                                Log.Logger.Warning($"Failed to receive item ID {itemInfo.ItemId} after loading save. Please report this in the Discord thread!");
                            }
                        }
                    }
                    IsReceivingItemsAfterLoad = false;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Warning("Encountered an error while managing the game loop.");
            Log.Logger.Warning(ex.ToString());
            Log.Logger.Warning("This is not necessarily a problem if it happens during release or collect.");
        }
        finally
        {
            System.Threading.Interlocked.Exchange(ref IsSlowLoopRunning, 0);
        }
        return;
    }

    private static void CheckGoalCondition()
    {
        if (
            APClient != null && (
                !HasSubmittedGoal ||
                LocationManager_EnableLocationsCondition()
            )
        )
        {
            APClient.Options.TryGetValue("goal", out var goalValueObj);
            if (APClient != null && APClient.Options != null)
            {
                int goalValue = goalValueObj as int? ?? 0;
                bool isGoalComplete = (CompletionGoal)goalValue switch
                {
                    CompletionGoal.Juno => MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedJuno),
                    _ => false
                };
                if (isGoalComplete)
                {
                    APClient.SendGoalCompletion();
                    HasSubmittedGoal = true;
                }
            }
        }
        return;
    }

    private static bool LocationManager_EnableLocationsCondition()
    {
        bool[] conditions = [
            MemoryHelpers.IsOutOfTitleScreen(),
            !MemoryHelpers.ReadAddressDataBit(Addresses.ScreenWipeFlag),
            !MemoryHelpers.ReadAddressDataBit(Addresses.LoadingFlag),
            !MemoryHelpers.ReadAddressDataBit(Addresses.SaveDataMenuFlag)
        ];
        return conditions.All(value => value);
    }

    private void LocationManager_LocationCompleted(object? sender, LocationCompletedEventArgs e)
    {
        if (
            APClient != null &&
            APClient.LocationManager != null &&
            APClient.CurrentSession != null &&
            ScoutedLocationItemData != null
        )
        {
            // Use scouted location item to rewrite textbox
            if (!DataDicts.LocationDataDict.TryGetValue(e.CompletedLocation.Id, out LocationData? locationData))
            {
                Log.Logger.Warning($"Completed location {e.CompletedLocation.Id} was not found in LocationDataDict.");
                return;
            }

            if (locationData.TextBoxStartAddress != null)
            {
                if (!ScoutedLocationItemData.TryGetValue(e.CompletedLocation.Id, out ItemData? itemData))
                {
                    Log.Logger.Warning($"Scouted item data is missing for completed location {e.CompletedLocation.Id}.");
                    return;
                }

                TextData overwrittenText = TextHelpers.OverwriteText(locationData.TextBoxStartAddress ?? 0, TextHelpers.EncodeYouGotItemWindow(itemData));
                TextDataToWriteStack.Push(overwrittenText);
            }
        }
        return;
    }

    private static void ItemManager_ItemReceived(object? sender, ItemReceivedEventArgs args)
    {
        if (
            APClient != null &&
            APClient.CurrentSession != null &&
            DataDicts.ItemDataDict.TryGetValue(args.Item.Id, out ItemData? itemData)
        )
        {
            ItemHelpers.ReceiveGenericItem(itemData);
            UpdateItemLog();
        }
        return;
    }

    private static async void Client_MessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        if (e.Message.Parts.Any(x => x.Text == "[Hint]: "))
        {
            LogHint(e.Message);
        }
        Log.Logger.Information(JsonConvert.SerializeObject(e.Message));
        return;
    }

    private static async void CurrentSession_CheckedLocationsUpdated(System.Collections.ObjectModel.ReadOnlyCollection<long> newCheckedLocations)
    {
        if (
            APClient != null &&
            APClient.ItemManager != null &&
            APClient.CurrentSession != null
        )
        {
            if (!LocationManager_EnableLocationsCondition())
            {
                Log.Logger.Error("Check sent while not in game. Please report this in the Discord thread!");
            }

        }
        return;
    }
}
