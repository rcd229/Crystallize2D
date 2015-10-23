using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SerializedQuestAttribute]
public class SakuraFirstQuest01 : LinearConversationQuest {

	NPCID Anna = NPCID.Anna;
	NPCID Sakura = NPCID.Sakura;

	#region implemented abstract members of BaseQuestGameData
	public override QuestTypeID ID {get {return QuestTypeID.SakuraFirstQuest;}}
	public override string QuestName {get{return "Help Sakura";}}
	#endregion

	public const string Get = "Get Quest";
	public const string GetAnnaPreference = "Get Anna Preference";
	public const string MeetAnna = "Meet Anna";
	//in between has anna quest
	public const string BefriendAnna = "Befriend Anna";
	public const string KnowPresent = "KnowPresent";
	public const string Done = "Done";

	PhraseSequence whatToDo {get{return set.GetPhrase("What should I do?");}}
	PhraseSequence getIt {get{return set.GetPhrase("Sure, let me do it");}}
	PhraseSequence tips {get{return set.GetPhrase("What does she like?");}}
	PhraseSequence greet {get{return set.GetPhrase("How do you do");}}
	PhraseSequence beFriends {get{return set.GetPhrase("Are we friends now?");}}
	PhraseSequence askPresent {get{return set.GetPhrase("What birthday Present Do you want?");}}
	PhraseSequence sheWantsCake {get{return set.GetPhrase("Anna wants you to buy her a cake");}}

	public override int RewardMoney {get{return 1000;}}

	public DialogueSequence SakuraIntroduceAnna{
		get{
			var b = set.GetDialogueBuilder();
			b.AddLine("My friend Anna is having her birthday");
			b.AddLine("I want to know what present she wants");
			b.AddLine("Please help me to get this information");
			b.AddEvents(
				ActionDialogueEvent.Get(QuestUtil.SetQuestState, QuestTypeID.SakuraFirstQuest, GetAnnaPreference),
				ActionDialogueEvent.Get(QuestUtil.SetViewed, QuestTypeID.SakuraFirstQuest),
				ActionDialogueEvent.Get(QuestUtil.RaiseFlag, NPCQuestFlag.AnnaIntroduced)
				);
			return b.Build();
		}
	}

	public DialogueSequence AnnaAnswerPresent {
		get{
			var b = set.GetDialogueBuilder();
			b.AddLine("Thank you");
			b.AddLine("Anything is good");
			b.AddLine("But I do hope my friend Anna can buy me a cake");
			b.AddEvent(ActionDialogueEvent.Get(QuestUtil.SetQuestState, QuestTypeID.SakuraFirstQuest, Done));
			return b.Build();
		}
	}

	public DialogueSequence SakuraKnowPresent{
		get{
			var b = set.GetDialogueBuilder();
			b.AddLine("Thank you");
			b.AddLine("You saved me");
			b.AddLine("How ever you need a job for my letter of recommendation");
			b.AddLine("Find a job and come back to me please");
			b.AddEvent(ActionDialogueEvent.Get(QuestUtil.EndQuest, QuestTypeID.SakuraFirstQuest));
			return b.Build();
		}
	}

	public SakuraFirstQuest01(){
		builderName = "SakuraFirstQuest01";
		stateMachine.SetStates(Get, GetAnnaPreference, MeetAnna, BefriendAnna, KnowPresent, Done);
		
		stateMachine.SetCurrentNPC(NPCID.Sakura);
		stateMachine.AddDialogueStateToCurrent(Get, whatToDo, SakuraIntroduceAnna);

		stateMachine.AddDialogueStateToCurrent(GetAnnaPreference, getIt, helperCreateDialogue("Good luck then", MeetAnna));
		stateMachine.AddDialogueStateToCurrent(GetAnnaPreference, tips, helperCreateDialogue("She likes watching movies", MeetAnna));
		
		stateMachine.SetCurrentNPC(NPCID.Anna);
		stateMachine.AddDialogueStateToCurrent(MeetAnna, greet, helperCreateDialogue("How do you do", UnlockQuestEvent(QuestTypeID.BecomeFriendWithAnna)));

		stateMachine.AddDialogueStateToCurrent(BefriendAnna, beFriends, helperCreateDialogue("I guess so", KnowPresent));

		stateMachine.AddDialogueStateToCurrent(KnowPresent, askPresent, AnnaAnswerPresent);
		
		stateMachine.SetCurrentNPC(NPCID.Sakura);
		stateMachine.AddDialogueStateToCurrent(Done, sheWantsCake, SakuraKnowPresent);
	}

	#region implemented abstract members of ConversationQuest
	public override DialogueSequence GetIntroductionForState (NPCID npcID, QuestTypeID questID, string state)
	{
		return null;
	}
	public override IEnumerable<QuestDialogueState> GetDialoguesForState (NPCID npcID, QuestTypeID questID, string state)
	{
		if((npcID == Anna || npcID == Sakura) && questID == ID){
			return stateMachine.GetDialoguesForState(npcID, questID, state);
		}
		return new QuestDialogueState[0];
	}
	#endregion
	
}
