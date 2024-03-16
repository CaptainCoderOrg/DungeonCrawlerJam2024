namespace CaptainCoder.Dungeoneering.DungeonMap.IO;

using CaptainCoder.Dungeoneering.DungeonCrawler;

using Newtonsoft.Json;

/// <summary>
/// JsonExtensions provides utility classes for serializing and deserializing
/// classes. Creating converter classes is necessary to serialize Dictionaries
/// that use non-string keys. 
/// </summary>
public static class JsonExtensions
{
    public static string ToJson(this WallMap wallMap) => JsonConvert.SerializeObject(wallMap, WallMapJsonConverter.Shared);
    public static string ToJson(this DungeonEventMap eventMap) => JsonConvert.SerializeObject(eventMap, DungeonEventMapJsonConverter.Shared);
    public static string ToJson(this DungeonCrawlerManifest crawler) => JsonConvert.SerializeObject(crawler, DungeonCrawlerManifestJsonConverter.Shared);

    public static T LoadModel<T>(string json)
    {
        if (typeof(T) == typeof(WallMap) && LoadWallMapFromJson(json) is T wallMapResult)
        {
            return wallMapResult;
        }
        else if (typeof(T) == typeof(DungeonEventMap) && LoadDungeonEventMapFromJson(json) is T dungeonEventMapResult)
        {
            return dungeonEventMapResult;
        }
        throw new JsonException($"Unable to load model of type \"{typeof(T)}\" from json \"{json}\".");
    }

    public static DungeonEventMap LoadDungeonEventMapFromJson(string json) =>
        JsonConvert.DeserializeObject<DungeonEventMap>(json, DungeonEventMapJsonConverter.Shared) ??
        throw new JsonException($"Could not parse DungeonEventMap from \"{json}\"");

    public static WallMap LoadWallMapFromJson(string json) =>
        JsonConvert.DeserializeObject<WallMap>(json, WallMapJsonConverter.Shared) ??
        throw new JsonException($"Could not parse WallMap from \"{json}\"");
}