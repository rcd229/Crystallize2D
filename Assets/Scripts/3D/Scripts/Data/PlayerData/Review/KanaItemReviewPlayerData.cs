using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class KanaItemReviewPlayerData : ItemReviewPlayerData<string> {

    public KanaItemReviewPlayerData() : base() {    }

    public KanaItemReviewPlayerData(string kana) : base() {
        Item = kana;
    }

}
