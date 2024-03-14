using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.DungeonMap.Unity;
using CaptainCoder.Dungeoneering.Player;
using CaptainCoder.Dungeoneering.Player.Unity;

using DungeonCrawler.Lua;

using UnityEngine;

namespace CaptainCoder.Dungeoneering.Unity;

public class CrawlingModeController : MonoBehaviour, IScriptContext
{
    [field: SerializeField]
    public DungeonController DungeonController { get; set; } = default!;
    [field: SerializeField]
    public PlayerViewController PlayerViewController { get; set; } = default!;
    public Player.PlayerView View
    {
        get => PlayerViewController.View;
        set
        {
            PlayerViewController.View = value;
            Debug.Log($"PlayerView: {value}");
        }
    }

    public void HandleMovement(MovementAction action)
    {
        PlayerViewController.View = DungeonController.Dungeon.Move(PlayerViewController.View, action);
    }
}