namespace CaptainCoder.DungeonCrawler;
public static class Characters
{
    public static CharacterCard CharacterA { get; } = new()
    {
        Name = "Zooperdan",
        BaseHealth = 5,
        BaseArmor = 0,
        BaseEnergy = 2,
        BaseSpeed = 3,
    };

    public static CharacterCard NoBody { get; } = new CharacterCard() { Name = "NoBody" };

    public static CharacterCard CharacterB { get; } = new()
    {
        Name = "Kordanor",
        BaseHealth = 8,
        BaseArmor = 1,
        BaseEnergy = 3,
        BaseSpeed = 4,
    };

    public static CharacterCard CharacterC { get; } = new()
    {
        Name = "Ronadrok",
        BaseHealth = 10,
        BaseArmor = 1,
        BaseEnergy = 4,
        BaseSpeed = 5,
    };

    public static CharacterCard CharacterD { get; } = new()
    {
        Name = "Nadrepooz",
        BaseHealth = 12,
        BaseArmor = 3,
        BaseEnergy = 3,
        BaseSpeed = 2,
    };
}