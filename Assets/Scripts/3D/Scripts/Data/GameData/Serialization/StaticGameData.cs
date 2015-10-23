using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public abstract class StaticGameData {
        public string Name {
            get {
                return GetType().Name;
            }
        }

        protected bool isTest = false;
		protected int index = 0;

        protected abstract void PrepareGameData();

		Dictionary<string, PhraseSequence> phrases = new Dictionary<string, PhraseSequence>(new PostCompilerStringComparer());

		protected PhraseSequence GetPhrase(string phraseKey) {
            if (isTest) {
                return new PhraseSequence(phraseKey);
            }
            
            var key = phraseKey.ToLower();
            if (phrases.ContainsKey(key)) {
                return phrases[key];
            } else {
                var p = PhraseSetCollectionGameData.GetOrCreateItem(Name).GetPhrase(key);
                    //PhraseSetCollectionGameData.GetOrCreateItem(Name).GetOrCreatePhrase(index);//
                GameDataInitializer.AddPhrase(Name, phraseKey, index);
                index++;
                phrases[key] = p;

                AfterGetPhrase(p);

                return p;
            }
		}

        protected virtual void AfterGetPhrase(PhraseSequence phrase) { }
    }

    public abstract class StaticNonserializedGameData : StaticGameData {
        public List<PhraseSequence> addedPhrases = new List<PhraseSequence>();

        protected override void AfterGetPhrase(PhraseSequence phrase) {
            if (phrase == null) {
                return;
            }

            foreach (var w in phrase.PhraseElements) {
                if (w.IsDictionaryWord) {
                    var wp = new PhraseSequence(w);
                    if (!addedPhrases.ContainsEquivalentPhrase(wp)) {
                        addedPhrases.Add(wp);
                    }
                }
            }
        }
    }
}