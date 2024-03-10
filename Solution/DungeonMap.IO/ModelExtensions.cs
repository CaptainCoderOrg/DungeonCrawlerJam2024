namespace CaptainCoder.Dungeoneering.DungeonMap.IO;

using Newtonsoft.Json;

public static class JsonExtensions
{

    public static string ToJson(this WallMap wallMap) => JsonConvert.SerializeObject(wallMap, WallMapJsonConverter.Shared);

    public static T LoadModel<T>(string json)
    {
        if (typeof(T) == typeof(WallMap) && LoadWallMapFromJson(json) is T result)
        {
            return result;
        }
        throw new JsonException($"Unable to load model of type \"{typeof(T)}\" from json \"{json}\".");
    }

    public static WallMap LoadWallMapFromJson(string json) => JsonConvert.DeserializeObject<WallMap>(json, WallMapJsonConverter.Shared) ?? throw new JsonException($"Could not parse WallMap from \"{json}\"");
}