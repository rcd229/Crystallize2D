using UnityEngine;
using System.Collections;
/**
 * This should be a persistent server handling data loading and saving
 */
public class ConversationContentDataServer : MonoBehaviour {

	void OnApplicationQuit(){
		ConversationTable.Save();
	}

	void Awake(){
		ConversationTable.Load();
		StartCoroutine(save());
	}
	
	IEnumerator save() {
		while(true){
			yield return new WaitForSeconds(30);
			ConversationTable.Save ();
		}
	}
	
	UIFactoryRef<UserInfo, object> TaskCreator;
	UIFactoryRef<object, UserInfo> Login;
	UserInfo user;
	// Use this for initialization
	void Start () {
		Login = new UIFactoryRef<object, UserInfo>();
		TaskCreator = new UIFactoryRef<UserInfo, object>();
		
		Login.Set<UserLoginUI>();
		TaskCreator.Set<ConversationTaskCreatorUI>();
		login(null, null);
	}
	
	void login(object o, object e){
		var loginPage = Login.Get(null);
		loginPage.Complete += HandleLoginComplete;
	}
	
	void HandleLoginComplete (object sender, EventArgs<UserInfo> e)
	{
		user = e.Data;
		if(user.CanCreate){
			createTask();
		}else{
			login(null, null);
		}

	}
	void createTask(){
		var createUI = TaskCreator.Get(user);
		createUI.Complete += (sender, e) => createTask();
	}
}
