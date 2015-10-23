using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SerializedQuestAttribute]
public class PersonHungryQuest01 : LinearConversationQuest {

	NPCID hungryPerson = NPCID.FeelHungryNPC;
	#region implemented abstract members of BaseQuestGameData
	public override QuestTypeID ID {get{return QuestTypeID.PersonHungryQuest;}}
	public override string QuestName {get{return "get the hungry person some food";}}
	#endregion

	public const string getQuest = "Get";
	public const string knowFood = "knowFood";
	public const string knowNoMoney = "knowNoMoney";
	public const string Done = "Done";

	PhraseSequence offerHelp {get{return set.GetPhrase("How are you");}}
	PhraseSequence askFood {get{return set.GetPhrase("What do you want to eat");}}
	PhraseSequence giveOption {get{return set.GetPhrase("Over there is a restaurant");}}
	PhraseSequence offerFood {get{return set.GetPhrase("Let me buy you");}}

	public PersonHungryQuest01(){
		builderName = "PersonHungryQuest01";

		stateMachine.SetStates(getQuest, knowFood, knowNoMoney, Done);
		
		stateMachine.SetCurrentNPC(NPCID.FeelHungryNPC);
		stateMachine.AddDialogueStateToCurrent(getQuest, offerHelp, helperCreateDialogue("I'm hungry", knowFood, SetViewedFunc()));
		stateMachine.AddDialogueStateToCurrent(knowFood, askFood, helperCreateDialogue("I want sushi", knowNoMoney));
		stateMachine.AddDialogueStateToCurrent(knowNoMoney, giveOption, helperCreateDialogue("I have no money", Done));
		var endDialogue = helperCreateDialogue("Thanks", EndFunc(QuestTypeID.PersonHungryQuest), UnlockQuestEvent(QuestTypeID.KnowSakuraQuest));
		stateMachine.AddDialogueStateToCurrent(Done, offerFood, endDialogue);
	}

	#region implemented abstract members of ConversationQuest
	public override DialogueSequence GetIntroductionForState (NPCID npcID, QuestTypeID questID, string state)
	{
		return null;
	}
	public override IEnumerable<QuestDialogueState> GetDialoguesForState (NPCID npcID, QuestTypeID questID, string state)
	{
		if(npcID == hungryPerson && questID == ID){
			return stateMachine.GetDialoguesForState(npcID, questID, state);
		}
		return new QuestDialogueState[0];
	}
	#endregion
	
}
