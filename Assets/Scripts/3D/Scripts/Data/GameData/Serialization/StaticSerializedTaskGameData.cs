using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public abstract class StaticSerializedTaskGameData : StaticNonserializedGameData {
        public abstract JobTaskGameData GetBaseTask();
    }

    public abstract class StaticSerializedTaskGameData<T> : StaticSerializedTaskGameData where T : JobTaskGameData, new() {
        protected T task = new T();

        public T GetTask() {
            PrepareGameData();
            return task;
        }

        public override JobTaskGameData GetBaseTask() {
            return GetTask();
        } 

        protected void Initialize(string name, string sceneName, string actor = "") {
            task.Name = name;
			task.PhraseSetName = Name;
            task.SceneName = sceneName;
            task.Actor = new SceneObjectGameData(actor);
            task.StartingPosition = task.AreaName;
        }

        protected void SetProcess<V>() where V : IProcess<JobTaskRef, JobTaskExitArgs> {
            task.ProcessType = new ProcessTypeRef(typeof(V));
        }

        protected void SetDialogue<V>() where V : StaticSerializedDialogueGameData, new() {
            if(task.Dialogues.Count == 0){
				AddDialogues<V>();
			}
			else{
                var d = new V();
				task.Dialogues.Insert(0, d.GetDialogue());

                foreach (var p in d.addedPhrases) {
                    addedPhrases.Add(p);
                }
			}
        }

		protected void SetProps(params int[] props){
			task.Props = new List<int>(props);
		}

		protected void AddDialogues<V>() where V : StaticSerializedDialogueGameData, new() {
			var d = new V();
            var dialogue = d.GetDialogue();
            task.Dialogues.Add(dialogue);
            
            foreach (var p in d.addedPhrases) {
                addedPhrases.Add(p);
            }
		}


    }
}