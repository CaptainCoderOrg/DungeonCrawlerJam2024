namespace CaptainCoder.Dungeoneering.Raylib;

using System.Numerics;

using Raylib_cs;

public static class WallMapRenderer
{
    public static void Render(this WallMap map, int left = 0, int top = 0)
    {
        foreach ((TileEdge edge, WallType wall) in map.Map)
        {
            Line line = edge.ToScreenCoords(DungeonEditorScreen.CellSize, left, top);
            line.Render(2, GetWallColor(wall));
        }
    }

    private static Color GetWallColor(WallType wall) => wall switch
    {
        WallType.Solid => Color.DarkGray,
        WallType.Door => Color.Beige,
        WallType.SecretDoor => Color.Blue,
        _ => throw new Exception($"Unknown wall type: {wall}"),
    };

    public static Line ToScreenCoords(this TileEdge edge, int cellSize, int left = 0, int top = 0)
    {
        float baseX = edge.Position.X * cellSize + left;
        float baseY = edge.Position.Y * cellSize + top;
        var (startX, startY) = edge.Facing switch
        {
            Facing.North or Facing.West => (baseX, baseY),
            Facing.South => (baseX, baseY + cellSize),
            Facing.East => (baseX + cellSize, baseY),
            _ => throw new NotImplementedException($"Unknown facing: {edge.Facing}"),
        };
        var (endX, endY) = edge.Facing switch
        {
            Facing.North or Facing.South => (startX + cellSize, startY),
            Facing.East or Facing.West => (startX, startY + cellSize),
            _ => throw new NotImplementedException($"Unknown facing: {edge.Facing}"),
        };
        return new Line(startX, startY, endX, endY);
    }

    public static void Render(this Line line, float thickness, Color color)
    {
        Raylib.DrawLineEx(
            new Vector2(line.StartX, line.StartY),
            new Vector2(line.EndX, line.EndY),
            thickness,
            color
        );
    }
}

public record struct Line(float StartX, float StartY, float EndX, float EndY);