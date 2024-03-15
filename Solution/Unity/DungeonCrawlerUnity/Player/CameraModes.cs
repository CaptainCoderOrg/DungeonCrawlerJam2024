using CaptainCoder.Dungeoneering.DungeonMap;

using UnityEngine;

namespace CaptainCoder.Dungeoneering.Player.Unity;

public static class CameraModes
{
    public static void InstantTransitionToPlayerView(this Transform transform, PlayerView view)
    {
        var (x, y) = view.Position;
        transform.position = new Vector3(y, 0, x);
        transform.rotation = view.Facing switch
        {
            Facing.North => Quaternion.Euler(0, -90, 0),
            Facing.East => Quaternion.Euler(0, 0, 0),
            Facing.South => Quaternion.Euler(0, 90, 0),
            Facing.West => Quaternion.Euler(0, 180, 0),
            _ => throw new NotImplementedException($"Unexpected Facing: {view.Facing}"),
        };
    }
}