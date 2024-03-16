namespace Tests;

using CaptainCoder.Dungeoneering.DungeonCrawler;
using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.DungeonMap.IO;

using Shouldly;

public class DungeonCrawlerManifest_should
{
    public static DungeonCrawlerManifest Empty
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public static DungeonCrawlerManifest Simple
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public static IEnumerable<object[]> BeJsonableData => [
        [Empty],
        [Simple],
    ];
    [Theory]
    [MemberData(nameof(BeJsonableData))]
    public void be_jsonable(DungeonCrawlerManifest underTest)
    {
        string json = underTest.ToJson();
        DungeonCrawlerManifest actual = JsonExtensions.LoadModel<DungeonCrawlerManifest>(json);
        actual.ShouldBe(underTest);
    }

    public static IEnumerable<object[]> AddDungeonsData => [

        ["simple", Dungeon_should.SimpleSquareDungeon],
        ["twobytwo", Dungeon_should.TwoByTwoRoom],
    ];
    [Theory]
    [MemberData(nameof(AddDungeonsData))]
    public void add_dungeons(string name, Dungeon toAdd)
    {
        DungeonCrawlerManifest underTest = new();

        underTest.AddDungeon(name, toAdd);

        underTest.DungeonManifest.Count.ShouldBe(1);
        underTest.DungeonManifest[name].ShouldBe(toAdd);
    }

    [Theory]
    [MemberData(nameof(AddDungeonsData))]
    public void not_allow_adding_dungeon_with_same_name(string name, Dungeon toAdd)
    {
        DungeonCrawlerManifest underTest = new();

        underTest.AddDungeon(name, toAdd);
        Should.Throw<InvalidOperationException>(() =>
        {
            underTest.AddDungeon(name, toAdd);
        });
    }

    public static IEnumerable<object[]> AddScriptData => [

        ["simple", new EventScript("simple")],
        ["twobytwo", new EventScript("twobytwo")],
    ];
    [Theory]
    [MemberData(nameof(AddScriptData))]
    public void add_script(string name, EventScript toAdd)
    {
        DungeonCrawlerManifest underTest = new();

        underTest.AddScript(name, toAdd);

        underTest.ScriptManifest.Count.ShouldBe(1);
        underTest.ScriptManifest[name].ShouldBe(toAdd);
    }

    [Theory]
    [MemberData(nameof(AddScriptData))]
    public void not_allow_adding_script_with_same_name(string name, EventScript toAdd)
    {
        DungeonCrawlerManifest underTest = new();

        underTest.AddScript(name, toAdd);
        Should.Throw<InvalidOperationException>(() =>
        {
            underTest.AddScript(name, toAdd);
        });
    }


}