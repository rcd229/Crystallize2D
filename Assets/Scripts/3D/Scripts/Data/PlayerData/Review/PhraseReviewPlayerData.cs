using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PhraseReviewPlayerData : ReviewPlayerData<PhraseSequence, PhraseItemReviewPlayerData> {

    public PhraseReviewPlayerData() : base() { }

    protected override bool ItemsEqual(PhraseSequence t1, PhraseSequence t2) {
        return PhraseSequence.PhrasesEquivalent(t1, t2);
    } 

    public void GetNewReviews() {
        foreach (var w in PlayerData.Instance.WordStorage.FoundWords) {
            var pse = new PhraseSequenceElement(w, 0);
            var p = new PhraseSequence();
            p.Add(pse);
            //Debug.Log("Contains review: " + p.GetText() + "; " + ContainsReview(p));
            if (!ContainsReview(p)) {
                //Debug.Log("Adding review: " + p.GetText());
                Reviews.Add(new PhraseItemReviewPlayerData(p));
            }
        }

        foreach (var p in PlayerData.Instance.PhraseStorage.Phrases) {
            if (!ContainsReview(p)) {
                //Debug.Log("Adding review: " + p.GetText());
                Reviews.Add(new PhraseItemReviewPlayerData(p));
            }
        }
    }

}
