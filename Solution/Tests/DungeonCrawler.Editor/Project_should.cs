namespace Tests;

using System.IO.Abstractions.TestingHelpers;

using CaptainCoder.Dungeoneering.DungeonCrawler;
using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap.IO;

using DungeonCrawler.Editor;

using Shouldly;

public class Project_should
{
    static readonly string root = Path.Combine("project-root");
    static readonly string dungeonsPath = Path.Combine(root, Project.DungeonDir);
    static readonly string scriptsPath = Path.Combine(root, Project.ScriptDir);
    public static MockFileData SimpleDungeonFile = new(JsonExtensions.ToJson(Dungeon_should.SimpleSquareDungeon));
    public static MockFileData TwoByTwoDungeonFile = new(JsonExtensions.ToJson(Dungeon_should.TwoByTwoRoom));

    public static MockFileSystem MakeProjectFileSystem()
    {
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>{
                {Path.Combine(dungeonsPath, "simple.json"), SimpleDungeonFile},
                {Path.Combine(dungeonsPath, "another.json"), TwoByTwoDungeonFile},
                {Path.Combine(scriptsPath, "tavern.lua"), new MockFileData("Tavern")},
                {Path.Combine(scriptsPath, "second.lua"), new MockFileData("Second")},
                {Path.Combine(scriptsPath, "third.lua"), new MockFileData("Third")},
            });
        return mockFileSystem;
    }

    [Fact]
    public void initialize_project_structure()
    {
        string root = Path.Combine("project-root");
        string dungeonsPath = Path.Combine(root, Project.DungeonDir);
        string scriptsPath = Path.Combine(root, Project.ScriptDir);
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.InitializeProjectDirectory(root);

        mockFileSystem.GetFile(dungeonsPath).IsDirectory.ShouldBeTrue();
        mockFileSystem.GetFile(scriptsPath).IsDirectory.ShouldBeTrue();
    }

    [Fact]
    public void validate_project_directory()
    {
        MakeProjectFileSystem().IsProjectDirectory(root).ShouldBeTrue();
    }

    [Fact]
    public void invalidate_non_project_directory()
    {
        var nonProjectFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>{
            {Path.Combine(root, "not-dungeons"), new MockFileData(string.Empty) },
            {Path.Combine(root, "not-scripts"), new MockFileData(string.Empty) }
        });
        nonProjectFileSystem.IsProjectDirectory(root).ShouldBeFalse();
    }

    [Fact]
    public void throws_io_exception_when_initializing_existing_project()
    {
        Should.Throw<IOException>(() => MakeProjectFileSystem().InitializeProjectDirectory(root));
    }

    [Fact]
    public void retrieve_all_dungeon_names_and_paths()
    {
        MockFileSystem mockFileSystem = MakeProjectFileSystem();
        var info = mockFileSystem.DriveInfo.GetDrives();
        IDictionary<string, string> dungeons = mockFileSystem.GetDungeonPaths(root);
        dungeons.Count.ShouldBe(2);
        dungeons.Keys.ShouldBeSubsetOf(["simple", "another"]);
        dungeons.Values.Select(RemoveDrive).ShouldBeSubsetOf(
            [$"/{Path.Combine(dungeonsPath, "simple.json")}",
                $"/{Path.Combine(dungeonsPath, "another.json")}"]);
        // Remove the drive name to make tests pass on linux machines
        static string RemoveDrive(string name) => name.Replace("C:\\", "/").Replace("C:/", "/");
    }

    [Fact]
    public void retrieve_all_script_names()
    {
        string[] scripts = [.. MakeProjectFileSystem().GetScriptNames(root)];
        scripts.Count().ShouldBe(3);
        scripts.ShouldBeSubsetOf(["tavern.lua", "second.lua", "third.lua"]);
    }

    [Fact]
    public void build_dungeon_manifest_with_dungeons_and_scripts()
    {
        DungeonCrawlerManifest actual = MakeProjectFileSystem().Build(root);

        DungeonCrawlerManifest expected = new();
        expected.AddDungeon("simple", Dungeon_should.SimpleSquareDungeon);
        expected.AddDungeon("another", Dungeon_should.TwoByTwoRoom);

        expected.AddScript("tavern.lua", new EventScript("Tavern"));
        expected.AddScript("second.lua", new EventScript("Second"));
        expected.AddScript("third.lua", new EventScript("Third"));

        actual.ShouldBe(expected);
    }

}