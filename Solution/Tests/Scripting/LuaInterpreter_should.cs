namespace Tests;

using CaptainCoder.Dungeoneering.DungeonMap;

using DungeonCrawler.Lua;

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
}

internal class TestContext : IScriptContext
{
    public CaptainCoder.Dungeoneering.Player.PlayerView View { get; set; } = new CaptainCoder.Dungeoneering.Player.PlayerView(new CaptainCoder.Dungeoneering.DungeonMap.Position(0, 0), Facing.North);
}