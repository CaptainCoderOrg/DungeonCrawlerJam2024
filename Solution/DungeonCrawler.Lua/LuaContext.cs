using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;

using MoonSharp.Interpreter;

namespace DungeonCrawler.Lua;

public interface IScriptContext
{
    public PlayerView View { get; set; }
}

[MoonSharpUserData]
public class LuaContext(IScriptContext context)
{
    private readonly IScriptContext _target = context;
    public void SetPlayerPosition(int x, int y)
    {
        _target.View = _target.View with { Position = new Position(x, y) };
    }
}