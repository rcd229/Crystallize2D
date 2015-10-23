using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SessionReviewEntry<T> {

    public ItemReviewPlayerData<T> Review { get; set; }
    public int BeforeRank { get; set; }
    public int AfterRank { get; set; }

    public int BeforeLevel {
        get {
            return ItemReview.GetLevelForRank(BeforeRank);
        }
    }

    public int AfterLevel {
        get {
            return ItemReview.GetLevelForRank(AfterRank);
        }
    }

    public bool LevelChanged {
        get {
            return BeforeLevel != AfterLevel;
        }
    }

    public SessionReviewEntry(ItemReviewPlayerData<T> review) {
        Review = review;
        BeforeRank = review.Rank;
    }

}
