using System.Diagnostics.CodeAnalysis;

namespace CaptainCoder.Dungeoneering.DungeonMap;

public class EventMap()
{
    public Dictionary<Position, List<TileEvent>> Events { get; set; } = [];

    public EventMap(IEnumerable<(Position, IEnumerable<TileEvent>)> events) : this()
    {
        Events = new();
        foreach ((Position pos, IEnumerable<TileEvent> eventsAtPosition) in events)
        {
            Events[pos] = [.. eventsAtPosition];
        }
    }
    public void AddEvent(Position position, TileEvent toAdd)
    {
        if (!Events.TryGetValue(position, out List<TileEvent> tileEvents))
        {
            tileEvents = new();
            Events[position] = tileEvents;
        }
        tileEvents.Add(toAdd);
    }

    public bool TryRemoveEvent(Position position, int ix, [NotNullWhen(true)] out TileEvent? removed)
    {
        if (Events.TryGetValue(position, out List<TileEvent> eventsAtPosition) && eventsAtPosition.Count > ix)
        {
            removed = eventsAtPosition[ix];
            eventsAtPosition.RemoveAt(ix);
            return true;
        }
        removed = default;
        return false;
    }
    public void RemoveAllEventsAt(Position position) => Events.Remove(position);

    public List<TileEvent> EventsAt(Position position) => Events.GetValueOrDefault(position, []);
}