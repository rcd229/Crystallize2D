using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PhraseReviewPlayerData : ReviewPlayerData<PhraseSequence, PhraseItemReviewPlayerData> {

    public PhraseReviewPlayerData() : base() { }

    protected override bool ItemsEqual(PhraseSequence t1, PhraseSequence t2) {
        return PhraseSequence.PhrasesEquivalent(t1, t2);
    } 

}
