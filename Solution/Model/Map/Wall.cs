namespace CaptainCoder.Dungeoneering.Model;
using static WallType;
public enum WallType
{
    Solid,
    Door,
    SecretDoor,
}

public static class WallTypeExtensions
{
    public static WallType Next(this WallType current) => current switch
    {
        Solid => Door,
        Door => SecretDoor,
        SecretDoor => Solid,
        _ => throw new Exception($"Unknown wall type {current}"),
    };
}