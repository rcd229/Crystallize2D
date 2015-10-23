using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class FindCatNPC : StaticSerializedQuestNPCGameData {
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			Initialize(NPCID.FindCatNPC, "FindCatNPC");
			NPC.OverrideOverheadName = "A worried person";
			NPC.QuestIDs.Add(QuestTypeID.FindCatQuest);
			SetEntryDialogue(new DialogueSequence(new PhraseSequence("Good Day")));
			SetExitDialogue(new DialogueSequence(new PhraseSequence("Good Bye")));
		}

		#endregion

	}

	public class FeelHungryNPC : StaticSerializedQuestNPCGameData{
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			Initialize(NPCID.FeelHungryNPC, "FeelHungryNPC");
			NPC.OverrideOverheadName = "A hungry person";
			NPC.QuestIDs.Add(QuestTypeID.FindCatQuest);
			NPC.QuestIDs.Add(QuestTypeID.PersonHungryQuest);
			NPC.QuestIDs.Add(QuestTypeID.KnowSakuraQuest);
			SetEntryDialogue(new DialogueSequence(new PhraseSequence("Hello")));
			SetExitDialogue(new DialogueSequence(new PhraseSequence("See you")));
			AddQuestFlagRequirement(NPCQuestFlag.CatColorKnown);
		}

		#endregion

	}

}
