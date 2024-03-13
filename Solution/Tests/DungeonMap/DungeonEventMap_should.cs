namespace Tests;

using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.DungeonMap.IO;

using Shouldly;

public class DungeonEventMap_should
{
    static readonly MockEventAction TeleportPlayerMock = new("Teleport Player");
    static readonly TileEvent TeleportEvent = new(EventTrigger.OnEnter, TeleportPlayerMock);
    static readonly MockEventAction DamagePlayerMock = new("Damage Player Mock");
    static readonly TileEvent DamageEvent = new(EventTrigger.OnExit, DamagePlayerMock);

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
    public void provide_view_of_events()
    {
        DungeonEventMap map = new();
        Position pos = new(0, 0);
        map.AddEvent(pos, DamageEvent);
        
        IReadOnlyDictionary<Position, List<TileEvent>> events = map.Events;

        events[pos].Count.ShouldBe(1);
        events[pos].ShouldContain(DamageEvent);

        map.AddEvent(pos, TeleportEvent);

        events[pos].Count.ShouldBe(2);
        events[pos].ShouldContain(TeleportEvent);
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
        DungeonEventMap map = new();
        JavaScriptEventAction event0 = new("event0");
        JavaScriptEventAction event1 = new("event1");
        JavaScriptEventAction event2 = new("event2");
        Position pos0 = new(0,0);
        Position pos1 = new(0,0);
        map.AddEvent(pos0, new TileEvent(EventTrigger.OnEnter, event0));
        map.AddEvent(pos0, new TileEvent(EventTrigger.OnExit, event1));
        map.AddEvent(pos1, new TileEvent(EventTrigger.OnEnter, event2)); 

        string json = map.ToJson();
        /*
        "{\"Events\":{\"Position { X = 0, Y = 0 }\":[{\"Trigger\":0,\"OnTrigger\":{\"Script\":\"event0\"}},{\"Trigger\":1,\"OnTrigger\":{\"Script\":\"event1\"}},{\"Trigger\":0,\"OnTrigger\":{\"Script\":\"event2\"}}]}}"
        */
        DungeonEventMap restored = JsonExtensions.LoadModel<DungeonEventMap>(json);

        List<TileEvent> expectedEventsAtPos0 = map.EventsAt(pos0);
        restored.EventsAt(pos0).Count.ShouldBe(expectedEventsAtPos0.Count);
        restored.EventsAt(pos0).ShouldBeSubsetOf(expectedEventsAtPos0);

        List<TileEvent> expectedEventsAtPos1 = map.EventsAt(pos1);
        restored.EventsAt(pos1).Count.ShouldBe(expectedEventsAtPos1.Count);
        restored.EventsAt(pos1).ShouldBeSubsetOf(expectedEventsAtPos1);
    }

}

internal record MockEventAction(string Identifier) : EventAction
{
    public override void Invoke(ITileEventContext context)
    {
    }
}