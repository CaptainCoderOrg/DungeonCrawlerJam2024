namespace CaptainCoder.DungeonCrawler.Combat;

public static class CombatAttackExtensions
{
    public static AttackResultEvent ApplyCharacterAttack(this CombatMap map, Position target, AttackResult attack)
    {
        if (!map.Enemies.TryGetValue(target, out Enemy enemy)) { return new NoEnemy(); }
        int damage = Math.Max(0, attack.Damage - enemy.Card.Armor);
        if (damage == 0) { return new ArmorAbsorbedHitEvent(enemy.Card.Name); }
        Enemy updated = enemy with { Wounds = enemy.Wounds + damage };
        if (updated.IsDead)
        {
            map.Enemies.Remove(target);
            return new TargetKilledEvent(updated.Card.Name);
        }
        map.Enemies[target] = updated;
        return new AttackHitEvent(updated.Card.Name, damage);
    }
}

public record AttackResult
{
    public int Damage { get; init; } = 0;

}

public abstract record AttackResultEvent;

public record AttackHitEvent(string TargetName, int Damage) : AttackResultEvent;
public record TargetKilledEvent(string TargetName) : AttackResultEvent;
public record ArmorAbsorbedHitEvent(string TargetName) : AttackResultEvent;
public record NoEnemy : AttackResultEvent;