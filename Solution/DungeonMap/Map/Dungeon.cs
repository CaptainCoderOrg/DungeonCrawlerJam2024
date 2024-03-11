namespace CaptainCoder.Dungeoneering.DungeonMap;

public class Dungeon()
{
    public WallMap Walls { get; private set; } = new();
    public Tile GetTile(Position position)
    {
        Walls.TryGetWall(position, Facing.North, out WallType north);
        Walls.TryGetWall(position, Facing.East, out WallType east);
        Walls.TryGetWall(position, Facing.South, out WallType south);
        Walls.TryGetWall(position, Facing.West, out WallType west);
        TileWalls walls = new(north, east, south, west);
        return new Tile(position, walls);
    }

    public Dungeon(WallMap walls) : this()
    {
        Walls = walls;
    }
}

public record Tile(Position Position, TileWalls Walls);
public record TileWalls(WallType North, WallType East, WallType South, WallType West);