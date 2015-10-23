using UnityEngine;
using System.Collections;

public class JobArgs  {

	public JobTaskRef Data;
	public JobNPC NPC;
	public JobArgs(JobTaskRef jobTaskRef, JobNPC npc){
		Data = jobTaskRef;
		NPC = npc;
	}
}
