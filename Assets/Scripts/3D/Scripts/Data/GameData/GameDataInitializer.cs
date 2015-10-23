using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrystallizeData;

public static class GameDataInitializer {

    public static Dictionary<string, List<string>> phraseSets = new Dictionary<string, List<string>>();
    public static HashSet<string> requiredResources = new HashSet<string>();

    public static Dictionary<string, QuestNPCItemData> npcs = new Dictionary<string, QuestNPCItemData>();
    public static Dictionary<string, NPCGroup> npcGroups;
    public static Dictionary<string, JobGameData> openJobs = new Dictionary<string, JobGameData>();

    static GameDataInitializer() {
        Initialize();
    }

    public static void AddPhrase(string setKey, string phraseKey) {
        if (!phraseSets.ContainsKey(setKey)) {
            phraseSets[setKey] = new List<string>();
        }

        if (!phraseSets[setKey].Contains(phraseKey)) {
            phraseSets[setKey].Add(phraseKey);
        }
    }

    public static void AddPhrase(string setKey, string phraseKey, int index) {
        if (!phraseSets.ContainsKey(setKey)) {
            phraseSets[setKey] = new List<string>();
        }

        while (phraseSets[setKey].Count <= index) {
            phraseSets[setKey].Add("");
        }
        phraseSets[setKey][index] = phraseKey;
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize() {
        //Debug.Log("Initializing code serialized GameData");//
        GameData.LoadInstance();
        var types = (from t in Assembly.GetAssembly(typeof(StaticSerializedGameData)).GetTypes()
                     where t.IsSubclassOf(typeof(StaticSerializedGameData)) && !t.IsAbstract
                     select t);
        foreach (var t in types) {
            var obj = (StaticSerializedGameData)Activator.CreateInstance(t);
            obj.ConstructGameData();
        }
    }

    public static void AddRequiredResource(string path) {
        requiredResources.Add(path);
    }

    public static void AddNPC(QuestNPCItemData npc) {
        var key = npc.TargetName;
        if (!key.Contains('/')) {
            key = "NPC/" + key;
        }
        npcs[key] = npc;
    }

    public static void AddNPCGroup(NPCGroup group) {
        if (npcGroups == null) {
            npcGroups = new Dictionary<string, NPCGroup>();
            foreach (var g in NPCGroup.GetValues()) {
                AddNPCGroup(g);
            }
        }

        var key = group.Name;
        if (!key.Contains('/')) {
            key = "Group/" + key;
        }
        npcGroups[key] = group;
    }

    public static void AddOpenJob(string name, JobGameData job) {
        openJobs[name] = job;
    }

}