using UnityEngine;
using System.Collections;

namespace CrystallizeData {
    public abstract class LearnAndReviewTaskBase : StaticSerializedTaskGameData<LearnAndReviewTaskGameData>{
        protected void AddPlayerPhrase(PhraseSequence phrase) {
            task.Phrase = phrase;
            task.TargetPhrases.Add(phrase);
        }

    }

    public class LearnAndReviewTask : LearnAndReviewTaskBase {

        protected override void PrepareGameData() {
            
        }

        public JobTaskGameData Get<D>(string level, string target, PhraseSequence phrase) 
             where D: StaticSerializedDialogueGameData, new(){
            Initialize(Name + "_" + phrase.Translation, level, target);
            AddPlayerPhrase(phrase);
            SetProcess<OverhearAndPracticeProcess>();
            SetDialogue<D>();
            return task;
        }

    }

}