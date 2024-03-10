namespace CaptainCoder.Dungeoneering.Model.IO;

using Newtonsoft.Json;

public class WallMapJsonConverter : JsonConverter<WallMap>
{
    public static WallMapJsonConverter Shared { get; } = new();
    public override WallMap? ReadJson(JsonReader reader, Type objectType, WallMap? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string data = reader.Value as string ?? throw new Exception($"Could not read WallMap.");
        (TileEdge edge, WallType wall)[] elems = JsonConvert.DeserializeObject<(TileEdge, WallType)[]>(data) ?? throw new Exception($"Could not read WallMap.");
        return new WallMap(elems);
    }

    public override void WriteJson(JsonWriter writer, WallMap? value, JsonSerializer serializer)
    {
        if (value is null) { throw new Exception($"Could not write null WallMap."); }
        (TileEdge, WallType)[] elems = value.Map.Select(kvp => (kvp.Key, kvp.Value)).ToArray();
        string json = JsonConvert.SerializeObject(elems);
        writer.WriteValue(json);
    }
}