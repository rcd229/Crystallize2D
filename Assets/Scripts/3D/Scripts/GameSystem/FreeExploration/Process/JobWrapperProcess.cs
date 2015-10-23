using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class JobWrapperProcess : IProcess<JobArgs, JobTaskExitArgs> {

	#region IInitializable implementation
	JobNPC NPC;
	DialogueSequence dialogue;
	JobTaskRef jobTaskRef;

	ProcessFactoryRef<JobTaskRef, JobTaskExitArgs> Job = new ProcessFactoryRef<JobTaskRef, JobTaskExitArgs>();

	public void Initialize (JobArgs param1)
	{
		NPC = param1.NPC;
		dialogue = param1.NPC.GetGreeting();
		jobTaskRef = param1.Data;
		ProcessLibrary.BeginConversation.Get(new ConversationArgs(param1.NPC.gameObject, param1.NPC.GetGreeting()), ConvSeg, this);

	}

	public void ConvSeg(object sender, object arg){
		ProcessLibrary.ConversationSegment.Get(new ConversationArgs(NPC.gameObject, dialogue), EndCov, this);
	}

	public void EndCov(object sender, object arg){
		ProcessLibrary.EndConversation.Get(ConversationArgs.ExitArgs(NPC.gameObject, dialogue), BeginJob, this);
	}

	public void BeginJob(object sender, object arg){

		//TODO what is a good way to manage NPC name. For now the first actor is the NPC talked to
		jobTaskRef.Data.Dialogues = jobTaskRef.Data.Dialogues.Select(s => {s.Actors[0] = new SceneObjectGameData(NPC.name); return s;}).ToList();
		jobTaskRef.Data.Actor = new SceneObjectGameData(NPC.name);
		//TODO temporary. should have a different way to deal with promotion
		Job.Set (jobTaskRef.Data.ProcessType.ProcessType);
		Job.Get(jobTaskRef, FinishJob, this);
	}

	public void FinishJob(object sender, JobTaskExitArgs arg){
//		NPC.ConversationEnds();
		Exit(arg);
	}
	#endregion

	#region IProcess implementation

	public void ForceExit ()
	{
		Exit (null);
	}
	

	public ProcessExitCallback OnExit {get;set;}


	#endregion

	void Exit(JobTaskExitArgs arg){
		GameObject.Destroy(NPC.gameObject);
		OnExit.Raise(this, arg);
	}


}
