using UnityEngine;
using System.Collections;

public class TaggedMenuItemEventArg : MenuItemEventArg {

	string[] texts;

	public TaggedMenuItemEventArg (string[] arg)
	{
		this.texts = arg;
	}
	
	public string[] getTexts() {return texts;} 
}
