namespace CaptainCoder.Dungeoneering.DungeonMap.IO;

using Newtonsoft.Json;

/// <summary>
/// JsonConverter for WallMaps. This is required to correctly serialize and
/// deserialize the underlying dictionary which uses TileEdge as a Key. The
/// default serializer for Dictionary converts the keys to a string which cannot
/// be correctly deserialized.
/// </summary>
public class WallMapJsonConverter : JsonConverter<WallMap>
{
    public static WallMapJsonConverter Shared { get; } = new();
    public override WallMap? ReadJson(JsonReader reader, Type objectType, WallMap? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        (TileEdge, WallType)[] elems = serializer.Deserialize<(TileEdge, WallType)[]>(reader) ?? throw new Exception($"Could not read WallMap.");
        return new WallMap(elems);
    }

    public override void WriteJson(JsonWriter writer, WallMap? value, JsonSerializer serializer)
    {
        if (value is null) { throw new Exception($"Could not write null WallMap."); }
        (TileEdge, WallType)[] elems = value.Map.Select(kvp => (kvp.Key, kvp.Value)).ToArray();
        serializer.Serialize(writer, elems, elems.GetType());
    }
}