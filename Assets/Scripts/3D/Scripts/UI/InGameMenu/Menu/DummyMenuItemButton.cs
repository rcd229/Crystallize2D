using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DummyMenuItemButton : IMenuItemButton {

	#region IMenuItemButton implementation
	public void Initialize (GameObject obj, GameMenuItem menuObj)
	{
		DummyMenuItem item = (DummyMenuItem)menuObj;
		obj.GetComponent<Image> ().sprite = item.image;
		obj.GetComponentInChildren<Text> ().text = item.itemName;
	}
	public MenuItemEventArg GetEventArgs (GameObject obj)
	{
		return new DummyMenuItemEventArg (obj.GetComponent<Image> ().sprite, obj.GetComponentInChildren<Text> ().text);
	}
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
