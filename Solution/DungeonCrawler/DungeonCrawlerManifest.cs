using System.Collections.ObjectModel;

using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;

namespace CaptainCoder.Dungeoneering.DungeonCrawler;

public class DungeonCrawlerManifest : IEquatable<DungeonCrawlerManifest>
{
    private readonly Dictionary<string, Dungeon> _dungeonManifest = new();
    public IReadOnlyDictionary<string, Dungeon> DungeonManifest => new ReadOnlyDictionary<string, Dungeon>(_dungeonManifest);
    private readonly Dictionary<string, EventScript> _scriptManifest = new();
    public IReadOnlyDictionary<string, EventScript> ScriptManifest => new ReadOnlyDictionary<string, EventScript>(_scriptManifest);

    public void AddDungeon(string name, Dungeon toAdd)
    {
        if (!_dungeonManifest.TryAdd(name, toAdd))
        {
            throw new InvalidOperationException($"A dungeon with the name '{name}' already exists.");
        }
    }

    public void AddScript(string name, EventScript toAdd)
    {
        if (!_scriptManifest.TryAdd(name, toAdd))
        {
            throw new InvalidOperationException($"A script with the name '{name}' already exists.");
        }
    }

    public bool Equals(DungeonCrawlerManifest other)
    {
        throw new NotImplementedException();
    }
}