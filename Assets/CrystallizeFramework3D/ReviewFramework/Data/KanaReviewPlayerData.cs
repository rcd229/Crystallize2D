using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class KanaReviewPlayerData : ReviewPlayerData<string, KanaItemReviewPlayerData> {

    public KanaReviewPlayerData() : base() { }

    public HashSet<string> GetCollectedKana() {
        var set = new HashSet<string>();
        foreach (var r in Reviews) {
            set.Add(r.Item);
        }
        return set;
    }

    public HashSet<string> GetKnownKana() {
        var set = new HashSet<string>();
        foreach (var r in Reviews) {
            if (r.Rank >= 2) {
                set.Add(r.Item);
            }
        }
        return set;
    }

}
