namespace CaptainCoder.Dungeoneering.DungeonMap.Unity;

using CaptainCoder.Dungeoneering.Modeling;

using UnityEngine;
using UnityEngine.ProBuilder;

public static class MapGeneratorExtensions
{
    public const float WallThickness = 0.25f;
    public const float TileScale = 1f;
    public const float HalfTile = TileScale * 0.5f;
    public static ProBuilderMesh ToProBuilderMesh(this WallMap wallMap)
    {
        List<Vector3> points = [];
        List<Face> faces = [];
        foreach ((TileEdge edge, WallType _) in wallMap.Map)
        {
            int ixOffset = points.Count;
            (IEnumerable<Point3D> ps, IEnumerable<int[]> fs) = edge.ToWallBase().ExtrudeToMeshData();
            points.AddRange(ps.Select(ToVector3));
            faces.AddRange(fs.Select(ixs => (int[])[.. ixs.Select(ix => ix + ixOffset)]).Select(ToFace));
        }

        return ProBuilderMesh.Create(points, faces);
    }

    static Vector3 ToVector3(Point3D p3d) => new(p3d.X, p3d.Y, p3d.Z);
    static Face ToFace(int[] ixs) => new(ixs);
}