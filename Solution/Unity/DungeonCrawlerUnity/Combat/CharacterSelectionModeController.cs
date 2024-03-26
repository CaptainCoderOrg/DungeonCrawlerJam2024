using CaptainCoder.DungeonCrawler.Unity;
using CaptainCoder.Dungeoneering.Player.Unity;

using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class CharacterSelectionModeController : MonoBehaviour
{
    public static CharacterSelectionModeController Shared { get; private set; } = default!;
    private CharacterSelectionMode _characterSelectionMode = default!;
    public PlayerInputMapping PlayerControls = default!;
    public CharacterSelectionModeController() { Shared = this; }

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

        for (int ix = 0; ix < Overlay.Shared.Cards.Length; ix++)
        {
            Overlay.Shared.Cards[ix].Character = characters[ix];
        }
        SelectCard(0, characters[0]);
        _characterSelectionMode.OnSelectionChange += SelectCard;
        _characterSelectionMode.OnSelected += (ix, selected) => CombatMapController.Shared.StartSpendActionPoints(selected);
    }
    private void SelectCard(int ix, PlayerCharacter character)
    {
        foreach (CharacterCardRenderer card in Overlay.Shared.Cards)
        {
            card.IsSelected = false;
        }
        Overlay.Shared.Cards[ix].IsSelected = true;
        CombatMapController.Shared.SelectCharacter(character);
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