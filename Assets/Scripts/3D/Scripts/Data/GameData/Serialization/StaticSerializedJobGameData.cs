using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {

    public abstract class StaticSerializedJobGameData : StaticSerializedGameData {
        protected JobGameData job = new JobGameData();

        protected virtual bool Include {
            get {
                return true;
            }
        }

        protected override void AddGameData() {
            if (Include) {
                GameData.Instance.Jobs.Add(job);
            }
        }

        protected void Initialize(JobID id, string name, int difficulty = 0, int ordering = 10000) {
			job.ID = id;
            job.Name = name;
			job.PhraseSetName = Name;
            job.Difficulty = difficulty;
            job.Ordering = ordering;
        }

        protected void AddTask<T>() where T : StaticSerializedTaskGameData, new() {
            var t = new T();
            job.Tasks.Add(t.GetBaseTask());
            AddPhrases(t.addedPhrases);
        }

        protected void AddLine(string key) {
            job.Lines.Add(new KeyedPhraseSequence(key, GetPhrase(key)));
        }

        protected JobTaskGameData AddTask<T, D>(string level, string target) 
            where T : StaticSerializedTaskGameData, new() 
            where D : StaticSerializedDialogueGameData, new() {
			var ts = new T();
			var d =  new D();
			var t = ts.GetBaseTask();
            t.SceneName = level;
            t.Actor = new SceneObjectGameData(target);
            t.Dialogues.Insert(0, d.GetDialogue());
            t.Name = d.Name;
            job.Tasks.Add(t);

            AddPhrases(ts.addedPhrases);
            AddPhrases(d.addedPhrases);

            return t;
        }

        protected JobTaskGameData AddOverhearAndPracticeTask<D>(string level, string target, string playerPhrase)
            where D : StaticSerializedDialogueGameData, new() {
            var l = new LearnAndReviewTask();
            var t = l.Get<D>(level, target, GetPhrase(playerPhrase));
            t.StartingPosition = t.AreaName;
            job.Tasks.Add(t);
            AddPhrases(l.addedPhrases);
            return t;
        }

        protected JobTaskGameData GetOverhearAndPracticeTask<D>(string level, string target, string playerPhrase)
            where D : StaticSerializedDialogueGameData, new() {
            var l = new LearnAndReviewTask();
            var t = l.Get<D>(level, target, GetPhrase(playerPhrase));
            AddPhrases(l.addedPhrases);
            return t;
        }

        protected override void AfterGetPhrase(PhraseSequence phrase) {
            AddPhrase(phrase);
        }

        void AddPhrases(IEnumerable<PhraseSequence> phrases) {
            foreach (var p in phrases) {
                AddPhrase(p);
            }
        }

        void AddPhrase(PhraseSequence phrase) {
			if(phrase == null){
				return;
			}
			if(phrase.PhraseElements.Count == 0){
				return;
			}
            foreach (var w in phrase.PhraseElements) {
                if (w.IsDictionaryWord) {
                    var wp = new PhraseSequence(w);
                    if (!job.AvailableWords.ContainsEquivalentPhrase(wp)) {
                        job.AvailableWords.Add(wp);
                    }
                }
            }
        }

    }
}