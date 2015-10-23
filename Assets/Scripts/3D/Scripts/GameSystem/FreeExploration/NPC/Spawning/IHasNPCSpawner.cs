using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IHasNPCSpawner {
    INPCSpawner Spawner { get; }
}
