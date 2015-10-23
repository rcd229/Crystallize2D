using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestNPCItemPlayerData : NPCItemPlayerData{

	//each List<int> represents a node in the permanent unlock quest dialogue
	//each int represents the index of the child in the children list
	//TODO when making changes to the quest dialogue previous records may become invalid
	public List<List<int>> PermanentUnlock{get;set;}
}
