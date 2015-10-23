using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Xml.Serialization;

public class SpawnableGameData {
    [XmlIgnore]
    public List<INPCSpawner> SpawnableNPCs {get; private set;}

    public SpawnableGameData() {
        SpawnableNPCs = new List<INPCSpawner>();
    }

    public void AddSpawner(INPCSpawner spawnable) {
        SpawnableNPCs.Add(spawnable);
    }
}
