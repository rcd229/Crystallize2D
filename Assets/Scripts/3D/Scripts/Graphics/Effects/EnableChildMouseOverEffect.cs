using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EnableChildMouseOverEffect : MonoBehaviour, IMouseOverEffect {

    public GameObject child;
    public GameObject target;

    void Start() {
        SetEnabled(false);
    }

    public void SetEnabled(bool enabled) {
        //Debug.Log("mouseover");
        child.SetActive(enabled);
        if (enabled) {
            target.GetComponent<Renderer>().material.color = Color.yellow;
        } else {
            target.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
