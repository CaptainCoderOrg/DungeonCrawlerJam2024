using UnityEngine;
using UnityEngine.Tilemaps;

namespace CaptainCoder.DungeonCrawler.Combat.Unity;

[CreateAssetMenu(fileName = "CombatIconDatabase", menuName = "Data/CombatIconDatabase")]
public class CombatIconDatabase : ScriptableObject
{
    [field: SerializeField]
    public IconEntry[] Entries { get; set; } = [];
    public TileBase Wall = default!;
    public TileBase Floor = default!;
    public TileBase DefaultIcon = default!;
    public TileBase Selected = default!;
    public TileBase Green = default!;
    public TileBase Outline = default!;
    public TileBase Yellow = default!;

    public TileBase GetTile(PlayerCharacter playerCharacter) =>
        Entries.FirstOrDefault(e => e.Name == playerCharacter.Card.Name)?.Icon ?? DefaultIcon;
}

[Serializable]
public class IconEntry
{
    public string Name = default!;
    public TileBase Icon = default!;
}