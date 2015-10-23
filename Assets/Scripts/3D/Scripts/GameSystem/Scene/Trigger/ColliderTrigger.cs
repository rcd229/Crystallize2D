using UnityEngine;
using System;
using System.Collections;

public class ColliderTrigger : MonoBehaviour {

	public event EventHandler<ColliderTriggerEventArgs> TriggerEnter;
	public event EventHandler<ColliderTriggerEventArgs> TriggerExit;

	void OnTriggerEnter (Collider other) {
		if (TriggerEnter != null) {
			TriggerEnter(this, new ColliderTriggerEventArgs(other));
		}
	}

	void OnTriggerExit(Collider other){
		if (TriggerExit != null) {
			TriggerExit(this, new ColliderTriggerEventArgs(other));
		}
	}
}
