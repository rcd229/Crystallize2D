using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ReviewProcess : EnumeratorProcess<object, object> {

    public override IEnumerator<SubProcess> Run(object args) {
        MainCanvas.main.PushLayer();
        WorldCanvas.Instance.gameObject.SetActive(false);

        var todaysWords = PlayerData.Instance.Session.TodaysCollectedWords;
        for (int i = 0; i < todaysWords.Count; i++) {
            var r = PlayerData.Instance.Reviews.GetReview(todaysWords[i]);
            if (r != null && r.Rank > 0) {
                todaysWords.RemoveAt(i);
                i--;
            }
        }

        var reviews = PlayerData.Instance.Reviews.GetCurrentReviews();
        var kanaReviews = PlayerData.Instance.Reviews.GetCurrentReviews();
        if (reviews.Count == 0 && kanaReviews.Count == 0) {
            yield return Get(ProcessLibrary.MessageBox, "You don't have any reviews now. Try waiting or collecting more words.");
        } else {
            yield return Get(ProcessLibrary.ReviewTutorial, "Reviewing words will remove them from your inventory.");
            yield return Get(UILibrary.Review, null);
            if (PlayerData.Instance.KanaReviews.GetCurrentReviews().Count > 0) {
                if (!PlayerData.Instance.Tutorial.GetTutorialViewed(TagLibrary.KanaReview)) {
                    PlayerData.Instance.Tutorial.SetTutorialViewed(TagLibrary.KanaReview);
                    yield return Get(ProcessLibrary.MessageBox, "Review the chart for the quiz!");
                }
                yield return Get(UILibrary.KanaTable, null);
            }
            yield return Get(UILibrary.KanaReview, null);
        }
    }

    protected override void BeforeExit() {
        MainCanvas.main.PopLayer();
        WorldCanvas.Instance.gameObject.SetActive(true);
    }

}