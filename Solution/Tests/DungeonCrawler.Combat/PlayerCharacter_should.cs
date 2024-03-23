namespace Tests;

using CaptainCoder.DungeonCrawler.Combat;

using Shouldly;

public class PlayerCharacter_should
{
    public static readonly CharacterCard DefaultCharacter = new("Default", 10, 4, 1, 4);
    public static readonly PlayerCharacter DefaultPlayerCharacter = new(DefaultCharacter, 0, 0);

    [Theory]
    [InlineData(10, 4, 6)]
    [InlineData(12, 2, 10)]
    [InlineData(7, 3, 4)]
    public void add_wounds(int baseHealth, int woundsToAdd, int expectedHealth)
    {
        CharacterCard card = DefaultCharacter with { BaseHealth = baseHealth };
        PlayerCharacter playerCharacter = new(card, 0, 0);

        PlayerCharacter actual = playerCharacter.AddWounds(woundsToAdd);
        actual.Health().ShouldBe(expectedHealth);
    }
}