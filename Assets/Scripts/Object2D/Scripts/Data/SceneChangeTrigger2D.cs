using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneChangeTrigger2D : Object2D, IHasTrigger {
    public string Scene { get; set; }
    public Guid Target { get; set; }
}
