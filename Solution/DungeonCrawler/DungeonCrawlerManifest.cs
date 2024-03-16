using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;

namespace CaptainCoder.Dungeoneering.DungeonCrawler;

public class DungeonCrawlerManifest : IEquatable<DungeonCrawlerManifest>
{
    public IReadOnlyDictionary<string, Dungeon> DungeonManifest => throw new NotImplementedException();
    public IReadOnlyDictionary<string, EventScript> ScriptManifest => throw new NotImplementedException();

    public void AddDungeon(string name, Dungeon toAdd)
    {
        throw new NotImplementedException();
    }

    public void AddScript(string name, EventScript toAdd)
    {
        throw new NotImplementedException();
    }

    public bool Equals(DungeonCrawlerManifest other)
    {
        throw new NotImplementedException();
    }
}