namespace CaptainCoder.DungeonCrawler.Combat;

public record EnemyCard
{
    public string Name { get; init; } = "Unnamed Monster";
    public int Speed { get; init; } = 3;
    public int MaxHealth { get; init; } = 2;
    public int Armor { get; init; } = 0;
}
public record Enemy
{
    public static int NextId { get; private set; } = 0;
    public int Id { get; init; } = NextId++;
    public EnemyCard Card { get; init; } = new EnemyCard();
    public int Wounds { get; init; } = 0;
    public int Health => Card.MaxHealth - Wounds;
    public bool IsDead => Health <= 0;
}

public static class Enemies
{
    public static EnemyCard SkeletonCard { get; } = new() { Name = "Skeleton", Speed = 5 };
    public static EnemyCard BeastCard { get; } = new() { Name = "Beast", Speed = 3, Armor = 1, MaxHealth = 4 };
}