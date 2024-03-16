using System.Collections.ObjectModel;

using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;

namespace CaptainCoder.Dungeoneering.DungeonCrawler;

public class DungeonCrawlerManifest : IEquatable<DungeonCrawlerManifest>
{
    public Dictionary<string, Dungeon> DungeonManifest { get; set; } = new();
    public Dictionary<string, EventScript> ScriptManifest { get; set; } = new();

    public void AddDungeon(string name, Dungeon toAdd)
    {
        if (!DungeonManifest.TryAdd(name, toAdd))
        {
            throw new InvalidOperationException($"A dungeon with the name '{name}' already exists.");
        }
    }

    public void AddScript(string name, EventScript toAdd)
    {
        if (!ScriptManifest.TryAdd(name, toAdd))
        {
            throw new InvalidOperationException($"A script with the name '{name}' already exists.");
        }
    }

    public bool Equals(DungeonCrawlerManifest other)
    {
        throw new NotImplementedException();
    }
}