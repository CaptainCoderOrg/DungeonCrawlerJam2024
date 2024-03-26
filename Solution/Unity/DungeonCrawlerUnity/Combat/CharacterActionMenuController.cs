using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Game.Unity;
using CaptainCoder.Dungeoneering.Player.Unity;

using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class CharacterActionMenuController : MonoBehaviour
{
    public static CharacterActionMenuController Shared { get; private set; } = default!;
    [SerializeField]
    public MenuItemMapping<CharacterAction>[] MenuItems = [];
    [SerializeField]
    public PlayerInputMapping PlayerInputMapping = default!;
    [SerializeField]
    public GameObject Menu = default!;

    private int _selectedIx = 0;
    private CharacterCard _card = default!;
    public CharacterActionMenuController() { Shared = this; }

    public void OnEnable()
    {
        SpendActionPointsModeController.Shared.gameObject.SetActive(false);
        CharacterMoveController.Shared.gameObject.SetActive(false);
        Menu.SetActive(true);
    }

    public void OnDisable()
    {
        Menu.SetActive(false);
        CombatHelpPanel.Shared.gameObject.SetActive(false);
    }

    public void Update()
    {
        PlayerInputMapping.OnMenuAction(HandleMenuAction);
    }

    private void HandleMenuAction(MenuControl control)
    {
        Action action = control switch
        {
            MenuControl.Down or MenuControl.Right => () => Next(1),
            MenuControl.Up or MenuControl.Left => () => Next(-1),
            MenuControl.Select => () => SelectOption(MenuItems[_selectedIx].Item),
            _ => () => { }
            ,
        };
        action.Invoke();
    }

    public void Initialize(PlayerCharacter character)
    {
        _card = character.Card;
        CombatMapController.Shared.CombatMap.UpdateCharacter(character);
        gameObject.SetActive(true);
        Select(0);
    }

    private void Next(int delta)
    {
        _selectedIx += delta;
        if (_selectedIx < 0) { _selectedIx = MenuItems.Length - 1; }
        else if (_selectedIx >= MenuItems.Length) { _selectedIx = 0; }
        Select(_selectedIx);
    }

    private void SelectOption(CharacterAction action)
    {
        Action toPerform = action switch
        {
            CharacterAction.ToggleHelp => CombatHelpPanel.Shared.Toggle,
            CharacterAction.Exert => () => TryExert(_card),
            CharacterAction.Move => () => CharacterMoveController.Shared.Initialize(_card),
            _ => () => Debug.Log($"Not implemented: {action}"),
        };
        toPerform.Invoke();
    }

    private void TryExert(CharacterCard card)
    {
        Position position = CombatMapController.Shared.CombatMap.GetPosition(card);
        PlayerCharacter character = CombatMapController.Shared.CombatMap.PlayerCharacters[position];
        if (character.Energy() <= 0)
        {
            MessageRenderer.Shared.AddMessage(new Message("Not enough energy!"));
            return;
        }
        ExertAction action = new(position);
        CombatMapController.Shared.CombatMap.ApplyExertAction(action);
        MessageRenderer.Shared.AddMessage(new Message($"{card.Name} exerts 1 energy and gains 1 movement point."));
    }

    public void Select(int ix)
    {
        foreach (var item in MenuItems)
        {
            item.MenuItem.IsSelected = false;
        }
        _selectedIx = ix;
        MenuItems[ix].MenuItem.IsSelected = true;
        CombatHelpPanel.Shared.Text = MenuItems[ix].Item switch
        {
            CharacterAction.Move => HelpDialogue.CombatMove,
            CharacterAction.Exert => HelpDialogue.MoveExert,
            CharacterAction.Attack => HelpDialogue.CombatAttack,
            CharacterAction.EndTurn => HelpDialogue.CombatEndTurn,
            CharacterAction.ToggleHelp => HelpDialogue.ToggleHelpInfo,
            var x => throw new NotImplementedException($"Unknown action: {x}"),
        };
    }
}

public enum CharacterAction
{
    Move,
    Exert,
    Attack,
    EndTurn,
    ToggleHelp,
}