namespace CaptainCoder.DungeonCrawler.Combat;


public class CharacterSelectionMode(IEnumerable<PlayerCharacter> characters)
{
    private readonly PlayerCharacter[] _characters = [.. characters];
    private int _selectedIx = 0;
    public event Action<int>? OnSelectionChange;
    public event Action<(int Index, PlayerCharacter Character)>? OnSelected;
    public void HandleInput(CharacterSelectionControl input)
    {
        Action action = input switch
        {
            CharacterSelectionControl.Next => () => Next(1),
            CharacterSelectionControl.Previous => () => Next(-1),
            CharacterSelectionControl.Select => () => OnSelected?.Invoke((_selectedIx, _characters[_selectedIx])),
            _ => throw new NotImplementedException($"Unknown input {input}"),
        };
        action.Invoke();
    }

    private void Next(int delta)
    {
        _selectedIx += delta;
        if (_selectedIx < 0) { _selectedIx = _characters.Length - 1; }
        else if (_selectedIx >= _characters.Length) { _selectedIx = 0; }
        OnSelectionChange?.Invoke(_selectedIx);
    }

}

public enum CharacterSelectionControl
{
    Next,
    Previous,
    Select,
}
