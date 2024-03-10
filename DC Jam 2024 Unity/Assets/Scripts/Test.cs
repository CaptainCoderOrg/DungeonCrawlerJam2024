using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using NaughtyAttributes;
using CaptainCoder.Dungeoneering.DungeonMap.IO;
using CaptainCoder.Dungeoneering.DungeonMap;

[CreateAssetMenu(fileName = "Map", menuName = "DCJam/MapData")]
public class MapData : ScriptableObject
{
    [field: SerializeField]
    public TextAsset WallData { get; private set; }

    [Button("Load Data")]
    public void DoThing()
    {
        WallMap data = JsonExtensions.LoadModel<WallMap>(WallData.text);
        foreach (var el in data.Map)
        {
            Debug.Log(el.Key + ": " + el.Value);
        }
    }

}
