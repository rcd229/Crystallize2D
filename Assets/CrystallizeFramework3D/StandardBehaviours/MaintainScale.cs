using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MaintainScale : MonoBehaviour {
    public Vector3 scale = Vector3.one;
    
    [ExecuteInEditMode]
    void Update() {
        var s = transform.parent.localScale;
        transform.localScale = new Vector3(scale.x * s.x, scale.y * s.y, scale.z * s.z);
    }
}
