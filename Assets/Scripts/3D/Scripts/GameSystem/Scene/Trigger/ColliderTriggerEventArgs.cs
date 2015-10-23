using UnityEngine;
using System;
using System.Collections;

public class ColliderTriggerEventArgs : EventArgs {

	public Collider Collider { get; set; }

	public ColliderTriggerEventArgs(Collider collider){
		Collider = collider;
	}

}
