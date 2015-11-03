using UnityEngine;
using System;
using System.Collections;

public class TriggerEventArgs : EventArgs {

	public Collider Collider { get; set; }

	public TriggerEventArgs (Collider collider){
		this.Collider = collider;
	}

}
