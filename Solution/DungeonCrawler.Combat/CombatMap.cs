namespace CaptainCoder.DungeonCrawler.Combat;

public class CombatMap
{
    public HashSet<Position> Tiles { get; init; } = new();
    public Dictionary<Position, PlayerCharacter> PlayerCharacters { get; init; } = [];
    public Dictionary<Position, Enemy> Enemies { get; init; } = [];
}

public static class CombatMapExtensions
{
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