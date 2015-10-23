using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Job NPC are purely for giving you things to do in a job, such as asking to buy things, etc
/// They can still have familiarity with you so that they tip you more (for example)
/// 
/// Serializable
/// </summary>
public class JobNPC : WorldNPC {

	public static string ResourcePath{
		get{
			return "FreeExploration/NPC/JobNPC";
		}
	}
	public TieredDialogue Greetings {get; set;}
	public event EventHandler<EventArgs<object>> OnDestroyed;

	public JobNPC (){
		Greetings = new TieredDialogue();
	}

	public DialogueSequence GetGreeting(){
		return Greetings.GetDialogue();
	}

	void OnDestroy(){
		OnDestroyed.Raise(this, null);
	}


}
