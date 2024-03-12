using UnityEngine;

namespace CaptainCoder.Dungeoneering.DungeonMap.Unity;
using CaptainCoder.Dungeoneering.DungeonMap.IO;

using NaughtyAttributes;

public class DungeonController : MonoBehaviour
{
    [field: SerializeField]
    public DungeonData Data { get; private set; } = null!;
    [field: SerializeField]
    public GameObject TileParent { get; private set; } = null!;
    [field: SerializeField]
    public DungeonTile TilePrefab { get; private set; } = null!;
    public Dungeon Dungeon => _dungeon;
    private Dungeon _dungeon = null!;

    void Awake()
    {
        _dungeon = new Dungeon(Data.LoadMap());
        BuildDungeon();
    }

    private void BuildDungeon(Action<GameObject> destroy)
    {
        TileParent.transform.DestroyAllChildren(destroy);

        for (int x = 0; x < 24; x++)
        {
            for (int y = 0; y < 24; y++)
            {
                DungeonTile newTile = Instantiate(TilePrefab, TileParent.transform);
                newTile.name = $"({x}, {y})";
                newTile.transform.position = new Vector3(y, 0, x);
                newTile.UpdateWalls(_dungeon.GetTile(new Position(x, y)).Walls);
            }
        }
    }
    private void BuildDungeon() => BuildDungeon(Destroy);

    [Button("Force Build Dungeon")]
    private void ForceBuildDungeon()
    {
        _dungeon = new Dungeon(Data.LoadMap());
        BuildDungeon(DestroyImmediate);
    }
}

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

[CreateAssetMenu(fileName = "DungeonData", menuName = "Data/Dungeon")]
public class DungeonData : ScriptableObject
{
    [field: SerializeField]
    public TextAsset? WallMapJson { get; private set; }

    // [Button("Load Data")]
    public WallMap LoadMap() => JsonExtensions.LoadModel<WallMap>(WallMapJson!.text);
}

public static class TransformExtensions
{
    public static void DestroyAllChildren(this Transform parent, Action<GameObject> destroy)
    {
        while (parent.childCount > 0)
        {
            Transform child = parent.GetChild(0);
            child.parent = null;
            destroy.Invoke(child.gameObject);
        }
    }
}