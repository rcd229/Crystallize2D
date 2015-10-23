using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[ResourcePathAttribute("UI/ContentCreation/Login")]
public class UserLoginUI : MonoBehaviour, ITemporaryUI<object, UserInfo> {
	#region ICompleteable implementation

	public event EventHandler<EventArgs<UserInfo>> Complete;

	#endregion
	public InputField nameInput;
	public InputField passWordField;
	public Text alertFields;
	public UIButton confirm;
	// Use this for initialization
	#region IInitializable implementation
	
	public void Initialize (object args1)
	{

		alertFields.gameObject.SetActive(false);
		confirm.OnClicked += HandleConfirm;
	}
	
	#endregion
	
	void HandleConfirm (object sender, EventArgs<UnityEngine.EventSystems.PointerEventData> e)
	{
		string userName = nameInput.text;
		string password = passWordField.text;
		UserInfo info = UserInfoTable.Instance.Get(userName, password);
		if(info == null){
			alertFields.gameObject.SetActive(true);
			alertFields.text = "Wrong username or password";
		}else{
			Complete.Raise(this, new EventArgs<UserInfo>(info));
			Close();
		}
	}

	#region ICloseable implementation

	public void Close ()
	{
		GameObject.Destroy(gameObject);
	}

	#endregion



}
