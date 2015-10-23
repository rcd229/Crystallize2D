using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ThingInventoryData {
    public List<ThingTemplate2D> Things { get; private set; }

    public ThingInventoryData() {
        Things = new List<ThingTemplate2D>();
    }
}
