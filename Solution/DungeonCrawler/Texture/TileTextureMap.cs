using CaptainCoder.Utils.DictionaryExtensions;

namespace CaptainCoder.Dungeoneering.DungeonMap;

public class TileTextureMap : IEquatable<TileTextureMap>
{
    public const string DefaultTexture = "default-tile.png";
    public string Default { get; set; } = DefaultTexture;
    public Dictionary<Position, string> Textures { get; init; } = new();
    public bool Equals(TileTextureMap other) =>
        Default == other.Default &&
        Textures.AllKeyValuesAreEqual(other.Textures);
}

public static class TileTextureMapExtensions
{
    public static string GetTileTextureName(this Dungeon dungeon, Position position) => throw new NotImplementedException();
    // dungeon.TileTextures.GetWall(position) switch
    // {
    //     _ when dungeon.WallTextures.Textures.TryGetValue((position, facing), out string texture) is true => texture,
    //     WallType.Solid => dungeon.WallTextures.DefaultSolid,
    //     WallType.Door => dungeon.WallTextures.DefaultDoor,
    //     WallType.SecretDoor => dungeon.WallTextures.DefaultSecretDoor,
    //     _ => "No Texture",
    // };
}