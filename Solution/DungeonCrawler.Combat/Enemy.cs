namespace CaptainCoder.DungeonCrawler.Combat;

public record EnemyCard
{
    public string Name { get; init; } = "Unnamed Monster";
    public int Speed { get; init; } = 3;
    public int MaxHealth { get; init; } = 2;
    public int Armor { get; init; } = 0;
    public IAttackRoll AttackRoll { get; init; } = new SimpleAttack(1, 2);
    public int Level { get; init; } = 1;
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
        MaxHealth = 2,
        AttackRoll = new SimpleAttack(1, 3)
    };

    public static EnemyCard EmployeeCard { get; } = new()
    {
        Name = "Disgruntled Employee",
        Speed = 2,
        Armor = 1,
        MaxHealth = 4,
        AttackRoll = new SimpleAttack(4, 4),
        Level = 2,
    };

    public static EnemyCard HangryEmployee { get; } = new()
    {
        Name = "Hangry Employee",
        Speed = 2,
        Armor = 0,
        MaxHealth = 8,
        AttackRoll = new SimpleAttack(2, 6),
        Level = 2,
    };

    public static EnemyCard MouthBreather { get; } = new()
    {
        Name = "Mouth Breather",
        Speed = 2,
        Armor = 2,
        MaxHealth = 1,
        AttackRoll = new SimpleAttack(2, 6),
        Level = 2,
    };

    public static EnemyCard EyeKeyUh { get; } = new()
    {
        Name = "Eye Key Uh",
        Speed = 2,
        Armor = 12,
        MaxHealth = 75,
        AttackRoll = new SimpleAttack(1, 20),
        Level = 5,
    };

    public static EnemyCard Meatball { get; } = new()
    {
        Name = "Cosmic Meatball",
        Speed = 6,
        Armor = 0,
        MaxHealth = 1,
        AttackRoll = new SimpleAttack(1, 10),
        Level = 2,
    };

    public static EnemyCard Wardrobe { get; } = new()
    {
        Name = "Cosmic Wardrobe",
        Speed = 2,
        Armor = 3,
        MaxHealth = 6,
        AttackRoll = new SimpleAttack(6, 6),
        Level = 3,
    };

    public static EnemyCard CosmicBed { get; } = new()
    {
        Name = "Cosmic Bed",
        Speed = 1,
        Armor = 5,
        MaxHealth = 8,
        AttackRoll = new SimpleAttack(10, 10),
        Level = 4,
    };

    public static EnemyCard Karen { get; } = new()
    {
        Name = "Karen",
        Speed = 6,
        Armor = 0,
        MaxHealth = 16,
        AttackRoll = new SimpleAttack(6, 10),
        Level = 4,
    };

    public static EnemyCard Chad { get; } = new()
    {
        Name = "Chad",
        Speed = 8,
        Armor = 6,
        MaxHealth = 1,
        AttackRoll = new SimpleAttack(6, 8),
        Level = 4,
    };

    public static EnemyCard BloodShot { get; } = new()
    {
        Name = "Bloodshot Meatball",
        Speed = 2,
        Armor = 0,
        MaxHealth = 10,
        AttackRoll = new SimpleAttack(10, 10),
        Level = 4,
    };
}