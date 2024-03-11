namespace Tests;

using CaptainCoder.Dungeoneering.DungeonMap;

using Shouldly;

public class Dungeon_should_
{
    public static Dungeon SimpleSquareDungeon
    {
        get
        {
            Dungeon dungeon = new();
            dungeon.Walls.SetWall(new Position(0, 0), Facing.North, WallType.Solid);
            dungeon.Walls.SetWall(new Position(0, 0), Facing.South, WallType.Solid);
            dungeon.Walls.SetWall(new Position(0, 0), Facing.East, WallType.Solid);
            dungeon.Walls.SetWall(new Position(0, 0), Facing.West, WallType.Solid);
            return dungeon;
        }
    }

    public static Dungeon TwoByTwoRoom
    {
        get
        {
            Dungeon dungeon = new();
            dungeon.Walls.SetWall(new Position(0, 0), Facing.North, WallType.Solid);
            dungeon.Walls.SetWall(new Position(0, 0), Facing.West, WallType.Solid);

            dungeon.Walls.SetWall(new Position(1, 0), Facing.North, WallType.Solid);
            dungeon.Walls.SetWall(new Position(1, 0), Facing.East, WallType.Solid);

            dungeon.Walls.SetWall(new Position(1, 1), Facing.South, WallType.Solid);
            dungeon.Walls.SetWall(new Position(1, 1), Facing.East, WallType.Solid);

            dungeon.Walls.SetWall(new Position(0, 1), Facing.South, WallType.Solid);
            dungeon.Walls.SetWall(new Position(0, 1), Facing.West, WallType.Solid);
            return dungeon;
        }
    }

    public static IEnumerable<object[]> ReturnTilesWithWallsData => [
        [SimpleSquareDungeon, new Position(0, 0), new Tile(new Position(0, 0), new TileWalls(WallType.Solid, WallType.Solid, WallType.Solid, WallType.Solid))],
        [TwoByTwoRoom, new Position(0, 0), new Tile(new Position(0, 0), new TileWalls(WallType.Solid, WallType.None, WallType.None, WallType.Solid))],
        [TwoByTwoRoom, new Position(1, 0), new Tile(new Position(1, 0), new TileWalls(WallType.Solid, WallType.Solid, WallType.None, WallType.None))],
        [TwoByTwoRoom, new Position(1, 1), new Tile(new Position(1, 1), new TileWalls(WallType.None, WallType.Solid, WallType.Solid, WallType.None))],
        [TwoByTwoRoom, new Position(0, 1), new Tile(new Position(0, 1), new TileWalls(WallType.None, WallType.None, WallType.Solid, WallType.Solid))],
    ];

    [Theory]
    [MemberData(nameof(ReturnTilesWithWallsData))]
    public void return_tiles_with_walls(Dungeon dungeon, Position toCheck, Tile expectedTile)
    {
        Tile actual = dungeon.GetTile(toCheck);
        actual.ShouldBe(expectedTile);
    }
}