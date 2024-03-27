namespace CaptainCoder.DungeonCrawler.Combat;

public class CharacterSelectionMode(IEnumerable<PlayerCharacter> characters)
{
    private readonly PlayerCharacter[] _characters = [.. characters];
    public int SelectedIx { get; private set; } = 0;
    public event Action<int, PlayerCharacter>? OnSelectionChange;
    public event Action<int, PlayerCharacter>? OnSelected;
    public void HandleInput(CharacterSelectionControl input)
    {
        Action action = input switch
        {
            CharacterSelectionControl.Next => () => Next(1),
            CharacterSelectionControl.Previous => () => Next(-1),
            CharacterSelectionControl.Select => () => OnSelected?.Invoke(SelectedIx, _characters[SelectedIx]),
            _ => throw new NotImplementedException($"Unknown input {input}"),
        };
        action.Invoke();
    }

    private void Next(int delta)
    {
        SelectedIx += delta;
        if (SelectedIx < 0) { SelectedIx = _characters.Length - 1; }
        else if (SelectedIx >= _characters.Length) { SelectedIx = 0; }
        OnSelectionChange?.Invoke(SelectedIx, _characters[SelectedIx]);
    }

}

public enum CharacterSelectionControl
{
    Next,
    Previous,
    Select,
}