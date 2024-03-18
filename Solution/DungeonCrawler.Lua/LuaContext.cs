using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Player;

using MoonSharp.Interpreter;

namespace CaptainCoder.Dungeoneering.Lua;

[MoonSharpUserData]
public class LuaContext(IScriptContext context)
{
    private readonly IScriptContext _target = context;
    public DynValue PlayerView => UserData.Create(_target.View);
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
        _target.View = new PlayerView(new CaptainCoder.Dungeoneering.DungeonMap.Position(x, y), (Facing)facing);
    }

    public int GetWall()
    {
        var (position, facing) = _target.View;
        _target.CurrentDungeon.Walls.TryGetWall(position, facing, out WallType wall);
        return (int)wall;
    }

    public int GetWallAt(int x, int y, int f)
    {
        _target.CurrentDungeon.Walls.TryGetWall(new Position(x, y), (Facing)f, out WallType wall);
        return (int)wall;
    }

    public void SetWall(int type)
    {
        WallType wall = (WallType)type;
        var (position, facing) = _target.View;
        _target.CurrentDungeon.Walls.SetWall(position, facing, wall);
    }

    public void SetWallAt(int x, int y, int f, int type)
    {
        WallType wall = (WallType)type;
        Position position = new(x, y);
        Facing facing = (Facing)f;
        _target.CurrentDungeon.Walls.SetWall(position, facing, wall);
    }

    public void SetVariable(string name, DynValue value) => _target.State.GlobalVariables[name] = value.ToObject();
    public object GetVariable(string name) => _target.State.GlobalVariables.GetValueOrDefault(name);
    public void WriteInfo(string message) => _target.SendMessage(new Message(MessageType.Info, message));
    public void Debug(string message) => _target.SendMessage(new Message(MessageType.Debug, message));
    public void ChangeDungeon(string dungeonName, int x, int y, Facing facing)
    {
        _target.CurrentDungeon = _target.Manifest.DungeonManifest[dungeonName];
        _target.View = new PlayerView(x, y, facing);
    }
}