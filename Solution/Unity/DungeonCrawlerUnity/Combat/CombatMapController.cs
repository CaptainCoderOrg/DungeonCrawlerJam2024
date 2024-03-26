using System.Collections;

using CaptainCoder.DungeonCrawler.Unity;
using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Game.Unity;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class CombatMapController : MonoBehaviour
{
    public static CombatMapController Shared { get; private set; } = default!;
    public float MoveAnimationSpeed = 0.1f;
    public Tilemap MapInfo = default!;
    public Tilemap Grid = default!;
    [field: SerializeField]
    public Tilemap TileMap { get; set; } = default!;
    [field: SerializeField]
    public Tilemap CursorMap { get; set; } = default!;
    [field: SerializeField]
    public TileBase DungeonTileBase { get; set; } = default!;
    [field: SerializeField]
    public Tilemap CharacterMap { get; set; } = default!;
    [field: SerializeField]
    public CombatIconDatabase IconDatabase { get; set; } = default!;
    [field: SerializeField]
    public Camera CombatCamera { get; private set; } = default!;
    public CombatMap CombatMap { get; private set; } = default!;
    private readonly PlayerCharacter[] _characters = [
        new PlayerCharacter() { Card = Characters.CharacterA, ActionPoints = 1 },
        new PlayerCharacter() { Card = Characters.CharacterB, ActionPoints = 2 },
        new PlayerCharacter() { Card = Characters.CharacterC, ActionPoints = 3 },
        new PlayerCharacter() { Card = Characters.CharacterD, ActionPoints = 4 },
    ];

    public CombatMapController() { Shared = this; }

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
        map.OnCharacterChange += HandleCharacterChange;
        map.OnMoveAction += HandleMove;
        CharacterActionMenuController.Shared.gameObject.SetActive(false);
        SpendActionPointsModeController.Shared.gameObject.SetActive(false);
        CharacterMoveController.Shared.gameObject.SetActive(false);
        StartCharacterSelect();
    }

    private void HandleMove(MoveActionEvent @event)
    {
        MessageRenderer.Shared.AddMessage(new Message($"{@event.Moving.Card.Name} moves {@event.Path.Count()} spaces."));
        StartCoroutine(AnimateMove(@event.Move.Start, @event.Path));
    }

    private IEnumerator AnimateMove(Position start, IEnumerable<Position> positions)
    {
        TileBase tile = CharacterMap.GetTile(start.ToVector3Int());
        TileBase? toSet = null;
        Position last = start;
        foreach (var position in positions)
        {
            CharacterMap.SetTile(last.ToVector3Int(), toSet);
            toSet = CharacterMap.GetTile(position.ToVector3Int());
            CharacterMap.SetTile(position.ToVector3Int(), tile);
            yield return new WaitForSeconds(MoveAnimationSpeed);
            last = position;
        }
    }

    private void HandleCharacterChange(PlayerCharacter character)
    {
        for (int ix = 0; ix < _characters.Length; ix++)
        {
            CharacterCard card = _characters[ix].Card;
            if (character.Card == card)
            {
                Overlay.Shared.Cards[ix].Render(character);
                break;
            }
        }
    }

    public void BuildMap(CombatMap toBuild)
    {
        CombatMap = toBuild;
        Grid.ClearAllTiles();
        TileMap.ClearAllTiles();
        MapInfo.ClearAllTiles();
        CharacterMap.ClearAllTiles();

        IEnumerable<Position> tiles = Grow(toBuild.Tiles);
        TileMap.SetTiles(tiles, DungeonTileBase);
        Grid.SetTiles(tiles, IconDatabase.Outline);
        CharacterMap.SetTiles(toBuild.PlayerCharacters.Keys, p => IconDatabase.GetTile(toBuild.PlayerCharacters[p]));
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
    public void SelectTiles(params Position[] position)
    {
        CursorMap.ClearAllTiles();
        CursorMap.SetTiles(position, IconDatabase.Selected);
    }

    public void HighlightTiles(IEnumerable<Position> positions, TileBase? tile = null, bool clear = true)
    {
        if (tile == null) { tile = IconDatabase.Green; }
        if (clear) { MapInfo.ClearAllTiles(); }
        MapInfo.SetTiles(positions, tile);
    }

    private Coroutine? _cameraCoroutine;
    public void PanTo(Position position)
    {
        Vector3 end = CombatCamera.transform.localPosition;
        end.x = position.ToVector3Int().x;
        end.y = position.ToVector3Int().y;
        if (_cameraCoroutine is not null) { StopCoroutine(_cameraCoroutine); };
        _cameraCoroutine = StartCoroutine(CombatCamera.transform.PanTo(end));
    }

    internal void PanToward(Position cursorPosition, float distance = 2)
    {
        Vector2 target = cursorPosition.ToVector2();
        Vector2 current = CombatCamera.transform.localPosition;
        // Do not pan if we are close enough
        if (Vector2.Distance(target, current) < distance) { return; }
        Vector3 end = Vector2.Lerp(current, target, 0.5f);
        end.z = CombatCamera.transform.localPosition.z;
        if (_cameraCoroutine is not null) { StopCoroutine(_cameraCoroutine); };
        _cameraCoroutine = StartCoroutine(CombatCamera.transform.PanTo(end));
    }

    internal void SelectCharacter(PlayerCharacter character)
    {
        Position position = CombatMap.GetPosition(character.Card);
        SelectTiles(position);
        PanTo(position);
    }

    public void StartCharacterSelect()
    {
        CharacterSelectionModeController.Shared.Initialize(_characters);
        CharacterSelectionModeController.Shared.gameObject.SetActive(true);
        SpendActionPointsModeController.Shared.gameObject.SetActive(false);
    }

    public void StartSpendActionPoints(PlayerCharacter selected)
    {
        SpendActionPointsModeController.Shared.Initialize(selected);
        CharacterSelectionModeController.Shared.gameObject.SetActive(false);
        SpendActionPointsModeController.Shared.gameObject.SetActive(true);
    }
}

public static class PositionExtensions
{
    public static readonly YieldInstruction WaitForEndOfFrame = new WaitForEndOfFrame();
    public static void SetTiles(this Tilemap map, IEnumerable<Position> positions, TileBase tile) => map.SetTiles([.. positions.Select(p => p.ToVector3Int())], [.. positions.Select(_ => tile)]);
    public static void SetTiles(this Tilemap map, IEnumerable<Position> positions, Func<Position, TileBase> lookup) => map.SetTiles([.. positions.Select(p => p.ToVector3Int())], [.. positions.Select(lookup)]);
    public static void SetTiles(this Tilemap map, IEnumerable<TileData> data) => map.SetTiles([.. data.Select(t => t.Position.ToVector3Int())], [.. data.Select(t => t.Tile)]);
    public static Vector3Int ToVector3Int(this Position position) => new(position.X, -position.Y, 0);
    public static Vector2 ToVector2(this Position position) => new(position.X, -position.Y);
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

public record class TileData(Position Position, TileBase Tile);