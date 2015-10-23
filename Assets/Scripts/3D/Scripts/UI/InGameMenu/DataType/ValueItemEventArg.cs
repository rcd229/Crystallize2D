using UnityEngine;
using System.Collections;

public class ValuedItemEventArg : MenuItemEventArg {

	string itemName;
	int value;

	public ValuedItemEventArg (string name, int v)
	{
		this.itemName = name;
		value = v;
	}
	
	public string getName() {return itemName;} 
	public int getValue() {return value;}
}
