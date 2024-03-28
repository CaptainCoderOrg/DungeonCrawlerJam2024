namespace CaptainCoder.DungeonCrawler.Combat;

public static class CombatAttackExtensions
{
    public static IEnumerable<Position> GetValidAttackTargets(this CombatMap map, Position target)
    {
        if (map.PlayerCharacters.ContainsKey(target)) { return target.Neighbors().Where(map.Enemies.ContainsKey); }
        if (map.Enemies.ContainsKey(target)) { return target.Neighbors().Where(map.PlayerCharacters.ContainsKey); }
        throw new Exception($"No attacker in position {target}.");
    }

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
        PlayerCharacter updated = character with { Wounds = character.Wounds + damage, State = CharacterState.Normal };
        if (updated.IsDead())
        {
            map.UpdateCharacter(updated);
            map.PlayerCharacters.Remove(target);
            return new TargetKilledEvent(updated.Card.Name);
        }
        List<AttackResultEvent> events = [];
        if (character.State is CharacterState.Guard) { events.Add(new LostGuardEvent(character.Card.Name)); }
        if (character.State is CharacterState.Rest) { events.Add(new LostRestEvent(character.Card.Name)); }
        map.UpdateCharacter(updated);
        events.Add(new AttackHitEvent(updated.Card.Name, damage, character.Card.BaseArmor));
        return new AttackResultEvents(events);
    }

    private static AttackResultEvent ApplyAttackOnEnemy(this CombatMap map, Position target, AttackResult attack)
    {
        Enemy enemy = map.Enemies[target];
        int damage = Math.Max(0, attack.Damage - enemy.Card.Armor);
        if (damage == 0) { return new ArmorAbsorbedHitEvent(enemy.Card.Name); }
        Enemy updated = enemy with { Wounds = enemy.Wounds + damage };
        if (updated.IsDead)
        {
            map.RemoveEnemy(target);
            return new TargetKilledEvent(updated.Card.Name);
        }
        map.Enemies[target] = updated;
        return new AttackHitEvent(updated.Card.Name, damage, enemy.Card.Armor);
    }
}