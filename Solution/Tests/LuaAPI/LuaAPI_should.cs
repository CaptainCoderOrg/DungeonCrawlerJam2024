namespace Tests;

using DungeonCrawler.Lua;

using Shouldly;

public class LuaAPI_should
{

    [Fact]
    public void interop_with_LuaAPI()
    {
        Interpreter.EvalLua<int>("return LuaAPI:Sum(5, 7)").ShouldBe(12);
    }
}