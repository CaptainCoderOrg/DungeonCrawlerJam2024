namespace Tests;

using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Player;

using Shouldly;

public class CrawlerMode_should
{
    [Theory]
    [InlineData(5, 5, Facing.East)]
    [InlineData(7, 2, Facing.West)]
    [InlineData(1, 9, Facing.North)]
    [InlineData(3, 18, Facing.South)]
    public void notify_observer_when_view_changes(int x, int y, Facing facing)
    {
        CrawlerMode underTest = new(new Dungeon(), new PlayerView(new Position(0, 0), Facing.North));
        PlayerView actual = null!;
        underTest.OnViewChange += (newView) => actual = newView;
        PlayerView expected = new(x, y, facing);
        underTest.CurrentView = expected;

        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(MessageType.Info, "Information")]
    [InlineData(MessageType.Debug, "Debug Message")]
    public void notify_observer_on_message(MessageType type, string message)
    {
        CrawlerMode underTest = new(new Dungeon(), new PlayerView(new Position(0, 0), Facing.North));
        Message actual = null!;
        underTest.OnMessageAdded += (message) => actual = message;
        Message expected = new(type, message);
        underTest.AddMessage(expected);
    }
}