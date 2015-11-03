using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ProcessInitializer2D : MonoBehaviour {
    void Start() {
        ProcessInitializer.Initialize();
        new GameProcess2D().Run(null, null, null);
    }
}
