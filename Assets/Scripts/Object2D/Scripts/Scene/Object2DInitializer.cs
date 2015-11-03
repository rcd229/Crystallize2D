using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class Object2DInitializer : MonoBehaviour {
    void Start() {
        Object2DSceneResourceManager.Instance.LoadAll();
    }
}
