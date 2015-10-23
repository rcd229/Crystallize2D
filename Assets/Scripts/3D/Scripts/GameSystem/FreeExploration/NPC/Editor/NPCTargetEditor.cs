using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(NPCTarget))]
public class NPCTargetEditor : Editor {

    Dictionary<string, Guid> npcIDs = new Dictionary<string, Guid>();
    string[] availableNames = new string[0];
	Dictionary<string, Guid> questIDs = new Dictionary<string, Guid>();
	string[] quests = new string[0];

    void OnEnable() {
        GameDataInitializer.npcs.Where(kv => kv.Value.IsStatic == true).ForEach(i => npcIDs[i.Key] = i.Value.ID.guid);
        GameDataInitializer.npcGroups.ForEach(i => npcIDs[i.Key] = i.Value.guid);
        GenericNPCTarget.GetValues().ForEach(i => npcIDs["Generic/" + i.Name] = i.guid);

		setAvailableNames(npcIDs.Keys);
		
		//pull all quests
		SerializedQuestExtensions.GetAllQuests().ForEach(s => questIDs[s.QuestName] = s.ID.guid);
		var questList = questIDs.Keys.OrderBy(s => s).ToList();
		questList.Insert(0, "Choose a quest...");
		questList.Insert(1, "Clear quest selection...");
		quests = questList.ToArray();
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        var questIndex = EditorGUILayout.Popup(0, quests);
		if(questIndex == 1){
			setAvailableNames(npcIDs.Keys);
		}
		else if (questIndex != 0) {
			var q = SerializedQuestExtensions.GetAllQuests().Where(s => s.ID.guid.Equals(questIDs[quests[questIndex]])).FirstOrDefault();
			var instanceNPCs = (from f in q.GetType().GetFieldsAndProperties<QuestNPCItemData>(BindingFlags.Public | BindingFlags.Instance)
			                    select f.GetMemberValue(q) as QuestNPCItemData).Select(i => i.ID.guid).ToList();
			var npcFieldsInQuest = (from f in q.GetType().GetFieldsAndProperties<NPCID>(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
			                        select f.GetMemberValue(q) as NPCID).Select(s => s.guid);
			instanceNPCs.AddRange(npcFieldsInQuest);

			setAvailableNames(npcIDs.Where(s => instanceNPCs.Contains(s.Value)).Select(s => s.Key));
		}
		var index = EditorGUILayout.Popup(0, availableNames);
        if (index != 0) {
            string name = GetName(availableNames[index]);
            (target as NPCTarget).SetGuid(npcIDs[availableNames[index]]);
            (target as Component).gameObject.name = name;

            EditorUtility.SetDirty(target);
        }
    }

    string GetName(string path) {
        var split = path.Split('/');
        return split[split.Length - 1];
    }

	void setAvailableNames(IEnumerable<string> names){
		var list = names.ToList();
		list.OrderBy(s => s);
		list.Insert(0, "Choose a name...");
		availableNames = list.ToArray();
	}

}
