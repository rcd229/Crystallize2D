using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class JobUnlockQuest : BaseQuestGameData, IQuestStateMachine, IHasQuestStateDescription {
    public override bool IsRepeatable { get { return false; } }
    public override IQuestStateMachine StateMachine { get { return this; } }

    public virtual JobID JobID { get { return JobID.OpenJanitor; } }

    public abstract string RewardString { get; }
    public abstract DialogueSequence QuestDialogue { get; }

    public string FirstState { get { return "Root"; } }
    public string GetNextState(string state, string transition) { return transition; }

    public void UpdateSceneForState(QuestRef quest) { }

    public QuestHUDItem GetDescriptionForState(QuestRef quest) {
        if (quest.IsAlive) {
            return new QuestHUDItem(QuestName, new QuestHUDSubItem("Complete the interview", false));
        } else {
            return null;
        }
    }

    public QuestNPCItemData GetNPC() {
        var npc = new QuestNPCItemData();
        npc.ID = new NPCID(ID.guid);
        npc.EntryDialogue = QuestDialogue;
        return npc;
    }

    protected void UnlockJob() {
        new IDJobRef(JobID).PlayerDataInstance.Unlocked = true;
    }

    public override IQuestReward GetReward(QuestRef quest) {
        return new QuestReward(RewardString, null);
    }
}
