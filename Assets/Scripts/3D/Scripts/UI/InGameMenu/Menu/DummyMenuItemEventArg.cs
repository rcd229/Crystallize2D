using UnityEngine;
using System.Collections;

public class DummyMenuItemEventArg : MenuItemEventArg {

	string itemName;
	Sprite image;
	public DummyMenuItemEventArg (Sprite image, string name)
	{
		this.itemName = name;
		this.image = image;
	}
	
	public string getName() {return itemName;} 
	public Sprite getImage() {return image;} 
}
