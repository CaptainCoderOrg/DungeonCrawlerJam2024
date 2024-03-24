using UnityEngine;
using UnityEngine.Tilemaps;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class CombatMapController : MonoBehaviour
{
    [field: SerializeField]
    public Tilemap TileMap { get; set; } = default!;

    [field: SerializeField]
    public TileBase DungeonTileBase { get; set; } = default!;

    public void Awake()
    {
        CombatMap map = new()
        {
            Tiles = CombatMapExtensions.ParseTiles(
                """
                 ####    ##
                ############
                ############
                 ####   ##
                """
            ),
        };
        BuildMap(map);
    }

    public void BuildMap(CombatMap toBuild)
    {
        TileMap.ClearAllTiles();
        foreach (Position tile in Grow(toBuild.Tiles))
        {
            TileMap.SetTile(new Vector3Int(tile.X, -tile.Y, 0), DungeonTileBase);
        }
    }

    private static HashSet<Position> Grow(HashSet<Position> toGrow)
    {
        HashSet<Position> grown = [];
        foreach (Position p in toGrow) { grown.UnionWith(Grow(p)); }
        return grown;
    }

    private static IEnumerable<Position> Grow(Position p)
    {
        return [
            new Position(p.X - 1, p.Y - 1),
            new Position(p.X, p.Y - 1),
            new Position(p.X + 1, p.Y - 1),
            new Position(p.X - 1, p.Y - 0),
            new Position(p.X, p.Y - 0),
            new Position(p.X + 1, p.Y - 0),
            new Position(p.X - 1, p.Y + 1),
            new Position(p.X, p.Y + 1),
            new Position(p.X + 1, p.Y + 1),
        ];
    }
}