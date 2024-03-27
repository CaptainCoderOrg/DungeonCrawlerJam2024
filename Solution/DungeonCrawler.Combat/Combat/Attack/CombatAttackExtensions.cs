namespace CaptainCoder.DungeonCrawler.Combat;

public static class CombatAttackExtensions
{
    public static AttackResultEvent ApplyCharacterAttack(this CombatMap map, Position target, AttackResult attack)
    {
        if (!map.Enemies.TryGetValue(target, out Enemy enemy)) { return new EmptyTarget(); }
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