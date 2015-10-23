using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MenuUI : MonoBehaviour
{

	public List<GameMenuItem> items = new List<GameMenuItem> ();
	public GameObject buttonPrefab;

	void Start ()
	{
		/**
		 * load created menuObjects into the menu by instantiating the 
		 * gameObject with the info from the menuObjects
		 * TODO a lot of information passed around. Is it necessary to have one ScriptableObject
		 * and one GameObject while they are essentially the same?
		 **/
		foreach (var item in items) {
			GameObject instance = Instantiate<GameObject> (buttonPrefab);
			instance.transform.SetParent (transform);
			//assign attributes
			instance.GetInterface<IMenuItemButton> ().Initialize(instance, item);
			//hook event handler
			instance.GetComponent<UIButton>().OnClicked += MenuUI_OnClicked;
		}
	}

	//let event manager fires menu item selected event, with info about the item selected
	void MenuUI_OnClicked (object sender, EventArgs e)
	{	
		//sender is the prefab gameobject, obtain attributes first
		// and then create MenuItemEventArg from the object attributes
		MenuItemEventArg arg = createEventArg ((((UIButton)sender).gameObject));
		//fire event using event manager
		//maybe can ignore sender
		CrystallizeEventManager.SelectFromMenu ((((UIButton)sender).gameObject), arg);
	}

	//obtain attributes necessary to pass to the event manager
	MenuItemEventArg createEventArg (GameObject obj) {
		//can get messy if a lot of info is passed around.
		return obj.GetInterface<IMenuItemButton> ().GetEventArgs (obj);
	}

}
