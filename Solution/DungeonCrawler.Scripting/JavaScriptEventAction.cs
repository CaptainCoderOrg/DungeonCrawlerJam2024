namespace CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;

using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;

using Jint;

using Newtonsoft.Json;

public record JavaScriptEventAction(string Script)
{
    private static readonly string Prelude = $$$"""
        const Facing = { 
            North: {{{(int)Facing.North}}}, 
            East: {{{(int)Facing.East}}}, 
            West: {{{(int)Facing.West}}}, 
            South: {{{(int)Facing.South}}}
        };

        const context = JSON.parse(rawContext);

        context.SetPlayerView = (x, y, f) => 
        {
            context.View = { Position: { X: x, Y: y }, Facing: f };
        }
        """;
    public void Invoke(ITileEventContext context)
    {
        Engine engine = new();
        string json = context.ToJson();
        engine.SetValue("rawContext", json);
        engine.Execute(Prelude);
        engine.Execute(Script);
        json = engine.Evaluate("JSON.stringify(context);").ToObject() as string ?? throw new Exception($"Could not evaluate context.");
        context.SyncWithJson(json);
    }

}

[Serializable]
public class Context(PlayerView view, Dungeon dungeon) : ITileEventContext
{
    public PlayerView View { get; set; } = view;
    public Dungeon Dungeon { get; set; } = dungeon;
}

public static class ITileEventContextExtensions
{
    public static string ToJson(this ITileEventContext context)
    {
        return JsonConvert.SerializeObject(new Context(context.View, context.Dungeon));
    }

    public static void SyncWithJson(this ITileEventContext context, string json)
    {
        Context restored = JsonConvert.DeserializeObject<Context>(json) ?? throw new Exception($"Could not deserialize {json} to ProxyContext");
        context.View = restored.View;
        context.Dungeon = restored.Dungeon;
    }
}