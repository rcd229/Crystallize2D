using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DummySelectReact : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CrystallizeEventManager.MenuSelectionEvent += DisplaySelection;
	}

	void DisplaySelection (object sender, MenuItemEventArg e)
	{
		if (e is DummyMenuItemEventArg) {
			DummyMenuItemEventArg arg = (DummyMenuItemEventArg) e;
			GetComponent<Image> ().sprite = arg.getImage();
			GetComponentInChildren<Text> ().text = arg.getName();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
