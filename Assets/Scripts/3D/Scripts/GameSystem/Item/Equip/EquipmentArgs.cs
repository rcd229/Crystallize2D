using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EquipmentArgs {

    public string Item { get; set; }
    public GameObject Target { get; set; }

    public EquipmentArgs(string item, GameObject target) {
        this.Item = item;
        this.Target = target;   
    }

    public void Set() {
        //Debug.Log("Setting");
        new EquipmentItemRef(Item).SetTo(Target);
    }

}
