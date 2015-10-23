using UnityEngine;
using System.Collections;

public abstract class LinearConversationQuest : ConversationQuest {
	protected LinearQuestStateMachine stateMachine;
	public override IQuestStateMachine StateMachine {get{return stateMachine;}}

	protected LinearConversationQuest(){
		stateMachine = new LinearQuestStateMachine();
	}
}
