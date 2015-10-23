using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class GlobalDebugMethods : IDebugMethods {

    public static readonly GlobalDebugMethods Instance = new GlobalDebugMethods();

    public IEnumerable<NamedMethod> GetMethods() {
        var methods = GetType().GetMethodsBySig(typeof(string), typeof(string));
        return methods.Select(
            (m) => new NamedMethod((Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), this, m))
            );
    }

    //public string GoToMultiplayerLobby(string s) {
    //    Application.LoadLevel("MultiplayerLobby");
    //    return "changing level";
    //}

    public string AddRandomWords(string count) {
        int c;
        if (!int.TryParse(count, out c)) {
            c = 10;
        }
        var words = new List<PhraseSequence>(PhraseSetCollectionGameData.Default.AggregateAllWords()).PickN(c);
        foreach(var w in words){
            PlayerDataConnector.CollectWord(w.Word);
        }

        return "Collected " + c + " words";
    }

    public string ClickRandomWords(string count) {
        int c;
        if (!int.TryParse(count, out c)) {
            c = 10;
        }
        var words = new List<PhraseSequence>(PhraseSetCollectionGameData.Default.AggregateAllWords()).PickN(1);
        foreach (var w in words) {
            CrystallizeEventManager.UI.RaiseBeforeWordClicked(this, new PhraseClickedEventArgs(words[0], ""));
            //PlayerDataConnector.CollectWord(w.Word);
        }

        return "Collected " + words[0].GetText();
    }

    public string AddRandomWordsAndPhrases(string count) {
        int c;
        if (!int.TryParse(count, out c)) {
            c = 10;
        }
        var phrases = new List<PhraseSequence>(PhraseSetCollectionGameData.Default.AggregateAllWordsAndPhrases()).PickN(c);
        foreach (var p in phrases) {
            if (p.IsWord) {
                PlayerDataConnector.CollectWord(p.Word);
            } else {
                PlayerDataConnector.CollectPhrase(p);
            }
        }

        return "Collected " + c + " words and phrases.";
    }

    public string RandomizeReviewRanks(string max) {
        foreach (var r in PlayerData.Instance.Reviews.Reviews) {
            r.Rank = UnityEngine.Random.Range(0, 6);
        }
        return "Randomized review ranks";
    }

    public string OpenReviewSummary(string s) {
        UILibrary.ReviewSummary.Get(null);

        return "Opened review summary";
    }

    public string OpenHiraganaShop(string s) {
        UILibrary.ShopPanel.Get(new ShopInitArgs("Kana shop", BuyableKana.Values.Cast<IBuyable>()));

        return "Opened Hiragana shop";
    }

    public string KanaReviewProcess(string s) {
        UILibrary.KanaReview.Get(null);

        return "reviewing kana";
    }

    public string PrintJobPhrases(string s) {
        var jobID = JobID.Tourist1;
        if (!s.IsEmptyOrNull()) {
            if (Enum.IsDefined(typeof(JobID), s)) {
                //jobID = (JobID)Enum.Parse(typeof(JobID), s);
            }
        }

        var jr = new IDJobRef(jobID);
        if (jr.GameDataInstance != null) {
            foreach (var w in jr.GameDataInstance.AvailableWords) {
                Debug.Log(w.GetText(JapaneseTools.JapaneseScriptType.Kanji));
            }
            return "printing job phrases";
        } else {
            return "job not found";
        }
    }

    public string AddMoney(string s) {
        var money = 1000;
        if (!int.TryParse(s, out money)) {
            money = 1000;
        }

        PlayerDataConnector.AddMoney(money);
        return "Added " + money;
    }

	public string AddReptitions(string s) {
		var jobID = JobID.Tourist1;
		if (!s.IsEmptyOrNull()) {
			if (JobID.ContainsID(s)) {
                jobID = JobID.GetValue(s);
			}
		}
		
		var jr = new IDJobRef(jobID);
		jr.PlayerDataInstance.Days += 1000;
		return "added 1000 days to " + jobID;
	}

	public string ClearReptitions(string s) {
		var jobID = JobID.Tourist1;
		if (!s.IsEmptyOrNull()) {
			if (JobID.ContainsID(s)){ //num.IsDefined(typeof(JobID), s)) {
				jobID = JobID.GetValue(s);
			}
		}
		
		var jr = new IDJobRef(jobID);
		jr.PlayerDataInstance.Days = 0;
		return "cleared days of " + jobID;
	}

	public string PromoteJob(string s) {
		var jobID = JobID.Tourist1;
		if (!s.IsEmptyOrNull()) {
			if (JobID.ContainsID(s)) {
                jobID = JobID.GetValue(s);
			}
		}

		if(s == "all"){
            var all_enums = JobID.GetValues();
			foreach(var id in all_enums){
				var job = new IDJobRef(new JobID(id));
				job.PlayerDataInstance.Promoted = true;
			}
			return "promoted all jobs";
		}

		var jr = new IDJobRef(jobID);
		jr.PlayerDataInstance.Promoted = true;
		return "promoted " + jobID;
	}

	public string UnPromoteJob (string s) {
		var jobID = JobID.Tourist1;
		if (!s.IsEmptyOrNull()) {
			if (JobID.ContainsID(s)){ //Enum.IsDefined(typeof(JobID), s)) {
                jobID = JobID.GetValue(s);// (JobID)Enum.Parse(typeof(JobID), s);
			}
		}

		if(s == "all"){
            var all_enums = JobID.GetValues();
			foreach(var id in all_enums){
				var job = new IDJobRef(new JobID(id));
				job.PlayerDataInstance.Promoted = false;
			}
			return "un promoted all jobs";
		}
		
		var jr = new IDJobRef(jobID);
		jr.PlayerDataInstance.Promoted = false;
		return "unpromoted " + jobID;
	}

    public string OutputPhrase(string s) {
        var phrases = PhraseSetCollectionGameData.Default.Phrases;
        var p = phrases[UnityEngine.Random.Range(0, phrases.Count)];

        return PlayerDataConnector.GetText(p) + "(" + p.GetText(JapaneseTools.JapaneseScriptType.Kana) + ")";
    }

    public string SetKanaLevel(string s) {
        int amt = 0;
        if(!int.TryParse(s, out amt)){
            amt = 0;
        }

        foreach (var c in KanaTableUI.AllHiragana()) {
            //Debug.Log("Adding to:" + c);
            var r = PlayerData.Instance.KanaReviews.GetOrCreateReview(c);
            r.Rank = amt;
        }

        return "Kana level set to " + amt;
    }

    public string PrintStat(string s) {
        Debug.Log("Total kana count is: " + KanaTableUI.AllHiragana().Length + "; revs: " + PlayerData.Instance.KanaReviews.Reviews.Count);
        return "Kana multiplier is: " + PlayerDataConnector.GetKanaMultiplier();
    }

    public string ClearTutorials(string s) {
        PlayerData.Instance.Tutorial.ViewedTutorialNames = new HashSet<string>();
        return "Cleared tutorials.";
    }

    public string ClearWords(string s) {
        PlayerData.Instance.Reviews = new PhraseReviewPlayerData();
        PlayerData.Instance.Session = new SessionPlayerData();
        return s;
    }

    public string SavePlayerData(string s) {
        PlayerDataLoader.Save();
        return "Saved player data";
    }

    public string ClearEntryTutorials(string s) {
        foreach (HUDPartType val in Enum.GetValues(typeof(HUDPartType))) {
            PlayerDataConnector.SetHUDPartEnabled(val, true);
        }

        foreach (var t in FreeExploreProcessSelector.Tutorials) {
            PlayerDataConnector.SetTutorialViewed(t);
        }
        return "Set tutorials complete";
    }

    public string GoToStreet(string s) {
        ProcessLibrary.ChangeScene.Get(SceneData.StreetFromSchool, null, null);
        return "Going to street";
    }

}
