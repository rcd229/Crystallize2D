using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseQuestGameData : HasDialogueBase, IQuestGameData, IHasQuestReward {

    public abstract QuestTypeID ID { get; }
    public QuestTypeID Key { get { return ID; } }
    public abstract string QuestName { get; }
    public abstract bool IsRepeatable { get; }
    public abstract IQuestStateMachine StateMachine { get; }

    public virtual int RewardMoney { get { return 100; } }
	
    public virtual IQuestReward GetReward(QuestRef quest) {
        return QuestReward.Money(RewardMoney);
    }

    public string GetTargetPath(string name) {
        return "Quests/" + QuestName + "/" + name;
    }

}
