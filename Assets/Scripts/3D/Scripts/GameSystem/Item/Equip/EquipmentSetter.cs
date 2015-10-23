using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EquipmentSetter : MonoBehaviour {

    public string itemName = "";

    void Start() {
        var item = new EquipmentItemRef(itemName);
        item.SetTo(gameObject);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            GetComponentInChildren<Animator>().CrossFade("Give", 0.1f);
        }
    }

}
