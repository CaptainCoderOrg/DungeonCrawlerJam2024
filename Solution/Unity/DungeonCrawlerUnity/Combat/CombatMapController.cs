using UnityEngine;
using UnityEngine.Tilemaps;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class CombatMapController : MonoBehaviour
{
    [field: SerializeField]
    public Tilemap TileMap { get; set; } = default!;
    [field: SerializeField]
    public TileBase DungeonTileBase { get; set; } = default!;

    [field: SerializeField]
    public Tilemap CharacterMap { get; set; } = default!;
    [field: SerializeField]
    public CombatIconDatabase IconDatabase { get; set; } = default!;
    [field: SerializeField]
    public Camera CombatCamera { get; private set; } = default!;
    private CombatMap _combatMap = default!;

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
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { new Position(2, 2), new PlayerCharacter(){ Card = Characters.CharacterA } },
                { new Position(4, 2), new PlayerCharacter(){ Card = Characters.CharacterB } },
                { new Position(4, 0), new PlayerCharacter(){ Card = Characters.CharacterC } },
                { new Position(9, 2), new PlayerCharacter(){ Card = Characters.CharacterD } },
            },
        };
        BuildMap(map);
    }

    public void BuildMap(CombatMap toBuild)
    {
        _combatMap = toBuild;
        TileMap.ClearAllTiles();
        foreach (Position tile in Grow(toBuild.Tiles))
        {
            TileMap.SetTile(tile.ToVector3Int(), DungeonTileBase);
        }
        CharacterMap.ClearAllTiles();
        foreach ((Position position, PlayerCharacter character) in toBuild.PlayerCharacters)
        {
            CharacterMap.SetTile(position.ToVector3Int(), IconDatabase.GetTileBase(character));
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

    public void PanTo(PlayerCharacter character)
    {
        Position position = _combatMap.GetPosition(character);
        Vector3 camPosition = CombatCamera.transform.localPosition;
        camPosition.x = position.ToVector3Int().x;
        camPosition.y = position.ToVector3Int().y;
        CombatCamera.transform.localPosition = camPosition;
    }
}

public static class PositionExtensions
{
    public static Vector3Int ToVector3Int(this Position position) => new(position.X, -position.Y, 0);
}