using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewStateArgs : EventArgs {

    public List<PhraseItemReviewPlayerData> Reviews { get; private set; }

    public ReviewStateArgs() {
        Reviews = PlayerData.Instance.Reviews.GetCurrentReviews();
    }

}
