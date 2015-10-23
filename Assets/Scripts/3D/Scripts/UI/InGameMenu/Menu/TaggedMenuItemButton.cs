using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TaggedMenuItemButton : MonoBehaviour {

	public void Initialize (GameObject obj, GameMenuItem menuObj)
	{
		TaggedMenuItem item = (TaggedMenuItem)menuObj;
		foreach(var t in item.texts) {
			var text = obj.AddComponent<Text>();
			text.text = t;
		}
	}
	public MenuItemEventArg GetEventArgs (GameObject obj)
	{
		Text[] UI_texts = obj.GetComponentsInChildren<Text> ();
		string[] texts = new string[UI_texts.Length];
		for(int i = 0; i < texts.Length; i++){
			texts[i] = UI_texts[i].text;
		}
		return new TaggedMenuItemEventArg (texts);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
