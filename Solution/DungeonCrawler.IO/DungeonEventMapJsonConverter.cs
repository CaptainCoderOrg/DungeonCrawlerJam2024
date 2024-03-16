namespace CaptainCoder.Dungeoneering.DungeonMap.IO;

using Newtonsoft.Json;

public class DungeonEventMapJsonConverter : JsonConverter<DungeonEventMap>
{
    public static DungeonEventMapJsonConverter Shared { get; } = new();
    public override DungeonEventMap? ReadJson(JsonReader reader, Type objectType, DungeonEventMap? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        (Position, TileEvent[])[] events = serializer.Deserialize<(Position, TileEvent[])[]>(reader) ?? throw new Exception($"Could not read DungeonEventMap.");
        return new DungeonEventMap(events.Select(pair => (pair.Item1, pair.Item2.AsEnumerable())));
    }

    public override void WriteJson(JsonWriter writer, DungeonEventMap? value, JsonSerializer serializer)
    {
        if (value is null) { throw new Exception($"Could not write null DungeonEventMap."); }
        (Position, TileEvent[])[] events = [.. value.Events.Select(kvp => (kvp.Key, kvp.Value.ToArray()))];
        serializer.Serialize(writer, events);
    }
}