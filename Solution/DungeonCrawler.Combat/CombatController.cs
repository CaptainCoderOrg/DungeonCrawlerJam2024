namespace CaptainCoder.DungeonCrawler.Combat;

public static class CombatController
{
    public static bool IsValidAction(this CombatMap map, CombatAction toValidate) => throw new NotImplementedException();
    public static void ApplyAction(this CombatMap map, CombatAction toApply) => throw new NotImplementedException();

    /// <summary>
    /// A valid ExertAction is one that targets a tile with a PlayerCharacter
    /// and that PlayerCharacter has energy remaining.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public static bool IsValidExertAction(this CombatMap map, ExertAction action)
    {
        if (!map.PlayerCharacters.TryGetValue(action.Target, out PlayerCharacter pc)) { return false; }
        return pc.Energy() > 0;
    }

    public static void ApplyExertAction(this CombatMap map, ExertAction action)
    {
        if (!map.IsValidExertAction(action)) { throw new ArgumentException($"Invalid ExertAction: {action}"); }
        PlayerCharacter pc = map.PlayerCharacters[action.Target];
        map.PlayerCharacters[action.Target] = pc with { Exertion = pc.Exertion + 1, MovementPoints = pc.MovementPoints + 1 };
    }

    public static bool IsValidMoveAction(this CombatMap map, MoveAction moveAction)
    {
        // Start position must contain a player
        if (!map.PlayerCharacters.TryGetValue(moveAction.Start, out PlayerCharacter pc)) { return false; }
        HashSet<Position> validMoves = map.FindValidMoves(moveAction.Start, pc.MovementPoints);
        return validMoves.Contains(moveAction.End);
    }
    public static void ApplyMoveAction(this CombatMap map, MoveAction moveAction)
    {
        PlayerCharacter character = map.PlayerCharacters[moveAction.Start];
        map.PlayerCharacters.Remove(moveAction.Start);
        int distance = map.FindShortestPath(moveAction.Start, moveAction.End).Count();
        if (distance > character.MovementPoints) { throw new ArgumentException("Invalid Move. No path found."); }
        map.PlayerCharacters[moveAction.End] = character with { MovementPoints = character.MovementPoints - distance };
    }

    public static IEnumerable<Position> FindShortestPath(this CombatMap map, Position start, Position end)
    {
        HashSet<Position> visited = new() { start };
        Queue<(Position, List<Position>)> queue = new();
        queue.Enqueue((start, new()));

        while (queue.TryDequeue(out var next))
        {
            (Position currentPosition, List<Position> currentPath) = next;

            if (currentPosition == end) { return currentPath; }

            foreach (Position neighbor in map.Neighbors(visited, currentPosition))
            {
                visited.Add(neighbor);
                List<Position> newPath = [.. currentPath, neighbor];
                queue.Enqueue((neighbor, newPath));
            }
        }
        throw new ArgumentException($"No valid path for specified start and end: {start}, {end}");
    }

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

            foreach (Position neighbor in map.Neighbors(visited, nextPosition))
            {
                if (!map.PlayerCharacters.ContainsKey(neighbor)) // Cannot move on other player
                {
                    validMoves.Add(neighbor);
                }
                visited.Add(neighbor);
                queue.Enqueue((neighbor, movesRemaining - 1));
            }
        }

        return validMoves;
    }

    static IEnumerable<Position> Neighbors(this CombatMap map, HashSet<Position> visited, Position position)
    {
        var (x, y) = position;
#pragma warning disable format // Format for clarity
        IEnumerable<Position> neighbors =
                   [ new Position(x - 1, y - 1), new Position(x, y - 1), new Position(x + 1, y - 1),
                     new Position(x - 1, y    ),                         new Position(x + 1, y    ),
                     new Position(x - 1, y + 1), new Position(x, y + 1), new Position(x + 1, y + 1),];
#pragma warning restore format
        return neighbors.Where(n => !visited.Contains(n))
                        .Where(map.Tiles.Contains)
                        .Where(n => !map.Enemies.ContainsKey(n));
    }
}

public record struct Position(int X, int Y);
public abstract record CombatAction;
/// <summary>
/// Represents a move from the start and end position
/// </summary>
public record MoveAction(Position Start, Position End) : CombatAction;
/// <summary>
/// Represents an exert action on a character at the specified position
/// </summary>
public record ExertAction(Position Target) : CombatAction;
public record Attack : CombatAction;
public record EndCharacterTurn : CombatAction;