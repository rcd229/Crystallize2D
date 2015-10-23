using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SpawnNPCContext {

    public Vector3 Point { get; private set; }
    public List<GameObject> CurrentNPCs { get; private set; }

    public SpawnNPCContext(Vector3 point, List<GameObject> currentNPCs) {
        Point = point;
        CurrentNPCs = currentNPCs;
    }

    public bool ContainsQuestNPC(NPCID npcID) {
        foreach (var npc in CurrentNPCs) {
            if (npc.GetComponent<QuestNPC>() && npc.GetComponent<QuestNPC>().NPC.ID == npcID) {
                return true;
            }
        }
        return false;
    }

}
