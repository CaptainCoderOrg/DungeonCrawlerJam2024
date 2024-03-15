using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;

namespace CaptainCoder.Dungeoneering.Game;

public class CrawlerMode(Dungeon dungeon, PlayerView playerView)
{
    public Dungeon CurrentDungeon { get; private set; } = dungeon;
    private PlayerView _currentView = playerView;
    public PlayerView CurrentView
    {
        get => _currentView;
        set
        {
            if (_currentView == value) { return; }
            _currentView = value;
            OnViewChange?.Invoke(_currentView);
        }
    }
    public event Action<PlayerView>? OnViewChange;
}