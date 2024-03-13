using DungeonCrawler.Lua;

using UnityEngine;
using UnityEngine.Events;

namespace CaptainCoder.Dungeoneering.Unity;

public class InterpreterTester : MonoBehaviour
{
    [field: SerializeField]
    public string Script { get; private set; } = string.Empty;

    public UnityEvent<string>? OnResult;

    public void Start()
    {
        Debug.Log($"Executing Lua: {Script}");
        Console.WriteLine($"Executing Lua: {Script}");
        object result = Interpreter.EvalRawLua<object>(Script);
        Debug.Log($"Lua Interpreter returned: {result}");
        Console.WriteLine($"Lua Interpreter returned: {result}");
        OnResult?.Invoke($"Result: {result}");
    }
}