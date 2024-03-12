namespace Tests;

using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;
using CaptainCoder.Dungeoneering.Scripting;

using NSubstitute;

using Shouldly;

public class LuaEventAction_should
{
    [Fact]
    public void update_context_view()
    {
        ITileEventContext context = Substitute.For<ITileEventContext>();
        LuaEventAction action = new("""
        context.View = PlayerView(5, 7, Facing.East)
        """);
        action.Invoke(context);

        context.View.ShouldBe(new PlayerView(new Position(5, 7), Facing.East));
        
    }
}