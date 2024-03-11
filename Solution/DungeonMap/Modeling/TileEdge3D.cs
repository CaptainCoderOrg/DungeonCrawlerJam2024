namespace CaptainCoder.Dungeoneering.Modeling;

using CaptainCoder.Dungeoneering.DungeonMap;

public record WallBase(Point2D BaseA, Point2D BaseB);
public record Point2D(float X, float Y);
public record Point3D(float X, float Y, float Z);

public static class WallExtensions
{
    public const float DefaultThickness = 0.1f;
    public const float DefaultCellSize = 1.0f;
    public const float DefaultHeight = 1.0f;
    public static WallBase ToWallBase(this TileEdge edge, float cellSize = DefaultCellSize, float thickness = DefaultThickness)
    {
        float halfCell = cellSize * 0.5f;
        float halfThick = thickness * 0.5f;
        var ((x, y), facing) = edge;
        float offset = facing switch
        {
            Facing.North or Facing.West => -halfCell,
            Facing.South or Facing.East => halfCell,
            _ => throw new NotImplementedException($"Unknown facing: {facing}"),
        };
        (Point2D baseA, Point2D baseB) = facing switch
        {
            Facing.North or Facing.South => (new Point2D(x - halfCell, y + offset + halfThick), new Point2D(x + halfCell, y + offset - halfThick)),
            Facing.East or Facing.West => (new Point2D(x + offset - halfThick, y - halfCell), new Point2D(x + offset + halfThick, y + halfCell)),
            _ => throw new NotImplementedException($"Unknown facing: {facing}"),
        };

        return new WallBase(baseA, baseB);
    }

    public static (IEnumerable<Point3D> Points, IEnumerable<int[]> FaceIndices) ExtrudeToMeshData(this WallBase wallBase, float extrudeHeight = DefaultHeight)
    {
        return (
        [
            new (wallBase.BaseA.X, 0, wallBase.BaseA.Y),
            new (wallBase.BaseA.X, 0, wallBase.BaseB.Y),
            new (wallBase.BaseA.X, extrudeHeight, wallBase.BaseB.Y),
            new (wallBase.BaseA.X, extrudeHeight, wallBase.BaseA.Y),

            new (wallBase.BaseB.X, 0, wallBase.BaseA.Y),
            new (wallBase.BaseB.X, 0, wallBase.BaseB.Y),
            new (wallBase.BaseB.X, extrudeHeight, wallBase.BaseB.Y),
            new (wallBase.BaseB.X, extrudeHeight, wallBase.BaseA.Y),
        ],

        // Face vertices must be listed in clockwise order
        [
            [0, 1, 2, 2, 3, 0], // Front
            [5, 4, 7, 7, 6, 5], // Back
            [3, 7, 6, 6, 2, 3], // Top
            [0, 4, 5, 5, 1, 0], // Bottom
            [4, 7, 3, 3, 0, 4], // Left
            [1, 2, 6, 6, 5, 1], // Right
        ]);
    }
}