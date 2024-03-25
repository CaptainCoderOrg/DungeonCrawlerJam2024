using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Player.Unity;
using CaptainCoder.Dungeoneering.Unity;

using TMPro;

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
            MenuControl.Down => () => _spendActionPointsMode.HandleInput(SpendActionPointsControls.Next),
            MenuControl.Up => () => _spendActionPointsMode.HandleInput(SpendActionPointsControls.Previous),
            MenuControl.Select => () => _spendActionPointsMode.HandleInput(SpendActionPointsControls.Select),
            MenuControl.Cancel => () => _spendActionPointsMode.HandleInput(SpendActionPointsControls.Cancel),
            _ => () => { }
            ,
        };
        toPerform.Invoke();
    }

    public void Initialize(CharacterCardRenderer renderer, PlayerCharacter playerCharacter)
    {
        CrawlingModeController.CrawlerMode.AddMessage(new Message($"{playerCharacter.Card.Name} prepares for battle. Select {playerCharacter.ActionPoints} actions."));
        _spendActionPointsMode = new SpendActionPointsMode(playerCharacter);
        _characterCardRenderer = renderer;
        _spendActionPointsMode.OnSelected += HandleSpendPoint;
        _spendActionPointsMode.OnSelectionChange += HandleSelectionChange;
        _spendActionPointsMode.OnCancel += Cancel;
        HandleSelectionChange(SpendActionMenuItem.Move);
    }

    private void Cancel(PlayerCharacter character)
    {
        _characterCardRenderer.Render(character);
        CombatMapController.StartCharacterSelect();
    }

    private void HandleSelectionChange(SpendActionMenuItem item)
    {
        foreach (var menuItem in MenuItems)
        {
            menuItem.MenuItem.IsSelected = menuItem.Item == item;
        }
    }

    private void HandleSpendPoint(SpendPointResult result)
    {
        CrawlingModeController.CrawlerMode.AddMessage(new Message(result.Message));
        _characterCardRenderer.Render(result.Character);
        int count = result.Character.ActionPoints;
        if (count > 0)
        {
            string actions = result.Character.ActionPoints == 1 ? "action" : "actions";
            CrawlingModeController.CrawlerMode.AddMessage($"Select {count} more {actions}.");
        }
        else
        {
            CrawlingModeController.CrawlerMode.AddMessage($"{result.Character.Card.Name} is ready for battle.");
        }
    }
}

[Serializable]
public class MenuItemMapping<T>
{
    public T Item = default!;
    public MenuItem MenuItem = default!;
}

public class MenuItem : MonoBehaviour
{
    [field: SerializeField]
    public TextMeshProUGUI Text { get; private set; } = default!;
    public Color SelectedColor = Color.yellow;
    public Color NormalColor = Color.black;
    public bool IsSelected
    {
        set => Text.color = value ? SelectedColor : NormalColor;
    }
}