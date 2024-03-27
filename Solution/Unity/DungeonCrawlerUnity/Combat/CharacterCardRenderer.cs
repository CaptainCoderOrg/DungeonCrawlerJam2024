using CaptainCoder.Dungeoneering.Unity;

using TMPro;

using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;
public class CharacterCardRenderer : MonoBehaviour
{
    public int CharacterIx;
    public GameObject Selected = default!;
    public GameObject Finished = default!;
    public TextMeshProUGUI Name = default!;
    public TwoValueRenderer Health = default!;
    public TwoValueRenderer Energy = default!;
    public ValueRenderer Armor = default!;
    public ValueRenderer Speed = default!;
    public ValueRenderer MovementPoints = default!;
    public ValueRenderer AttackPoints = default!;
    public bool IsSelected { set => Selected.SetActive(value); }
    public bool IsFinished { set => Finished.SetActive(value); }
    public PlayerCharacter? Character
    {
        set
        {
            if (value is null)
            {
                RenderNoCharacter();
            }
            else
            {
                Render(value);
            }
        }
    }

    public void Render(PlayerCharacter character)
    {
        Name.text = character.Card.Name;
        Health.Value = character.Health();
        Health.BaseValue = character.Card.BaseHealth;

        Energy.Value = character.Energy();
        Energy.BaseValue = character.Card.BaseEnergy;

        Armor.Value = character.Card.BaseArmor;
        Speed.Value = character.Card.BaseSpeed;
        MovementPoints.Value = character.MovementPoints;
        AttackPoints.Value = character.AttackPoints;
    }

    public void RenderNoCharacter()
    {
        Debug.Assert(false, "RenderNoCharacter() is not implemented");
    }

    public void Start()
    {
        if (CharacterIx == 0) { CrawlingModeController.Shared.Party.OnTopLeftChange += Render; }
        else if (CharacterIx == 1) { CrawlingModeController.Shared.Party.OnTopRightChange += Render; }
        else if (CharacterIx == 2) { CrawlingModeController.Shared.Party.OnBottomLeftChange += Render; }
        else if (CharacterIx == 3) { CrawlingModeController.Shared.Party.OnBottomRightChange += Render; }
        else { throw new Exception($"Invalid Character IX"); }
        Render(CrawlingModeController.Shared.Party[CharacterIx]);
    }
}


public class ValueRenderer : MonoBehaviour
{
    public TextMeshProUGUI Text = default!;
    public int Value
    {
        set => Text.text = value.ToString();
    }
}

public class TwoValueRenderer : MonoBehaviour
{
    public TextMeshProUGUI Text = default!;
    private int _value = 10;
    private int _baseValue = 10;
    public int Value
    {
        set
        {
            _value = value;
            Text.text = $"{_value}/{_baseValue}";
        }
    }
    public int BaseValue
    {
        set
        {
            _baseValue = value;
            Text.text = $"{_value}/{_baseValue}";
        }
    }
}