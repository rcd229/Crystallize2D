using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SerializedQuestAttribute]
public class BefriendOfAnnaQuest01 : LinearConversationQuest {

	NPCID Anna = NPCID.Anna;
	NPCID Sakura = NPCID.Sakura;

	public const string Introduction = "Introduction";
	public const string PlayHobby = "play hobby";
	public const string wentMovie = "went movie";

	PhraseSequence intro {get{return set.GetPhrase("I heard you from Sakura");}}
	PhraseSequence suggestPlay {get{return set.GetPhrase("Do you mind me joining you>");}}
	PhraseSequence suggestMovie {get{return set.GetPhrase("Let's go to movies");}}
	PhraseSequence suggestFood  {get{return set.GetPhrase("Let's go buy some snacks");}}
	PhraseSequence suggestShopping {get{return set.GetPhrase("Let's go shopping");}}

	public BefriendOfAnnaQuest01(){
		builderName = "BefriendOfAnnaQuest01";

		stateMachine.SetStates(Introduction, PlayHobby, wentMovie);
		
		stateMachine.SetCurrentNPC(NPCID.Anna);
		stateMachine.AddDialogueStateToCurrent(Introduction, intro, helperCreateDialogue("I'm Anna. Nice to meet you", PlayHobby, SetViewedFunc()));

		stateMachine.AddDialogueStateToCurrent(PlayHobby, suggestPlay, helperCreateDialogue("Please come. What do you want to do?", wentMovie));

		stateMachine.AddDialogueStateToCurrent(wentMovie, suggestShopping, helperCreateDialogue("I'm not interested in shopping", wentMovie, ConfidenceEvent(-1)));
		stateMachine.AddDialogueStateToCurrent(wentMovie, suggestFood, helperCreateDialogue("Sorry. I'm not hungry", wentMovie, ConfidenceEvent(-1))); 
		stateMachine.AddDialogueStateToCurrent(wentMovie, suggestMovie, helperCreateDialogue("Let's go!", EndFunc(), UpdateStateEvent(SakuraFirstQuest01.BefriendAnna, QuestTypeID.SakuraFirstQuest)));
		
	}
	
	#region implemented abstract members of BaseQuestGameData
	public override QuestTypeID ID {get{return QuestTypeID.BecomeFriendWithAnna;}}
	public override string QuestName {get{return "become friend with anna";}}
	#endregion
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
