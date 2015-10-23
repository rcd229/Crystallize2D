using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[ResourcePathAttribute("UI/ContentCreation/TaskCreator")]
public class ConversationTaskCreatorUI : MonoBehaviour, ITemporaryUI<UserInfo, object>{
	public UIButton SubmitButton;
	public InputField title;
	public InputField description;
	UserInfo user;
	
	void HandleOnClicked (object sender, EventArgs e)
	{
		ConversationDatabaseController.Instance.create(user, title.text, description.text);
		Complete.Raise(this, null);
		Close();
	}
	#region ICompleteable implementation

	public event EventHandler<EventArgs<object>> Complete;

	#endregion

	#region IInitializable implementation

	public void Initialize (UserInfo args1)
	{
		SubmitButton.OnClicked += HandleOnClicked;
		user = args1;
	}

	#endregion

	#region ICloseable implementation

	public void Close ()
	{
		GameObject.Destroy(gameObject);
	}

	#endregion


}
