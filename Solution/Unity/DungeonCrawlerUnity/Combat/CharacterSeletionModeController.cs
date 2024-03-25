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
        CombatMapController.PanTo(character);
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

public class CharacterCardRenderer : MonoBehaviour
{
    public GameObject Selected = default!;
    public TextMeshProUGUI Name = default!;
    public TwoValueRenderer Health = default!;
    public TwoValueRenderer Energy = default!;
    public ValueRenderer Armor = default!;
    public ValueRenderer Speed = default!;
    public ValueRenderer MovementPoints = default!;
    public ValueRenderer AttackPoints = default!;
    public bool IsSelected { set => Selected.SetActive(value); }
    public PlayerCharacter? Character
    {
        set
        {
            if (value is null)
            {
                RenderNoCharacter();
            }
            else
            {
                Render(value);
            }
        }
    }

    public void Render(PlayerCharacter character)
    {
        Name.text = character.Card.Name;
        Health.Value = character.Health();
        Health.BaseValue = character.Card.BaseHealth;

        Energy.Value = character.Energy();
        Energy.BaseValue = character.Card.BaseEnergy;

        Armor.Value = character.Card.BaseArmor;
        Speed.Value = character.Card.BaseSpeed;
        MovementPoints.Value = character.MovementPoints;
    }

    public void RenderNoCharacter()
    {
        Debug.Assert(false, "RenderNoCharacter() is not implemented");
    }
}