namespace Tests;

using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.DungeonMap.IO;

using Shouldly;

public class TileTextureMap_should
{

    public static TileTextureMap MakeWithTextures() => new()
    {
        Default = "Some Default",
        Textures = new Dictionary<Position, string>{
            { new Position(5, 7), "stone-wall.png" },
            { new Position(5, 6), "stone-wall-variant.png" },
            { new Position(3, 3), "tavern-wall.png" },
            { new Position(0, 0), "well-wall.png" },
        },
    };

    [Fact]
    public void be_equals()
    {
        MakeWithTextures().ShouldBe(MakeWithTextures());
    }

    [Fact]
    public void not_be_equals()
    {
        TileTextureMap first = MakeWithTextures();
        TileTextureMap second = MakeWithTextures();
        second.Default = "Different";
        second.Textures[new Position(7, 7)] = "foo.png";
        first.ShouldNotBe(second);
    }

    [Fact]
    public void be_jsonable()
    {
        TileTextureMap underTest = MakeWithTextures();
        string json = JsonExtensions.ToJson(underTest);
        TileTextureMap actual = JsonExtensions.LoadModel<TileTextureMap>(json);
        actual.ShouldBe(underTest);
    }
}