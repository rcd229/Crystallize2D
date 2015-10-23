using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EquipmentItemGameData : UniqueNameGameData {

    public const string ResourcePath = "Equip/Item/";

    public string Class { get; set; }
    public string Effect { get; set; }

    public EquipmentItemGameData() : base() {
        Class = "";
    }

    public EquipmentItemGameData(string name, string cls) : this() {
        Name = name;
        Class = cls;
    }

    public bool HasPrefab() {
        return GameObjectUtil.ResourceExists(ResourcePath + Name);
    }

    public GameObject GetPrefab() {
        return Resources.Load<GameObject>(ResourcePath + Name);
    }

}
