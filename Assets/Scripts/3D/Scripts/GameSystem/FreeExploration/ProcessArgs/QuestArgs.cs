using UnityEngine;
using System.Collections;

public class QuestArgs {
	public GameObject NPCTarget{get;set;}
	public QuestNPCItemData NPC {get;set;}
	public QuestArgs(GameObject target, QuestNPCItemData npc){
		NPCTarget = target;
		NPC = npc;
	}
}
