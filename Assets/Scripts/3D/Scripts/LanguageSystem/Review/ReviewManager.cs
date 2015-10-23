using UnityEngine;
using System;
using System.Collections;

public class ReviewManager : MonoBehaviour {

	public static ReviewManager main { get; set; }

	public ReviewLog reviewLog;

	public DateTime simulatedTime;

	void Awake(){
		main = this;
		simulatedTime = DateTime.Now;
		reviewLog = new ReviewLog ();
	}
	
	// Update is called once per frame
	void Update () {
		simulatedTime += new TimeSpan (0, 0, 0, 0, (int)(Time.deltaTime * 1000f));
	}

	public void AddSimulatedTime(float hours){
		simulatedTime += new TimeSpan (0, (int)(hours * 60f), 0);
	}

	public void LoadReviewLog(ReviewLog log){
		reviewLog = log;
	}

}
