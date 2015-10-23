using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class SayPhraseQuest : BaseQuestGameData, IQuestStateMachine, IHasQuestStateDescription {
    protected static readonly string[] States = new string[] { "Root" };

    public override bool IsRepeatable { get { return false; } }
    public override IQuestStateMachine StateMachine { get { return this; } }
    public string FirstState { get { return States[0]; } }
    public string GetNextState(string state, string transition) { return transition; }
    public void UpdateSceneForState(QuestRef quest) { }

    public abstract string ItemKey { get; }
    public abstract QuestNPCItemData NPC { get; }

    public virtual string TaskDescription { get { return "Say the correct phrase"; } }

    public QuestHUDItem GetDescriptionForState(QuestRef quest) {
        if (!quest.PlayerDataInstance.Finished) {
            return new QuestHUDItem(QuestName,
                new QuestHUDSubItem(TaskDescription, false)
                );
        }
        return null;
    }
}
