namespace Tests;

using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;

using DungeonCrawler.Lua;

using Shouldly;

public class LuaInterpreter_should
{

    [Fact]
    public void interop_with_LuaAPI()
    {
        Interpreter.EvalLua<int>("return LuaAPI:Sum(5, 7)").ShouldBe(12);
    }

    [Fact]
    public void set_player_position()
    {
        IScriptContext context = new TestContext();
        Interpreter.ExecLua("""
        context.SetPlayerPosition(5, 7)
        """, context);
        context.View.ShouldBe(new PlayerView(new Position(5, 7), Facing.North));
    }
}

internal class TestContext : IScriptContext
{
    public PlayerView View { get; set; } = new PlayerView(new Position(0, 0), Facing.North);
}