namespace CaptainCoder.DungeonCrawler.Combat;

public class CombatMap
{
    public HashSet<Position> Tiles { get; init; } = new();
    public Dictionary<Position, PlayerCharacter> PlayerCharacters { get; init; } = [];
}

public static class CombatMapExtensions
{
    public static HashSet<Position> ParseTiles(string toParse)
    {
        string[] rows = toParse.Replace(@"\r\n?|\n", Environment.NewLine).Split(Environment.NewLine);
        HashSet<Position> tiles = new();
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                if (rows[y][x] != '#') { continue; }
                tiles.Add(new Position(x, y));
            }
        }
        return tiles;
    }
}