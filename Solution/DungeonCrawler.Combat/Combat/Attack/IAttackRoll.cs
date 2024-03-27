namespace CaptainCoder.DungeonCrawler.Combat;

public interface IAttackRoll
{
    public AttackResult GetRoll(IRandom randomSource);
}


public record SimpleAttack(int Min, int Max) : IAttackRoll
{
    public AttackResult GetRoll(IRandom rng) => new() { Damage = rng.Next(Min, Max) };
}