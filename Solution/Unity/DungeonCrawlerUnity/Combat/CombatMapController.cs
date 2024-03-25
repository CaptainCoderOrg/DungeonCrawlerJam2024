using System.Collections;

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
    public CharacterSelectionModeController CharacterSelectionModeController = default!;
    public SpendActionPointsModeController SpendActionPointsModeController = default!;
    private readonly PlayerCharacter[] _characters = [
        new PlayerCharacter() { Card = Characters.CharacterA, ActionPoints = 1 },
        new PlayerCharacter() { Card = Characters.CharacterB, ActionPoints = 2 },
        new PlayerCharacter() { Card = Characters.CharacterC, ActionPoints = 3 },
        new PlayerCharacter() { Card = Characters.CharacterD, ActionPoints = 4 },
    ];

    public void Awake()
    {
        CombatMap map = new()
        {
            Tiles = CombatMapExtensions.ParseTiles(
                """       
                 ###
                #########
                #########
                 ###   ##
                       ##
                      ####
                     ######
                     ######
                      ####
                """
            ),
        };
        map.PlayerCharacters[new Position(2, 2)] = _characters[0];
        map.PlayerCharacters[new Position(9, 6)] = _characters[1];
        map.PlayerCharacters[new Position(0, 2)] = _characters[2];
        map.PlayerCharacters[new Position(8, 5)] = _characters[3];
        Initialize(map);
    }

    public void Initialize(CombatMap map)
    {
        BuildMap(map);
        StartCharacterSelect();
    }

    public void StartCharacterSelect()
    {
        CharacterSelectionModeController.Initialize(_characters);
        CharacterSelectionModeController.gameObject.SetActive(true);
        SpendActionPointsModeController.gameObject.SetActive(false);
    }

    public void StartSpendActionPoints(CharacterCardRenderer renderer, PlayerCharacter selected)
    {
        SpendActionPointsModeController.Initialize(renderer, selected);
        CharacterSelectionModeController.gameObject.SetActive(false);
        SpendActionPointsModeController.gameObject.SetActive(true);
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

    private Coroutine? _cameraCoroutine;
    public void PanTo(PlayerCharacter character)
    {
        Position position = _combatMap.GetPosition(character);
        Vector3 end = CombatCamera.transform.localPosition;
        end.x = position.ToVector3Int().x;
        end.y = position.ToVector3Int().y;
        if (_cameraCoroutine is not null) { StopCoroutine(_cameraCoroutine); };
        _cameraCoroutine = StartCoroutine(CombatCamera.transform.PanTo(end));
    }
}

public static class PositionExtensions
{
    public static readonly YieldInstruction WaitForEndOfFrame = new WaitForEndOfFrame();
    public static Vector3Int ToVector3Int(this Position position) => new(position.X, -position.Y, 0);
    public static IEnumerator PanTo(this Transform transform, Vector3 end, float duration = 0.3f)
    {
        Vector3 start = transform.localPosition;
        float elapsedTime = 0;
        while (Percent() < 1)
        {
            transform.localPosition = Vector3.Lerp(start, end, Percent());
            elapsedTime += Time.deltaTime;
            yield return WaitForEndOfFrame;
        }
        transform.localPosition = Vector3.Lerp(start, end, 1);
        float Percent() => elapsedTime / duration;
    }
}