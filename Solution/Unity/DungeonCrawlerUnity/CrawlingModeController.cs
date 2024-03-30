using System.Collections;

using CaptainCoder.DungeonCrawler;
using CaptainCoder.DungeonCrawler.Combat;
using CaptainCoder.DungeonCrawler.Combat.Unity;
using CaptainCoder.DungeonCrawler.Unity;
using CaptainCoder.Dungeoneering.DungeonCrawler;
using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.DungeonMap.IO;
using CaptainCoder.Dungeoneering.DungeonMap.Unity;
using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Lua;
using CaptainCoder.Dungeoneering.Lua.Dialogue;
using CaptainCoder.Dungeoneering.Player;
using CaptainCoder.Dungeoneering.Player.Unity;

using UnityEngine;

namespace CaptainCoder.Dungeoneering.Unity;

public class CrawlingModeController : MonoBehaviour, IScriptContext
{
    public static CrawlingModeController Shared { get; private set; } = default!;
    public CrawlingModeController() { Shared = this; }
    public Party Party { get; set; } = new();
    public static event Action<CrawlerMode>? OnCrawlerModeChange;
    private static CrawlerMode? s_crawlerMode;
    public static CrawlerMode CrawlerMode
    {
        get
        {
            Debug.Assert(s_crawlerMode != null, "CrawlerMode has not been initialized.");
            return s_crawlerMode!;
        }
        private set
        {
            s_crawlerMode = value;
            OnCrawlerModeChange?.Invoke(s_crawlerMode);
        }
    }
    [field: SerializeField]
    public PlayerViewData PlayerViewData { get; private set; } = new(new PlayerView(0, 0, Facing.North));
    [field: SerializeField]
    public DungeonManifestData DungeonData { get; private set; } = null!;
    [field: SerializeField]
    public DungeonBuilder DungeonBuilder { get; set; } = default!;
    [field: SerializeField]
    public Transform PlayerCamera { get; set; } = default!;
    [field: SerializeField]
    public PlayerInputHandler PlayerInputHandler { get; set; } = default!;
    public PlayerView View { get => CrawlerMode.CurrentView; set => CrawlerMode.CurrentView = value; }
    public Dungeon CurrentDungeon { get => CrawlerMode.CurrentDungeon; set => CrawlerMode.CurrentDungeon = value; }
    [field: SerializeField]
    public GameState State { get; set; } = new();
    public DungeonCrawlerManifest Manifest { get => CrawlerMode.Manifest; set => CrawlerMode.Manifest = value; }
    [field: SerializeField]
    public DialogueController DialogueController { get; set; } = default!;
    public CrawlerMode Crawler => CrawlerMode;

    private Coroutine? _currentTransition;

    public void Awake()
    {
        LuaContext.LoadFromURL = (string url) => StartCoroutine(WebLoader.GetTextFromURL(url, Init, Fail));
        LuaContext.RebuildFromURL = (string url) => StartCoroutine(WebLoader.GetTextFromURL(url, Rebuild, Fail));
    }

    public void Start()
    {
        Init(DungeonData.ManifestJson!.text);
    }

    public void Fail(string failMessage)
    {
        throw new Exception("Failed to load URL");
    }
    private static readonly Dungeon EmptyDungeon = new();

    public void Rebuild(string projectJson)
    {
        DungeonCrawlerManifest manifest = JsonExtensions.LoadModel<DungeonCrawlerManifest>(projectJson);
        _ = DungeonBuilder.InitializeMaterialCache(manifest);
        PlayerView previousView = CrawlerMode.CurrentView;
        CrawlerMode.CurrentDungeon = manifest.Dungeons[CrawlerMode.CurrentDungeon.Name];
        CrawlerMode.CurrentView = previousView;
    }

