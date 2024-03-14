using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Scripting;

using MoonSharp.Interpreter;

namespace DungeonCrawler.Lua;

public static class Interpreter
{
    private static bool s_initialized = false;
    public static T EvalLua<T>(string script, IScriptContext context) => EvalLua(script, context).ToObject<T>();
    public static void ExecLua(string script, IScriptContext context) => EvalLua(script, context);
    public static DynValue EvalLua(string script, IScriptContext context) => Initialize(context).DoString(script);
    public static T EvalRawLua<T>(string script) => Initialize().DoString(script).ToObject<T>();
    private static Script Initialize(IScriptContext? context = null)
    {
        if (!s_initialized)
        {
            LuaInitializer.Initialize();
            s_initialized = true;
        }
        Script lua = new();
        if (context is not null)
        {
            lua.Globals.Set("context", UserData.Create(new LuaContext(context)));
        }
        lua.Globals.Set("LuaAPI", UserData.Create(new LuaAPI()));
        lua.Globals[Facing.North.ToString()] = (int)Facing.North;
        lua.Globals[Facing.East.ToString()] = (int)Facing.East;
        lua.Globals[Facing.South.ToString()] = (int)Facing.South;
        lua.Globals[Facing.West.ToString()] = (int)Facing.West;
        return lua;
    }
}