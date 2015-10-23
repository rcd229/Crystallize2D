using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseReviewData {

	static TimeSpan BaseTime = new TimeSpan (0, 1, 0);

	public int ID { get; set; }
	public List<PhraseReviewSessionData> ReviewData { get; set; }
	public DateTime RootTime { get; set; }
	public int Level { get; set; }

	public PhraseReviewData() { 
		ReviewData = new List<PhraseReviewSessionData> ();
	}

	public PhraseReviewData(int id) {
		ID = id;
		ReviewData = new List<PhraseReviewSessionData> ();
		Level = 0;
	}

	public TimeSpan GetDurationToNextReview(){
		int total = 1;
		for (int i = 0; i < Level; i++) {
			total *= 2;
		}
		return TimeSpan.FromTicks(total * BaseTime.Ticks);
	}

	public DateTime GetNextReviewTime(){
		if (ReviewData.Count == 0) {
			return DateTime.Now;
		}
		return RootTime + GetDurationToNextReview();
	}

	public void Reset(){
		RootTime = ReviewManager.main.simulatedTime;
		Level = 0;
	}

	public void LogReview(DateTime time, bool result, bool setRootTime){
		if (setRootTime) {
			RootTime = ReviewManager.main.simulatedTime;
			if(result){
				Level++;
			}
		}
		ReviewData.Add(new PhraseReviewSessionData(time, result));
	}

	public bool IsLive(DateTime time){
		if(ReviewData.Count == 0){
			return false;
		}
		if (!ReviewData.Last ().SessionResult) {
			return false;
		}
		return time < GetNextReviewTime ();
	}

	public bool GetLastResult(){
		if (ReviewData.Count == 0) {
			return true;
		}
		return ReviewData.Last ().SessionResult;
	}

}
