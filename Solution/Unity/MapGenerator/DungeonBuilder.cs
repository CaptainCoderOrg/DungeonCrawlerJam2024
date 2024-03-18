using UnityEngine;

namespace CaptainCoder.Dungeoneering.DungeonMap.Unity;

using CaptainCoder.Dungeoneering.DungeonCrawler;
using CaptainCoder.Dungeoneering.DungeonMap.IO;
using CaptainCoder.Unity;

public class DungeonBuilder : MonoBehaviour
{
    [field: SerializeField]
    public Transform TileParent { get; private set; } = null!;
    [field: SerializeField]
    public DungeonTile TilePrefab { get; private set; } = null!;
    public void Build(Dungeon dungeon) => BuildDungeon(TileParent, TilePrefab, Destroy, dungeon);

    public static void BuildDungeon(Transform parent, DungeonTile tilePrefab, Action<GameObject> destroy, Dungeon dungeon)
    {
        Dictionary<Position, DungeonTile> allTiles = new();
        parent.DestroyAllChildren(destroy);

        for (int x = 0; x < 24; x++)
        {
            for (int y = 0; y < 24; y++)
            {
                DungeonTile newTile = Instantiate(tilePrefab, parent);
                newTile.name = $"({x}, {y})";
                newTile.transform.position = new Vector3(y, 0, x);
                newTile.UpdateWalls(dungeon.GetTile(new Position(x, y)).Walls);
                allTiles[new Position(x, y)] = newTile;
            }
        }

        // TODO: Is there a memory leak here when the dungeon changes?
        dungeon.Walls.OnWallChanged += UpdateWalls;
        void UpdateWalls(Position position, Facing facing, WallType wall)
        {
            DungeonTile toUpdate = allTiles[position];
            toUpdate.UpdateWalls(dungeon.GetTile(position).Walls);
        }
    }
}

[CreateAssetMenu(fileName = "DungeonData", menuName = "Data/Manifest")]
public class DungeonManifestData : ScriptableObject
{
    [field: SerializeField]
    public TextAsset? ManifestJson { get; private set; }

    public DungeonCrawlerManifest LoadManifest() => JsonExtensions.LoadModel<DungeonCrawlerManifest>(ManifestJson!.text);
}