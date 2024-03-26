namespace CaptainCoder.DungeonCrawler.Combat;

public class CombatMap
{
    public HashSet<Position> Tiles { get; init; } = new();
    public Dictionary<Position, PlayerCharacter> PlayerCharacters { get; init; } = [];
    public Dictionary<Position, Enemy> Enemies { get; init; } = [];
    public Action<PlayerCharacter>? OnCharacterChange;
    public Action<MoveActionEvent>? OnMoveAction;
}

public abstract record CombatMapEvent;
public record ExertEvent(PlayerCharacter Original, PlayerCharacter New, Position Position);
public record MoveActionEvent(PlayerCharacter Moving, MoveAction Move, IEnumerable<Position> Path);

public static class CombatMapExtensions
{

    public static PlayerCharacter GetCharacter(this CombatMap map, CharacterCard card)
    {
        foreach (PlayerCharacter pc in map.PlayerCharacters.Values)
        {
            if (pc.Card == card) { return pc; }
        }
        throw new ArgumentOutOfRangeException($"No character found: {card}");
    }
    public static void UpdateCharacter(this CombatMap map, PlayerCharacter character)
    {
        Position position = map.GetPosition(character.Card);
        map.PlayerCharacters[position] = character;
        map.OnCharacterChange?.Invoke(character);
    }
    public static Position GetPosition(this CombatMap map, CharacterCard card)
    {
        foreach ((Position p, PlayerCharacter pc) in map.PlayerCharacters)
        {
            if (pc.Card == card) { return p; }
        }
        throw new ArgumentOutOfRangeException($"No character found: {card}");
    }
    public static HashSet<Position> ParseTiles(string toParse) => ParseCharPositions(toParse).GetValueOrDefault('#', new HashSet<Position>());

    public static Dictionary<char, HashSet<Position>> ParseCharPositions(string toParse)
    {
        string[] rows = toParse.Replace(@"\r\n?|\n", Environment.NewLine).Split(Environment.NewLine);
        Dictionary<char, HashSet<Position>> tiles = new();
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                char ch = rows[y][x];
                if (ch == ' ') { continue; }
                HashSet<Position> set = GetOrInit(ch);
                set.Add(new Position(x, y));
            }
        }
        return tiles;

        HashSet<Position> GetOrInit(char ch)
        {
            if (!tiles.TryGetValue(ch, out HashSet<Position> set))
            {
                set = new HashSet<Position>();
                tiles[ch] = set;
            }
            return set;
        }
    }
}