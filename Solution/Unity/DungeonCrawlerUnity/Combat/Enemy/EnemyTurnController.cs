using System.Collections;

using CaptainCoder.Dungeoneering.Game.Unity;
using CaptainCoder.Dungeoneering.Player.Unity;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class EnemyTurnController : MonoBehaviour
{
    public static EnemyTurnController Shared { get; private set; } = default!;
    public EnemyTurnController() { Shared = this; }
    private CombatMap Map => CombatMapController.Shared.CombatMap;
    private Queue<Position>? _enemyPositions;
    private Queue<Position> EnemyPositions => _enemyPositions ?? throw new Exception("Enemy positions is not initialized");

    public void Update()
    {
        PlayerInputHandler.Shared.InputMapping.OnMenuAction(HandleInput);
    }

    private void HandleInput(MenuControl control)
    {
        if (control is MenuControl.Select or MenuControl.Cancel)
        {
            Debug.Log("Not implement: Pause Enemy Turn / Guard");
        }
    }

    public void OnEnable()
    {
        CharacterSelectionModeController.Shared.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        _enemyPositions = new Queue<Position>(Map.Enemies.Keys);
        gameObject.SetActive(true);
        DoNextMove();
    }

    private void DoNextMove()
    {
        if (!EnemyPositions.TryDequeue(out Position position))
        {
            EndEnemyTurn();
            return;
        }
        StartCoroutine(PerformMove(position));
    }

    public IEnumerator PerformMove(Position position)
    {
        Enemy e = Map.Enemies[position];
        MessageRenderer.Shared.AddMessage($"{e.Card.Name} prepares for battle.");
        CombatMapController.Shared.SelectTiles(position);
        CombatMapController.Shared.PanTo(position);
        yield return new WaitForSeconds(CombatConstants.ShowEnemyInfoDuration);
        Position[] path = [.. Map.GetEnemyMove(position)];
        if (path.Count() > 0)
        {
            foreach (var pause in ShowMove(e, position, path))
            {
                yield return pause;
            }
            position = path.Last();
        }
        Position[] attacks = [.. Map.GetValidAttackTargets(position)];
        if (attacks.Count() > 0)
        {
            foreach (var pause in ShowAttack(e, attacks))
            {
                yield return pause;
            }
        }
        CombatMapController.Shared.HighlightTiles([]);
        DoNextMove();
    }

    public IEnumerable ShowAttack(Enemy e, Position[] options)
    {
        Position target = options.OrderBy(p => Map.PlayerCharacters[p].Health()).First();
        PlayerCharacter character = Map.PlayerCharacters[target];
        MessageRenderer.Shared.AddMessage($"{e.Card.Name} prepares to attack {character.Card.Name}.");
        CombatMapController.Shared.SelectTiles(target);
        yield return new WaitForSeconds(CombatConstants.ShowEnemyInfoDuration);
        AttackResult result = e.Card.AttackRoll.GetRoll(IRandom.Default);
        AttackResultEvent attackEvent = Map.ApplyAttack(target, result);
        string message = attackEvent switch
        {
            AttackHitEvent hit => $"{hit.TargetName} takes {hit.Damage} damage.",
            ArmorAbsorbedHitEvent hit => $"{hit.TargetName}'s armor absorbs the blow.",
            TargetKilledEvent hit => $"{hit.TargetName} falls to the ground.",
            EmptyTarget => $"{e.Card.Name} misses!",
            _ => throw new NotImplementedException($"Unknown attackEvent {attackEvent}"),
        };
        MessageRenderer.Shared.AddMessage(message);
    }

    public IEnumerable ShowMove(Enemy e, Position start, Position[] path)
    {
        Tilemap characterMap = CombatMapController.Shared.CharacterMap;
        MessageRenderer.Shared.AddMessage($"{e.Card.Name} moves {path.Count()} spaces.");
        CombatMapController.Shared.HighlightTiles(path, CombatMapController.Shared.IconDatabase.Yellow);
        yield return new WaitForSeconds(CombatConstants.ShowEnemyInfoDuration);

        TileBase tile = characterMap.GetTile(start.ToVector3Int());
        TileBase? toSet = null;
        Position last = start;
        CombatMapController.Shared.PanTo(path.Last(), CombatConstants.EnemyMoveDelay * path.Count());
        foreach (Position position in path)
        {
            characterMap.SetTile(last.ToVector3Int(), toSet);
            toSet = characterMap.GetTile(position.ToVector3Int());
            characterMap.SetTile(position.ToVector3Int(), tile);
            yield return new WaitForSeconds(CombatConstants.EnemyMoveDelay);
            last = position;
        }
        Map.MoveEnemy(start, path.Last());
    }

    private void EndEnemyTurn()
    {
        Debug.Log("Not implemented: EndEnemyTurn");
    }
}