using CaptainCoder.Dungeoneering.DungeonCrawler;
using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.DungeonMap.Unity;
using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Game.Unity;
using CaptainCoder.Dungeoneering.Lua;
using CaptainCoder.Dungeoneering.Player;
using CaptainCoder.Dungeoneering.Player.Unity;


using UnityEngine;

namespace CaptainCoder.Dungeoneering.Unity;

public class CrawlingModeController : MonoBehaviour, IScriptContext
{
    private CrawlerMode _crawlerMode = default!;
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
    public PlayerView View { get => _crawlerMode.CurrentView; set => _crawlerMode.CurrentView = value; }
    public Dungeon CurrentDungeon { get => _crawlerMode.CurrentDungeon; set => _crawlerMode.CurrentDungeon = value; }
    [field: SerializeField]
    public QueuedMessageRenderer InfoMessageRenderer { get; set; } = default!;
    public GameState State { get; set; } = new();
    public DungeonCrawlerManifest Manifest { get => _crawlerMode.Manifest; set => _crawlerMode.Manifest = value; }

    public void Awake()
    {
        DungeonCrawlerManifest manifest = DungeonData.LoadManifest();
        Dungeon dungeon = manifest.Dungeons["Town"];
        _crawlerMode = new CrawlerMode(manifest, dungeon, new PlayerView(PlayerViewData.X, PlayerViewData.Y, PlayerViewData.Facing));
        DungeonBuilder.Build(dungeon);
        PlayerCamera.InstantTransitionToPlayerView(_crawlerMode.CurrentView);
        _crawlerMode.OnViewChange += (viewChangeEvent) => PlayerViewData = new(viewChangeEvent.Entered);
        _crawlerMode.OnViewChange += viewChangeEvent => PlayerCamera.InstantTransitionToPlayerView(viewChangeEvent.Entered);
        _crawlerMode.OnPositionChange += HandleOnEnterEvents;
        _crawlerMode.OnPositionChange += HandleOnExitEvents;
        _crawlerMode.OnMessageAdded += message =>
        {
            InfoMessageRenderer.EnqueueMessage(message, 5f);
            Debug.Log(message);
        };
        _crawlerMode.OnDungeonChange += ChangeDungeon;
    }

    public void ChangeDungeon(DungeonChangeEvent evt)
    {
        evt.Exited.Walls.ClearOnWallChangedEvents();
        DungeonBuilder.Build(evt.Entered);
    }

    public void SendMessage(Message message) => _crawlerMode.AddMessage(message);

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
        _crawlerMode.CurrentView = _crawlerMode.CurrentDungeon.Move(_crawlerMode.CurrentView, action);
    }

    public void HandleOnEnterEvents(PositionChangeEvent change)
    {
        IEnumerable<TileEvent> enterEvents = _crawlerMode.CurrentDungeon.EventMap
                                                         .EventsAt(change.Entered)
                                                         .Where(evt => evt.Trigger is EventTrigger.OnEnter);
        foreach (TileEvent triggered in enterEvents)
        {
            EventScript script = _crawlerMode.Manifest.Scripts[triggered.ScriptName];
            Interpreter.ExecLua(script.Script, this);
        }
    }

    public void HandleOnExitEvents(PositionChangeEvent change)
    {
        IEnumerable<TileEvent> enterEvents = _crawlerMode.CurrentDungeon.EventMap
                                                         .EventsAt(change.Exited)
                                                         .Where(evt => evt.Trigger is EventTrigger.OnExit);
        foreach (TileEvent triggered in enterEvents)
        {
            EventScript script = _crawlerMode.Manifest.Scripts[triggered.ScriptName];
            Interpreter.ExecLua(script.Script, this);
        }
    }

}

[Serializable]
public class PlayerViewData(PlayerView view)
{
    public int X = view.Position.X;
    public int Y = view.Position.Y;
    public Facing Facing = view.Facing;
}