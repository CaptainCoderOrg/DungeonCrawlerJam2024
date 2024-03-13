using MoonSharp.Interpreter;

namespace DungeonCrawler.Lua;

[MoonSharpUserData]
public class LuaAPI
{
    public int Sum(int x, int y) => x + y;
}