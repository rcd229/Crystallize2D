using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseReviewSessionResultArgs  {

    public IEnumerable<SessionReviewEntry<PhraseSequence>> SessionReviews { get; set; }

    public PhraseReviewSessionResultArgs() {
        SessionReviews = new List<SessionReviewEntry<PhraseSequence>>();
    }

    public PhraseReviewSessionResultArgs(IEnumerable<SessionReviewEntry<PhraseSequence>> reviews) {
        SessionReviews = reviews;
    }

}