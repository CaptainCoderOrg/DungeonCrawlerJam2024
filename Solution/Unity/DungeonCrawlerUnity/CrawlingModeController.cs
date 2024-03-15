using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.DungeonMap.Unity;
using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Player;
using CaptainCoder.Dungeoneering.Player.Unity;

using DungeonCrawler.Lua;

using UnityEngine;

namespace CaptainCoder.Dungeoneering.Unity;

public class CrawlingModeController : MonoBehaviour, IScriptContext
{
    private CrawlerMode _crawlerMode = default!;
    [field: SerializeField]
    public PlayerViewData PlayerViewData { get; private set; } = new(new PlayerView(0, 0, Facing.North));
    [field: SerializeField]
    public DungeonData DungeonData { get; private set; } = null!;
    [field: SerializeField]
    public DungeonBuilder DungeonBuilder { get; set; } = default!;
    [field: SerializeField]
    public Transform PlayerCamera { get; set; } = default!;
    [field: SerializeField]
    public PlayerInputHandler PlayerInputHandler { get; set; } = default!;
    public PlayerView View { get => _crawlerMode.CurrentView; set => _crawlerMode.CurrentView = value; }
    public Dungeon CurrentDungeon { get => _crawlerMode.CurrentDungeon; }
    public void Awake()
    {
        Dungeon dungeon = new(DungeonData.LoadMap());
        _crawlerMode = new CrawlerMode(dungeon, new PlayerView(PlayerViewData.X, PlayerViewData.Y, PlayerViewData.Facing));
        DungeonBuilder.Build(dungeon);
        PlayerCamera.InstantTransitionToPlayerView(_crawlerMode.CurrentView);
        _crawlerMode.OnViewChange += (newView) => PlayerViewData = new(newView);
        _crawlerMode.OnViewChange += PlayerCamera.InstantTransitionToPlayerView;
    }

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
}

[Serializable]
public class PlayerViewData(PlayerView view)
{
    public int X = view.Position.X;
    public int Y = view.Position.Y;
    public Facing Facing = view.Facing;
}