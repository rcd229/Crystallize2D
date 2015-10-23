using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

public class NPCPlacingEditor : EditorWindow { 

	Dictionary<string, Guid> npcIDs = new Dictionary<string, Guid>();
	string[] availableNames = new string[0];
	Dictionary<string, Guid> questIDs = new Dictionary<string, Guid>();
	string[] quests = new string[0];

	int npcPopUpIndex = 0;
	string scenepath = "OutdoorSchoolSession";
	string resourcePath = "QuestNPCTarget";

	[MenuItem("Utils/NPC")]
	public static void Open(){

		var window = GetWindow<NPCPlacingEditor> ();
		window.PreProcess();
	}

	void PreProcess(){
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
	
	void OnGUI(){
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
		int selected = EditorGUILayout.Popup(0, availableNames);
		npcPopUpIndex = selected == 0 ? npcPopUpIndex : selected;
		if(GUILayout.Button("Place Target")){
			if (npcPopUpIndex != 0) {
				Debug.Log("generating");
				generateTarget(npcPopUpIndex);
			}
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

	void generateTarget(int index){
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		EditorApplication.OpenScene(scenepath);
		var target = GameObjectUtil.GetResourceInstance<NPCTarget>(resourcePath);
		target.gameObject.transform.position = Vector3.zero;

		string name = GetName(availableNames[index]);
		(target as NPCTarget).SetGuid(npcIDs[availableNames[index]]);
		(target as Component).gameObject.name = name;
		target.gameObject.transform.SetParent(GameObject.Find("QuestNPCs").transform);
	}
	
}
