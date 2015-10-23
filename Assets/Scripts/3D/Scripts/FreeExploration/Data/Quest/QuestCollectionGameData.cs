using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;


public class QuestCollectionGameData : SerializableDictionary<QuestTypeID, IQuestGameData> {
	
    //contains the quests that will be unlocked at the very beginning
    public QuestTypeID[] DefaultUnlock {
        get {
            return new QuestTypeID[] { QuestTypeID.FindCatQuest, new OverhearTutorialQuest().ID };
        }
    }

}
