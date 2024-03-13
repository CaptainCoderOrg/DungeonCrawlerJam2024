using System;
using System.IO;

using DungeonCrawler.Lua;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Serialization;
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Generating hardwired Lua output");
        File.WriteAllText("hardwired.lua", Hardwired.GenerateHardwiredLua());
    }
}


public static class Hardwired
{
    public static string GenerateHardwiredLua()
    {
        UserData.RegisterAssembly(typeof(LuaAPI).Assembly);
        return UserData.GetDescriptionOfRegisteredTypes(true).Serialize();
    }
}