namespace CaptainCoder.DungeonCrawler.Combat;

public record CharacterCard(string Name)
{
    public int BaseHealth { get; init; } = 10;
    public int BaseEnergy { get; init; } = 4;
    public int BaseArmor { get; init; } = 0;
    public int BaseSpeed { get; init; } = 4;
    public CharacterCard() : this("Unnamed") { }
}
public record PlayerCharacter(CharacterCard Card)
{
    public int Wounds { get; init; } = 0;
    public int Exertion { get; init; } = 0;
    public CharacterState State { get; init; } = CharacterState.Normal;
    public PlayerCharacter() : this(new CharacterCard()) { }
}

public static class CharacterExtensions
{
    public static PlayerCharacter AddWounds(this PlayerCharacter pc, int wounds) => pc with { Wounds = pc.Wounds + wounds };
    public static int Health(this PlayerCharacter pc) => pc.Card.BaseHealth - pc.Wounds;
    public static bool IsDead(this PlayerCharacter pc) => pc.Health() <= 0;
    public static PlayerCharacter AddExertion(this PlayerCharacter pc, int exertion) => pc with { Exertion = pc.Exertion + exertion };
    public static int Energy(this PlayerCharacter pc) => pc.Card.BaseEnergy - pc.Exertion;
}

public enum CharacterState
{
    Normal,
    Guard,
    Rest
}