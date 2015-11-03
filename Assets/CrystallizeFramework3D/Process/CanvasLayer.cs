using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public enum CanvasLayer {
    None = 0,
    Default  = 100,
    Dialogue = 200
}

public enum CanvasBranch {
    Root = 0,
    None = 500,
    Persistent = 100,
    Default = 200,
    Tutorial = 10000
}
