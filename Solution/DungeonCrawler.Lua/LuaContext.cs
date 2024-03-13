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
    public DynValue PlayerView => UserData.Create(new PlayerViewProxy(_target.View));
    public void SetPlayerPosition(int x, int y)
    {
        _target.View = _target.View with { Position = new Position(x, y) };
    }

    public void SetPlayerFacing(int facing)
    {
        _target.View = _target.View with { Facing = (Facing)facing };
    }

    public void SetPlayerView(int x, int y, int facing)
    {
        _target.View = new PlayerView(new Position(x, y), (Facing)facing);
    }
}

[MoonSharpUserData]
public class PlayerViewProxy(PlayerView target)
{
    private readonly PlayerView _target = target;
    public DynValue Position => UserData.Create(new PositionProxy(_target.Position));
    public int Facing => (int)_target.Facing;
}

[MoonSharpUserData]
public class PositionProxy(Position target)
{
    private readonly Position _target = target;
    public int X => _target.X;
    public int Y => _target.Y;
}