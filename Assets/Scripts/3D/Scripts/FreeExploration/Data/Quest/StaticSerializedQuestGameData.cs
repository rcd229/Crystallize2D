using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using DialogueBuilder;

namespace CrystallizeData {
    public class StaticSerializedQuestsGameData : StaticSerializedGameData {

        List<IQuestGameData> classQuests = new List<IQuestGameData>();
        int dialogueCount = 0;
        int elementCount = 0;

        protected override void AddGameData() {
            foreach (var q in classQuests) {
                GameData.Instance.Quests.Add(q);
            }
        }

        protected override void PrepareGameData() {
            var instances = SerializedQuestExtensions.GetAllQuests();
            foreach(var instance in instances){
                if(instance is IQuestGameData){
                    GetEachDialogue(instance as IQuestGameData);
                    GetEachElement(instance as IQuestGameData);
                    classQuests.Add(instance as IQuestGameData);
                    if (instance is IHasNPCSpawner) {
                        GameData.Instance.Spawn.AddSpawner((instance as IHasNPCSpawner).Spawner);
                    }
                } else {
                    Debug.LogError(instance + " does not implement IQuestGameData");
                }
            }
            //foreach (var spwn in GameData.Instance.Spawn.SpawnableNPCs) {
            //    Debug.Log("Spawner: " + spwn);
            //}
            Debug.Log("Found " + classQuests.Count + " serialized quests. Dialogues: " + dialogueCount);
        }

        void GetEachDialogue(IQuestGameData quest) {
            //Debug.Log("Checking: " + quest);
            var dialogues = from p in quest.GetType().GetProperties()
                            where p.PropertyType == typeof(DialogueSequence)
                            select p;
            foreach (var p in dialogues) {
                //Debug.Log(p.Name);
                p.GetValue(quest, new object[0]);
                dialogueCount++;
            }
        }

        void GetEachElement(IQuestGameData quest) {
            foreach (var e in quest.GetType().GetFieldAndPropertyValues<Element>(quest)) {
                HasDialogueBase.BuildDialogue("Default", e);
            }
        }

    }

    public abstract class StaticSerializedQuestGameData<T> : StaticSerializedGameData where T : IQuestStateMachine, new() {

        protected QuestGameData quest = new QuestGameData();
        protected T stateMachine;

        INPCSpawner spawnable;

        protected override void AddGameData() {
            GameData.Instance.Quests.Add(quest);
            if (spawnable != null) {
                GameData.Instance.Spawn.AddSpawner(spawnable);
            }
        }

        protected override void PrepareGameData() {
            Prepare();
            //mostly for development testing
            //if(!quest.IsDictionaryInvariant()){
            //    Debug.LogError("quest state invariant violated. quest NPC have states not in the quest.");
            //}
        }

        protected abstract void Prepare();

        protected T Initialize(string name, bool isRepeatable, QuestTypeID id) {
            quest.QuestName = name;
            quest.IsRepeatable = isRepeatable;
            quest.ID = id;
            var t = new T();
            quest.StateMachine = t;
            stateMachine = t;
            return t;
        }

        protected void AddSpawner(INPCSpawner spawner) {
            spawnable = spawner;
        }

        /** Helper methods **/

        /********************************* useful methods to construct a callback ****************************************/

        //return an action that updates the quest state fo this quest to specified state
        protected IDialogueEvent UpdateStateEvent(string state, QuestTypeID questID) {
            return ActionDialogueEvent.Get(QuestUtil.SetQuestState, questID, state);
        }

        protected IDialogueEvent UpdateStateEvent(string state) {
            return UpdateStateEvent(state, quest.ID);
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
            //var action = () => FreeExploreQuestManager.Instance.SetViewed(quest.ID);
            //var a = ActionDialogueEvent.Get(FreeExploreQuestManager.Instance.SetViewed, quest.ID);
            //Debug.Log(a.Event.Method.Name);
            return ActionDialogueEvent.Get(QuestUtil.SetViewed, quest.ID);
        }

        protected IDialogueEvent EndFunc() {
            return EndFunc(quest.ID);
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
            elem.Line.Phrase = GetPhrase(content);
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
}
