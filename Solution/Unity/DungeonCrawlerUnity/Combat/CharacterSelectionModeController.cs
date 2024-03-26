using CaptainCoder.Dungeoneering.Player.Unity;

using TMPro;

using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class CharacterSelectionModeController : MonoBehaviour
{
    [field: SerializeField]
    public CharacterCardRenderer[] Cards { get; private set; } = [];
    [field: SerializeField]
    public CombatMapController CombatMapController { get; private set; } = default!;
    private CharacterSelectionMode _characterSelectionMode = default!;
    public PlayerInputMapping PlayerControls = default!;

    public void Update()
    {
        foreach (MenuActionMapping mapping in PlayerControls.MenuActionMappings)
        {
            if (Input.GetKeyDown(mapping.Key))
            {
                HandleUserInput(mapping.Action);
            }
        }
    }

    public void Initialize(PlayerCharacter[] characters)
    {
        _characterSelectionMode = new(characters);

        for (int ix = 0; ix < Cards.Length; ix++)
        {
            Cards[ix].Character = characters[ix];
        }
        SelectCard(0, characters[0]);
        _characterSelectionMode.OnSelectionChange += SelectCard;
        _characterSelectionMode.OnSelected += (ix, selected) => CombatMapController.StartSpendActionPoints(Cards[ix], selected);
    }
    private void SelectCard(int ix, PlayerCharacter character)
    {
        foreach (CharacterCardRenderer card in Cards)
        {
            card.IsSelected = false;
        }
        Cards[ix].IsSelected = true;
        CombatMapController.SelectCharacter(character);
    }
    public void HandleUserInput(MenuControl input)
    {
        Action action = input switch
        {
            MenuControl.Right => () => _characterSelectionMode.HandleInput(CharacterSelectionControl.Next),
            MenuControl.Left => () => _characterSelectionMode.HandleInput(CharacterSelectionControl.Previous),
            MenuControl.Select => () => _characterSelectionMode.HandleInput(CharacterSelectionControl.Select),
            _ => () => { }
        };
        action.Invoke();
    }
}

public class ValueRenderer : MonoBehaviour
{
    public TextMeshProUGUI Text = default!;
    public int Value
    {
        set => Text.text = value.ToString();
    }
}

public class TwoValueRenderer : MonoBehaviour
{
    public TextMeshProUGUI Text = default!;
    private int _value = 10;
    private int _baseValue = 10;
    public int Value
    {
        set
        {
            _value = value;
            Text.text = $"{_value}/{_baseValue}";
        }
    }
    public int BaseValue
    {
        set
        {
            _baseValue = value;
            Text.text = $"{_value}/{_baseValue}";
        }
    }
}