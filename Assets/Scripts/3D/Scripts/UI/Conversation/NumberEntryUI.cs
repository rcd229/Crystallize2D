using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class NumberEntryUI : UIPanel, ITemporaryUI<object, int> {

	int result;
	GameObject display;
	GameObject entry;
	Text displayed;

	int Result{
		get{
			return result;
		}
		set{
			result = value;
			displayed.text = result.ToString();
		}
	}

	const string ResourcePath = "UI/NumberInput";
	public static NumberEntryUI GetInstance() {
		return GameObjectUtil.GetResourceInstance<NumberEntryUI>(ResourcePath);
	}
	#region ICompleteable implementation
	public event EventHandler<EventArgs<int>> Complete;
	#endregion
	
	#region IInitializable implementation
	public void Initialize (object param1)
	{
		result = 0;
		display = transform.Find("Display").gameObject;
		entry = transform.Find("Entry").gameObject;

		foreach (var v in entry.GetComponentsInChildren<UIButton>()){
			v.OnClicked += HandleOnClicked;
		}

		displayed = display.GetComponentInChildren<Text>();
	}

	void HandleOnClicked (object sender, EventArgs e)
	{
		string name = ((UIButton)sender).gameObject.name ;
		Debug.Log (name);
		if(name == "Enter"){
			EnterPressed();
		}
		else if (name == "Del"){
			DelPressed();
		}
		else{
			int i;
			int.TryParse(name, out i);
			NumberPressed(i);
		}
	}

	void NumberPressed(int number){
		Result = Result * 10 + number;
	}

	void DelPressed(){
		Result = Result / 10;
	}
	void EnterPressed(){
		RaiseComplete();
	}
	#endregion

	void Start ()
	{
		var parent = GameObject.FindGameObjectWithTag(TagLibrary.SubStatus);
		if (parent) {
			transform.SetParent(parent.transform, false);
		}else{
            MainCanvas.main.Add(transform);
		}
		transform.localPosition = new Vector3 (0f, 0f, 0f);
	}
		
	void RaiseComplete ()
	{
		GameObject.Destroy(gameObject);
		Complete.Raise(this, new EventArgs<int>(Result));
	}
}
