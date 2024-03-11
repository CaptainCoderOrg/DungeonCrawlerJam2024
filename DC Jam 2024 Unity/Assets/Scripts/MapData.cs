using UnityEngine;
using NaughtyAttributes;
using CaptainCoder.Dungeoneering.DungeonMap.IO;
using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.DungeonMap.Unity;
using UnityEngine.ProBuilder;

[CreateAssetMenu(fileName = "Map", menuName = "DCJam/MapData")]
public class MapData : ScriptableObject
{
    [field: SerializeField]
    public TextAsset WallData { get; private set; }
    [field: SerializeField]
    public Material WallMaterial { get; private set; }

    [Button("Load Data")]
    public void DoThing()
    {
        WallMap data = JsonExtensions.LoadModel<WallMap>(WallData.text);
        ProBuilderMesh mesh = MapGeneratorExtensions.ToProBuilderMesh(data);
        mesh.SetMaterial(mesh.faces, WallMaterial);
        mesh.name = "New Map";
        // mesh.Refresh();
        // mesh.ToMesh();
    }

}
