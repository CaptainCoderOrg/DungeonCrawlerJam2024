namespace CaptainCoder.Dungeoneering.Raylib;

using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
using CaptainCoder.Dungeoneering.DungeonMap.IO;

using Raylib_cs;

public class DungeonEditorScreen : IScreen
{
    public const int MaxMapSize = 24;
    public const int CellSize = 16;
    public Cursor Cursor { get; private set; } = new(new Position(0, 0), Facing.West);
    public WallType Wall { get; private set; } = WallType.Solid;
    public Dungeon CurrentDungeon { get; private set; } = new(WallMapExtensions.CreateEmpty(MaxMapSize, MaxMapSize));
    private string? _filename = null;
    private readonly InfoOverLayScreen _overlay = new();
    private IScreen? _editorMenu;
    public IScreen EditorMenu
    {
        get
        {
            if (_editorMenu is null)
            {
                _editorMenu = new ModalMenuScreen(
                    this,
                    new MenuScreen("Menu",
                    [
                        new DynamicEntry(
                            () => "Save " + (_filename ?? string.Empty),
                            _filename is null ? SaveAs : Save
                        ),
                        new StaticEntry("Save As", SaveAs),
                        new StaticEntry("New Map", NewMap),
                        new StaticEntry("Randomize Map", RandomizeMap),
                        new StaticEntry("Load", LoadMap),
                        new StaticEntry("Return to Editor", () => Program.Screen = this),
                        new StaticEntry("Exit Editor", Program.Exit),
                    ]
                ));
            }
            return _editorMenu;
        }
    }

    private void RandomizeMap()
    {
        CurrentDungeon = new Dungeon(WallMapExtensions.RandomMap(MaxMapSize, MaxMapSize, .50, .25, .10));
        Program.Screen = this;
    }

    private void NewMap()
    {
        CurrentDungeon = new Dungeon(WallMapExtensions.CreateEmpty(MaxMapSize, MaxMapSize));
        Program.Screen = this;
    }

    public void Save()
    {
        if (_filename is null)
        {
            SaveAs();
            return;
        }
        File.WriteAllText(_filename, CurrentDungeon.Walls.ToJson());
        _overlay.AddMessage($"File saved: {_filename}!", Color.Green);
        Program.Screen = this;
    }

    public void SaveAs()
    {
        Program.Screen = new PromptScreen("Save As", this, OnFinished);
        void OnFinished(string filename)
        {
            _filename = Path.Combine(".save-data", $"{filename}.json");
            Save();
        }
    }

    public void LoadMap()
    {
        Directory.CreateDirectory(Path.Combine(".save-data"));
        string[] filenames = Directory.GetFiles(Path.Combine(".save-data"));
        Program.Screen = new ModalMenuScreen(
            this,
            new MenuScreen(
                "Load Map",
                filenames.Select((file, ix) => new StaticEntry(file, () => LoadMap(file)))
            )
        );

        void LoadMap(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.Error.WriteLine($"File not found: {filename}");
                _overlay.AddMessage($"File not found: {filename}", Color.Red);
                return;
            }
            _overlay.AddMessage($"File loaded!", Color.Green);

            _filename = filename;
            string json = File.ReadAllText(filename);
            CurrentDungeon = new Dungeon(JsonExtensions.LoadModel<WallMap>(json));
            Program.Screen = this;
        }
    }

    public void Render()
    {
        Raylib.ClearBackground(Color.Black);
        int offset = CellSize * 2;
        CurrentDungeon.Walls.Render(offset, offset);
        Cursor.Render(CellSize, offset, offset);
        RenderInfoAtCursor(offset + (MaxMapSize + 1) * CellSize, 2 * CellSize);
        _overlay.Render();
    }

    private void RenderInfoAtCursor(int left, int top)
    {
        const int fontSize = 20;
        const int padding = 2;
        var (pos, facing) = Cursor;
        DrawText($"({pos.X}, {pos.Y}) - {facing}");
        WallType wallType = CurrentDungeon.Walls.GetWall(pos, facing);
        DrawText($"WallType: {wallType}");
        DrawText("Scripts:");
        foreach (TileEvent evt in CurrentDungeon.EventMap.EventsAt(pos))
        {
            DrawText($"  {evt.Trigger}: ???");
        }

        void DrawText(string text)
        {
            Raylib.DrawText(text, left, top, fontSize, Color.White);
            top += fontSize + padding;
        }
    }

    public void HandleUserInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Minus))
        {
            CurrentDungeon.EventMap.AddEvent(Cursor.Position, new TileEvent(EventTrigger.OnEnter, new EventScript("Some Script")));
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Tab))
        {
            Wall = Wall.Next();
            _overlay.AddMessage($"WallType: {Wall}", Color.Green, .5f);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            Program.Screen = EditorMenu;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (CurrentDungeon.Walls.TryGetWall(Cursor.Position, Cursor.Facing, out WallType wall) && wall == Wall)
            {
                CurrentDungeon.Walls.RemoveWall(Cursor.Position, Cursor.Facing);
            }
            else
            {
                CurrentDungeon.Walls.SetWall(Cursor.Position, Cursor.Facing, Wall);
            }
        }
        HandleCursorMovement();
    }

    private bool IsShiftDown => Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift);
    private bool IsWestKeyPressed => Raylib.IsKeyPressed(KeyboardKey.A) || Raylib.IsKeyPressed(KeyboardKey.Left);
    private bool IsNorthKeyPressed => Raylib.IsKeyPressed(KeyboardKey.W) || Raylib.IsKeyPressed(KeyboardKey.Up);
    private bool IsSouthKeyPressed => Raylib.IsKeyPressed(KeyboardKey.S) || Raylib.IsKeyPressed(KeyboardKey.Down);
    private bool IsEastKeyPressed => Raylib.IsKeyPressed(KeyboardKey.D) || Raylib.IsKeyPressed(KeyboardKey.Right);

    private void HandleCursorMovement()
    {
        if (IsShiftDown && IsNorthKeyPressed)
        {
            Cursor = Cursor.Move(Facing.North);
        }
        else if (IsShiftDown && IsSouthKeyPressed)
        {
            Cursor = Cursor.Move(Facing.South);
        }
        else if (IsShiftDown && IsEastKeyPressed)
        {
            Cursor = Cursor.Move(Facing.East);
        }
        else if (IsShiftDown && IsWestKeyPressed)
        {
            Cursor = Cursor.Move(Facing.West);
        }
        else if (IsNorthKeyPressed)
        {
            Cursor = MoveOrRotate(Facing.North);
        }
        else if (IsEastKeyPressed)
        {
            Cursor = MoveOrRotate(Facing.East);
        }
        else if (IsSouthKeyPressed)
        {
            Cursor = MoveOrRotate(Facing.South);
        }
        else if (IsWestKeyPressed)
        {
            Cursor = MoveOrRotate(Facing.West);
        }

        Cursor = Cursor with { Position = Cursor.Position.Clamp(MaxMapSize, MaxMapSize) };
    }

    private Cursor MoveOrRotate(Facing facing)
    {
        if (Cursor.Facing == facing)
        {
            return Cursor.MoveAndRotate(facing);
        }
        else
        {
            return Cursor.Turn(facing);
        }
    }
}