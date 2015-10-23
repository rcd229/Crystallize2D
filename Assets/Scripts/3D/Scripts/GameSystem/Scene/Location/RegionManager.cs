using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class RegionManager : MonoBehaviour {

    public static RegionManager Instance { get; private set; }

    public Region CurrentRegion { get; private set; }

    void Start() {
        Instance = this;
    }

    void OnDestroy() {
        Instance = null;
    }

    public void SetRegion(Region region) {
        CurrentRegion = region;
    }

}
