namespace CaptainCoder.Dungeoneering.Raylib;

using Raylib_cs;

public class ProjectScreen : IScreen
{
    public string ProjectName { get; }
    private readonly MenuScreen _menu;

    public ProjectScreen(string projectName)
    {
        ProjectName = projectName;
        _menu = new MenuScreen(projectName,
        [
            new StaticEntry("Dungeon Editor", () => Program.Screen = new DungeonEditorScreen(projectName)),
            new StaticEntry("Build", () => { }),
            new StaticEntry("Close Project", () => Program.Screen = new ProjectSelectionScreen())
        ]
        );
    }
    public void HandleUserInput()
    {
        _menu.HandleUserInput();
    }

    public void Render()
    {
        Raylib.ClearBackground(Color.Black);
        _menu.Render();
    }
}