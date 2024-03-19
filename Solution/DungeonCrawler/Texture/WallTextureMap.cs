using CaptainCoder.Utils.DictionaryExtensions;

namespace CaptainCoder.Dungeoneering.DungeonMap;

public class WallTextureMap() : IEquatable<WallTextureMap>
{
    public const string DefaultSolidTexture = "default-wall.png";
    public const string DefaultDoorTexture = "default-door.png";
    public const string DefaultSecretDoorTexture = "default-secret-door.png";
    public string DefaultSolid { get; init; } = DefaultSolidTexture;
    public string DefaultDoor { get; init; } = DefaultDoorTexture;
    public string DefaultSecretDoor { get; init; } = DefaultSecretDoorTexture;
    public Dictionary<(Position, Facing), string> Textures { get; init; } = new();
    public bool Equals(WallTextureMap other) =>
        DefaultSolid == other.DefaultSolid &&
        DefaultDoor == other.DefaultDoor &&
        DefaultSecretDoor == other.DefaultSecretDoor &&
        Textures.AllKeyValuesAreEqual(other.Textures);
}

public static class WallTextureMapExtensions
{
    public static string GetTextureName(this Dungeon dungeon, Position position, Facing facing) =>
    dungeon.Walls.GetWall(position, facing) switch
    {
        _ when dungeon.WallTextures.Textures.TryGetValue((position, facing), out string texture) is true => texture,
        WallType.Solid => dungeon.WallTextures.DefaultSolid,
        WallType.Door => dungeon.WallTextures.DefaultDoor,
        WallType.SecretDoor => dungeon.WallTextures.DefaultSecretDoor,
        WallType wallType => throw new NotImplementedException($"Unknown wall texture type: {wallType}")
    };

    public static void SetTexture(this Dungeon dungeon, Position position, Facing facing, string textureName) => dungeon.WallTextures.Textures[(position, facing)] = textureName;
}