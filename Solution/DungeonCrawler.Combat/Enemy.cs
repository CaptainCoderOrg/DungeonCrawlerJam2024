namespace CaptainCoder.DungeonCrawler.Combat;

public record EnemyCard
{
    public string Name { get; init; } = "Unnamed Monster";
    public int Speed { get; init; } = 3;
    public int MaxHealth { get; init; } = 2;
    public int Armor { get; init; } = 0;
    public IAttackRoll AttackRoll { get; init; } = new SimpleAttack(1, 2);
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
    public static EnemyCard SkeletonCard { get; } = new()
    {
        Name = "Skeleton",
        Speed = 5,
        AttackRoll = new SimpleAttack(1, 3)
    };

    public static EnemyCard BeastCard { get; } = new()
    {
        Name = "Beast",
        Speed = 3,
        Armor = 1,
        MaxHealth = 4,
        AttackRoll = new SimpleAttack(2, 4),
    };

    public static EnemyCard EyeKeyUh { get; } = new()
    {
        Name = "Eye Key Uh",
        Speed = 4,
        Armor = 2,
        MaxHealth = 16,
        AttackRoll = new SimpleAttack(3, 6),
    };

    public static EnemyCard Meatball { get; } = new()
    {
        Name = "Meatball",
        Speed = 6,
        Armor = 0,
        MaxHealth = 1,
        AttackRoll = new SimpleAttack(1, 10),
    };
}