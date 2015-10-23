using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ReviewLog {

	public List<PhraseReviewData> Reviews { get; set; }

	public ReviewLog() { 
		Reviews = new List<PhraseReviewData> ();
	}

	public PhraseReviewData GetReview(int key){
		var rev = (from r in Reviews where r.ID == key select r).FirstOrDefault();
		if (rev == null) {
			rev = new PhraseReviewData(key);
			Reviews.Add(rev);
		}
		return rev;
	}

	public void LogReview(int key, DateTime time, bool result){
		GetReview (key).LogReview (time, result, true);
	}

}
