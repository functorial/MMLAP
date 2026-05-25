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
    private static int _hasSubmittedGoal = 0;
    public static bool HasSubmittedGoal
    {
        get => _hasSubmittedGoal == 1;
        set => _hasSubmittedGoal = value ? 1 : 0;
    }
    private static int IsInGameSyncInitialized = 0;
    private static Timer? SlowGameLoopTimer { get; set; }
    private static int IsSlowLoopRunning = 0;
    private static Timer? FastGameLoopTimer { get; set; }
    private static int IsFastLoopRunning = 0;
    private static Timer? StartMMLTimer { get; set; }
    private static ConcurrentStack<TextData> TextDataToWriteStack { get; set; } = new();
    private static ushort? PreviousLevelID_Slow { get; set; }
    private static ushort? PreviousLevelID_Fast { get; set; }
    private static byte CurrentProgressionCounter { get; set; } = 0x0;
    private static ConcurrentDictionary<string, byte> VisitedAreaNames { get; set; } = new();
    private static bool IsManagingLevelChange { get; set; } = false;
    private static bool IsPreviouslyInTitleScreen { get; set; } = false;
    private static bool IsLoadingIntoGame { get; set; } = false;
    private static bool WasSaving { get; set; } = false;
    private static uint? APZennyCommittedToSave { get; set; } = null;
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
                Log.Logger.Information("!debug - Print debugging information about current game and client state.");
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
                    CompletionGoal goal = (CompletionGoal)int.Parse(goalValueObj.ToString());
                    goalText = goal switch
                    {
                        CompletionGoal.JUNO => "Defeat Juno.",
                        CompletionGoal.ALL_BOSSES => "Defeat all bosses with a healthbar.",
                        _ => "Unknown",
                    };
                }
                else
                {
                    goalText = "Unknown";
                }
                Log.Logger.Information($"Your goal is: {goalText}");
                break;
            case "!debug":
                Log.Logger.Information($"> {a.Command}");
                LogDebugInfo();
                break;
            case "!options":
                if (APClient != null && APClient.Options != null)
                {
                    foreach (string option in APClient.Options.Keys)
                    {
                        Log.Logger.Information($"{option}: {APClient.Options[option]}");
                    }
                }
                else
                {
                    Log.Logger.Information("Options not found.");
                }
                break;
            default:
                APClient?.SendMessage(a.Command);
                break;
        }
        return;
    }

    private static void LogDebugInfo()
    {
        try
        {
            ushort currentLevelID = Memory.ReadUShort(Addresses.CurrentLevel.Address, Enums.Endianness.Big);
            byte memoryProgressionCounter = Memory.ReadByte(Addresses.CurrentProgressionCounter.Address);
            string currentLevelName = DataDicts.LevelDataDict.TryGetValue(currentLevelID, out LevelData? currentLevelData)
                ? $"{currentLevelData.AreaName}: {currentLevelData.RoomName}"
                : "Unknown";

            bool hasRescuedShopOwnersHusband = MemoryHelpers.ReadAddressDataBit(Addresses.HasRescuedShopOwnersHusband);
            bool hasEarnedCitizenship = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedCitizenship);
            bool hasDefeatedBonBonne = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBonBonne);
            bool hasEarnedClassBLicense = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassBLicense);
            bool hasEarnedClassALicense = MemoryHelpers.ReadAddressDataBit(Addresses.HasEarnedClassALicense);
            bool hasCompletedCardonTankEvent = MemoryHelpers.ReadAddressDataBit(Addresses.HasCompletedCardonTankEvent);
            bool hasTakenYellowRefractor = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenYellowRefractor);
            bool hasCalledRollToFixBoat = MemoryHelpers.ReadAddressDataBit(Addresses.HasCalledRollToFixBoat);
            bool hasDefeatedBalkonGerat = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat);
            bool hasTakenRedRefractor = MemoryHelpers.ReadAddressDataBit(Addresses.HasTakenRedRefractor);
            bool hasShownRollRedRefractor = MemoryHelpers.ReadAddressDataBit(Addresses.HasShownRollRedRefractor);
            bool hasActivatedEmergencySystem = MemoryHelpers.ReadAddressDataBit(Addresses.HasActivatedEmergencySystem);
            bool hasDefeatedJuno = MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedJuno);
            bool hasUnlockedMainGate = ItemHelpers.HasReceivedItem(0x0001);
            bool hasUnlockedSubCities = ItemHelpers.HasReceivedItem(0x0002);

            bool isLoading = MemoryHelpers.ReadAddressDataBit(Addresses.LoadingFlag);
            bool isScreenWipe = MemoryHelpers.ReadAddressDataBit(Addresses.ScreenWipeFlag);
            bool isCutscene = MemoryHelpers.ReadAddressDataBit(Addresses.CutsceneFlag);
            bool isInTitleScreen = MemoryHelpers.IsInTitleScreen();
            bool isInSaveMenu = MemoryHelpers.ReadAddressDataBit(Addresses.SaveDataMenuFlag);

            int checkedLocations = APClient?.CurrentSession?.Locations.AllLocationsChecked.Count ?? 0;
            int receivedItems = APClient?.CurrentSession?.Items.AllItemsReceived.Count ?? 0;
            ItemInfo? lastReceivedItem = APClient?.CurrentSession?.Items?.AllItemsReceived.LastOrDefault();
            string lastReceivedItemText = lastReceivedItem != null
                ? $"{lastReceivedItem.ItemName} (0x{lastReceivedItem.ItemId:X4})"
                : "None";
            List<string> visitedAreas = VisitedAreaNames.Keys.ToList();

            Log.Logger.Information("===== DEBUG INFO =====");
            Log.Logger.Information($"Progression: tracked=0x{CurrentProgressionCounter:X2}, memory=0x{memoryProgressionCounter:X2}");
            Log.Logger.Information($"Level: 0x{currentLevelID:X4} ({currentLevelName})");
            Log.Logger.Information($"Connection: connected={APClient?.IsConnected ?? false}, loggedIn={APClient?.IsLoggedIn ?? false}, inGameSyncInitialized={IsInGameSyncInitialized}");
            Log.Logger.Information($"Loop state: isManagingLevelChange={IsManagingLevelChange}, isLoadingIntoGame={IsLoadingIntoGame}, previousLevelIDSlow={(PreviousLevelID_Slow != null ? $"0x{PreviousLevelID_Slow.Value:X4}" : "null")}, previousLevelIDFast={(PreviousLevelID_Fast != null ? $"0x{PreviousLevelID_Fast.Value:X4}" : "null")}");
            Log.Logger.Information($"Game state: loading={isLoading}, screenWipe={isScreenWipe}, cutscene={isCutscene}, titleScreen={isInTitleScreen}, saveMenu={isInSaveMenu}");
            Log.Logger.Information($"AP state: checkedLocations={checkedLocations}, receivedItems={receivedItems}, pendingTextRestores={TextDataToWriteStack.Count}");
            Log.Logger.Information($"Last item received: {lastReceivedItemText}");
            LogRestoreOpCodeState();

            Log.Logger.Information("Cheat argument booleans:");
            Log.Logger.Information($"- hasRescuedShopOwnersHusband={hasRescuedShopOwnersHusband}, hasEarnedCitizenship={hasEarnedCitizenship}");
            Log.Logger.Information($"- hasEarnedClassBLicense={hasEarnedClassBLicense}, hasEarnedClassALicense={hasEarnedClassALicense}");
            Log.Logger.Information($"- hasDefeatedBonBonne={hasDefeatedBonBonne}, hasCompletedCardonTankEvent={hasCompletedCardonTankEvent}");
            Log.Logger.Information($"- hasTakenYellowRefractor={hasTakenYellowRefractor}, hasCalledRollToFixBoat={hasCalledRollToFixBoat}");
            Log.Logger.Information($"- hasDefeatedBalkonGerat={hasDefeatedBalkonGerat}, hasTakenRedRefractor={hasTakenRedRefractor}, hasShownRollRedRefractor={hasShownRollRedRefractor}");
            Log.Logger.Information($"- hasActivatedEmergencySystem={hasActivatedEmergencySystem}, hasDefeatedJuno={hasDefeatedJuno}");
            Log.Logger.Information($"- hasUnlockedMainGate={hasUnlockedMainGate}, hasUnlockedSubCities={hasUnlockedSubCities}");

            Log.Logger.Information($"Visited areas this session ({visitedAreas.Count}):\n{(visitedAreas.Count > 0 ? string.Join("\n- ", visitedAreas) : "None")}");
            Log.Logger.Information("======================");
        }
        catch (Exception ex)
        {
            Log.Logger.Warning("Failed to gather debug information.");
            Log.Logger.Warning(ex.ToString());
        }
    }

    private static void LogRestoreOpCodeState()
    {
        List<string> notRestored = [];
        foreach (var (name, code) in Cheats.GetAllRestoreOpCodes())
        {
            uint actualInstruction = Memory.ReadUInt(code.StartAddress);
            if (actualInstruction != code.Instruction)
            {
                notRestored.Add($"- {name} @ 0x{code.StartAddress:X8}: expected=0x{code.Instruction:X8}, actual=0x{actualInstruction:X8}");
            }
        }

        if (notRestored.Count == 0)
        {
            Log.Logger.Information("Restore OpCode check: all restore opcodes are currently restored.");
            return;
        }

        Log.Logger.Warning("Restore OpCode check: some restore opcodes are not restored:");
        foreach (string line in notRestored)
        {
            Log.Logger.Warning(line);
        }
    }

    private static void StopAndDisposeTimers()
    {
        if (StartMMLTimer != null)
        {
            StartMMLTimer.Enabled = false;
            StartMMLTimer.Dispose();
            StartMMLTimer = null;
        }

        if (SlowGameLoopTimer != null)
        {
            SlowGameLoopTimer.Enabled = false;
            SlowGameLoopTimer.Dispose();
            SlowGameLoopTimer = null;
        }

        if (FastGameLoopTimer != null)
        {
            FastGameLoopTimer.Enabled = false;
            FastGameLoopTimer.Dispose();
            FastGameLoopTimer = null;
        }
    }

    private static void ResetGlobalRuntimeState()
    {
        HasSubmittedGoal = false;
        ScoutedLocationItemData = null;
        GameLocations = null;
        PlayerName = null;
        PreviousLevelID_Slow = null;
        PreviousLevelID_Fast = null;
        CurrentProgressionCounter = 0x0;
        IsManagingLevelChange = false;
        IsPreviouslyInTitleScreen = false;
        IsLoadingIntoGame = false;
        WasSaving = false;
        APZennyCommittedToSave = null;
        TextDataToWriteStack = new();
        VisitedAreaNames.Clear();

        System.Threading.Interlocked.Exchange(ref IsInGameSyncInitialized, 0);
        System.Threading.Interlocked.Exchange(ref IsSlowLoopRunning, 0);
        System.Threading.Interlocked.Exchange(ref IsFastLoopRunning, 0);
    }

    private async void Context_ConnectClicked(object? sender, ConnectClickedEventArgs e)
    {
        Context.ConnectButtonEnabled = false;
        Log.Logger.Information("Connecting...");
        StopAndDisposeTimers();

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

        ResetGlobalRuntimeState();

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
        PlayerName = (e.Slot ?? "").Trim();
        await APClient.Login(PlayerName, !string.IsNullOrWhiteSpace(e.Password) ? e.Password : null);
        if (!APClient.IsLoggedIn)
        {
            Log.Logger.Error("Failed to login.  Please check your host, name, and password.");
            Context.ConnectButtonEnabled = true;
            return;
        }

        APZennyCommittedToSave = (uint?)APClient.CurrentSession.DataStorage[Scope.Slot, "mml_ap_zenny_committed"];

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
        ArchipelagoClient? apClient = APClient;
        if (apClient != null)
        {
            StopAndDisposeTimers();

            // Ensure player is in game before starting gameplay loops
            StartMMLTimer = new Timer();
            StartMMLTimer.Elapsed += new ElapsedEventHandler(StartMMLGame);
            StartMMLTimer.Interval = 500;
            StartMMLTimer.Enabled = true;

            System.Threading.Thread.Sleep(250);

            SlowGameLoopTimer = new Timer();
            SlowGameLoopTimer.Elapsed += new ElapsedEventHandler(SlowGameLoop);
            SlowGameLoopTimer.Interval = 500;
            SlowGameLoopTimer.Enabled = true;

            FastGameLoopTimer = new Timer();
            FastGameLoopTimer.Elapsed += new ElapsedEventHandler(FastGameLoop);
            FastGameLoopTimer.Interval = 1;
            FastGameLoopTimer.Enabled = true;

            Log.Logger.Information("Connected to Archipelago");
            Log.Logger.Information($"Playing {apClient.CurrentSession.ConnectionInfo.Game} as {apClient.CurrentSession.Players.GetPlayerName(apClient.CurrentSession.ConnectionInfo.Slot)}");

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
            apClient.SendMessage("!hint");
            UpdateItemLog();
        }
        return;
    }

    private static async void Client_Disconnected(object? sender, EventArgs args)
    {
        Log.Logger.Information("Disconnected from Archipelago");
        // Avoid ongoing timers affecting a new game.
        StopAndDisposeTimers();
        ResetGlobalRuntimeState();
        return;
    }

    private static async void StartMMLGame(object? sender, ElapsedEventArgs e)
    {
        ArchipelagoClient? apClient = APClient;
        List<ILocation>? gameLocations = GameLocations;
        if (
            apClient?.ItemManager != null &&
            apClient?.CurrentSession != null &&
            gameLocations != null &&
            LocationManager_EnableLocationsCondition() &&
            System.Threading.Interlocked.CompareExchange(ref IsInGameSyncInitialized, 1, 0) == 0 // Only start the game loop once per connection
        )
        {
            StartMMLTimer?.Enabled = false;
            apClient.MonitorLocationsAsync(gameLocations);
            await apClient.ReceiveReady();
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
            ArchipelagoClient? apClient = APClient;
            if (
                apClient?.ItemManager != null &&
                apClient?.CurrentSession != null
            )
            {
                // Do things after saving the game
                bool isSaving = MemoryHelpers.ReadAddressDataBit(Addresses.SavingFlag);
                if (isSaving != WasSaving)
                {
                    bool isInSaveDataMenu = MemoryHelpers.ReadAddressDataBit(Addresses.SaveDataMenuFlag);
                }
                if (WasSaving && !isSaving)
                {
                    APZennyCommittedToSave = GetReceivedAPZennyTotal(apClient.CurrentSession.Items.AllItemsReceived);
                    APClient?.CurrentSession.DataStorage[Scope.Slot, "mml_ap_zenny_committed"] = APZennyCommittedToSave;
                }
                WasSaving = isSaving;

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
                        // Handle text and possible overflows from locations that give items during text windows Mine Parts Kit from the rescue shop owner's husband location
                        LoopHelpers.HandleOddLocationText(currentLevelData, ScoutedLocationItemData, TextDataToWriteStack);
                        // Fix cutscene
                        LoopHelpers.HandleRedRefractorInSupportCar();
                        //LoopHelpers.HandleCutsceneSkipItemObtains(); // Moving to slowgameloop
                    }

                    // Run these after loading
                    // These will happen before AND after slow game loop, so be mindful
                    // and only put stuff here that needs to be fast and doesn't interact with slow game loop
                    // (e.g. overwriting things happening in the internal game loop)
                    else
                    {
                        if (currentLevelID != PreviousLevelID_Fast)
                        {
                            Cheats.Restore1FXXXWrites(currentLevelData);
                            PreviousLevelID_Fast = currentLevelID;
                        }

                        LoopHelpers.HandleYellowRefractorTerminal(currentLevelData);
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
            ArchipelagoClient? apClient = APClient;
            if (
                apClient?.ItemManager != null &&
                apClient?.CurrentSession != null
            )
            {
                // Don't do these loop actions if in title menu or save menu
                if (
                    MemoryHelpers.IsOutOfTitleScreen() &&
                    !MemoryHelpers.ReadAddressDataBit(Addresses.SaveDataMenuFlag)
                )
                {
                    // Task 1: Read useful memory
                    CurrentProgressionCounter = Memory.ReadByte(Addresses.CurrentProgressionCounter.Address);
                    CheckGoalCondition();


                    ushort currentLevelID = Memory.ReadUShort(Addresses.CurrentLevel.Address, Enums.Endianness.Big);
                    if (currentLevelID != PreviousLevelID_Slow)
                    {
                        apClient.CurrentSession.DataStorage[Scope.Slot, "MML_ROOM"] = currentLevelID;
                        IsManagingLevelChange = true;
                    }
                    PreviousLevelID_Slow = currentLevelID;
                    if (DataDicts.LevelDataDict.TryGetValue(currentLevelID, out LevelData? currentLevelData))
                    {
                        VisitedAreaNames.TryAdd(currentLevelData.AreaName, 0);

                        bool isSafeToManageAndRestoreMemory =
                            !MemoryHelpers.ReadAddressDataBit(Addresses.LoadingFlag) &&
                            !MemoryHelpers.ReadAddressDataBit(Addresses.ScreenWipeFlag) &&
                            !MemoryHelpers.ReadAddressDataBit(Addresses.CameraAlteredFlag);

                        if (isSafeToManageAndRestoreMemory)
                        {
                            // Remove items that may be given from skipping cutscenes
                            LoopHelpers.HandleCutsceneSkipItemObtains(currentLevelData);

                            //Log.Logger.Information($"IsManagingLevelChange: {IsManagingLevelChange}");
                            // Task 2.b: Do things when changing level like overwrite text, write code that isnt needed during loading, and locking doors
                            if (IsManagingLevelChange)
                            {
                                // Do slow memory writes, typically ones that are low priority or cause problems in fast write
                                LoopHelpers.HandleSlowCodeWrites(currentLevelData, CurrentProgressionCounter);
                                LoopHelpers.HandleAreaExitLocks(currentLevelData);
                                LoopHelpers.HandleFlutterFixedBrokenDistinction(currentLevelData);
                                LoopHelpers.HandleOddPails(currentLevelData);
                                IsManagingLevelChange = false;
                            }

                            // Task 3: Do things regardless of level change
                            // Task 3.a: Restore overwritten memory
                            // If we have overwritten text for a scouted location, check if the textbox is closed, and if so, restore the original text
                            if (!MemoryHelpers.ReadAddressDataBit(Addresses.TextBoxOpenFlag))
                            {
                                while (TextDataToWriteStack.TryPop(out var overwrittenTextData))
                                {
                                    Memory.WriteByteArray(overwrittenTextData.StartAddress, overwrittenTextData.TextByteArr);
                                }
                            }
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
                    IsLoadingIntoGame = true;
                }
                IsPreviouslyInTitleScreen = isCurrentlyInTitleScreen;

                if (
                    IsLoadingIntoGame &&
                    LocationManager_EnableLocationsCondition()
                )
                {
                    SyncSyntheticLocations();

                    IReadOnlyCollection<long> allLocationsChecked = apClient.CurrentSession.Locations.AllLocationsChecked;
                    RecheckPreviouslyCheckedContainerLocations(allLocationsChecked);

                    IReadOnlyCollection<ItemInfo> allItemsReceived = apClient.CurrentSession.Items.AllItemsReceived;
                    ReceivePreviouslyReceivedItems(allItemsReceived);

                    IsLoadingIntoGame = false;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Warning("Encountered an error while managing the game loop.");
            Log.Logger.Warning(ex.ToString());
        }
        finally
        {
            System.Threading.Interlocked.Exchange(ref IsSlowLoopRunning, 0);
        }
        return;
    }

    private static void SyncSyntheticLocations()
    {
        IReadOnlyCollection<long>? allLocationsChecked = APClient?.CurrentSession?.Locations?.AllLocationsChecked;
        if (allLocationsChecked == null)
        {
            return;
        }

        _ = MemoryHelpers.WriteAddressDataBit(Addresses.CardonForestSubGateJakkoStarterKeyPickup, allLocationsChecked.Contains(60));
        _ = MemoryHelpers.WriteAddressDataBit(Addresses.CardonForestSubGateConveyorStarterKeyPickup, allLocationsChecked.Contains(61));
        _ = MemoryHelpers.WriteAddressDataBit(Addresses.CardonForestSubGateThreeSwitchStarterKeyPickup, allLocationsChecked.Contains(62));
    }

    private static void RecheckPreviouslyCheckedContainerLocations(IReadOnlyCollection<long> allLocationsChecked)
    {
        if (allLocationsChecked.Count == 0)
        {
            return;
        }

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

    private static uint GetReceivedAPZennyTotal(IReadOnlyCollection<ItemInfo> allItemsReceived)
    {
        uint total = 0;
        foreach (ItemInfo itemInfo in allItemsReceived)
        {
            if (
                DataDicts.ItemDataDict.TryGetValue(itemInfo.ItemId, out ItemData? itemData) &&
                itemData.Category == ItemCategory.Zenny
            )
            {
                total += itemData.Quantity;
            }
        }
        return total;
    }

    private static void ReceivePreviouslyReceivedItems(IReadOnlyCollection<ItemInfo> allItemsReceived)
    {
        if (allItemsReceived.Count == 0)
        {
            return;
        }


        foreach (ItemInfo itemInfo in allItemsReceived)
        {
            if (DataDicts.ItemDataDict.TryGetValue(itemInfo.ItemId, out ItemData? itemData))
            {

                // Prevent giving duplicate item if buster part is equipped
                byte equippedBusterPart1 = Memory.ReadByte(0xB5600);
                byte equippedBusterPart2 = Memory.ReadByte(0xB5601);
                byte equippedBusterPart3 = Memory.ReadByte(0xB5602);
                if (itemData.Category == ItemCategory.Buster && itemData.ItemCode != null)
                {
                    // Equipped buster slots use the same +1 encoding used by inventory slots
                    byte equippedItemCode = (byte)(itemData.ItemCode.Value + 1);
                    if (
                        equippedItemCode == equippedBusterPart1 ||
                        equippedItemCode == equippedBusterPart2 ||
                        equippedItemCode == equippedBusterPart3
                    )
                    {
                        continue;
                    }
                }

                // Zenny is handled below
                if (itemData.Category == ItemCategory.Zenny)
                {
                    continue;
                }

                // Else give item
                ItemHelpers.ReceiveGenericItem(itemData);
            }
            else
            {
                Log.Logger.Warning($"Failed to receive item ID {itemInfo.ItemId} after loading save. Please report this in the Discord thread!");
            }
        }

        uint receivedAPZennyTotal = GetReceivedAPZennyTotal(allItemsReceived);
        if (APZennyCommittedToSave == null)
        {
            APZennyCommittedToSave = receivedAPZennyTotal;
            return;
        }

        if (receivedAPZennyTotal > APZennyCommittedToSave.Value)
        {
            uint missingZenny = receivedAPZennyTotal - APZennyCommittedToSave.Value;
            ItemHelpers.ReceiveGenericItem(new ItemData(ItemCategory.Zenny, "Zenny", missingZenny));
        }
    }

    private static void CheckGoalCondition()
    {
        ArchipelagoClient? apClient = APClient;
        if (
            HasSubmittedGoal ||
            apClient?.Options == null ||
            !LocationManager_EnableLocationsCondition() ||
            !apClient.Options.TryGetValue("goal", out var goal)
        )
        {
            return;
        }

        bool isGoalComplete = (CompletionGoal)int.Parse(goal.ToString()) switch
        {
            CompletionGoal.JUNO => MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedJuno),
            CompletionGoal.ALL_BOSSES =>
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedFerdinand) &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBonBonne) &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedMarlwolf) &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedBalkonGerat) &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedGarudoriten) &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedKarumunaBashTrio) &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedFockeWulf) &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedTheodoreBruno) &&
                MemoryHelpers.ReadAddressDataBit(Addresses.HasDefeatedJuno),
            _ => false
        };

        if (isGoalComplete)
        {
            if (System.Threading.Interlocked.CompareExchange(ref _hasSubmittedGoal, 1, 0) != 0)
            {
                return;
            }
            apClient.SendGoalCompletion();
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
        ArchipelagoClient? apClient = APClient;
        if (
            apClient?.CurrentSession != null &&
            DataDicts.ItemDataDict.TryGetValue(args.Item.Id, out ItemData? itemData)
        )
        {
            ItemHelpers.ReceiveGenericItem(itemData);

            // Unlock regions for items which do so
            if (
                args.Item.Id is 0x022A or 0x022B or 0x022C
            )
            {
                ushort currentLevelID = Memory.ReadUShort(Addresses.CurrentLevel.Address, Enums.Endianness.Big);
                if (DataDicts.LevelDataDict.TryGetValue(currentLevelID, out LevelData? currentLevelData))
                {
                    LoopHelpers.HandleAreaExitUnlocks(currentLevelData, args.Item.Id);
                }
            }

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
        ArchipelagoClient? apClient = APClient;
        if (
            apClient?.ItemManager != null &&
            apClient?.CurrentSession != null
        )
        {
            if (!LocationManager_EnableLocationsCondition() && !HasSubmittedGoal)
            {
                Log.Logger.Error("Check sent while not in game. Please report this in the Discord thread!");
            }

        }
        return;
    }
}
