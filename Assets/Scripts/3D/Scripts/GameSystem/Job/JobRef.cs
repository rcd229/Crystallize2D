using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;


public interface IJobRef : JoinedGameDataRef<JobGameData, JobPlayerData> { }

public class IDJobRef : GuidRef<JobGameData, JobPlayerData>, IJobRef {
	
    public JobGameData GameDataInstance {
		get {
			return GameData.Instance.Jobs.Get (new JobID(ID));
		}
    }

    public JobPlayerData PlayerDataInstance {
		get {
			return PlayerData.Instance.Jobs.GetOrCreateItem (new JobID(ID));
		}
    }

    public IDJobRef(JobID jobID) : base(jobID.guid) {
        var j = GameData.Instance.Jobs.Get(jobID);
        if (j == null) {
            Debug.Log(jobID + " is null");
        }
    }

}

public static class JobRefExtensions {

    public static string ViewedEventsString(this IJobRef job) {
        return string.Format("{0}/{1}", job.PlayerDataInstance.UniqueViewedTaskCount(), job.GameDataInstance.Tasks.Count);
    }

    public static float Progress(this IJobRef job) {
        int total = job.GameDataInstance.AvailableWords.Count;
        return (float)(total - GetUnlearnedWords(job).Count) / total;
    }

    public static List<PhraseSequence> GetRemainingNeededWords(this IJobRef job) {
        var remaining = new List<PhraseSequence>();
        foreach (var req in job.GameDataInstance.GetPhraseRequirements()) {
            var p = req.Phrase;
            if (!PlayerDataConnector.ContainsLearnedItem(p)) {
                remaining.Add(p);
            }
        }
        return remaining;
    }

    public static List<PhraseSequence> GetUnlearnedWords(this IJobRef job) {
        var phraseSequenceSet = job.GameDataInstance.AvailableWords;
        var unlearned = from word in phraseSequenceSet
                        where !PlayerDataConnector.ContainsLearnedItem(new PhraseSequence(word))
                        select new PhraseSequence(word);
        return unlearned.ToList();
    }

    public static PhraseSequence GetPhrase(this IJobRef job, string key) {
        return job.GameDataInstance.Lines.Get(key).Phrase;
    }

    // TODO: Helpers: should be moved out
    static void AddWordsToSet(List<PhraseSequence> phrases, HashSet<PhraseSequenceElement> set) {
        foreach (var phrase in phrases) {
            AddWordToSet(phrase, set);
        }
    }

    static void AddWordToSet(PhraseSequence phrase, HashSet<PhraseSequenceElement> set) {
        if (phrase.IsWord && phrase.PhraseElements.FirstOrDefault().IsDictionaryWord) {
            set.Add(phrase.GetElements().FirstOrDefault());
        } else {
            foreach (var elem in phrase.GetElements()) {
                if (elem.IsDictionaryWord) {
                    set.Add(elem);
                }
            }
        }
    }
}
