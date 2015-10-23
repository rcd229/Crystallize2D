using UnityEngine;
using System.Collections;

[SerializedQuestAttribute]
public class KnowSakuraQuest01 : LinearConversationQuest {

	public const string askLetter = "ask letter";
	public const string suggestSakura = "Suggest Sakura";
	public const string learnWords = "Learn Words";
	public const string talkToSakura = "talk to Sakura";
	public const string askSakura = "Ask Sakura for letter";

	NPCID hungryPerson = NPCID.FeelHungryNPC;
	NPCID Sakura = NPCID.Sakura;
	NPCID Tourist = NPCID.Tourist1;

	public DialogueSequence AskHungryPersonLetter{
		get{
			var b = set.GetDialogueBuilder();
			b.AddLine("I have given my letter");
			b.AddLine("You can look for Sakura");
			
			b.AddEvent(() => {
				QuestUtil.SetViewed(QuestTypeID.KnowSakuraQuest);
				QuestUtil.SetQuestState(QuestTypeID.KnowSakuraQuest, suggestSakura);
				QuestUtil.RaiseFlags(NPCQuestFlag.SakuraIntroduced);
			});
			
			
			b.AddLine("Learn some words before you talk to her");
			return b.Build();
		}
	}

	public DialogueSequence TouristDialogue{
		get{
			var b = set.GetDialogueBuilder();
			BranchRef b1 = new BranchRef();
			BranchRef b2 = new BranchRef();
			BranchRef b3 = new BranchRef();
			BranchRef b4 = new BranchRef();
			b1.Prompt = GetPhrase("Hello");
			b2.Prompt = GetPhrase("What is your name");
			b3.Prompt = GetPhrase("Nice to meet you");
			b4.Prompt = GetPhrase("Thank you for teaching me");
			b.AddBranch(false, true, false, b1, b2, b3, b4);
			
			b1.Index = b.AddLine("Can I help you");
			b.Break();
			b2.Index = b.AddLine("I am Nazu");
			b.Break();
			b3.Index = b.AddLine("Nice to meet you");
			b.Break();
			
			b4.Index = b.AddLine("No problem");
			b.AddEvent(() => QuestUtil.SetQuestState(QuestTypeID.KnowSakuraQuest, learnWords));
			return b.Build();
		}
	}



	PhraseSequence GetPhrase(string text){
		return set.GetPhrase(text);
	}

	public KnowSakuraQuest01(){
		builderName = "KnowSakuraQuest01";

		stateMachine.SetStates(askLetter, suggestSakura, learnWords, talkToSakura, askSakura);
		
		stateMachine.SetCurrentNPC(NPCID.FeelHungryNPC);
		var askForLetter = GetPhrase("Can you give me a recommendation letter?");
		stateMachine.AddDialogueStateToCurrent(askLetter, askForLetter, AskHungryPersonLetter);
		
		stateMachine.SetCurrentNPC(NPCID.Sakura);
		var greeting = GetPhrase("Nice to meet you");
		stateMachine.AddDialogueStateToCurrent(learnWords, greeting, helperCreateDialogue("Nice to meet you", talkToSakura));
		stateMachine.AddDialogueStateToCurrent(talkToSakura, askForLetter, helperCreateDialogue("I need your help before I can decide", askSakura));
		var yes = GetPhrase("I would like to");
		var no = GetPhrase("Sorry");
		stateMachine.AddDialogueStateToCurrent(askSakura, yes, helperCreateDialogue("very well", EndFunc(), UnlockQuestEvent(QuestTypeID.SakuraFirstQuest)));
		stateMachine.AddDialogueStateToCurrent(askSakura, no, helperCreateDialogue("I can't just give you the letter then", talkToSakura));
		
		stateMachine.SetCurrentNPC(NPCID.Tourist1);
		var askTeach = GetPhrase("Let's talk");
		stateMachine.AddDialogueStateToCurrent(suggestSakura, askTeach, TouristDialogue);
	}

	#region implemented abstract members of BaseQuestGameData
	public override QuestTypeID ID {get{return QuestTypeID.KnowSakuraQuest;}}
	public override string QuestName {get{return "get to know Sakura";}}
	#endregion

	#region implemented abstract members of ConversationQuest
	public override DialogueSequence GetIntroductionForState (NPCID npcID, QuestTypeID questID, string state)
	{
		return null;
	}
	public override System.Collections.Generic.IEnumerable<QuestDialogueState> GetDialoguesForState (NPCID npcID, QuestTypeID questID, string state)
	{
		if((npcID == hungryPerson || npcID == Sakura || npcID == Tourist) && questID == ID){
			return stateMachine.GetDialoguesForState(npcID, questID, state);
		}
		return new QuestDialogueState[0];
	}
	#endregion

	
}
