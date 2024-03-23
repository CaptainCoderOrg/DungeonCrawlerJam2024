namespace CaptainCoder.DungeonCrawler.Combat;

public record CharacterCard(string Name, int BaseHealth, int BaseEnergy, int BaseArmor, int BaseSpeed);
public record PlayerCharacter(CharacterCard Card, int Wounds, int Exertion);

public static class CharacterExtensions
{
    public static PlayerCharacter AddWounds(this PlayerCharacter pc, int wounds) => pc with { Wounds = pc.Wounds + wounds };
    public static int Health(this PlayerCharacter pc) => pc.Card.BaseHealth - pc.Wounds;
    public static bool IsDead(this PlayerCharacter pc) => pc.Health() <= 0;
}