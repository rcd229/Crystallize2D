using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class NPCManager {

    static NPCManager _instance;

    public static NPCManager Instance {
        get {
            if (_instance == null) {
                _instance = new NPCManager();
            }
            return _instance;
        }
    }

    public GameObject GetNPC(NPCID npcID) {
        var npcs = GameObject.FindObjectsOfType<QuestNPC>();
        return (from npc in npcs
                where npc.NPC.ID == npcID
                select npc.gameObject).FirstOrDefault();
    }

    //spawn NPC's in targets, if NPC's requirements are fulfilled and it has not been spawned
    public void SpawnNPCs() {
        var npcs = GameData.Instance.NPCs.QuestNPCs.UnlockedItems;
        foreach (var npc in npcs) {
            var target = NPCTarget.Get(npc.ID.guid);
            if (target != null && !target.Spawned) {
                var go = SpawnNPC(npc, target.gameObject.transform.position);
                target.NPC = go;
            }
        }

        var groups = NPCGroup.FindAll().Where(g => g.AlwaysAvailable);
        foreach (var g in groups) {
            var target = NPCTarget.Get(g.guid);
            if (target != null && !target.Spawned) {
                target.NPC = SpawnNPCGroup(target.transform, g);
            }
        }
    }

    public GameObject SpawnNPCGroup(Transform target, NPCGroup group) {
        return SpawnNPCGroup(target.position, target.rotation, group);
    }

    public GameObject SpawnNPCGroup(Vector3 position, NPCGroup group) {
        return SpawnNPCGroup(position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0), group);
    }

    public GameObject SpawnNPCGroup(Vector3 position, Quaternion rotation, NPCGroup group) {
        var slots = group.Formation.GetInstance().transform;
        slots.SetTransform(position, rotation);
        slots.name = group.Name;
        for (int i = 0; i < slots.childCount; i++) {
            var app = AppearancePlayerData.GetRandom();
            if (i < group.Appearances.Length) { app = group.Appearances[i]; }
            var a = DialogueActorUtil.GetNewActor(app.GetResourceData());
            a.transform.SetTransform(slots.GetChild(i));
            a.GetComponent<Rigidbody>().isKinematic = true;
            a.GetComponent<DialogueActor>().canReduceConfidence = false;
            CoroutineManager.Instance.WaitAndDo(() => a.transform.parent = slots);
        }
        slots.gameObject.GetOrAddComponent<SceneNPCGroup>().Initialize(group);
        slots.gameObject.GetOrAddComponent<IndicatorComponent>().Initialize(
            "", new OverheadIcon(IconType.SpeechBubble), new MapIndicator(MapResourceType.Standard), false);
        return slots.gameObject;
    }

    public GameObject SpawnNPC(NPCID npcID, NPCTarget target, bool checkAvailable = true) {
        if (target != null) {
            var go = SpawnNPC(npcID, target.transform.position, target.transform.rotation, checkAvailable);
            target.NPC = go;
            return go;
        }
        return null;
    }

    public GameObject SpawnNPC(NPCID npcID, Vector3 position, bool checkAvailable = true) {
        return SpawnNPC(npcID, position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0), checkAvailable);
    }

    public GameObject SpawnNPC(NPCID npcID, Vector3 position, Quaternion rotation, bool checkAvailable = true) {
        var npc = GameData.Instance.NPCs.QuestNPCs.Items.Where(s => s.ID == npcID).FirstOrDefault();
        return SpawnNPC(npc, position, rotation);
    }

    public GameObject SpawnNPC(QuestNPCItemData npc, NPCTarget target, bool checkAvailable = true) {
        var available = GameData.Instance.NPCs.QuestNPCs.UnlockedItems.Where(s => s.ID == npc.ID).FirstOrDefault() != null;
        if ((checkAvailable && available) || !checkAvailable) {
            var go = SpawnNPC(npc, target.transform.position, target.transform.rotation);
            target.NPC = go;
            return go;
        } else {
            return null;
        }
    }

    public GameObject SpawnNPC(QuestNPCItemData npc, Vector3 position, Quaternion rotation = default(Quaternion), bool checkAvailable = true) {
        if (npc != null) {
            var app = AppearanceLibrary.GetRandomAppearance();
            if (npc.CharacterData != null) {
                app = npc.CharacterData.Appearance.GetResourceData();
            }
            var go = DialogueActorUtil.GetNewActor(app, npc.Name);

            go.AddComponent<QuestNPC>().Initialize(npc);
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.GetComponent<Rigidbody>().isKinematic = true;
            return go;
        }
        return null;
    }


    public void RemoveNPC(GameObject npc) {
        GameObject.Destroy(npc);
    }

    public void SpawnNPCDelegate(object sender, object arg) {
        SpawnNPCs();
    }
}
