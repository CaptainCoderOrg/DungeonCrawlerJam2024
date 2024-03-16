namespace CaptainCoder.Dungeoneering.DungeonMap.IO;

using Newtonsoft.Json;

/// <summary>
/// JsonConverter for DungeonEventMap. This is required to correctly serialize and
/// deserialize the underlying dictionary which uses non-string as a Key. The
/// default serializer for Dictionary converts the keys to a string which cannot
/// be correctly deserialized.
/// </summary>
public class DungeonEventMapJsonConverter : JsonConverter<DungeonEventMap>
{
    public static DungeonEventMapJsonConverter Shared { get; } = new();
    public override DungeonEventMap? ReadJson(JsonReader reader, Type objectType, DungeonEventMap? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string data = reader.Value as string ?? throw new Exception($"Could not read DungeonEventMap.");
        (Position, TileEvent[])[] events = JsonConvert.DeserializeObject<(Position, TileEvent[])[]>(data) ?? throw new Exception($"Could not read DungeonEventMap.");
        return new DungeonEventMap(events.Select(pair => (pair.Item1, pair.Item2.AsEnumerable())));
    }

    public override void WriteJson(JsonWriter writer, DungeonEventMap? value, JsonSerializer serializer)
    {
        // throw new NotImplementedException();
        if (value is null) { throw new Exception($"Could not write null DungeonEventMap."); }
        (Position, TileEvent[])[] events = [.. value.Events.Select(kvp => (kvp.Key, kvp.Value.ToArray()))];
        string json = JsonConvert.SerializeObject(events);
        writer.WriteValue(json);
    }
}