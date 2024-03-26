using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Game.Unity;
using CaptainCoder.Dungeoneering.Player.Unity;

using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;
public class CharacterMoveController : MonoBehaviour
{
    public static CharacterMoveController Shared { get; private set; } = default!;
    public CharacterMoveController() { Shared = this; }
    private CharacterCard? _card = default!;
    private CharacterCard Card => _card ?? throw new Exception($"Card was not initialized");
    private Position _cursorPosition = default;
    private Position _startPosition = default!;
    private CombatMap CombatMap => CombatMapController.Shared.CombatMap;

    public void OnEnable()
    {
        CharacterActionMenuController.Shared.gameObject.SetActive(false);
    }

    public void Update()
    {
        PlayerInputHandler.Shared.InputMapping.OnMenuAction(HandleInput);
    }

    private void HandleInput(MenuControl action)
    {
        Action toPerform = action switch
        {
            MenuControl.Up => () => MoveCursor(_cursorPosition with { Y = _cursorPosition.Y - 1 }),
            MenuControl.Down => () => MoveCursor(_cursorPosition with { Y = _cursorPosition.Y + 1 }),
            MenuControl.Left => () => MoveCursor(_cursorPosition with { X = _cursorPosition.X - 1 }),
            MenuControl.Right => () => MoveCursor(_cursorPosition with { X = _cursorPosition.X + 1 }),
            MenuControl.Cancel => Cancel,
            _ => () => { }
            ,
        };
        toPerform.Invoke();
    }

    public void Cancel()
    {
        var map = CombatMapController.Shared.CombatMap;
        PlayerCharacter character = map.PlayerCharacters[map.GetPosition(Card)];
        _card = null;

        CombatMapController.Shared.HighlightTiles([]);
        CombatMapController.Shared.SelectCharacter(character);

        CharacterActionMenuController.Shared.Initialize(character);
    }

    public void MoveCursor(Position newPosition)
    {
        _cursorPosition = newPosition;
        CombatMapController.Shared.SelectTiles(_cursorPosition);
        CombatMapController.Shared.PanToward(_cursorPosition);
    }

    public void Initialize(CharacterCard card)
    {
        _card = card;
        PlayerCharacter character = CombatMap.GetCharacter(card);
        MessageRenderer.Shared.AddMessage(new Message($"{card.Name} has {character.MovementPoints} movement points."));
        string keys = string.Join(" or ", PlayerInputHandler.Shared.InputMapping.MenuActionMappings.Where(mapping => mapping.Action is MenuControl.Cancel).Select(m => m.Key.ToString()));
        MessageRenderer.Shared.AddMessage(new Message($"Press {keys} to cancel."));
        _startPosition = CombatMap.GetPosition(character.Card);
        _cursorPosition = _startPosition;
        HashSet<Position> validMoves = CombatMap.FindValidMoves(_startPosition, character.MovementPoints);
        CombatMapController.Shared.HighlightTiles(validMoves, CombatMapController.Shared.IconDatabase.Green);

        gameObject.SetActive(true);
    }
}