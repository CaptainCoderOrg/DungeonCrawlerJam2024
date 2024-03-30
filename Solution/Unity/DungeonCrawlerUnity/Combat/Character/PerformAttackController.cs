using CaptainCoder.Dungeoneering.Game.Unity;
using CaptainCoder.Dungeoneering.Player.Unity;

using UnityEngine;
namespace CaptainCoder.DungeonCrawler.Combat.Unity;
public class PerformAttackController : MonoBehaviour
{
    public static PerformAttackController Shared { get; private set; } = default!;
    public PerformAttackController() { Shared = this; }
    public DamageMessage DamageMessageTemplate = default!;
    public Transform DamageMessageParent = default!;
    private CombatMap Map => CombatMapController.Shared.CombatMap;
    private int _exert = 0;
    private Action<AttackResultEvent> _onFinish = default!;
    private Action _onCancel = default!;
    private CharacterCard _card = default!;
    private Position _target = default!;
    public AttackRollRenderer AttackRollRenderer = default!;
    public void OnEnable()
    {
        CharacterAttackController.Shared.gameObject.SetActive(false);
        GuardMenuController.Shared.gameObject.SetActive(false);
        AttackRollRenderer.gameObject.SetActive(true);
    }
    public void OnDisable()
    {
        AttackRollRenderer.gameObject.SetActive(false);
    }

    public void Update() => PlayerInputHandler.Shared.InputMapping.OnMenuAction(HandleInput);
    public void Initialize(CharacterCard card, Position target, Action<AttackResultEvent> onFinish, Action onCancel)
    {
        _target = target;
        _card = card;
        _onFinish = onFinish;
        _onCancel = onCancel;
        _exert = 0;

        var keys = string.Join(" or ", [PlayerInputHandler.Shared.GetKeys(MenuControl.Up), PlayerInputHandler.Shared.GetKeys(MenuControl.Down)]);
        MessageRenderer.Shared.AddMessage($"Increase/Decrease Energy: {keys}");
        MessageRenderer.Shared.AddMessage($"Press {PlayerInputHandler.Shared.GetKeys(MenuControl.Cancel)} to cancel.");
        MessageRenderer.Shared.AddMessage($"Press {PlayerInputHandler.Shared.GetKeys(MenuControl.Select)} to confirm.");
        PlayerCharacter pc = Map.GetCharacter(_card);
        AttackRollRenderer.Render(pc.Weapon, _exert, pc.Energy(), GetAttackRoll());
        gameObject.SetActive(true);
    }

    private void HandleInput(MenuControl control)
    {
        Action action = control switch
        {
            MenuControl.Down => () => Exert(-1),
            MenuControl.Up => () => Exert(1),
            MenuControl.Cancel => Cancel,
            MenuControl.Select => PerformAttack,
            _ => () => Debug.Log($"Not implemented: {control}"),
        };
        action.Invoke();
    }

    private IAttackRoll GetAttackRoll()
    {
        PlayerCharacter pc = Map.GetCharacter(_card);
        SimpleAttack roll = new(pc.Weapon.AttackRoll.Min + (_exert / 2), pc.Weapon.AttackRoll.Max + _exert);
        return roll;
    }

    private void PerformAttack()
    {
        PlayerCharacter pc = Map.GetCharacter(_card);
        Map.UpdateCharacter(pc with { Exertion = pc.Exertion + _exert });
        AttackResultEvent result = Map.DoAttack(_card, GetAttackRoll(), _target);

        if (!result.IsTargetKilledEvent())
        {
            DamageMessage newMessage = Instantiate(DamageMessageTemplate, DamageMessageParent);
            newMessage.Render(result.TotalDamage(), _target);
        }
        _onFinish.Invoke(result);
        gameObject.SetActive(false);
    }

    private void Exert(int delta)
    {
        PlayerCharacter pc = Map.GetCharacter(_card);
        _exert = Math.Clamp(_exert + delta, 0, pc.Energy());
        AttackRollRenderer.Render(pc.Weapon, _exert, pc.Energy(), GetAttackRoll());
    }

    private void Cancel()
    {
        gameObject.SetActive(false);
        _onCancel.Invoke();
    }

}