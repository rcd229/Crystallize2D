using UnityEngine;
using System.Collections;

public class ConversationCreatorController : MonoBehaviour {

	UIFactoryRef<ConversationCreationArg, object> JTextEditor;
	UIFactoryRef<object, ConversationID> TaskSelector;
	UIFactoryRef<object, UserInfo> Login;
	UserInfo user;
	// Use this for initialization
	void Awake () {
		Login = new UIFactoryRef<object, UserInfo>();
		JTextEditor = new UIFactoryRef<ConversationCreationArg, object>();
		TaskSelector = new UIFactoryRef<object, ConversationID>();

		Login.Set<UserLoginUI>();
		JTextEditor.Set<ConversationCreatorUI>();
		TaskSelector.Set<ConversationCreationTaskSelector>();
		login(null, null);
	}

	void login(object o, object e){
		var loginPage = Login.Get(null);
		loginPage.Complete += HandleLoginComplete;
	}

	void HandleLoginComplete (object sender, EventArgs<UserInfo> e)
	{
		user = e.Data;
		chooseTask(null, null);
	}
	void chooseTask(object o, object e){
		var selector = TaskSelector.Get(null);
		selector.Complete += HandleTaskChosen;
	}

	void HandleTaskChosen (object sender, EventArgs<ConversationID> e)
	{
		var editor = JTextEditor.Get(ConversationDatabaseController.Instance.read(user, e.Data).CreateArg(user));
		editor.Complete += chooseTask;
	}
}
