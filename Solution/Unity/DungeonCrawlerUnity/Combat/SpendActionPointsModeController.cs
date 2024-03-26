using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Player.Unity;
using CaptainCoder.Dungeoneering.Unity;


using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class SpendActionPointsModeController : MonoBehaviour
{
    private SpendActionPointsMode _spendActionPointsMode = default!;
    private CharacterCardRenderer _characterCardRenderer = default!;
    [field: SerializeField]
    public PlayerInputMapping PlayerInputMapping { get; private set; } = default!;
    [field: SerializeField]
    public MenuItemMapping<SpendActionMenuItem>[] MenuItems = [];
    [field: SerializeField]
    public CombatMapController CombatMapController { get; private set; } = default!;
    [field: SerializeField]
    public GameObject Menu { get; private set; } = default!;
    public ConfirmDialogue ConfirmDialogue = default!;
    private readonly List<SpendActionMenuItem> _actionsSelected = [];
    public void Update()
    {
        foreach (var mapping in PlayerInputMapping.MenuActionMappings)
        {
            if (Input.GetKeyDown(mapping.Key))
            {
                HandleUserInput(mapping.Action);
            }
        }
    }

    public void OnEnable()
    {
        Menu.SetActive(true);
    }

    public void OnDisable()
    {
        Menu.SetActive(false);
    }

    private void HandleUserInput(MenuControl action)
    {
        Action toPerform = action switch
        {
            MenuControl.Down or MenuControl.Right => () => _spendActionPointsMode.HandleInput(SpendActionPointsControls.Next),
            MenuControl.Up or MenuControl.Left => () => _spendActionPointsMode.HandleInput(SpendActionPointsControls.Previous),
            MenuControl.Select => () => _spendActionPointsMode.HandleInput(SpendActionPointsControls.Select),
            MenuControl.Cancel => () => _spendActionPointsMode.HandleInput(SpendActionPointsControls.Cancel),
            _ => () => { }
            ,
        };
        toPerform.Invoke();
    }

    public void Initialize(CharacterCardRenderer renderer, PlayerCharacter playerCharacter)
    {
        if (CombatHelpPanel.Shared.IsOn) { CombatHelpPanel.Shared.gameObject.SetActive(true); }
        _actionsSelected.Clear();
        CrawlingModeController.CrawlerMode.AddMessage(new Message($"{playerCharacter.Card.Name} prepares for battle. Select {playerCharacter.ActionPoints} actions."));
        _spendActionPointsMode = new SpendActionPointsMode(playerCharacter);
        _characterCardRenderer = renderer;
        _spendActionPointsMode.OnSelected += HandleSpendPoint;
        _spendActionPointsMode.OnSelectionChange += HandleSelectionChange;
        _spendActionPointsMode.OnCancel += Cancel;
        _spendActionPointsMode.OnToggleHelp += () => CombatHelpPanel.Shared.IsOn = !CombatHelpPanel.Shared.IsOn;
        HandleSelectionChange(SpendActionMenuItem.Move);
    }

    private void Cancel(PlayerCharacter character)
    {
        CombatHelpPanel.Shared.gameObject.SetActive(false);
        _characterCardRenderer.Render(character);
        CombatMapController.StartCharacterSelect();
    }

    private void HandleSelectionChange(SpendActionMenuItem item)
    {
        foreach (var menuItem in MenuItems)
        {
            menuItem.MenuItem.IsSelected = menuItem.Item == item;
        }
        CombatHelpPanel.Shared.Text = item switch
        {
            SpendActionMenuItem.Move => HelpDialogue.Move,
            SpendActionMenuItem.Rest => HelpDialogue.Rest,
            SpendActionMenuItem.Attack => HelpDialogue.Attack,
            SpendActionMenuItem.Guard => HelpDialogue.GuardState,
            SpendActionMenuItem.Cancel => HelpDialogue.CancelActionSpending,
            SpendActionMenuItem.ToggleHelp => HelpDialogue.ToggleHelpInfo,
            _ => throw new NotImplementedException($"Unknown action: {item}"),
        };
    }

    private void HandleSpendPoint(SpendPointResult result)
    {
        CrawlingModeController.CrawlerMode.AddMessage(new Message(result.Message));
        _characterCardRenderer.Render(result.Character);
        _actionsSelected.Add(result.SelectedAction);
        int count = result.Character.ActionPoints;
        if (count > 0)
        {
            string actions = result.Character.ActionPoints == 1 ? "action" : "actions";
            CrawlingModeController.CrawlerMode.AddMessage($"Select {count} more {actions}.");
        }
        else
        {
            CrawlingModeController.CrawlerMode.AddMessage($"{result.Character.Card.Name} is ready for battle.");
            string message = string.Join("/", _actionsSelected);
            ConfirmDialogue.Initialize(message, () => { Debug.Log("Not implemented. Show character perform action mode."); }, () => CombatMapController.StartCharacterSelect());
            gameObject.SetActive(false);
            CombatHelpPanel.Shared.gameObject.SetActive(false);
        }
    }
}