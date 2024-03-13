namespace CaptainCoder.Dungeoneering.DungeonMap;

using CaptainCoder.Dungeoneering.Player;

public abstract record EventAction
{
    public abstract void Invoke(ITileEventContext context);
}

public interface ITileEventContext
{
    public PlayerView View { get; set; }
    public Dungeon Dungeon { get; set; }
}