using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

// if you want to store other kinds of information, you can make another class that derives from Object2D
// for example, "TreasureChest : Object2D" might store the amount of money that is inside and have a behaviour 
// that when you interact with it, it gives you that money
public class SceneChangeTrigger2D : Object2D {
    public string Scene { get; set; }
    public Guid Target { get; set; }
}
