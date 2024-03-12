namespace CaptainCoder.Dungeoneering.Scripting;

using System.Reflection;

using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;

using NLua;


public record LuaEventAction(string Script) : EventAction
{
    private static readonly string Prelude = $$"""
    Facing = {}
    Facing.North = {{(int)Facing.North}}
    Facing.East = {{(int)Facing.East}}
    Facing.South = {{(int)Facing.South}}
    Facing.West = {{(int)Facing.West}}
    """;
    public override void Invoke(ITileEventContext context)
    {
        using Lua state = new();
        state.RegisterFunction("PlayerView", RuntimeReflectionExtensions.GetMethodInfo(PlayerView));
        state["context"] = context;
        state.DoString(Prelude);
        state.DoString(Script);
    }

    private static PlayerView PlayerView(int x, int y, int facing) => new(new Position(x, y), (Facing)facing);
}