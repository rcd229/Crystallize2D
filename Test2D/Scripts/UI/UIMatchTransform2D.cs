using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UIMatchTransform2D : MonoBehaviour {

    public Transform target;

    void Update() {
        transform.position = target.position;
    }

}
