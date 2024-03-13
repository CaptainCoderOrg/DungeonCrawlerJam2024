using DungeonCrawler.Lua;

namespace CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;

public record EventScript(string Script);
public static class EventScriptExtensions
{
    public static void Invoke(this EventScript script, IScriptContext context) => Interpreter.ExecLua(script.Script, context);
}