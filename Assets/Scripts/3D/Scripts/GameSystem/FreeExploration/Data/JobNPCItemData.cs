using UnityEngine;
using System.Collections;

public class JobNPCItemData : ISerializableDictionaryItem<string> {

	public TieredDialogue Greetings {get; set;}
	public string ConsumerName {get;set;}

	#region ISerializableDictionaryItem implementation
	public string Key {
		get {
			return ConsumerName;
		}
	}
	#endregion

	public JobNPCItemData (){
		ConsumerName = "";
		Greetings = new TieredDialogue();
	}

	public JobNPC Get(JobNPC npc){
		npc.Greetings = Greetings;
		npc.CharacterName = ConsumerName;
		return npc;
	}
}
