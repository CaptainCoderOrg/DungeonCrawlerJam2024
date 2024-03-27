namespace CaptainCoder.DungeonCrawler.Combat;

public static class CombatAttackExtensions
{
    public static AttackResultEvent ApplyAttack(this CombatMap map, Position target, AttackResult attack)
    {
        if (map.Enemies.ContainsKey(target)) { return map.ApplyAttackOnEnemy(target, attack); }
        if (map.PlayerCharacters.ContainsKey(target)) { return map.ApplyAttackOnCharacter(target, attack); }
        return new EmptyTarget();
    }

    private static AttackResultEvent ApplyAttackOnCharacter(this CombatMap map, Position target, AttackResult attack)
    {
        PlayerCharacter character = map.PlayerCharacters[target];
        int damage = Math.Max(0, attack.Damage - character.Card.BaseArmor);
        if (damage == 0) { return new ArmorAbsorbedHitEvent(character.Card.Name); }
        PlayerCharacter updated = character with { Wounds = character.Wounds + damage };
        if (updated.IsDead())
        {
            map.PlayerCharacters.Remove(target);
            return new TargetKilledEvent(updated.Card.Name);
        }
        map.PlayerCharacters[target] = updated;
        return new AttackHitEvent(updated.Card.Name, damage, character.Card.BaseArmor);
    }

    private static AttackResultEvent ApplyAttackOnEnemy(this CombatMap map, Position target, AttackResult attack)
    {
        Enemy enemy = map.Enemies[target];
        int damage = Math.Max(0, attack.Damage - enemy.Card.Armor);
        if (damage == 0) { return new ArmorAbsorbedHitEvent(enemy.Card.Name); }
        Enemy updated = enemy with { Wounds = enemy.Wounds + damage };
        if (updated.IsDead)
        {
            map.Enemies.Remove(target);
            return new TargetKilledEvent(updated.Card.Name);
        }
        map.Enemies[target] = updated;
        return new AttackHitEvent(updated.Card.Name, damage, enemy.Card.Armor);
    }
}