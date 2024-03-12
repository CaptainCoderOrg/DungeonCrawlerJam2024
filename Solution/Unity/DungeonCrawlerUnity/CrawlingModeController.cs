using CaptainCoder.Dungeoneering.DungeonMap.Unity;
using CaptainCoder.Dungeoneering.Player;
using CaptainCoder.Dungeoneering.Player.Unity;

using UnityEngine;

namespace CaptainCoder.Dungeoneering.Unity;

public class CrawlingModeController : MonoBehaviour
{
    [field: SerializeField]
    public DungeonController DungeonController { get; set; } = default!;
    [field: SerializeField]
    public PlayerViewController PlayerViewController { get; set; } = default!;

    public void HandleMovement(MovementAction action)
    {
        PlayerViewController.View = DungeonController.Dungeon.Move(PlayerViewController.View, action);
    }
}