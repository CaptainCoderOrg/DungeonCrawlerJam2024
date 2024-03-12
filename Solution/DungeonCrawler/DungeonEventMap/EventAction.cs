using CaptainCoder.Dungeoneering.Player;

namespace CaptainCoder.Dungeoneering.DungeonMap;
public abstract record EventAction
{
    public abstract void Invoke(ITileEventContext context);
}

public interface ITileEventContext
{
    public PlayerView View { get; set; }
    public Dungeon Dungeon { get; set; }
}