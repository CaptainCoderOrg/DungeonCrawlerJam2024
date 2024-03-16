using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Utils.DictionaryExtensions;

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
        return DungeonManifest.AllKeyValuesAreEqual(other.DungeonManifest, DungeonEquality) &&
               ScriptManifest.AllKeyValuesAreEqual(other.ScriptManifest);
        static bool DungeonEquality(Dungeon d0, Dungeon d1) => d0.Equals(d1);
    }
}