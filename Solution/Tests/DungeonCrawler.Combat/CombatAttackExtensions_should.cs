using CaptainCoder.DungeonCrawler.Combat;

using Shouldly;

namespace Tests;
public class CombatAttackExtensions_should
{
    [Theory]
    [InlineData(1, 2, 2, 1)]
    [InlineData(0, 3, 2, 2)]
    [InlineData(4, 4, 6, 2)]
    [InlineData(2, 5, 5, 3)]
    public void apply_character_attack_does_damage(int targetArmor, int targetHealth, int damage, int expectedDamage)
    {
        Position position = new(Random.Shared.Next(10), Random.Shared.Next(10));
        Enemy enemy = new() { Card = new() { Name = "Skeleton", Armor = targetArmor, MaxHealth = targetHealth } };
        CombatMap map = new()
        {
            Enemies = new Dictionary<Position, Enemy>()
            {
                { position, enemy }
            }
        };

        AttackResult attackResult = new() { Damage = damage };

        // Apply the attack
        AttackResultEvent actual = map.ApplyCharacterAttack(position, attackResult);

        // Test result
        actual.ShouldBe(new AttackHitEvent("Skeleton", expectedDamage));
        // Test map state changed
        map.Enemies[position].Wounds.ShouldBe(expectedDamage);
    }

    [Theory]
    [InlineData(1, 0, 1)]
    [InlineData(1, 1, 2)]
    [InlineData(4, 4, 10)]
    [InlineData(6, 2, 8)]
    public void apply_character_attack_kills_enemy(int targetHealth, int targetArmor, int damage)
    {
        Position position = new(Random.Shared.Next(10), Random.Shared.Next(10));
        Enemy enemy = new() { Card = new() { Name = "Skeleton", Armor = targetArmor, MaxHealth = targetHealth } };
        CombatMap map = new()
        {
            Enemies = new Dictionary<Position, Enemy>()
            {
                { position, enemy }
            }
        };

        AttackResult attackResult = new() { Damage = damage };

        // Apply the attack
        AttackResultEvent actual = map.ApplyCharacterAttack(position, attackResult);

        // Test result
        actual.ShouldBe(new TargetKilledEvent("Skeleton"));
        // Test map state changed
        map.Enemies.ShouldNotContainKey(position);
    }

}