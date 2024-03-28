namespace CaptainCoder.DungeonCrawler.Combat;

public class CombatMap
{
    public HashSet<Position> Tiles { get; init; } = new();
    public Dictionary<Position, PlayerCharacter> PlayerCharacters { get; init; } = [];
    public Dictionary<Position, Enemy> Enemies { get; init; } = [];
    public Action<PlayerCharacter>? OnCharacterChange;
    public Action<MoveActionEvent>? OnMoveAction;
    public Action<Position>? OnEnemyRemoved;
}

public abstract record CombatMapEvent;
public record ExertEvent(PlayerCharacter Original, PlayerCharacter New, Position Position);
public record MoveActionEvent(PlayerCharacter Moving, MoveAction Move, IEnumerable<Position> Path);

public static class CombatMapExtensions
{
    public static CombatMap ParseMap(string setup, Func<char, PlayerCharacter?> characterLookup, Func<char, Enemy?> enemyLookup)
    {
        Dictionary<char, HashSet<Position>> allData = ParseCharPositions(setup);
        HashSet<Position> tiles = allData.Values.Aggregate((acc, next) => { acc.UnionWith(next); return acc; });
        CombatMap map = new() { Tiles = tiles };
        foreach (char ch in allData.Keys)
        {
            if (ch == '#') { continue; }
            PlayerCharacter? pc = characterLookup.Invoke(ch);
            if (pc is not null)
            {
                map.PlayerCharacters[allData[ch].First()] = pc;
                continue;
            }
            Enemy? enemy = enemyLookup.Invoke(ch);
            if (enemy is null) { throw new Exception($"Character '{ch}' did not parse to player or enemy."); }
            foreach (Position pos in allData[ch])
            {
                map.Enemies[pos] = enemy;
            }
        }
        return map;
    }

    public static void RemoveEnemy(this CombatMap map, Position target)
    {
        if (map.Enemies.Remove(target))
        {
            map.OnEnemyRemoved?.Invoke(target);
        }
    }

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