using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace CaptainCoder.Dungeoneering.DungeonMap;

public class DungeonEventMap()
{
    private readonly Dictionary<Position, List<TileEvent>> _events = [];

    public DungeonEventMap(IEnumerable<(Position, IEnumerable<TileEvent>)> events) : this()
    {
        _events = new();
        foreach ((Position pos, IEnumerable<TileEvent> eventsAtPosition) in events)
        {
            _events[pos] = [.. eventsAtPosition];
        }
    }
    public void AddEvent(Position position, TileEvent toAdd)
    {
        if (!_events.TryGetValue(position, out List<TileEvent> tileEvents))
        {
            tileEvents = new();
            _events[position] = tileEvents;
        }
        tileEvents.Add(toAdd);
    }

    public bool TryRemoveEvent(Position position, int ix, [NotNullWhen(true)] out TileEvent? removed)
    {
        if (_events.TryGetValue(position, out List<TileEvent> eventsAtPosition) && eventsAtPosition.Count > ix)
        {
            removed = eventsAtPosition[ix];
            eventsAtPosition.RemoveAt(ix);
            return true;
        }
        removed = default;
        return false;
    }
    public void RemoveAllEventsAt(Position position) => _events.Remove(position);

    public List<TileEvent> EventsAt(Position position) => _events.GetValueOrDefault(position, []);
    public IReadOnlyDictionary<Position, List<TileEvent>> Events => new ReadOnlyDictionary<Position, List<TileEvent>>(_events);
}