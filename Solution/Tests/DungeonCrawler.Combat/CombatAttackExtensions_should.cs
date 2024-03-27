using CaptainCoder.DungeonCrawler;
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
        AttackResultEvent actual = map.ApplyAttack(position, attackResult);

        // Test result
        actual.ShouldBe(new AttackHitEvent("Skeleton", expectedDamage, targetArmor));
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
        AttackResultEvent actual = map.ApplyAttack(position, attackResult);

        // Test result
        actual.ShouldBe(new TargetKilledEvent("Skeleton"));
        // Test map state changed
        map.Enemies.ShouldNotContainKey(position);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(4, 2)]
    [InlineData(4, 3)]
    [InlineData(6, 6)]
    public void report_armor_absorbed_attack_against_enemy(int targetArmor, int damage)
    {
        Position position = new(Random.Shared.Next(10), Random.Shared.Next(10));
        Enemy enemy = new() { Card = new() { Name = "Skeleton", Armor = targetArmor, MaxHealth = 4 } };
        CombatMap map = new()
        {
            Enemies = new Dictionary<Position, Enemy>()
            {
                { position, enemy }
            }
        };

        AttackResult attackResult = new() { Damage = damage };

        // Apply the attack
        AttackResultEvent actual = map.ApplyAttack(position, attackResult);

        // Test result
        actual.ShouldBe(new ArmorAbsorbedHitEvent("Skeleton"));
        // Test no damage was taken
        map.Enemies[position].Wounds.ShouldBe(0);
    }

    [Fact]
    public void report_empty_target()
    {
        Position position = new(Random.Shared.Next(10), Random.Shared.Next(10));
        CombatMap map = new()
        {
            Enemies = new Dictionary<Position, Enemy>() { },
        };

        AttackResult attackResult = new() { Damage = Random.Shared.Next(5) };

        // Apply the attack
        AttackResultEvent actual = map.ApplyAttack(position, attackResult);

        // Test result
        actual.ShouldBe(new EmptyTarget());
    }

    [Theory]
    [InlineData(1, 2, 2, 1)]
    [InlineData(0, 3, 2, 2)]
    [InlineData(4, 4, 6, 2)]
    [InlineData(2, 5, 5, 3)]
    public void apply_attack_does_damage_to_character(int targetArmor, int targetHealth, int damage, int expectedDamage)
    {
        Position position = new(Random.Shared.Next(10), Random.Shared.Next(10));
        PlayerCharacter character = new() { Card = new() { Name = "Bob", BaseArmor = targetArmor, BaseHealth = targetHealth } };
        CombatMap map = new()
        {
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { position, character }
            }
        };

        AttackResult attackResult = new() { Damage = damage };

        // Apply the attack
        AttackResultEvent actual = map.ApplyAttack(position, attackResult);

        // Test result
        actual.ShouldBe(new AttackHitEvent("Bob", expectedDamage, targetArmor));
        // Test map state changed
        map.PlayerCharacters[position].Wounds.ShouldBe(expectedDamage);
    }

    [Theory]
    [InlineData(1, 0, 1)]
    [InlineData(1, 1, 2)]
    [InlineData(4, 4, 10)]
    [InlineData(6, 2, 8)]
    public void apply_attack_kills_character(int targetHealth, int targetArmor, int damage)
    {
        Position position = new(Random.Shared.Next(10), Random.Shared.Next(10));
        PlayerCharacter character = new() { Card = new() { Name = "Bob", BaseArmor = targetArmor, BaseHealth = targetHealth } };
        CombatMap map = new()
        {
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { position, character }
            }
        };

        AttackResult attackResult = new() { Damage = damage };

        // Apply the attack
        AttackResultEvent actual = map.ApplyAttack(position, attackResult);

        // Test result
        actual.ShouldBe(new TargetKilledEvent("Bob"));
        // Test map state changed
        map.PlayerCharacters.ShouldNotContainKey(position);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(4, 2)]
    [InlineData(4, 3)]
    [InlineData(6, 6)]
    public void report_character_armor_absorbed_attack(int targetArmor, int damage)
    {
        Position position = new(Random.Shared.Next(10), Random.Shared.Next(10));
        PlayerCharacter character = new() { Card = new() { Name = "Bob", BaseArmor = targetArmor, BaseHealth = 4 } };
        CombatMap map = new()
        {
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { position, character }
            }
        };

        AttackResult attackResult = new() { Damage = damage };

        // Apply the attack
        AttackResultEvent actual = map.ApplyAttack(position, attackResult);

        // Test result
        actual.ShouldBe(new ArmorAbsorbedHitEvent("Bob"));
        // Test no damage was taken
        map.PlayerCharacters[position].Wounds.ShouldBe(0);
    }

    [Theory]
    [InlineData(
        """
        ####
        ####
        ####
        """,
        """
        ###2
        #1##
        A#B#
        """,
        (char[])['1'])
    ]
    [InlineData(
        """
        ####
        ####
        ####
        """,
        """
        ####
        B1##
        A2##
        """,
        (char[])['1', '2'])
    ]
    [InlineData(
        """
        ####
        ####
        ####
        """,
        """
        ###2
        ##1#
        AB##
        """,
        (char[])[])
    ]
    public void get_valid_player_attack_targets(string map, string setup, char[] expectedTargets)
    {
        Dictionary<char, HashSet<Position>> entityLookup = CombatMapExtensions.ParseCharPositions(setup);
        Position characterPosition = entityLookup['A'].First();
        CombatMap combatMap = new()
        {
            Tiles = CombatMapExtensions.ParseTiles(map),
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { characterPosition, new PlayerCharacter() },
                { entityLookup['B'].First(), new PlayerCharacter() }
            },
            Enemies = new Dictionary<Position, Enemy>()
            {
                { entityLookup['1'].First(), new Enemy() },
                { entityLookup['2'].First(), new Enemy() },
            }
        };

        HashSet<Position> actualTargets = [.. combatMap.GetValidAttackTargets(characterPosition)];

        HashSet<Position> expectedPositions = [.. expectedTargets.Select(ch => entityLookup[ch].First())];

        bool result = actualTargets.SetEquals(expectedPositions);
        result.ShouldBeTrue($"Expected targets to be\n\n{string.Join(", ", expectedPositions)}\n\nbut was\n\n {string.Join(", ", actualTargets)}");
    }

    [Theory]
    [InlineData(
        """
        ####
        ####
        ####
        """,
        """
        ###2
        #1##
        A#B#
        """,
        (char[])['A', 'B'])
    ]
    [InlineData(
        """
        ####
        ####
        ####
        """,
        """
        ####
        B1##
        A2##
        """,
        (char[])['A', 'B'])
    ]
    [InlineData(
        """
        ####
        ####
        ####
        """,
        """
        ###2
        ##1#
        AB##
        """,
        (char[])['B'])
    ]
    [InlineData(
        """
        ####
        ####
        ####
        """,
        """
        ###2
        ###1
        AB##
        """,
        (char[])[])
    ]
    public void get_valid_enemy_attack_targets(string map, string setup, char[] expectedTargets)
    {
        Dictionary<char, HashSet<Position>> entityLookup = CombatMapExtensions.ParseCharPositions(setup);
        Position enemyPosition = entityLookup['1'].First();
        CombatMap combatMap = new()
        {
            Tiles = CombatMapExtensions.ParseTiles(map),
            PlayerCharacters = new Dictionary<Position, PlayerCharacter>()
            {
                { entityLookup['A'].First(), new PlayerCharacter() },
                { entityLookup['B'].First(), new PlayerCharacter() }
            },
            Enemies = new Dictionary<Position, Enemy>()
            {
                { enemyPosition, new Enemy() },
                { entityLookup['2'].First(), new Enemy() },
            }
        };

        HashSet<Position> actualTargets = [.. combatMap.GetValidAttackTargets(enemyPosition)];

        HashSet<Position> expectedPositions = [.. expectedTargets.Select(ch => entityLookup[ch].First())];

        bool result = actualTargets.SetEquals(expectedPositions);
        result.ShouldBeTrue($"Expected targets to be\n\n{string.Join(", ", expectedPositions)}\n\nbut was\n\n {string.Join(", ", actualTargets)}");
    }

}