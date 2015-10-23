using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SerializedQuestAttribute]
public class FindCatQuest01 : LinearConversationQuest, IHasQuestStateDescription {


//	protected override DialogueSetBuilder set {get{return new DialogueSetBuilder("FindCatQuest01");

	#region implemented abstract members of BaseQuestGameData
	
	public override QuestTypeID ID {get{return QuestTypeID.FindCatQuest;}}
	public override string QuestName {get{return "find a lost cat";}}
	#endregion

	NPCID lostCatPerson {get{return NPCID.FindCatNPC;}}
	NPCID hungryPerson {get{return NPCID.FeelHungryNPC;}}
	DialogueSequence FirstGreeting {get{return LineBuilder("Excuse me").Build();}}
	DialogueSequence LaterGreeting {get{return LineBuilder("Hello").Build();}}
	DialogueSequence HelloGreeting {get{return LineBuilder("Hello").Build();}}
	PhraseSequence offerHelp {get{return set.GetPhrase("Can I help you");}}
	PhraseSequence askColor {get{return set.GetPhrase("what's its color?");}}
	PhraseSequence giveCat {get{return set.GetPhrase("Here it is");}}

	public const string getQuest = "Get";
	public const string knowLost = "Know lost";
	public const string knowColor = "Know Color";
	public const string Done = "Done";

	public FindCatQuest01() : base(){
		builderName = "FindCatQuest01";
		stateMachine.SetStates(getQuest, knowLost, knowColor, Done);
		//focus on a npc
		stateMachine.SetCurrentNPC(NPCID.FindCatNPC);
		stateMachine.AddDialogueStateToCurrent(getQuest, offerHelp, helperCreateDialogue("I lost my cat", knowLost, SetViewedFunc()));
		stateMachine.AddDialogueStateToCurrent(knowLost, askColor, helperCreateDialogue("It is black", knowColor, RaiseQuestFlagFunc(NPCQuestFlag.CatColorKnown)));
		
		var endDialogue = helperCreateDialogue("Thanks", EndFunc(QuestTypeID.FindCatQuest), UnlockQuestEvent(QuestTypeID.PersonHungryQuest));
		stateMachine.AddDialogueStateToCurrent(Done, giveCat, endDialogue);
		//focus on another npc
		stateMachine.SetCurrentNPC(NPCID.FeelHungryNPC);
		
		stateMachine.AddDialogueStateToCurrent(knowColor, new PhraseSequence("Have you seen a black cat?"),
		                          helperCreateDialogue("Yes", Done, RaiseQuestFlagFunc(NPCQuestFlag.CatFound)));
	}

	#region IHasQuestStateDescription implementation
	public QuestHUDItem GetDescriptionForState(QuestRef quest) {
		var state = quest.PlayerDataInstance.State;
		if (state.IsEmptyOrNull()) {
			return null;
		} else {
			int currentStateIndex = stateMachine.States.IndexOf(state);
			Debug.Log("Current state: " + state + "; " + currentStateIndex);
			var hudItem = new QuestHUDItem(QuestName,
			                               new QuestHUDSubItem("Find out the color of the cat", currentStateIndex > 1),
			                               new QuestHUDSubItem("Ask around to locate the cat", currentStateIndex > 2)
			                               );
			return hudItem;
		}
	}
	#endregion	

	#region implemented abstract members of ConversationQuest

	public override DialogueSequence GetIntroductionForState (NPCID npcID, QuestTypeID questID, string state)
	{
		if(questID == ID && npcID == lostCatPerson){
			if(state == stateMachine.FirstState){
				return FirstGreeting;
			}else{
				return LaterGreeting;
			}
		}
		return null;
	}

	public override IEnumerable<QuestDialogueState> GetDialoguesForState (NPCID npcID, QuestTypeID questID, string state)
	{
		if(questID == ID && (npcID == hungryPerson || npcID == lostCatPerson)){
			return stateMachine.GetDialoguesForState(npcID, questID, state);
		}
		return new QuestDialogueState[0];
	}

	#endregion


}
