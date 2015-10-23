using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrystallizeData{
	public abstract class StaticSerializedJobNPCGameData : StaticSerializedGameData {

		protected JobNPCItemData NPC = new JobNPCItemData();
		#region implemented abstract members of StaticGameData


		#endregion

		#region implemented abstract members of StaticSerializedGameData

		protected override void AddGameData ()
		{
			GameData.Instance.NPCs.JobNPCs.Add(NPC);
		}


		#endregion

		protected void Initialize(string consumerName){
			NPC.ConsumerName = consumerName;
		}

		protected void AddDialogues(IEnumerable<DialogueSequence> Dialogues, IEnumerable<int> tiers){
			NPC.Greetings.Dialogues = Dialogues.ToList();
			NPC.Greetings.Tiers = tiers.ToList();
		}


	}
}
