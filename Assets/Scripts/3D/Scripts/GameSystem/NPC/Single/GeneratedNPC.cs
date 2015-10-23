using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DialogueBuilder;

public class GeneratedNPC : ContainsDialogueUniqueIDData<GeneratedNPC> {

    static Dictionary<string, int> counts = new Dictionary<string, int>();

    public static QuestNPCItemData GetNew(Guid id, bool hasName = false, string name = null, bool isStatic = false, QuestTypeID questID = null) {
        var npc = new QuestNPCItemData();
        npc.ID = new NPCID(id);
        var charData = NPCCharacterData.GetRandom();
        if (name == null) {
            if (charData.Appearance.Gender == 0) {
                if (hasName) {
                    charData.Name = RandomNameGenerator.GetRandomMaleName();
                } else {
                    charData.Name = "Boy";
                }
            } else {
                if (hasName) {
                    charData.Name = RandomNameGenerator.GetRandomFemaleName();
                } else {
                    charData.Name = "Girl";
                }
            }
        } else {
            charData.Name = name;
        }
        npc.CharacterData = charData;
        npc.IsStatic = isStatic;
        npc.QuestID = questID;
        return npc;
    }

    static Element GetElement(string prompt, params string[] responses) {
        var list = new List<Element>();
        foreach (var r in responses) {
            list.Add(new LineElement(r));
        }
        return new BranchElement(false, new PromptResponsePair(prompt, false, list.ToArray()));
    }

    static string GetCountString(string s) {
        if (!counts.ContainsKey(s)) {
            counts[s] = 0;
        }
        counts[s]++;
        return s + counts[s];
    }

    public readonly QuestNPCItemData NPC;
    public readonly int Level = 0;

    public GeneratedNPC(string name, Guid id, int level, params Element[] elements)
        : base(name, id) {
        this.Level = level;
        this.NPC = GetNew(id);
        this.NPC.EntryDialogue = BuildDialogue(elements);
    }

    public GeneratedNPC(int level, string prompt, params string[] responses)
        : this(level, GetElement(prompt, responses)) { }

    public GeneratedNPC(int level, params Element[] elements)
        : this(GetCountString(elements[0].ToString()), Guid.NewGuid(), level, elements) { }

}
