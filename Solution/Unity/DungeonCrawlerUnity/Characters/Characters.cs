namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public static class Characters
{
    public static CharacterCard CharacterA { get; } = new()
    {
        Name = "Bob",
        BaseHealth = 12,
        BaseArmor = 2,
        BaseEnergy = 3,
        BaseSpeed = 3,
    };

    public static CharacterCard CharacterB { get; } = new()
    {
        Name = "Alice",
        BaseHealth = 8,
        BaseArmor = 0,
        BaseEnergy = 6,
        BaseSpeed = 3,
    };

    public static CharacterCard CharacterC { get; } = new()
    {
        Name = "Sally",
        BaseHealth = 10,
        BaseArmor = 1,
        BaseEnergy = 4,
        BaseSpeed = 4,
    };

    public static CharacterCard CharacterD { get; } = new()
    {
        Name = "Greg",
        BaseHealth = 14,
        BaseArmor = 1,
        BaseEnergy = 4,
        BaseSpeed = 3,
    };
}