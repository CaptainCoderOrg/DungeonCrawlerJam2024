namespace CaptainCoder.DungeonCrawler.Combat;

public static class CombatController
{
    public static bool IsValidAction(this CombatMap map, CombatAction toValidate) => throw new NotImplementedException();
    public static bool IsValidMoveAction(this CombatMap map, MoveAction moveAction)
    {
        // Start position must contain a player
        if (!map.PlayerCharacters.TryGetValue(moveAction.Start, out PlayerCharacter pc)) { return false; }
        HashSet<Position> validMoves = map.FindValidMoves(moveAction.Start, pc.MovementPoints);
        return validMoves.Contains(moveAction.End);
    }
    public static void ApplyAction(this CombatMap map, CombatAction toApply) => throw new NotImplementedException();
    public static void ApplyMoveAction(this CombatMap map, MoveAction moveAction) => throw new NotImplementedException();

    public static HashSet<Position> FindValidMoves(this CombatMap map, Position start, int maxDistance)
    {
        HashSet<Position> visited = new() { start };
        HashSet<Position> validMoves = new() { };
        Queue<(Position, int movesRemaining)> queue = new();
        queue.Enqueue((start, maxDistance));

        while (queue.TryDequeue(out (Position, int) next))
        {
            var (nextPosition, movesRemaining) = next;
            if (movesRemaining == 0) { continue; }

            foreach (Position neighbor in Neighbors(nextPosition))
            {
                if (visited.Contains(neighbor)) { continue; } // Don't repeat position
                if (!map.Tiles.Contains(neighbor)) { continue; } // Don't move out of map
                if (map.Enemies.ContainsKey(neighbor)) { continue; } // Cannot move through enemies
                if (!map.PlayerCharacters.ContainsKey(neighbor)) // Cannot move on other player
                {
                    validMoves.Add(neighbor);
                }
                visited.Add(neighbor);
                queue.Enqueue((neighbor, movesRemaining - 1));

            }
        }

        return validMoves;

        static IEnumerable<Position> Neighbors(Position position)
        {
            var (x, y) = position;
#pragma warning disable format // Format for clarity
            return [ new Position(x - 1, y - 1), new Position(x, y - 1), new Position(x + 1, y - 1),
                     new Position(x - 1, y    ),                         new Position(x + 1, y    ),
                     new Position(x - 1, y + 1), new Position(x, y + 1), new Position(x + 1, y + 1),];
#pragma warning restore format
        }
    }
}

public record struct Position(int X, int Y);
public abstract record CombatAction;
public record MoveAction(Position Start, Position End) : CombatAction;
public record Exert : CombatAction;
public record Attack : CombatAction;
public record EndCharacterTurn : CombatAction;