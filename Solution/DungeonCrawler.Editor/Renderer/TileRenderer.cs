using System.Numerics;

namespace CaptainCoder.Dungeoneering.Raylib;

using Raylib_cs;

public sealed class TileInfo(Texture2D texture)
{
    public Texture2D Texture { get; init; } = texture;
    public Rectangle Source { get; init; } = new(0, 0, texture.Width, texture.Height);
    public Rectangle Target { get; init; } = new(0, 0, texture.Width, texture.Height);
    public Vector2 Origin { get; init; } = new(0, 0);
    public float Rotation { get; init; } = 0;
    public Color Tint { get; init; } = Color.White;
}

public static class TileRenderer
{
    public static void Render(this TileInfo tile)
    {
        Raylib.DrawTexturePro(tile.Texture, tile.Source, tile.Target, tile.Origin, tile.Rotation, tile.Tint);
    }
}