using UnityEngine;
using System;
using System.Collections;

public class TriggerEventObject : MonoBehaviour {

	public event EventHandler<TriggerEventArgs> OnTriggerEnterEvent;
	public event EventHandler<TriggerEventArgs> OnTriggerExitEvent;

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		if (OnTriggerEnterEvent != null) {
			OnTriggerEnterEvent(this, new TriggerEventArgs(other));
		}
	}
	
	// Update is called once per frame
	void OnTriggerExit (Collider other) {
		if (OnTriggerExitEvent != null) {
			OnTriggerExitEvent(this, new TriggerEventArgs(other));
		}
	}
}
