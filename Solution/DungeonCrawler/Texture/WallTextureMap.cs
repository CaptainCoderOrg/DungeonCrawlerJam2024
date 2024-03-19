using CaptainCoder.Utils.DictionaryExtensions;

namespace CaptainCoder.Dungeoneering.DungeonMap;

public class WallTextureMap() : IEquatable<WallTextureMap>
{
    public string DefaultSolid { get; init; } = "default-wall.png";
    public string DefaultDoor { get; init; } = "default-door.png";
    public string DefaultSecretDoor { get; init; } = "default-secret-door.png";
    public Dictionary<(Position, Facing), string> Textures { get; init; } = new();
    public bool Equals(WallTextureMap other) =>
        DefaultSolid == other.DefaultSolid &&
        DefaultDoor == other.DefaultDoor &&
        DefaultSecretDoor == other.DefaultSecretDoor &&
        Textures.AllKeyValuesAreEqual(other.Textures);
}

public static class WallTextureMapExtensions
{
    public static string GetTextureName(this Dungeon dungeon, Position position, Facing facing) => throw new NotImplementedException();
    public static void SetTexture(this Dungeon dungeon, Position position, Facing facing, string textureName) => throw new NotImplementedException();
}