using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationNPCSpawner : INPCSpawner {

    public bool CanSpawn(SpawnNPCContext context) {
        return true;
    }

    public GameObject SpawnNPC(SpawnNPCContext context) {
        var npc = GeneratedNPCs.NPCs.GetRandom().NPC;
        var instance = DialogueActorUtil.GetNewActor(npc.CharacterData.Appearance.GetResourceData());
        instance.AddComponent<QuestNPC>().Initialize(npc);
        instance.AddComponent<ConversationNPC>();
        return instance;
    }

}