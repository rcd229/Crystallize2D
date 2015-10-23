using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface INPCSpawner {
    bool CanSpawn(SpawnNPCContext context);
    GameObject SpawnNPC(SpawnNPCContext context);
}
