using CaptainCoder.DungeonCrawler.Unity;
using CaptainCoder.Dungeoneering.Player.Unity;
using CaptainCoder.Dungeoneering.Unity;

using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class CharacterSelectionModeController : MonoBehaviour
{
    public static CharacterSelectionModeController Shared { get; private set; } = default!;
    private CharacterSelectionMode _characterSelectionMode = default!;
    public PlayerInputMapping PlayerControls = default!;
    public CharacterSelectionModeController() { Shared = this; }

    public void OnEnable()
    {
        SpendActionPointsModeController.Shared.gameObject.SetActive(false);
    }

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

    public void Initialize()
    {
        _characterSelectionMode = new(CrawlingModeController.Shared.Party);
        SelectCard(0, CrawlingModeController.Shared.Party[0]);
        _characterSelectionMode.OnSelectionChange += SelectCard;
        _characterSelectionMode.OnSelected += (ix, selected) => CombatMapController.Shared.StartSpendActionPoints(selected);
        gameObject.SetActive(true);
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
            MenuControl.Right or MenuControl.Down => () => _characterSelectionMode.HandleInput(CharacterSelectionControl.Next),
            MenuControl.Left or MenuControl.Up => () => _characterSelectionMode.HandleInput(CharacterSelectionControl.Previous),
            MenuControl.Select => () => _characterSelectionMode.HandleInput(CharacterSelectionControl.Select),
            _ => () => { }
        };
        action.Invoke();
    }
}