namespace CaptainCoder.Dungeoneering.DungeonMap.IO;

using CaptainCoder.Dungeoneering.DungeonCrawler;
using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;

using Newtonsoft.Json;
/// <summary>
/// JsonConverter for DungeonCrawlerManifest. This is required to correctly serialize and
/// deserialize the underlying dictionary which uses non-string as a Key. The
/// default serializer for Dictionary converts the keys to a string which cannot
/// be correctly deserialized.
/// </summary>
// public class DungeonCrawlerManifestJsonConverter : JsonConverter<DungeonCrawlerManifest>
// {
//     public static DungeonCrawlerManifestJsonConverter Shared { get; } = new();
//     public override DungeonCrawlerManifest? ReadJson(JsonReader reader, Type objectType, DungeonCrawlerManifest? existingValue, bool hasExistingValue, JsonSerializer serializer)
//     {
//         SerializableDungeonCrawlerManifest manifest = serializer.Deserialize<SerializableDungeonCrawlerManifest>(reader) ?? throw new Exception($"Could not read WallMap.");
//         return manifest.ToDungeonCrawlerManifest();
//     }

//     public override void WriteJson(JsonWriter writer, DungeonCrawlerManifest? value, JsonSerializer serializer)
//     {
//         if (value is null) { throw new Exception($"Could not write null DungeonCrawlerManifest."); }
//         SerializableDungeonCrawlerManifest serializable = new(value);
//         serializer.Serialize(writer, serializable);
//     }
// }

public class DictionaryJsonConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>>
{
    public override Dictionary<TKey, TValue>? ReadJson(JsonReader reader, Type objectType, Dictionary<TKey, TValue>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, Dictionary<TKey, TValue>? value, JsonSerializer serializer)
    {
        if (value is null) { throw new Exception($"Could not write null to dictionary."); }
        // serializer.Serialize(writer, value.ToSeri);
    }
}

internal class SerializableDungeonCrawlerManifest(DungeonCrawlerManifest manifest)
{
    public (string, Dungeon)[] DungeonManifest { get; set; } = manifest.DungeonManifest.ToSerializableArray();
    public (string, EventScript)[] ScriptManifest { get; set; } = manifest.ScriptManifest.ToSerializableArray();

    public DungeonCrawlerManifest ToDungeonCrawlerManifest()
    {
        DungeonCrawlerManifest manifest = new();
        throw new NotImplementedException();
    }
}

public static class DictionaryExtensions
{
    public static (TKey, TValue)[] ToSerializableArray<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> toSerialize) =>
        [.. toSerialize.Select(kvp => (kvp.Key, kvp.Value))];

    public static IEnumerable<KeyValuePair<TKey, TValue>> ToKevValuePairs<TKey, TValue>(this (TKey, TValue)[] toDeserialize)
    {
        throw new Exception();
    }
}