using UnityEngine;
using System.Collections;

namespace CrystallizeData{

	public class SakuraNPC : StaticSerializedQuestNPCGameData{
		#region implemented abstract members of StaticGameData
		
		protected override void PrepareGameData ()
		{
			isTest = true;
			PhraseSequence Greeting = GetPhrase("What can I do for you?");
			PhraseSequence Exit = GetPhrase("See you");
			Initialize(NPCID.Sakura, "Sakura");
			NPC.OverrideOverheadName = "Sakura";
			NPC.QuestIDs.Add (QuestTypeID.KnowSakuraQuest);
			NPC.QuestIDs.Add(QuestTypeID.SakuraFirstQuest);
			NPC.QuestIDs.Add(QuestTypeID.BecomeFriendWithAnna);
			SetEntryDialogue(new DialogueSequence(Greeting));
			SetExitDialogue(new DialogueSequence(Exit));
			AddQuestFlagRequirement(NPCQuestFlag.SakuraIntroduced);
		}
		
		#endregion
		
	}

	public class Tourist1 : StaticSerializedQuestNPCGameData{
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			isTest = true;
			PhraseSequence Greeting = GetPhrase("What can I do for you?");
			PhraseSequence Exit = GetPhrase("See you");
			Initialize(NPCID.Tourist1, "Tourist1");
			NPC.OverrideOverheadName = "Tourist1";
			NPC.QuestIDs.Add (QuestTypeID.KnowSakuraQuest);
			SetEntryDialogue(new DialogueSequence(Greeting));
			SetExitDialogue(new DialogueSequence(Exit));
			AddQuestFlagRequirement(NPCQuestFlag.SakuraIntroduced);
		}

		#endregion

	}

	public class Anna : StaticSerializedQuestNPCGameData{
		#region implemented abstract members of StaticGameData
		
		protected override void PrepareGameData ()
		{
			isTest = true;
			PhraseSequence Greeting = GetPhrase("Good Day");
			PhraseSequence Exit = GetPhrase("See you");
			Initialize(NPCID.Anna, "Anna");
			NPC.OverrideOverheadName = "Anna";
			NPC.QuestIDs.Add(QuestTypeID.SakuraFirstQuest);
			NPC.QuestIDs.Add(QuestTypeID.BecomeFriendWithAnna);
			SetEntryDialogue(new DialogueSequence(Greeting));
			SetExitDialogue(new DialogueSequence(Exit));
			AddQuestFlagRequirement(NPCQuestFlag.AnnaIntroduced);
		}
		
		#endregion
		
	}



}
