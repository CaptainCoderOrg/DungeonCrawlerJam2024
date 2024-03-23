namespace CaptainCoder.DungeonCrawler.Combat;

public static class CombatController
{
    public static bool IsValidAction(this CombatMap map, CombatAction toValidate) => throw new NotImplementedException();
    public static bool IsValidMoveAction(this CombatMap map, MoveAction moveAction) => throw new NotImplementedException();
    public static void ApplyAction(this CombatMap map, CombatAction toApply) => throw new NotImplementedException();
    public static void ApplyMoveAction(this CombatMap map, MoveAction moveAction) => throw new NotImplementedException();
}

public record struct Position(int X, int Y);
public abstract record CombatAction;
public record MoveAction(PlayerCharacter Character, Position Target) : CombatAction;
public record Exert : CombatAction;
public record Attack : CombatAction;
public record EndCharacterTurn : CombatAction;