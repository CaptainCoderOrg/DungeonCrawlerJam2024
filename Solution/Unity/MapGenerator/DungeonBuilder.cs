using UnityEngine;

namespace CaptainCoder.Dungeoneering.DungeonMap.Unity;

using CaptainCoder.Dungeoneering.DungeonCrawler;
using CaptainCoder.Dungeoneering.DungeonMap.IO;
using CaptainCoder.Unity;

public class DungeonBuilder : MonoBehaviour
{
    private static Dictionary<string, Material>? s_materialCache;
    public static Dictionary<string, Material> MaterialCache => s_materialCache ?? throw new Exception("Material Cache has not yet been initialized.");
    [field: SerializeField]
    public Transform TileParent { get; private set; } = null!;
    [field: SerializeField]
    public DungeonTile TilePrefab { get; private set; } = null!;
    public static Dictionary<string, Material> InitializeMaterialCache(DungeonCrawlerManifest manifest)
    {
        s_materialCache = manifest.Textures.Values.ToDictionary(t => t.Name, t => t.ToMaterial());
        return s_materialCache;
    }
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
                Position position = new(x, y);
                newTile.UpdateWalls(dungeon.GetTile(position).Walls, MaterialCache.GetTileWallMaterials(dungeon, position));
                allTiles[new Position(x, y)] = newTile;
            }
        }

        // TODO: Is there a memory leak here when the dungeon changes?
        dungeon.Walls.OnWallChanged += UpdateWalls;
        void UpdateWalls(Position position, Facing facing, WallType wall)
        {
            DungeonTile toUpdate = allTiles[position];
            toUpdate.UpdateWalls(dungeon.GetTile(position).Walls, MaterialCache.GetTileWallMaterials(dungeon, position));
        }
    }
}

public static class DungeonExtensions
{
    public static TileWallMaterials GetTileWallMaterials(this Dictionary<string, Material> cache, Dungeon dungeon, Position position)
    {
        return new TileWallMaterials()
        {
            North = cache.GetValueOrDefault(dungeon.GetTextureName(position, Facing.North)),
            East = cache.GetValueOrDefault(dungeon.GetTextureName(position, Facing.East)),
            South = cache.GetValueOrDefault(dungeon.GetTextureName(position, Facing.South)),
            West = cache.GetValueOrDefault(dungeon.GetTextureName(position, Facing.West)),
        };
    }
}

[CreateAssetMenu(fileName = "DungeonData", menuName = "Data/Manifest")]
public class DungeonManifestData : ScriptableObject
{
    [field: SerializeField]
    public TextAsset? ManifestJson { get; private set; }

    public DungeonCrawlerManifest LoadManifest() => JsonExtensions.LoadModel<DungeonCrawlerManifest>(ManifestJson!.text);
}

public static class TextureExtensions
{
    public static Material ToMaterial(this Texture texture)
    {
        Texture2D t2d = new(2, 2);
        ImageConversion.LoadImage(t2d, texture.Data);
        Material material = new(Shader.Find("Standard"));
        material.mainTexture = t2d;
        return material;
    }
}