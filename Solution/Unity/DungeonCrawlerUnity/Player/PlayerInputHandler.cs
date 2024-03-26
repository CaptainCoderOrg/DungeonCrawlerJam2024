using CaptainCoder.Dungeoneering.Lua.Dialogue;

using UnityEngine;
using UnityEngine.Events;
namespace CaptainCoder.Dungeoneering.Player.Unity;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Shared { get; private set; } = default!;
    public PlayerInputHandler() { Shared = this; }
    [field: SerializeField]
    public PlayerInputMapping InputMapping { get; set; } = default!;
    public UnityEvent<MovementAction>? OnMovementAction;
    public UnityEvent<DialogueAction>? OnDialogueAction;
    public UnityEvent<MenuControl>? OnMenuControl;
    public void Update()
    {
        foreach (MovementActionMapping mapping in InputMapping.MovementActions)
        {
            if (Input.GetKeyDown(mapping.Key))
            {
                OnMovementAction?.Invoke(mapping.Action);
            }
        }

        foreach (DialogueActionMapping mapping in InputMapping.DialogueActions)
        {
            if (Input.GetKeyDown(mapping.Key))
            {
                OnDialogueAction?.Invoke(mapping.Action);
            }
        }

        foreach (MenuActionMapping mapping in InputMapping.MenuActionMappings)
        {
            if (Input.GetKeyDown(mapping.Key))
            {
                OnMenuControl?.Invoke(mapping.Action);
            }
        }
    }
}

[CreateAssetMenu(fileName = "PlayerInputMapping", menuName = "Config/PlayerInputMapping")]
public class PlayerInputMapping : ScriptableObject
{
    [field: SerializeField]
    public MovementActionMapping[] MovementActions { get; set; } = default!;
    [field: SerializeField]
    public DialogueActionMapping[] DialogueActions { get; set; } = default!;
    [field: SerializeField]
    public MenuActionMapping[] MenuActionMappings { get; set; } = default!;

    public void OnMenuAction(Action<MenuControl> handler)
    {
        foreach (var mapping in MenuActionMappings)
        {
            if (Input.GetKeyDown(mapping.Key))
            {
                handler.Invoke(mapping.Action);
            }
        }
    }
}

[Serializable]
public class MovementActionMapping
{
    public KeyCode Key;
    public MovementAction Action;
}

[Serializable]
public class DialogueActionMapping
{
    public KeyCode Key;
    public DialogueAction Action;
}

[Serializable]
public class MenuActionMapping : ActionMapping<MenuControl> { }

public enum MenuControl
{
    Up,
    Down,
    Left,
    Right,
    Select,
    Cancel
}

public class ActionMapping<T>
{
    public KeyCode Key;
    public T? Action;
}