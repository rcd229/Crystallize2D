using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseReviewUI<T> : UIPanel, ITemporaryUI<object, PhraseReviewSessionResultArgs> {

    public event EventHandler<EventArgs<PhraseReviewSessionResultArgs>> Complete;

    public ItemReviewPlayerData<T> ActiveReview { get; set; }

    protected int count = 0;
    protected Dictionary<ItemReviewPlayerData<T>, SessionReviewEntry<T>> results = new Dictionary<ItemReviewPlayerData<T>, SessionReviewEntry<T>>();
    //List<ItemReviewPlayerData> results = new List<ItemReviewPlayerData>();

    int prevLevel = 0;
    int prevConfidence = 0;
    int prevWords = 0;
    int prevPhrases = 0;

    public abstract void Initialize(object obj);

    protected virtual void Refresh() {
        PlayerData.Instance.Proficiency.RecalculateExperience();
        prevLevel = PlayerData.Instance.Proficiency.GetReviewLevel();
        prevConfidence = PlayerData.Instance.Proficiency.Confidence;
        prevWords = PlayerData.Instance.Proficiency.Words;
        prevPhrases = PlayerData.Instance.Proficiency.Phrases;
    }

    protected virtual void OnSetResult() { 
    }

    public void Skip() {
        Exit();
    }

    public void SetResult(int result) {
        if (ActiveReview != null) {
            DataLogger.LogTimestampedData("Review", ActiveReview.GetText(), result.ToString());

            if (!results.ContainsKey(ActiveReview)) {
                results[ActiveReview] = new SessionReviewEntry<T>(ActiveReview);
            }

            ActiveReview.AddEntry(result);

            if (result == 0) {
                results[ActiveReview].AfterRank = ActiveReview.Rank;
            } else {
                results[ActiveReview].AfterRank = ActiveReview.Rank;
            }

            PlayerDataConnector.AddReview(ActiveReview, result);

            ActiveReview = null;
            count++;
        }
        OnSetResult();
    }

    public void Continue() {
        PlayerData.Instance.Proficiency.RecalculateExperience();
        var newLevel = PlayerData.Instance.Proficiency.GetReviewLevel();

        if (newLevel > prevLevel) {
            var confIncr = PlayerData.Instance.Proficiency.Confidence - prevConfidence;
            var wordsIncr = PlayerData.Instance.Proficiency.Words - prevWords;
            var phraseIncr = PlayerData.Instance.Proficiency.Phrases - prevPhrases;

            var s = "Level up!\n";
            s += "+" + confIncr + " confidence";
            if(wordsIncr > 0){
                s += "   +" + wordsIncr + " word";
            }

            if(phraseIncr > 0){
                s += "   +" + phraseIncr + " phrase";
            }

            UILibrary.ActivityText.Get(s, HandleLevelUpComplete);
            gameObject.SetActive(false);
        } else {
            Refresh();
        }
    }

    void HandleLevelUpComplete(object sender, object args) {
        gameObject.SetActive(true);
        Refresh();
    }

    public void Exit() {
        Complete.Raise(this, new EventArgs<PhraseReviewSessionResultArgs>(GetExitArgs()));
        Close();
    }

    protected virtual PhraseReviewSessionResultArgs GetExitArgs(){
        return null;
    }

}
