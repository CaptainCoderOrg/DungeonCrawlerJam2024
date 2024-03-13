namespace Tests;

using CaptainCoder.Dungeoneering.DungeonMap;

using Shouldly;

public class DungeonEventMap_should
{
    static readonly MockEventAction TeleportPlayerMock = new("Teleport Player");
    static readonly TileEvent TeleportEvent = new("Teleport Player", EventTrigger.OnEnter, TeleportPlayerMock);
    static readonly MockEventAction DamagePlayerMock = new("Damage Player Mock");
    static readonly TileEvent DamageEvent = new("Test Exit", EventTrigger.OnExit, DamagePlayerMock);

    public static IEnumerable<object[]> AddEventsAtPositionData => [
        [new Position(5, 5), TeleportEvent],
        [new Position(2, 7), DamageEvent],
    ];

    [Theory]
    [MemberData(nameof(AddEventsAtPositionData))]
    public void add_event_at_position(Position position, TileEvent eventToAdd)
    {
        DungeonEventMap map = new();
        map.AddEvent(position, eventToAdd);
        map.EventsAt(position).Count.ShouldBe(1);
        map.EventsAt(position).ShouldContain(eventToAdd);
    }

    public static IEnumerable<object[]> AddMultipleEventsAtPositionData => [
        [new Position(5, 5), TeleportEvent, DamageEvent],
        [new Position(2, 7), DamageEvent, TeleportEvent],
    ];

    [Theory]
    [MemberData(nameof(AddMultipleEventsAtPositionData))]

    public void add_multiple_events_at_position(Position position, TileEvent firstEvent, TileEvent secondEvent)
    {
        DungeonEventMap map = new();
        map.AddEvent(position, firstEvent);
        map.AddEvent(position, secondEvent);
        map.EventsAt(position).Count.ShouldBe(2);
        map.EventsAt(position).ShouldContain(firstEvent);
        map.EventsAt(position).ShouldContain(secondEvent);
    }

    [Fact]
    public void remove_event_by_index()
    {
        DungeonEventMap map = new();
        Position pos = new(0, 0);
        map.AddEvent(pos, DamageEvent);
        map.AddEvent(pos, TeleportEvent);
        List<TileEvent> events = map.EventsAt(pos);

        int ix = events.IndexOf(DamageEvent);
        bool wasSuccessful = map.TryRemoveEvent(pos, ix, out TileEvent? removed);

        wasSuccessful.ShouldBeTrue();
        removed.ShouldBe(DamageEvent);
        map.EventsAt(pos).Count.ShouldBe(1);
    }

    [Fact]
    public void remove_event_by_index_returns_false_on_no_entry()
    {
        DungeonEventMap map = new();
        bool actual = map.TryRemoveEvent(new Position(0, 0), 0, out _);
        actual.ShouldBeFalse();
    }

    [Fact]
    public void remove_all_events_at_position()
    {
        DungeonEventMap map = new();
        Position pos = new(0, 0);
        map.AddEvent(pos, DamageEvent);
        map.AddEvent(pos, TeleportEvent);

        map.RemoveAllEventsAt(pos);

        map.EventsAt(pos).Count.ShouldBe(0);
    }

    [Fact]
    public void be_jsonable()
    {

    }

}

internal record MockEventAction(string Identifier) : EventAction
{
    public override void Invoke(ITileEventContext context)
    {
    }
}