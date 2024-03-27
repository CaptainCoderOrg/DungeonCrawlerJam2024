namespace CaptainCoder.DungeonCrawler.Combat;
public record AttackResult
{
    public int Damage { get; init; } = 0;
}

public abstract record AttackResultEvent;
public record AttackHitEvent(string TargetName, int Damage) : AttackResultEvent;
public record TargetKilledEvent(string TargetName) : AttackResultEvent;
public record ArmorAbsorbedHitEvent(string TargetName) : AttackResultEvent;
public record EmptyTarget : AttackResultEvent;