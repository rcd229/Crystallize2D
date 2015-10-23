using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IQuestGameData : ISerializableDictionaryItem<QuestTypeID> {
    QuestTypeID ID { get; }
    string QuestName { get; }
    bool IsRepeatable { get; }
    IQuestStateMachine StateMachine { get; }
}
