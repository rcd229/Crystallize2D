using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SunInstance : MonoBehaviour {

    public static SunInstance Instance { get; private set; }

    void Awake() {
        Instance = this;
    }

    public void SetNight() {
        transform.rotation = Quaternion.Euler(-90f, 0, 0);
    }

    public void SetMorning() {
        transform.rotation = Quaternion.Euler(5f, 180f, 180f);
    }

}
