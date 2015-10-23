using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class NPCGroupSpawner : INPCSpawner {
    const int MaxGroupCount = 1;

    public bool CanSpawn(SpawnNPCContext context) {
        int count = 0;
        foreach (var npc in context.CurrentNPCs) {
            if (npc.GetComponent<SceneNPCGroup>()) {
                count++;
            }
        }
        return count < MaxGroupCount;
    }

    public GameObject SpawnNPC(SpawnNPCContext context) {
        var group = StandardNPCGroup.GetValues().GetRandomFromEnumerable();
            //NPCGroup.GetValues().ToArray().GetRandom();
        return NPCManager.Instance.SpawnNPCGroup(context.Point, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0), group);
    }
}
