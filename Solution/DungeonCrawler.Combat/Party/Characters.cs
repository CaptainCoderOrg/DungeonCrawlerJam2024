namespace CaptainCoder.DungeonCrawler;
public static class Characters
{
    public static CharacterCard CharacterA { get; } = new()
    {
        Name = "Bob",
        BaseHealth = 5,
        BaseArmor = 2,
        BaseEnergy = 3,
        BaseSpeed = 3,
    };

    public static CharacterCard CharacterB { get; } = new()
    {
        Name = "Alice",
        BaseHealth = 4,
        BaseArmor = 0,
        BaseEnergy = 6,
        BaseSpeed = 3,
    };

    public static CharacterCard CharacterC { get; } = new()
    {
        Name = "Sally",
        BaseHealth = 3,
        BaseArmor = 1,
        BaseEnergy = 4,
        BaseSpeed = 4,
    };

    public static CharacterCard CharacterD { get; } = new()
    {
        Name = "Greg",
        BaseHealth = 2,
        BaseArmor = 1,
        BaseEnergy = 4,
        BaseSpeed = 3,
    };
}