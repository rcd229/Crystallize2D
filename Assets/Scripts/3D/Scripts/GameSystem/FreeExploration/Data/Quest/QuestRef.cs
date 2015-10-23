using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestRef {
    public QuestTypeID ID { get; private set; }
    //Invariant: all the states must exist in QuestStates
    //Dictionary<Guid, List<QuestDialogueState>> NPCQuestDialogueTable;

    public QuestRef(QuestTypeID id) {
        ID = id;
        //NPCQuestDialogueTable = new Dictionary<Guid, List<QuestDialogueState>>();
        //InitDictionary();
    }

    public IQuestGameData GameDataInstance {
        get {
            return GameData.Instance.Quests.Get(ID);
        }
    }

    public QuestProgress PlayerDataInstance {
        get {
            return PlayerData.Instance.QuestData.GetOrCreateItem(ID);
        }
    }

    public string QuestName {
        get {
            return GameDataInstance.QuestName;
        }
    }

    public bool IsRepeatable {
        get {
            return GameDataInstance.IsRepeatable;
        }
    }

    public bool IsAlive {
        get {
            return !PlayerDataInstance.State.IsEmptyOrNull();
        }
    }

    public bool isUnlocked {
        get {
            return !PlayerDataInstance.State.IsEmptyOrNull() || PlayerDataInstance.Finished;
        }
    }

    public bool isViewed {
        get {
            return PlayerDataInstance.Viewed;
        }
    }

    public void SetViewed() {
        PlayerDataInstance.Viewed = true;
    }

    public void Unlock() {
        if (!isUnlocked) {
			Debug.Log(GameDataInstance);
			Debug.Log(GameDataInstance.StateMachine);
            SetState(GameDataInstance.StateMachine.FirstState);
        }
    }

    //test for dictionary invariant
    //public bool IsDictionaryInvariant() {
    //    foreach (var list in NPCQuestDialogueTable.Values) {
    //        if (!list.TrueForAll(s => quest.StateMachine.ContainState(s.State))) {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //transfer data in serialized list to the dictionary. These data are static so it should be safe
    //public void InitDictionary() {
    //    foreach (var state in quest.SerializedDialogueTable) {
    //        if (!NPCQuestDialogueTable.ContainsKey(state.npcID)) {
    //            NPCQuestDialogueTable.Add(state.npcID, state.DialogueStates);
    //        } else {
    //            NPCQuestDialogueTable[state.npcID].AddRange(state.DialogueStates);
    //        }
    //    }
    //}

    public DialogueElement GetDialogue(NPCID npc) {
        if (GameDataInstance.StateMachine is IHasQuestNPCDialogues) { //NPCQuestDialogueTable.ContainsKey(npc)) {
            return (GameDataInstance.StateMachine as IHasQuestNPCDialogues).GetDialoguesForState(npc, GameDataInstance.ID, PlayerDataInstance.State).FirstOrDefault().Dialogue;
            //List<QuestDialogueState> dialogueList = (quest.StateMachine as IHasQuestNPCDialogues).GetDialoguesForState(quest.ID, PlayerDataInstance.State).ToList(); //NPCQuestDialogueTable[npc];
            //var stateList = dialogueList.Select(s => s.State);
            //var relevantState = quest.StateMachine.MostRelevantState(PlayerDataInstance.State, stateList);
            //return dialogueList.Where(s => s.State == relevantState).FirstOrDefault().Dialogue;
        } else {
            return null;
        }
    }

    public IEnumerable<QuestDialogueState> GetQuestDialogueStates(NPCID npc) {
        //Debug.Log("Getting quest states: " + npc.guid + ";" + (GameDataInstance.StateMachine is IHasQuestNPCDialogues));
        if (GameDataInstance.StateMachine is IHasQuestNPCDialogues) {
            return (GameDataInstance.StateMachine as IHasQuestNPCDialogues).GetDialoguesForState(npc, GameDataInstance.ID, PlayerDataInstance.State);
        } else {
            return new QuestDialogueState[0];
        }
    }

    //return true iff state is set successfully
    public void SetState(string nextState) {
        PlayerDataInstance.State = GameDataInstance.StateMachine.GetNextState(PlayerDataInstance.State, nextState);
        //Debug.Log("Player: " + PlayerDataInstance.State + "; " + nextState);
		if(nextState != PlayerDataInstance.State){
			PlayerDataInstance.State = nextState;
            CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(null, null);
            Debug.Log("Setting state to: " + nextState);
		}
        if (GameDataInstance.StateMachine != null) {
            GameDataInstance.StateMachine.UpdateSceneForState(this);
        }
    }

    public void Reset() {
        if (GameDataInstance.IsRepeatable) {
            PlayerDataInstance.Viewed = false;
            PlayerDataInstance.State = null;
            SetState(null);
        }
    }

    public void SetFinish() {
        PlayerDataInstance.FinishedTimes++;
        SetState(null);
    }

}
