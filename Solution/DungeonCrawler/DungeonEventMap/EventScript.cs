using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Player;


namespace CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;

public record EventScript(string Script);

public interface IScriptContext
{
    public GameState State { get; set; }
    public PlayerView View { get; set; }
    public Dungeon CurrentDungeon { get; set; }
    public DungeonCrawlerManifest Manifest { get; set; }
    public void SendMessage(Message message);
}