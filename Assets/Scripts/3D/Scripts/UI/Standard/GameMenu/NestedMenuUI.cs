using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NestedMenuUI<T> : ConfirmMenuUI<T>
	where T : MenuAcceptor

{
	const string ResourcePath = "UI/NestedMenu";
	new public static NestedMenuUI<T> GetInstance() {
		return GameObjectUtil.GetResourceInstance<NestedMenuUI<T>>(ResourcePath);
	}
	
	public override void Initialize (List<T> items)
	{
		buttonPrefab = new GameObject();
		/**
		 * load created menuObjects into the menu by instantiating the 
		 * gameObject with the info from the menuObjects
		 * TODO a lot of information passed around. Is it necessary to have one ScriptableObject
		 * and one GameObject while they are essentially the same?
		 **/
		foreach (T item in items) {
			GameObject instance = Instantiate<GameObject> (buttonPrefab);
			InitializeButton(instance, item);
			instance.transform.SetParent (transform);
			foreach (var c in instance.GetComponentsInChildren<RectTransform>()){
				c.anchoredPosition = new Vector2(0f, 0f);
			}
			instance.transform.localPosition = new Vector3 (0f, 0f, 0f);
			//assign attributes
			InitializeButton(instance, item);
			
			//hook event handler
			instance.GetComponent<UIButton>().OnClicked += MenuUI_OnClicked;
		}
	}




	protected override void InitializeButton (GameObject obj, T item)
	{
		obj = MenuItemBuilder.BuildMenuItemObject(item).GO;
		obj.AddComponent<DataContainer>().Store(item);
	}


}
