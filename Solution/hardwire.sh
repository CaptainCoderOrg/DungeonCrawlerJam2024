#!/bin/bash
cd DungeonCrawler.Lua.Hardwired
dotnet build
dotnet run
./repl/MoonSharp.exe -W hardwired.moonlua ../DungeonCrawler.Lua/LuaInitializer.cs --internals --class:LuaInitializer --namespace:CaptainCoder.Dungeoneering.Lua