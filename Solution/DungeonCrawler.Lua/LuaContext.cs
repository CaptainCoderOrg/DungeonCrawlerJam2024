using MoonSharp.Interpreter;

using DM = CaptainCoder.Dungeoneering.DungeonMap;

namespace DungeonCrawler.Lua;

public interface IScriptContext
{
    public CaptainCoder.Dungeoneering.Player.PlayerView View { get; set; }
}

[MoonSharpUserData]
public class LuaContext(IScriptContext context)
{
    private readonly IScriptContext _target = context;
    public DynValue PlayerView => UserData.Create(_target.View);
    public void SetPlayerPosition(int x, int y)
    {
        _target.View = _target.View with { Position = new DM.Position(x, y) };
    }

    public void SetPlayerFacing(int facing)
    {
        _target.View = _target.View with { Facing = (DM.Facing)facing };
    }

    public void SetPlayerView(int x, int y, int facing)
    {
        _target.View = new CaptainCoder.Dungeoneering.Player.PlayerView(new CaptainCoder.Dungeoneering.DungeonMap.Position(x, y), (DM.Facing)facing);
    }
}