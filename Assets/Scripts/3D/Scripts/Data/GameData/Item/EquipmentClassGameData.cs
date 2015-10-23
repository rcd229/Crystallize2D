using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EquipmentClassGameData : UniqueNameGameData {

    public const string ResourcePath = "Equip/Class/";

    public EquipmentClassGameData() : base() {    }

    public EquipmentClassGameData(string name) : this() {
        this.Name = name;
    }

    public EquipmentClassResources GetResources() {
        return Resources.Load<EquipmentClassResources>(ResourcePath + Name);
    }

}
