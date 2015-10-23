using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class LearnAndReviewTaskGameData : JobTaskGameData {

    public PhraseSequence Phrase { get; set; }

    public LearnAndReviewTaskGameData() : base() {
        Phrase = new PhraseSequence();
    }

}
