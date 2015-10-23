using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IQuestStateMachine {
    string FirstState { get; }
    string GetNextState(string state, string transition);
    void UpdateSceneForState(QuestRef quest); // Guid questID, string state);
}

/// <summary>
/// represents the states of a quest
/// supports operation of:
/// 1. Detecting if the quest is unlocked
/// 2. Find the most relevant state
/// </summary>
//public abstract class QuestStateMachine : IQuestStateMachine {

//    /// <summary>
//    /// From a collection of states, choose the most updated state that is earlier or
//    /// equal to the current state 
//    /// </summary>
//    public abstract string MostRelevantState(string currentState, IEnumerable<string> states);

//    //public abstract void AddState(string state);
//    public abstract bool ContainState(string otherStates);

//    public abstract string FirstState { get; }
//    public abstract string GetNextState(string currentState, string state);
//    public abstract void UpdateSceneForState(Guid questID, string state);
//}
