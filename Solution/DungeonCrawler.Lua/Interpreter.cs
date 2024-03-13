using CaptainCoder.Dungeoneering.Scripting;

using MoonSharp.Interpreter;

namespace DungeonCrawler.Lua;

public static class Interpreter
{
    private static bool s_initialized = false;

    public static void ExecLua(string script)
    {
        if (!s_initialized)
        {
            LuaInitializer.Initialize();
            s_initialized = true;
        }
        Script lua = new();
        lua.Globals.Set("LuaAPI", UserData.Create(new LuaAPI()));
        _ = lua.DoString(script);
    }
    public static T EvalLua<T>(string script)
    {
        if (!s_initialized)
        {
            LuaInitializer.Initialize();
            s_initialized = true;
        }
        Script lua = new();
        lua.Globals.Set("LuaAPI", UserData.Create(new LuaAPI()));
        DynValue result = lua.DoString(script);
        return result.ToObject<T>();
    }
    public static int CallInterpreterWithSum5_7()
    {
        if (!s_initialized)
        {
            LuaInitializer.Initialize();
            s_initialized = true;
        }
        Script lua = new();
        lua.Globals.Set("LuaAPI", UserData.Create(new LuaAPI()));
        DynValue result = lua.DoString("return LuaAPI:Sum(5, 7)");
        return result.ToObject<int>();
    }
}