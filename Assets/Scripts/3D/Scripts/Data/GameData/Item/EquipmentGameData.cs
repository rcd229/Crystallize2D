using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EquipmentGameData {

    public EquipmentClassCollectionGameData Classes { get; set; }
    public EquipmentItemCollectionGameData Items { get; set; }

    public EquipmentGameData() {
        Classes = new EquipmentClassCollectionGameData();
        Items = new EquipmentItemCollectionGameData();
    }

}
