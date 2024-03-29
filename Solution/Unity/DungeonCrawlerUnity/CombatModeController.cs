using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

public class CombatModeController : MonoBehaviour
{
    public static CombatModeController Shared { get; private set; } = default!;
    public CombatModeController() { Shared = this; }

    public void OnEnable()
    {
        CombatViewPortController.Shared.gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        CombatViewPortController.Shared.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        gameObject.SetActive(true);

    }
}