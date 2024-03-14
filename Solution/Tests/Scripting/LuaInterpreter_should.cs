namespace Tests;

using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;

using DungeonCrawler.Lua;

using NSubstitute;

using Shouldly;

public class LuaInterpreter_should
{

    [Fact]
    public void interop_with_LuaAPI()
    {
        Interpreter.EvalRawLua<int>("return LuaAPI:Sum(5, 7)").ShouldBe(12);
    }

    [Fact]
    public void set_player_position()
    {
        IScriptContext context = new TestContext();
        Interpreter.ExecLua("""
        context.SetPlayerPosition(5, 7)
        """, context);
        context.View.ShouldBe(new CaptainCoder.Dungeoneering.Player.PlayerView(new CaptainCoder.Dungeoneering.DungeonMap.Position(5, 7), Facing.North));
    }

    [Theory]
    [InlineData(Facing.North)]
    [InlineData(Facing.East)]
    [InlineData(Facing.South)]
    [InlineData(Facing.West)]
    public void set_player_facing(Facing facing)
    {
        IScriptContext context = new TestContext();
        Interpreter.ExecLua($"""
        context.SetPlayerFacing({facing})
        """, context);
        context.View.ShouldBe(new CaptainCoder.Dungeoneering.Player.PlayerView(new CaptainCoder.Dungeoneering.DungeonMap.Position(0, 0), facing));
    }

    [Fact]
    public void set_player_view()
    {
        IScriptContext context = new TestContext();
        Interpreter.ExecLua("""
        context.SetPlayerView(3, 4, East)
        """, context);
        context.View.ShouldBe(new CaptainCoder.Dungeoneering.Player.PlayerView(new CaptainCoder.Dungeoneering.DungeonMap.Position(3, 4), Facing.East));
    }

    [Theory]
    [InlineData("return context.PlayerView.Position.X", 5)]
    [InlineData("return context.PlayerView.Position.Y", 7)]
    [InlineData("return context.PlayerView.Facing", (int)Facing.East)]
    public void have_access_to_player_view(string script, int expectedResult)
    {
        IScriptContext context = new TestContext();
        context.View = new CaptainCoder.Dungeoneering.Player.PlayerView(new CaptainCoder.Dungeoneering.DungeonMap.Position(5, 7), Facing.East);

        int result = Interpreter.EvalLua<int>(script, context);

        result.ShouldBe(expectedResult);
    }

    public static Dungeon TwoByTwoRoom
    {
        get
        {
            Dungeon dungeon = new();
            dungeon.Walls.SetWall(new Position(0, 0), Facing.North, WallType.Solid);
            dungeon.Walls.SetWall(new Position(0, 0), Facing.West, WallType.Solid);

            dungeon.Walls.SetWall(new Position(1, 0), Facing.North, WallType.Solid);
            dungeon.Walls.SetWall(new Position(1, 0), Facing.East, WallType.Solid);

            dungeon.Walls.SetWall(new Position(1, 1), Facing.South, WallType.Solid);
            dungeon.Walls.SetWall(new Position(1, 1), Facing.East, WallType.Solid);

            dungeon.Walls.SetWall(new Position(0, 1), Facing.South, WallType.Solid);
            dungeon.Walls.SetWall(new Position(0, 1), Facing.West, WallType.Solid);
            return dungeon;
        }
    }

    [Theory]
    [InlineData("return context.GetWall()", WallType.Solid)]
    [InlineData("""
        context.SetPlayerFacing(East)
        return context.GetWall()
        """, 
        WallType.None)
    ]
    public void get_wall(string script, WallType expected)
    {
        IScriptContext context = Substitute.For<IScriptContext>();
        context.View = new PlayerView(new Position(0, 0), Facing.North);
        context.CurrentDungeon.Returns(TwoByTwoRoom);

        WallType actual = Interpreter.EvalLua<WallType>(script, context);
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(0, 0, Facing.North)]
    [InlineData(1, 0, Facing.South)]
    [InlineData(1, 1, Facing.East)]
    [InlineData(0, 0, Facing.West)]
    public void get_wall_at(int x, int y, Facing facing)
    {
        string script = $"""
        return context.GetWallAt({x}, {y}, {facing})
        """;
        IScriptContext context = Substitute.For<IScriptContext>();
        context.View = new PlayerView(new Position(0, 0), Facing.North);
        context.CurrentDungeon.Returns(TwoByTwoRoom);

        WallType actual = Interpreter.EvalLua<WallType>(script, context);
        WallType expected = TwoByTwoRoom.Walls.GetWall(new Position(x, y), facing);
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("context.SetWall(Solid)", 5, 5, Facing.North, WallType.Solid)]
    [InlineData("context.SetWall(None)", 0, 0, Facing.East, WallType.None)]
    [InlineData("context.SetWall(Door)", 2, 2, Facing.West, WallType.Door)]
    [InlineData("context.SetWall(SecretDoor)", 7, 9, Facing.South, WallType.SecretDoor)]
    public void set_wall(string script, int x, int y, Facing facing, WallType expected)
    {
        IScriptContext context = Substitute.For<IScriptContext>();
        context.View = new PlayerView(new Position(x, y), facing);
        Dungeon emptyDungeon = new();
        context.CurrentDungeon.Returns(emptyDungeon);

        Interpreter.ExecLua(script, context);
        
        WallType actual = emptyDungeon.Walls.GetWall(context.View.Position, context.View.Facing);
        actual.ShouldBe(expected);
        actual = emptyDungeon.Walls.GetWall(context.View.Position.Step(context.View.Facing), context.View.Facing.Opposite());
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(3, 4, Facing.East, WallType.None)]
    [InlineData(5, 5, Facing.North, WallType.Solid)]
    [InlineData(0, 0, Facing.East, WallType.None)]
    [InlineData(2, 2, Facing.West, WallType.Door)]
    [InlineData(7, 9, Facing.South, WallType.SecretDoor)]
    public void set_wall_at(int x, int y, Facing facing, WallType expected)
    {
        string script = $"""
        context.SetWallAt({x}, {y}, {facing}, {expected})
        """;
        IScriptContext context = Substitute.For<IScriptContext>();
        context.View = new PlayerView(new Position(x, y), facing);
        Dungeon emptyDungeon = new();
        context.CurrentDungeon.Returns(emptyDungeon);

        Interpreter.ExecLua(script, context);
        
        WallType actual = emptyDungeon.Walls.GetWall(new Position(x, y), facing);
        actual.ShouldBe(expected);
        actual = emptyDungeon.Walls.GetWall(new Position(x, y).Step(facing), facing.Opposite());
        actual.ShouldBe(expected);
    }
}

internal class TestContext : IScriptContext
{
    public CaptainCoder.Dungeoneering.Player.PlayerView View { get; set; } = new CaptainCoder.Dungeoneering.Player.PlayerView(new CaptainCoder.Dungeoneering.DungeonMap.Position(0, 0), Facing.North);

    public Dungeon CurrentDungeon => throw new NotImplementedException();
}