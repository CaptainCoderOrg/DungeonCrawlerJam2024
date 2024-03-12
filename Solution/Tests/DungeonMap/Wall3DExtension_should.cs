namespace Tests;

using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Modeling;

using Shouldly;

using static CaptainCoder.Dungeoneering.Modeling.WallExtensions;

public class WallExtensions_should
{
    public const float HalfCellSize = DefaultCellSize * 0.5f;
    public const float HalfThickness = DefaultThickness * 0.5f;

    public static IEnumerable<object[]> ConvertTileEdgeTo3DEdgeData()
    {

        return [
            [
                new TileEdge(new Position(0, 0), Facing.North),
                new WallBase(new Point2D(-0.5f, -HalfCellSize + HalfThickness), new Point2D(0.5f, -HalfCellSize - HalfThickness)),
            ],
            [
                new TileEdge(new Position(0, 0), Facing.South),
                new WallBase(new Point2D(-0.5f, HalfCellSize + HalfThickness), new Point2D(0.5f, HalfCellSize - HalfThickness)),
            ],
            [
                new TileEdge(new Position(0, 0), Facing.East),
                new WallBase(new Point2D(HalfCellSize - HalfThickness, -HalfCellSize), new Point2D(HalfCellSize + HalfThickness, HalfCellSize))
            ],
            [
                new TileEdge(new Position(0, 0), Facing.West),
                new WallBase(new Point2D(-HalfCellSize - HalfThickness, -HalfCellSize), new Point2D(-HalfCellSize + HalfThickness, HalfCellSize))
            ],

            [
                new TileEdge(new Position(5, 5), Facing.North),
                new WallBase(new Point2D(5 - 0.5f, 5 - HalfCellSize + HalfThickness), new Point2D(5 + 0.5f, 5 - HalfCellSize - HalfThickness)),
            ],
            [
                new TileEdge(new Position(2, 3), Facing.South),
                new WallBase(new Point2D(2 - 0.5f, 3 + HalfCellSize + HalfThickness), new Point2D(2 + 0.5f, 3 + HalfCellSize - HalfThickness)),
            ],
            [
                new TileEdge(new Position(6, 1), Facing.East),
                new WallBase(new Point2D(6 + HalfCellSize - HalfThickness, 1 - HalfCellSize), new Point2D(6 + HalfCellSize + HalfThickness, 1 + HalfCellSize))
            ],
            [
                new TileEdge(new Position(2, 1), Facing.West),
                new WallBase(new Point2D(2 - HalfCellSize - HalfThickness, 1 - HalfCellSize), new Point2D(2 - HalfCellSize + HalfThickness, 1 + HalfCellSize))
            ],
        ];
    }
    [Theory]
    [MemberData(nameof(ConvertTileEdgeTo3DEdgeData))]
    public void convert_tile_edge_to_wall_base(TileEdge tileEdge, WallBase expected)
    {
        WallBase actual = WallExtensions.ToWallBase(tileEdge);
        actual.ShouldBe(expected);
    }


    // public static IEnumerable<object[]> ExtrudeWallBaseTo3DMeshData()
    // {

    //     return [
    //         [
    //             new WallBase(new Point2D(0, 0), new Point2D(1, 1)),
    //             (Point3D[])
    //             [
    //                 new(0, 0, 0),
    //                 new(0, 0, 1),
    //                 new(0, 1, 1),
    //                 new(0, 1, 0),
    //                 new(1, 0, 0),
    //                 new(1, 0, 1),
    //                 new(1, 1, 1),
    //                 new(1, 1, 0),
    //             ],
    //             (int[][])
    //             [
    //                 [0, 1, 2, 2, 3, 1], // Front
    //                 [4, 5, 6, 6, 7, 4], // Back
    //                 [3, 2, 6, 6, 7, 3], // Top
    //                 [0, 1, 5, 5, 4, 0], // Bottom
    //                 [0, 3, 7, 7, 4, 0], // Left
    //                 [1, 2, 6, 6, 5, 1], // Right
    //             ]
    //         ],
    //         [
    //             new WallBase(new Point2D(3, 3), new Point2D(4, 4)),
    //             (Point3D[])
    //             [
    //                 new(3, 0, 3),
    //                 new(3, 0, 4),
    //                 new(3, 1, 4),
    //                 new(3, 1, 3),
    //                 new(4, 0, 3),
    //                 new(4, 0, 4),
    //                 new(4, 1, 4),
    //                 new(4, 1, 3),
    //             ],
    //             (int[][])
    //             [
    //                 [0, 1, 2, 2, 3, 0], // Front
    //                 [5, 4, 7, 7, 6, 5], // Back
    //                 [3, 7, 6, 6, 2, 3], // Top
    //                 [0, 4, 5, 5, 1, 0], // Bottom
    //                 [4, 7, 3, 3, 0, 4], // Left
    //                 [1, 2, 6, 6, 5, 1], // Right
    //             ]
    //         ],
    //     ];
    // }
    // [Theory]
    // [MemberData(nameof(ExtrudeWallBaseTo3DMeshData))]
    // public void extrude_wallbase_to_3d_mesh_data(WallBase wallBase, Point3D[] expectedPoints, int[][] expectedFaceIndices)
    // {
    //     var actual = WallExtensions.ExtrudeToMeshData(wallBase);

    //     Point3D[] actualPoints = [.. actual.Points];
    //     Assert.Equal(expectedPoints, actualPoints);

    //     int[][] actualFaceIndices = [.. actual.FaceIndices];
    //     Assert.Equal(expectedFaceIndices, actualFaceIndices);
    // }
}