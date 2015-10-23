using UnityEngine;
using System;
using System.Collections;

public class UIRequestEventArgs : EventArgs {

	public GameObject MenuParent { get; set; }

	public UIRequestEventArgs(GameObject menuParent){
		MenuParent = menuParent;
	}

}
