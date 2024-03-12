using CaptainCoder.Dungeoneering.DungeonMap;

using UnityEngine;

namespace CaptainCoder.Dungeoneering.Player.Unity;

public class PlayerViewController : MonoBehaviour
{
    private PlayerView _view = default!;
    public PlayerView View
    {
        get => _view;
        set
        {
            _view = value;
            var (x, y) = _view.Position;
            transform.position = new Vector3(y, 0, x);
            transform.rotation = _view.Facing switch
            {
                Facing.North => Quaternion.Euler(0, -90, 0),
                Facing.East => Quaternion.Euler(0, 0, 0),
                Facing.South => Quaternion.Euler(0, 90, 0),
                Facing.West => Quaternion.Euler(0, 180, 0),
                _ => throw new NotImplementedException($"Unexpected Facing: {_view.Facing}"),
            };
        }
    }

    public void Awake()
    {
        View = new PlayerView(new Position(0, 0), Facing.East);
    }
}