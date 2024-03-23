namespace Tests;

using CaptainCoder.DungeonCrawler.Combat;

using Shouldly;

public class PlayerCharacter_should
{

    [Theory]
    [InlineData(10, 4, 6)]
    [InlineData(12, 2, 10)]
    [InlineData(7, 3, 4)]
    public void add_wounds(int baseHealth, int woundsToAdd, int expectedHealth)
    {
        CharacterCard card = new() { BaseHealth = baseHealth };
        PlayerCharacter playerCharacter = new() { Card = card };

        PlayerCharacter actual = playerCharacter.AddWounds(woundsToAdd);
        actual.Health().ShouldBe(expectedHealth);
    }

    [Theory]
    [InlineData(4, 1, 3)]
    [InlineData(3, 3, 0)]
    [InlineData(7, 5, 2)]
    public void add_exertion(int baseEnergy, int exertionToAdd, int expectedEnergy)
    {
        CharacterCard card = new() { BaseEnergy = baseEnergy };
        PlayerCharacter playerCharacter = new() { Card = card };

        PlayerCharacter actual = playerCharacter.AddExertion(exertionToAdd);
        actual.Energy().ShouldBe(expectedEnergy);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(8)]
    [InlineData(9)]
    public void be_dead(int baseHealth)
    {
        CharacterCard card = new() { BaseHealth = baseHealth };
        PlayerCharacter underTest = new() { Card = card, Wounds = baseHealth };
        underTest.IsDead().ShouldBeTrue();
    }

    [Theory]
    [InlineData(5, 4)]
    [InlineData(8, 0)]
    [InlineData(9, 2)]
    public void not_be_dead(int baseHealth, int wounds)
    {
        CharacterCard card = new() { BaseHealth = baseHealth };
        PlayerCharacter underTest = new() { Card = card, Wounds = wounds };
        underTest.IsDead().ShouldBeFalse();
    }
}