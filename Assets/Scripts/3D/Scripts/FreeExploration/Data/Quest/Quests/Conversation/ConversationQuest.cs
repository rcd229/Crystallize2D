using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public abstract class ConversationQuest : BaseQuestGameData, IHasQuestIntroductionDialogue, IHasQuestNPCDialogues {

	protected string builderName;

	protected DialogueSetBuilder _set;

	protected DialogueSetBuilder set {get
		{
			if(_set == null){
				_set = new DialogueSetBuilder(builderName);
			}
			return _set;
		}
	}
	#region implemented abstract members of BaseQuestGameData

	public override bool IsRepeatable {
		get {
			return false;
		}
	}

	#endregion

	#region IHasQuestIntroductionDialogue implementation

	public abstract DialogueSequence GetIntroductionForState (NPCID npcID, QuestTypeID questID, string state);

	#endregion

	#region IHasQuestNPCDialogues implementation

	public abstract System.Collections.Generic.IEnumerable<QuestDialogueState> GetDialoguesForState (NPCID npcID, QuestTypeID questID, string state);

	#endregion

	protected DialogueSequenceBuilder LineBuilder(string text){
		var b = set.GetDialogueBuilder();
		b.AddLine(text);
		return b;
	}

	/** Helper methods **/
	
	/********************************* useful methods to construct a callback ****************************************/
	
	//return an action that updates the quest state fo this quest to specified state
	protected IDialogueEvent UpdateStateEvent(string state, QuestTypeID questID) {
		return ActionDialogueEvent.Get(QuestUtil.SetQuestState, questID, state);
	}
	
	protected IDialogueEvent UpdateStateEvent(string state) {
		return UpdateStateEvent(state, this.ID);
	}
	
	//return an action that unlocks another quest
	protected IDialogueEvent UnlockQuestEvent(QuestTypeID questID) {
		return ActionDialogueEvent.Get(QuestUtil.UnlockQuest, questID);// new UnlockQuestDialogueEvent(questID);
	}
	
	protected IDialogueEvent RaiseQuestFlagFunc(params Guid[] flags) {
		foreach (var flag in flags) {
			if (!NPCQuestFlag.GetIDs().Where(s => s.Guid == flag).Any()) {
				Debug.LogError("not a valid guid for quest flag " + flag.ToString());
			}
		}
		
		return ActionDialogueEvent.Get(QuestUtil.RaiseFlags, flags);
	}
	
	protected IDialogueEvent SetViewedFunc() {
		//var action = () => QuestUtil.Instance.SetViewed(quest.ID);
		//var a = ActionDialogueEvent.Get(QuestUtil.Instance.SetViewed, quest.ID);
		//Debug.Log(a.Event.Method.Name);
		return ActionDialogueEvent.Get(QuestUtil.SetViewed, this.ID);
	}
	
	protected IDialogueEvent EndFunc() {
		return EndFunc(this.ID);
	}
	
	protected IDialogueEvent EndFunc(QuestTypeID questID) {
		return ActionDialogueEvent.Get(QuestUtil.EndQuest, questID);
	}
	
	protected IDialogueEvent ActionEvent(Action action) {
		return ActionDialogueEvent.Get(action);
	}
	
	protected IDialogueEvent ConfidenceEvent(int amount) {
		return ActionDialogueEvent.Get(PlayerDataConnector.AddConfidence, amount);
	}
	
	/******************************** useful methods to construct a dialogue *****************************************/
	//help create linear, one sentence dialogue with state update
	protected DialogueSequence helperCreateDialogue(string content, string stateUpdate) {
		return helperCreateDialogue(content, UpdateStateEvent(stateUpdate));
	}
	

	//help create linear, one sentence dialogue with a callback
	protected DialogueSequence helperCreateDialogue(string content, params IDialogueEvent[] events) {
		DialogueSequence dialogue = new DialogueSequence();
		LineDialogueElement elem = dialogue.GetNewDialogueElement<LineDialogueElement>();
		elem.Line.Phrase = set.GetPhrase(content);
		foreach (var e in events) {
			dialogue.AddEvent(elem.ID, e);
		}
		return dialogue;
	}
	
	protected DialogueSequence helperCreateDialogue(string content, string stateUpdate, params IDialogueEvent[] callbacks) {
		var callbackList = callbacks.ToList();
		callbackList.Add(UpdateStateEvent(stateUpdate));
		return helperCreateDialogue(content, callbackList.ToArray());
	}


}
