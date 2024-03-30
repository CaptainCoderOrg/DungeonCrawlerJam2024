using TMPro;

using UnityEngine;
namespace CaptainCoder.DungeonCrawler.Combat.Unity;
public class AttackRollRenderer : MonoBehaviour
{
    public TextMeshProUGUI WeaponName = default!;
    public TextMeshProUGUI DamageText = default!;
    public TextMeshProUGUI ExertText = default!;
    public void OnDisable()
    {
        CombatHelpPanel.Shared.gameObject.SetActive(false);
    }
    public void Render(Weapon weapon, int exert, int energy, IAttackRoll roll)
    {
        gameObject.SetActive(true);
        CombatHelpPanel.Shared.Text = "Spend energy to increase the damage of this attack.";
        WeaponName.text = weapon.Name;
        DamageText.text = $"Damage: {roll.Min} - {roll.Max}";
        ExertText.text = $"Exert: {exert} / {energy}";
        if (energy == 0) { ExertText.text = "No Energy"; }
    }
}