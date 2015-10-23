using UnityEngine;
using System.Collections;

public class NPCGameData  {

	public QuestNPCGameData QuestNPCs {get;set;}
	public JobNPCGameData JobNPCs {get;set;}

	public NPCGameData(){
		QuestNPCs = new QuestNPCGameData();
		JobNPCs = new JobNPCGameData();
	}
}
