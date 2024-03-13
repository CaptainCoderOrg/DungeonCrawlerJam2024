namespace CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;

using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;

public interface ITileEventContext
{
    public PlayerView View { get; set; }
    public Dungeon Dungeon { get; set; }
}