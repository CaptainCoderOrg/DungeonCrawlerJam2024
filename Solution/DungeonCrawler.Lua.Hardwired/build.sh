#!/bin/bash
dotnet run
./repl/MoonSharp.exe -W hardwired.lua ../DungeonCrawler.Lua/LuaInitializer.cs --internals --class:LuaInitializer --namespace:CaptainCoder.Dungeoneering.Scripting