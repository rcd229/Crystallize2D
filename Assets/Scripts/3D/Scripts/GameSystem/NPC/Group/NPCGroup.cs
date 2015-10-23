using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DialogueBuilder;

public class SerializedNPCGroupsAttribute : Attribute { }

public class NPCGroup : ContainsDialogueUniqueIDData<NPCGroup> {

    public static IEnumerable<NPCGroup> FindAll() {
        return GameDataInitializer.npcGroups.Values;
    }

    static NPCGroup() {
        int count = 0;
        var staticTypes = from t in Assembly.GetAssembly(typeof(NPCGroup)).GetTypes()
                          where Attribute.IsDefined(t, typeof(SerializedNPCGroupsAttribute))
                          select t;
        foreach (var t in staticTypes) {
            foreach (var f in t.GetFieldsAndProperties<NPCGroup>(BindingFlags.Static | BindingFlags.Public)) {
                f.GetMemberValue(null);
                count++;
            }
        }

        //var instanceTypes = from t in Assembly.GetAssembly(typeof(NPCGroup)).GetTypes()
        //                    where Attribute.IsDefined(t, typeof(SerializedNPCGroupsAttribute)) && t.GetConstructor(Type.EmptyTypes) != null
        //                    select t;
        //foreach (var t in staticTypes) {
        //    foreach (var f in t.GetFieldsAndProperties<NPCGroup>(BindingFlags.Static | BindingFlags.Public)) {
        //        f.GetMemberValue(null);
        //        count++;
        //    }
        //}
        Debug.Log("NPCGroup count: " + count);
    }

    public DialogueSequence Dialogue { get; private set; }
    public NPCGroupFormation Formation { get; private set; }
    public bool AlwaysAvailable { get; private set; }
    public AppearancePlayerData[] Appearances { get; private set; }

    public NPCGroup(string guid, string name, params Element[] elements)
        : this(guid, name, null, elements) { }

    public NPCGroup(string guid, string name, NPCGroupFormation formation, params Element[] elements)
        : this(guid, name, formation, false, elements) { }

    public NPCGroup(string guid, string name, NPCGroupFormation formation, bool alwaysAvailable, params Element[] elements)
        : base(name, new Guid(guid)) {
        Dialogue = BuildDialogue(elements);
        AlwaysAvailable = alwaysAvailable;

        if (formation == null) {
            Formation = NPCGroupFormation.DialoguePair;
        } else {
            Formation = formation;
        }

        var actorCount = GetActorCount(elements);
        Appearances = new AppearancePlayerData[actorCount];
        for (int i = 0; i < Appearances.Length; i++) {
            Appearances[i] = AppearancePlayerData.GetRandom();
        }
    }

    int GetActorCount(Element[] elements) {
        int count = 0;
        foreach (var e in elements) {
            count = Math.Max(count, e.actor + 1);
        }
        return count;
    }

}