    public void Init(string projectJson)
    {
        Party.ApplyValues(new Party());
        State = new GameState();
        DungeonCrawlerManifest manifest = JsonExtensions.LoadModel<DungeonCrawlerManifest>(projectJson);
        _ = DungeonBuilder.InitializeMaterialCache(manifest);
        CrawlerMode = new CrawlerMode(manifest, EmptyDungeon, new PlayerView(PlayerViewData.X, PlayerViewData.Y, PlayerViewData.Facing));
        PlayerCamera.InstantTransitionToPlayerView(CrawlerMode.CurrentView);
        CrawlerMode.OnViewChange += (viewChangeEvent) => PlayerViewData = new(viewChangeEvent.Entered);
        CrawlerMode.OnViewChange += (viewChangeEvent) => CompassController.Shared.Render(viewChangeEvent.Entered);
        CrawlerMode.OnViewChange += HandleMoveTransition;
        CrawlerMode.OnPositionChange += HandleOnEnterEvents;
        CrawlerMode.OnPositionChange += HandleOnExitEvents;
        CrawlerMode.OnPositionChange += (_) => SFXController.Shared.PlaySound(Sound.Footstep);
        CrawlerMode.OnDungeonChange += ChangeDungeon;
        StartCoroutine(InitNextFrame());
    }

    private IEnumerator InitNextFrame()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Initialize("start.lua");
    }


    private void HandleMoveTransition(ViewChangeEvent evt)
    {
        if (_currentTransition != null) { StopCoroutine(_currentTransition); }
        _currentTransition = StartCoroutine(PlayerCamera.LerpTransitionToPlayerView(evt.Exited, evt.Entered));
    }

    public void ChangeDungeon(DungeonChangeEvent evt)
    {
        evt.Exited.Walls.ClearOnWallChangedEvents();
        DungeonBuilder.Build(evt.Entered);
    }

    public void SendMessage(Message message) => CrawlerMode.AddMessage(message);

    public void OnEnable()
    {
        PlayerInputHandler.OnMovementAction?.AddListener(HandleMovement);
    }

    public void OnDisable()
    {
        PlayerInputHandler.OnMovementAction?.RemoveListener(HandleMovement);
    }

    public void HandleMovement(MovementAction action)
    {
        CrawlerMode.CurrentView = CrawlerMode.CurrentDungeon.Move(CrawlerMode.CurrentView, action);
    }

    public void HandleOnEnterEvents(PositionChangeEvent change)
    {
        IEnumerable<TileEvent> enterEvents = CrawlerMode.CurrentDungeon.EventMap
                                                         .EventsAt(change.Entered)
                                                         .Where(evt => evt.Trigger is EventTrigger.OnEnter);
        foreach (TileEvent triggered in enterEvents)
        {
            EventScript script = CrawlerMode.Manifest.Scripts[triggered.ScriptName];
            Interpreter.ExecLua(script.Script, this);
        }
    }

    public void HandleOnExitEvents(PositionChangeEvent change)
    {
        IEnumerable<TileEvent> enterEvents = CrawlerMode.CurrentDungeon.EventMap
                                                         .EventsAt(change.Exited)
                                                         .Where(evt => evt.Trigger is EventTrigger.OnExit);
        foreach (TileEvent triggered in enterEvents)
        {
            EventScript script = CrawlerMode.Manifest.Scripts[triggered.ScriptName];
            Interpreter.ExecLua(script.Script, this);
        }
    }

    public void ShowDialogue(Dialogue dialogue) => DialogueController.Show(dialogue);

    internal void Initialize(string? nameOfScript = null)
    {
        CrawlingViewPortController.Shared.gameObject.SetActive(true);
        gameObject.SetActive(true);
        if (nameOfScript is not null)
        {
            EventScript script = CrawlerMode.Manifest.Scripts[nameOfScript];
            Interpreter.ExecLua(script.Script, this);
        }
    }

    public void StartCombat(string mapSetup, string onWinScript, string onGiveUpScript) => CombatMapController.Shared.Initialize(mapSetup, onWinScript, onGiveUpScript);

    public void GiveWeapon(Weapon weapon) => WeaponDialogueScreen.Shared.Initialize(weapon);
}

[Serializable]
public class PlayerViewData(PlayerView view)
{
    public int X = view.Position.X;
    public int Y = view.Position.Y;
    public Facing Facing = view.Facing;
}