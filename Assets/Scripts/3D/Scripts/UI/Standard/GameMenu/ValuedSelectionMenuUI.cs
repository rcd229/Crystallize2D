using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ValuedSelectionMenuUI : ConfirmMenuUI<ValuedItem> {
	

	const string ResourcePath = "UI/ValuedSelectionMenu";
	new public static ValuedSelectionMenuUI GetInstance() {
		return GameObjectUtil.GetResourceInstance<ValuedSelectionMenuUI>(ResourcePath);
	}

	#region implemented abstract members of SelectionMenuUI
	protected override void InitializeButton (GameObject obj, ValuedItem item)
	{
		var texts = obj.GetComponentsInChildren<Text> ();
		texts [0].text = item.Text.GetText();
		if(item.ShowValue)
			texts [1].text = item.Value.ToString();
		else
			Destroy(texts[1]);
		obj.AddComponent<DataContainer>().Store(item);

	}
	#endregion
	
}
