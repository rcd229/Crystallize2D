using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class LostPersonNPC : StaticSerializedQuestNPCGameData{
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			Initialize(NPCID.LostPerson, "A lost person");
//			var lostPhrase = GetPhrase("I am lost");
//			SetEntryDialogue(new DialogueSequence(lostPhrase));
			var thank = GetPhrase("Thank you");
			SetExitDialogue(new DialogueSequence(thank));
			AddQuestFlagRequirement(NPCQuestFlag.NeverRaisedFlag);
		}

		#endregion


	}
}