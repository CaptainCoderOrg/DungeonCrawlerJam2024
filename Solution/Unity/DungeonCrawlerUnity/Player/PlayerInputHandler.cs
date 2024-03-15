using UnityEngine;
using UnityEngine.Events;
namespace CaptainCoder.Dungeoneering.Player.Unity;

public class PlayerInputHandler : MonoBehaviour
{
    [field: SerializeField]
    public PlayerInputMapping InputMapping { get; set; } = default!;
    public UnityEvent<MovementAction>? OnMovementAction;
    public void Update()
    {
        foreach (MovementActionMapping mapping in InputMapping.MovementActions)
        {
            if (Input.GetKeyDown(mapping.Key))
            {
                OnMovementAction?.Invoke(mapping.Action);
            }
        }
    }
}

[CreateAssetMenu(fileName = "PlayerInputMapping", menuName = "Config/PlayerInputMapping")]
public class PlayerInputMapping : ScriptableObject
{
    [field: SerializeField]
    public MovementActionMapping[] MovementActions { get; set; } = default!;
}

[Serializable]
public class MovementActionMapping
{
    public KeyCode Key;
    public MovementAction Action;
}