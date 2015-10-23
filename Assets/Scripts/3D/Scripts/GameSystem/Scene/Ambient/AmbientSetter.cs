using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class AmbientSetter : MonoBehaviour {

    public float ambientIntensity = 1f;
    public bool setColor = false;
    public Color ambientColor = Color.white;

    public void Start() {
        RenderSettings.ambientIntensity = ambientIntensity;
        if (setColor) {
            RenderSettings.ambientLight = ambientColor;
        }
    }

}
