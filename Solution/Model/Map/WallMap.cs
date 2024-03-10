
using System.Collections.ObjectModel;

namespace CaptainCoder.Dungeoneering.Model;

public class WallMap()
{
    public WallType this[Position position, Facing facing]
    {
        get => GetWall(position, facing);
        set => SetWall(position, facing, value);
    }
    private readonly Dictionary<TileEdge, WallType> _map = new();
    public IReadOnlyDictionary<TileEdge, WallType> Map => new ReadOnlyDictionary<TileEdge, WallType>(_map);
    public void SetWall(Position position, Facing facing, WallType wall) => _map[new TileEdge(position, facing).Normalize()] = wall;
    public bool RemoveWall(Position position, Facing facing) => _map.Remove(new TileEdge(position, facing).Normalize());
    public WallType GetWall(Position position, Facing facing) => _map[new TileEdge(position, facing).Normalize()];
    public bool TryGetWall(Position position, Facing facing, out WallType wall) => _map.TryGetValue(new TileEdge(position, facing).Normalize(), out wall);
    public int Count => _map.Count;

    public WallMap(IEnumerable<(TileEdge, WallType)> edges) : this()
    {
        foreach ((TileEdge edge, WallType wall) in edges)
        {
            _map.Add(edge, wall);
        }
    }
}

public static class WallMapExtensions
{
    public static WallMap CreateEmpty(int width, int height)
    {
        WallMap map = new();
        for (int x = 0; x < width; x++)
        {
            map.SetWall(new Position(x, 0), Facing.North, WallType.Solid);
            map.SetWall(new Position(x, height - 1), Facing.South, WallType.Solid);
        }

        for (int y = 0; y < height; y++)
        {
            map.SetWall(new Position(0, y), Facing.West, WallType.Solid);
            map.SetWall(new Position(width - 1, y), Facing.East, WallType.Solid);
        }
        return map;
    }

    public static WallMap RandomMap(int width, int height, double wallDensity, double doorDensity, double secretDoorDensity)
    {
        Random random = new();
        WallMap map = CreateEmpty(width, height);
        Facing[] directions = [Facing.North, Facing.West];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                foreach (Facing f in directions)
                {
                    if (random.NextDouble() < wallDensity)
                    {
                        if (random.NextDouble() < doorDensity)
                        {
                            if (random.NextDouble() < secretDoorDensity)
                            {
                                map.SetWall(new Position(x, y), f, WallType.SecretDoor);
                            }
                            else
                            {
                                map.SetWall(new Position(x, y), f, WallType.Door);
                            }
                        }
                        else
                        {
                            map.SetWall(new Position(x, y), f, WallType.Solid);
                        }
                    }
                }
            }
        }
        return map;
    }
}