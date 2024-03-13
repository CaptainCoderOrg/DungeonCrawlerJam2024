namespace Tests;

using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;

using NSubstitute;

using Shouldly;

public class JavaScriptEventAction_should
{

    [Fact]
    public void modify_player_view()
    {
        ITileEventContext context = Substitute.For<ITileEventContext>();
        JavaScriptEventAction action = new("""
        context.SetPlayerView(5, 7, Facing.East);
        """);
        action.Invoke(context);

        context.View.ShouldBe(new PlayerView(new Position(5, 7), Facing.East));
    }
}