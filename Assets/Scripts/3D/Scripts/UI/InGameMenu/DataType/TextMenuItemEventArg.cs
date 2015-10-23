using UnityEngine;
using System.Collections;

public class TextMenuItemEventArg : MenuItemEventArg {

	string itemName;

	public TextMenuItemEventArg (string name)
	{
		this.itemName = name;
	}
	
	public string getName() {return itemName;} 
}
