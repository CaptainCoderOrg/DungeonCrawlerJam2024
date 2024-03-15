using UnityEngine;

namespace CaptainCoder.Dungeoneering.DungeonMap.Unity;

public class DungeonTile : MonoBehaviour
{
    [field: SerializeField]
    public GameObject NorthWall { get; private set; } = default!;
    [field: SerializeField]
    public GameObject EastWall { get; private set; } = default!;
    [field: SerializeField]
    public GameObject SouthWall { get; private set; } = default!;
    [field: SerializeField]
    public GameObject WestWall { get; private set; } = default!;
    public void UpdateWalls(TileWalls configuration)
    {
        NorthWall.SetActive(configuration.North is not WallType.None);
        EastWall.SetActive(configuration.East is not WallType.None);
        WestWall.SetActive(configuration.West is not WallType.None);
        SouthWall.SetActive(configuration.South is not WallType.None);
    }
}