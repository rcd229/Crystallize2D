using UnityEngine;
using System.Collections;
using System;

public interface IMenuItemButton {

	//initialize gameobject obj with the info given by menuObj
	void Initialize(GameObject obj, GameMenuItem menuObj);
	//create eventArgs from gameObjects
	MenuItemEventArg GetEventArgs(GameObject obj);

}
