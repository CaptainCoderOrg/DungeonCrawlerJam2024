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
        DynValue dynContext = UserData.Create(new LuaContext(context));
        lua.Globals.Set("context", dynContext);
        return lua.DoString(script);
    }
    public static void ExecLua(string script) => EvalLua(script);
    public static T EvalLua<T>(string script) => EvalLua(script).ToObject<T>();
    private static DynValue EvalLua(string script)
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