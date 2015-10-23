using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneNPC : SceneGuid<NPCID> {

    public QuestNPCItemData GetNPCData() {
        return GameData.Instance.NPCs.QuestNPCs.Get(ID);
    }

    void Start() {
        gameObject.AddComponent<QuestNPC>().Initialize(GetNPCData());
    }


    public override NPCID ID {
        get { return new NPCID(Guid); }
    }
}
