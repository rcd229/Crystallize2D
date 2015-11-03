using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ShadowSetter : MonoBehaviour {

    void Start() {
        foreach (var r in GetComponentsInChildren<MeshRenderer>()) {
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows = false;
        }
    }

}
