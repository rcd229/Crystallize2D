using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// represents quest status where the stages progress in a linear direction
/// </summary>
//public class LinearQuestStateMachine : QuestStateMachine {

//    public List<string> QuestStates { get; private set; }

//    public LinearQuestStateMachine()
//        : base() {
//        QuestStates = new List<string>();
//    }

//    #region implemented abstract members of QuestState
//    int CompareStates(string current, string otherState) {
//        if (QuestStates.IndexOf(current) == QuestStates.IndexOf(otherState)) {
//            return 0;
//        } else if (QuestStates.IndexOf(current) < QuestStates.IndexOf(otherState)) {
//            return 1;
//        } else {
//            return -1;
//        }
//    }
//    public override string MostRelevantState(string currentState, IEnumerable<string> states) {
//        return states.Where(s => CompareStates(currentState, s) <= 0).OrderByDescending(s => QuestStates.IndexOf(s)).FirstOrDefault();
//    }

//    public override bool ContainState(string otherState) {
//        return QuestStates.Contains(otherState);
//    }

//    public override void AddState(string state) {
//        QuestStates.Add(state);
//    }

//    #endregion

//    #region implemented abstract members of QuestState
//    public override string FirstState ()
//    {
//        return QuestStates.FirstOrDefault();
//    }
//    public override string GetNextState (string currentState, string state)
//    {
//        return CompareStates(currentState, state) > 0 ? state : currentState;
//    }
//    #endregion
//}

public class LinearQuestStateMachine : IQuestStateMachine, IHasQuestNPCDialogues {

	protected List<QuestDialogueState> currentStates = new List<QuestDialogueState>();

    public List<string> States { get; private set; }
    public List<QuestDialogueData> Dialogues { get; set; }

	public void SetCurrentNPC(NPCID npc) {
		var questDialogueData = Dialogues.Where(s => s.NPCID == npc).FirstOrDefault();
		if (questDialogueData == null) {
			var newData = new QuestDialogueData();
			newData.NPCID = npc;
			currentStates = newData.DialogueStates;
			Dialogues.Add(newData);
		} else {
			currentStates = questDialogueData.DialogueStates;
		}
	}
	
	/// <summary>
	/// add a dialogue state to the current npc
	/// </summary>
	public void AddDialogueStateToCurrent(string questState, PhraseSequence prompt, DialogueSequence dialogue, Func<ContextData> contextGetter = null) {
		currentStates.Add(new QuestDialogueState(questState, dialogue, prompt, contextGetter));
	}
	
	///add a context getter to the current npc and the last state in CurrentState
	public void AddContextGetter(Func<ContextData> contextGetter) {
		var last = currentStates.LastOrDefault();
		if (last != null) {
			last.ContextGetter = contextGetter;
		}
	}

    public string FirstState { get { return States.FirstOrDefault(); } }

    public LinearQuestStateMachine() {
        States = new List<string>();
        Dialogues = new List<QuestDialogueData>();
    }

    int CompareStates(string current, string otherState) {
        int currentIndex = States.IndexOf(current);
        int newIndex = States.IndexOf(otherState);
        if (currentIndex == newIndex) {
            return 0;
        } else if (currentIndex < newIndex) {
            return 1;
        } else {
            return -1;
        }
    }

    public void SetStates(params string[] states) {
        States = new List<string>(states);
    }

    public string MostRelevantState(string currentState, IEnumerable<string> states) {
        return states.Where(s => CompareStates(currentState, s) <= 0).OrderByDescending(s => States.IndexOf(s)).FirstOrDefault();
    }

    public bool ContainState(string otherState) {
        return States.Contains(otherState);
    }

    public string GetNextState(string state, string transition) {
        return CompareStates(state, transition) > 0 ? transition : state;
    }

    public virtual void UpdateSceneForState(QuestRef quest) { }

    public virtual IEnumerable<QuestDialogueState> GetDialoguesForState(NPCID npcID, QuestTypeID questID, string state) {
        if (state.IsEmptyOrNull()) {
            return new QuestDialogueState[0];
        }
        var dialogueList = Dialogues.Where(d => d.NPCID == npcID).SelectMany(d => d.DialogueStates);
        var stateList = dialogueList.Select(s => s.State);
        var relevantState = MostRelevantState(state, stateList);
        return dialogueList.Where(s => s.State.Equals(relevantState));
    }

}
