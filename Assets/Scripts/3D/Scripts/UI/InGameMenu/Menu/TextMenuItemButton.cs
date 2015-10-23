using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextMenuItemButton : MonoBehaviour {

	public void Initialize (GameObject obj, GameMenuItem menuObj)
	{
		TextMenuItem item = (TextMenuItem)menuObj;
		obj.GetComponentInChildren<Text> ().text = item.text;
	}
	public MenuItemEventArg GetEventArgs (GameObject obj)
	{
		return new TextMenuItemEventArg (obj.GetComponentInChildren<Text> ().text);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
