using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[ResourcePathAttribute("UI/ContentCreation/TaskSelector")]
public class ConversationCreationTaskSelector : UIMonoBehaviour, ITemporaryUI<object, ConversationID> {

	public GameObject buttonPrefab;
	public GameObject buttonParent;
	List<GameObject> buttonInstances = new List<GameObject>();
	
	public event EventHandler<EventArgs<ConversationID>> Complete;
	#region IInitializable implementation
	public void Initialize (object args1)
	{
		List<TaskSelectionInfo> options = ConversationDatabaseController.Instance.readAllIDs();
		Debug.Log(options.Count);
		UIUtil.GenerateChildren(options, buttonInstances, buttonParent.transform, GenerateButton);
		Debug.Log(buttonInstances.Count);
	}

	GameObject GenerateButton(TaskSelectionInfo info){
		var instance = Instantiate<GameObject>(buttonPrefab);
		instance.GetComponentInChildren<Text>().text = info.title;
		instance.GetComponent<UIButton>().OnClicked += (s, e) => submit (info.ID);
		Debug.Log("info:" + info.ID);
		return instance;
	}

	void submit(ConversationID id){
		close ();
		Complete.Raise(this,new EventArgs<ConversationID>(id));
	}

	#endregion
	#region ICloseable implementation
	public void Close ()
	{
		close();
	}
	#endregion

	void close(){
		GameObject.Destroy(this.gameObject);
	}
}
