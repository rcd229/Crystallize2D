using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class SelectionMenuUI<T> : UIPanel, ITemporaryUI<List<T>, T> 
{
	const string ResourcePath = "UI/SelectionMenu";
	public static SelectionMenuUI<T> GetInstance() {
		return GameObjectUtil.GetResourceInstance<SelectionMenuUI<T>>(ResourcePath);
	}

	List<T> items;
	public GameObject buttonPrefab;
	protected T arg;
	
	public event EventHandler<EventArgs<T>> Complete;

	public virtual void Initialize(List<T> param1) {
		items = param1;
		/**
		 * load created menuObjects into the menu by instantiating the 
		 * gameObject with the info from the menuObjects
		 * TODO a lot of information passed around. Is it necessary to have one ScriptableObject
		 * and one GameObject while they are essentially the same?
		 **/
		foreach (var item in items) {
			GameObject instance = Instantiate<GameObject> (buttonPrefab);
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

	protected abstract void InitializeButton (GameObject obj, T item);
	//obtain attributes necessary to pass to the event manager
	protected T createEventArg (GameObject obj){
		return obj.GetComponent<DataContainer>().Retrieve<T>();
	}
	
	protected void Start ()
	{
		transform.localPosition = new Vector3 (0f, 0f, 0f);
	}
	
	//let event manager fires menu item selected event, with info about the item selected
	protected void MenuUI_OnClicked (object sender, EventArgs e)
	{	
		//sender is the prefab gameobject, obtain attributes first
		// and then create MenuItemEventArg from the object attributes
		arg = createEventArg ((((UIButton)sender).gameObject));
	}

	/**
	 * Raise the complete events with the arg as event argument
	 * There is no way to access arg from subclasses
	 */
	protected void RaiseComplete(){
		Complete.Raise (this, new EventArgs<T> (arg));
	}

	protected bool NoSelection(){
		return arg == null;
	}
}
