using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Scripting;

using MoonSharp.Interpreter;

namespace DungeonCrawler.Lua;

public static class Interpreter
{
    private static bool s_initialized = false;
    public static T EvalLua<T>(string script, IScriptContext context) => EvalLua(script, context).ToObject<T>();
    public static void ExecLua(string script, IScriptContext context) => EvalLua(script, context);
    public static DynValue EvalLua(string script, IScriptContext context)
    {
        Initialize();
        Script lua = new();
        lua.Globals.Set("context", UserData.Create(new LuaContext(context)));
        lua.Globals[Facing.North.ToString()] = (int)Facing.North;
        lua.Globals[Facing.East.ToString()] = (int)Facing.East;
        lua.Globals[Facing.South.ToString()] = (int)Facing.South;
        lua.Globals[Facing.West.ToString()] = (int)Facing.West;
        return lua.DoString(script);
    }
    public static T EvalRawLua<T>(string script) => EvalRawLua(script).ToObject<T>();
    private static DynValue EvalRawLua(string script)
    {
        Initialize();
        Script lua = new();
        lua.Globals.Set("LuaAPI", UserData.Create(new LuaAPI()));
        return lua.DoString(script);
    }
    private static void Initialize()
    {
        if (!s_initialized)
        {
            LuaInitializer.Initialize();
            s_initialized = true;
        }
    }
}