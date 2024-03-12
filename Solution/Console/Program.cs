using NLua;
using CaptainCoder.Dungeoneering.DungeonMap;

Console.Clear();
Position pos = new(0, 0);
Lua state = new();
state["pos"] = pos;
state.LoadCLRPackage();
var res = state.DoString(
    """
    import("System")
    --import("CaptainCoder.Dungeoneering.DungeonMap.Position")
    --test = Position(0, 0)
    test = Position
    pos.X = 6 
    return test
    """
)[0];
Console.WriteLine(res);
Console.WriteLine(pos);

public class SomeClass
{

    public SomeClass(int X)
    {

    }

}