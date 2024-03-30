using System.Collections;

namespace CaptainCoder.DungeonCrawler;

public class Party : IEnumerable<PlayerCharacter>
{
    public Party Copy()
    {
        Party newParty = new();
        newParty._topLeft = _topLeft;
        newParty._topRight = _topRight;
        newParty._bottomLeft = _bottomLeft;
        newParty._bottomRight = _bottomRight;
        return newParty;
    }
    public PlayerCharacter this[int ix]
    {
        get => ToArray[ix];
    }
    private PlayerCharacter _topLeft = new() { Card = Characters.CharacterA };
    private PlayerCharacter _topRight = new() { Card = Characters.NoBody };
    private PlayerCharacter _bottomLeft = new() { Card = Characters.NoBody };
    private PlayerCharacter _bottomRight = new() { Card = Characters.NoBody };
    private PlayerCharacter[] ToArray => [_topLeft, _topRight, _bottomLeft, _bottomRight];

    public PlayerCharacter TopLeft
    {
        get => _topLeft;
        set
        {
            _topLeft = value; OnTopLeftChange?.Invoke(value);
        }
    }
    public PlayerCharacter TopRight
    {
        get => _topRight;
        set
        {
            _topRight = value;
            OnTopRightChange?.Invoke(value);
        }
    }

    public PlayerCharacter BottomLeft
    {
        get => _bottomLeft;
        set
        {
            _bottomLeft = value;
            OnBottomLeftChange?.Invoke(value);
        }
    }

    public PlayerCharacter BottomRight
    {
        get => _bottomRight;
        set
        {
            _bottomRight = value;
            OnBottomRightChange?.Invoke(value);
        }
    }

    public event Action<PlayerCharacter>? OnTopLeftChange;
    public event Action<PlayerCharacter>? OnTopRightChange;
    public event Action<PlayerCharacter>? OnBottomLeftChange;
    public event Action<PlayerCharacter>? OnBottomRightChange;

    public bool IsDead => ToArray.All(pc => pc.IsDead() || pc.Card == Characters.NoBody);

    public void UpdateCharacter(PlayerCharacter toUpdate)
    {
        if (TopLeft.Card == toUpdate.Card) { TopLeft = toUpdate; }
        else if (TopRight.Card == toUpdate.Card) { TopRight = toUpdate; }
        else if (BottomLeft.Card == toUpdate.Card) { BottomLeft = toUpdate; }
        else if (BottomRight.Card == toUpdate.Card) { BottomRight = toUpdate; }
    }

    public IEnumerator<PlayerCharacter> GetEnumerator() => ToArray.AsEnumerable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void ApplyValues(Party party)
    {
        foreach (PlayerCharacter character in party)
        {
            UpdateCharacter(character);
        }
    }
}