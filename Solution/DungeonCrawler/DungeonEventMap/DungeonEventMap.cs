namespace CaptainCoder.Dungeoneering.DungeonMap;

public class DungeonEventMap()
{
    public void AddEvent(Position position, TileEvent toAdd) => throw new NotImplementedException();
    public bool TryRemoveEvent(Position position, string name) => throw new NotImplementedException();
    public IEnumerable<TileEvent> EventsAt(Position position) => throw new NotImplementedException();
}

public abstract record EventAction
{
    public abstract void Invoke();
}

public record LuaEventAction(string Script)
{
    public void Invoke() => throw new NotImplementedException();
}

public record TileEvent(string Name, EventTrigger Trigger, EventAction OnTrigger);

public enum EventTrigger
{
    OnEnter,
    OnExit,
}