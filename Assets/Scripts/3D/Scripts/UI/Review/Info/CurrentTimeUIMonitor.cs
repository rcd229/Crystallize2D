using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CurrentTimeUIMonitor : MonoBehaviour {
	
	public Text text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		text.text = ReviewManager.main.simulatedTime.ToString ();
	}
}
