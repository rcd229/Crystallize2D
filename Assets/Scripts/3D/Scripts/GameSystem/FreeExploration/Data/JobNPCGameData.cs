using UnityEngine;
using System.Collections;

public class JobNPCGameData : SerializableDictionary<string, JobNPCItemData> {
	public static JobNPCItemData DefaultJobNPCGameData{
		get{
			JobNPCItemData data = new JobNPCItemData();
			data.ConsumerName = "JobNPC";
			data.Greetings.Dialogues.Add(new DialogueSequence(new PhraseSequence("Hello")));
			data.Greetings.Tiers.Add(1);
			return data;
		}
	}
}
