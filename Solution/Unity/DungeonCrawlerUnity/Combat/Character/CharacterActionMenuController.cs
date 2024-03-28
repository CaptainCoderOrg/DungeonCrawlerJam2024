using CaptainCoder.Dungeoneering.Game;
using CaptainCoder.Dungeoneering.Game.Unity;

using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class CharacterActionMenuController : AbstractMenuController<CharacterAction>
{
    public static CharacterActionMenuController Shared { get; private set; } = default!;
    public CharacterActionMenuController() { Shared = this; }
    private CharacterCard _card = default!;
    private CombatMap CombatMap => CombatMapController.Shared.CombatMap;

    public override void OnEnable()
    {
        SpendActionPointsModeController.Shared.gameObject.SetActive(false);
        CharacterMoveController.Shared.gameObject.SetActive(false);
        Menu.SetActive(true);
    }

    public override void OnDisable()
    {
        Menu.SetActive(false);
        CombatHelpPanel.Shared.gameObject.SetActive(false);
    }

    public void Initialize(CharacterCard card)
    {
        PlayerCharacter character = CombatMap.PlayerCharacters[CombatMap.GetPosition(card)];
        CombatMapController.Shared.SelectCharacter(character);
        _card = card;
        gameObject.SetActive(true);
        Select(0);
    }

    protected override void SelectOption(CharacterAction action)
    {
        Action toPerform = action switch
        {
            CharacterAction.ToggleHelp => CombatHelpPanel.Shared.Toggle,
            CharacterAction.Exert => () => TryExert(_card),
            CharacterAction.Move => () => CharacterMoveController.Shared.Initialize(_card),
            CharacterAction.EndTurn => () => EndTurn(_card),
            CharacterAction.Attack => () => CharacterAttackController.Shared.Initialize(_card),
            _ => () => Debug.Log($"Not implemented: {action}"),
        };
        toPerform.Invoke();
    }

    private void EndTurn(CharacterCard card)
    {
        gameObject.SetActive(false);
        ConfirmDialogue.Shared.Initialize("End Turn", OnConfirm, OnCancel);
        void OnConfirm()
        {
            Position position = CombatMapController.Shared.CombatMap.GetPosition(card);
            EndTurnAction action = new(position);
            CombatMap.ApplyEndCharacterTurn(action);
            MessageRenderer.Shared.AddMessage(new Message($"{card.Name} ends their turn."));
            CharacterSelectionModeController.Shared.Initialize();
        }
        void OnCancel() { gameObject.SetActive(true); }
    }

    private static void TryExert(CharacterCard card)
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
}

public enum CharacterAction
{
    Move,
    Exert,
    Attack,
    EndTurn,
    ToggleHelp,
}