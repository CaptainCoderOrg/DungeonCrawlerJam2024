using CaptainCoder.Dungeoneering.Game.Unity;
using CaptainCoder.Dungeoneering.Unity;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class GuardMenuController : AbstractMenuController<GuardActions>
{
    public static GuardMenuController Shared { get; private set; } = default!;
    public GuardMenuController() { Shared = this; }
    private CanGuard[] _guards = default!;
    private Position _enemyPosition = default!;
    private CombatMap Map => CombatMapController.Shared.CombatMap;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public void Initialize(CanGuard[] guards, Position enemyPosition)
    {
        MessageRenderer.Shared.AddMessage($"Who will guard?");
        _guards = guards;
        _enemyPosition = enemyPosition;
        gameObject.SetActive(true);
    }

    protected override void SelectOption(GuardActions action)
    {
        Action toInvoke = action switch
        {
            GuardActions.Cancel => () => EnemyTurnController.Shared.Resume(new Continue()),
            var member => () => CheckMember(member.ToCard()),
        };
        toInvoke.Invoke();
    }

    private void PerformGuard(CanGuard guard)
    {
        MessageRenderer.Shared.AddMessage($"{guard.Character.Card.Name} <color=red>Guards</color>!");

        // Character returns to normal state
        PlayerCharacter updated = guard.Character with { State = CharacterState.Normal };
        Map.UpdateCharacter(updated);

        // Perform the attack
        AttackResultEvent result = Map.DoAttack(guard.Character.Card, _enemyPosition);

        // If the target is killed, resume enemy turn
        if (result.IsTargetKilledEvent())
        {
            EnemyTurnController.Shared.Resume(new EnemyIsDead());
            return;
        }

        // Remove this character from possible guards
        _guards = [.. _guards.Where(g => g != guard)];

        // If there are no more possible guards, resume
        if (_guards.Length > 0)
        {
            EnemyTurnController.Shared.Resume(new Continue());
            return;
        }

        // Otherwise, display who can guard
        foreach (CanGuard g in _guards)
        {
            MessageRenderer.Shared.AddMessage($"{guard.Character.Card.Name} can <color=red>Guard</color> the enemy.");
        }
    }

    private void CheckMember(PlayerCharacter character)
    {
        foreach (CanGuard guard in _guards.Where(g => g.Character.Card == character.Card))
        {
            PerformGuard(guard);
            return;
        }
        MessageRenderer.Shared.AddMessage($"{character.Card.Name} cannot guard this enemy.");
    }

    private void TestKill()
    {
        MessageRenderer.Shared.AddMessage($"Zooperdan kills the enemy.");
        Map.Enemies.Remove(_enemyPosition);
        CombatMapController.Shared.CharacterMap.SetTile(_enemyPosition.ToVector3Int(), null);
        EnemyTurnController.Shared.Resume(new EnemyIsDead());
    }

}

public static class GuardActionsExtensions
{
    // This is a horrible piece of code... my goodness why am I here?
    public static PlayerCharacter ToCard(this GuardActions action) => action switch
    {
        GuardActions.Zooperdan => CrawlingModeController.Shared.Party.TopLeft,
        GuardActions.Kordanor => CrawlingModeController.Shared.Party.TopRight,
        GuardActions.Ronadrok => CrawlingModeController.Shared.Party.BottomLeft,
        GuardActions.Nadrepooz => CrawlingModeController.Shared.Party.BottomRight,
        _ => throw new NotImplementedException($"Invalid action {action}."),
    };
}

public enum GuardActions
{
    Zooperdan,
    Kordanor,
    Nadrepooz,
    Ronadrok,
    Cancel,
}
