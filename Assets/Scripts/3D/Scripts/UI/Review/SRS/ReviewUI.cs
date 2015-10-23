using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewUI : BaseReviewUI<PhraseSequence> {

    const string ResourcePath = "UI/Review";
    public static ReviewUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<ReviewUI>(ResourcePath);
    }

    public Text itemText;
    public Text answerText;
    public GameObject checkButton;

    public override void Initialize(object param1) {
        Refresh();
    }

    protected override void Refresh() {
        //Debug.Log(PlayerData.Instance.PhraseStorage.Phrases.Count);
        PlayerData.Instance.Reviews.GetNewReviews();
        var reviews = PlayerData.Instance.Reviews.GetCurrentReviews();


        if (reviews.Count > 0 && count < 20) {
            ActiveReview = reviews[UnityEngine.Random.Range(0, reviews.Count)];
            DisplayReview(ActiveReview);
        } else {
            Exit();
        }
    }

    void DisplayReview(ItemReviewPlayerData<PhraseSequence> review) {
        itemText.text = PlayerDataConnector.GetText(review.Item);//.GetText(JapaneseTools.JapaneseScriptType.Romaji);
        if(review.Item.IsWord){
            var entry =  DictionaryData.Instance.GetEntryFromID(review.Item.Word.WordID);
            if (entry == null) {
                Debug.Log("No entry for: [" + review.Item.Word + "] ID:" + review.Item.Word.WordID);
            }
            var s = "";
            foreach(var e in entry.English){
                s += e + "\n";
            }
            s = s.Substring(0, s.Length - 1);
            answerText.text = s;
        } else {
            answerText.text = review.Item.Translation;
        }

        checkButton.SetActive(true);
    }

    public void ShowAnswer() {
        if (ActiveReview != null) {
            checkButton.SetActive(false);
        } else {
            Refresh();
        }
    }

    protected override void OnSetResult() {
        Refresh();
    }
    
    
}
