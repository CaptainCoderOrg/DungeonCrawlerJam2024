using CaptainCoder.DungeonCrawler.Combat;

using Shouldly;

namespace Tests;
public class CombatAttackExtensions_should
{
    [Theory]
    [InlineData(2, 2, 1, 2, 2, 1)]
    [InlineData(2, 4, 0, 3, 2, 2)]
    [InlineData(4, 2, 4, 4, 6, 2)]
    [InlineData(1, 2, 2, 5, 5, 3)]
    public void apply_character_attack_does_damage(int x, int y, int targetArmor, int targetHealth, int damage, int expectedDamage)
    {
        Position position = new(x, y);
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

}