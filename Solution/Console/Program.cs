using NLua;

Console.Clear();

Position pos = new(5, 7);

Lua state = new();
state["pos"] = pos;
var res = state.DoString("pos.X = 6 return pos")[0];
Console.WriteLine(res);
Console.WriteLine(pos);

record Position(int X, int Y);