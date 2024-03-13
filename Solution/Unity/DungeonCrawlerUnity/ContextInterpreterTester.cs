using DungeonCrawler.Lua;

using UnityEngine;
using UnityEngine.Events;

namespace CaptainCoder.Dungeoneering.Unity;

public class ContextInterpreterTester : MonoBehaviour
{
    [TextArea]
    [field: SerializeField]
    public string Script = string.Empty;
    [field: SerializeField]
    public CrawlingModeController? Context { get; private set; }

    public UnityEvent<string>? OnResult;

    public void Start()
    {
        if (Context is not null)
        {
            Debug.Log($"Executing Lua: {Script}");
            Interpreter.ExecLua(Script, Context);
            OnResult?.Invoke($"Script executed");
        }
        else
        {
            Debug.Log($"No script executed");
            OnResult?.Invoke($"No script executed");
        }
    }
}