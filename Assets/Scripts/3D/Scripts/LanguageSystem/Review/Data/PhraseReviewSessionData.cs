using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PhraseReviewSessionData {

	public DateTime SessionTime { get; set; }
	public bool SessionResult { get; set; }

	public PhraseReviewSessionData() { }

	public PhraseReviewSessionData(DateTime time, bool sessionResult) { 
		SessionTime = time;
		SessionResult = sessionResult;
	}

}
