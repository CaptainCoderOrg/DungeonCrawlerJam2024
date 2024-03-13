#!/bin/bash
dotnet bin/Debug/net8.0/DungeonCrawler.Lua.Hardwired.dll
./repl/MoonSharp.exe -W hardwired.lua ../DungeonCrawler.Lua/LuaInitializer.cs --internals --class:LuaInitializer --namespace:CaptainCoder.Dungeoneering.Scripting