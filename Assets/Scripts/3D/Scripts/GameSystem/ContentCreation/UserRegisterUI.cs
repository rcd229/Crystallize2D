using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserRegisterUI : UIMonoBehaviour {

	public Text Alert;
	public Text userNameAlertText;
	public InputField UserName;
	public Text passwordAlertText;
	public InputField Password;
	public Text repeatPasswordAlertText;
	public InputField RepeatPassword;
	public Toggle isAdmin;
	public Text adminRegistrationAlertText;
	public InputField AdminRegistrationCode;
	public UIButton Submit;

	// Use this for initialization
	void Start () {
		Submit.OnClicked += HandleSubmit;
	}

	void ClearAll(){
		userNameAlertText.gameObject.SetActive(false);
		passwordAlertText.gameObject.SetActive(false);
		repeatPasswordAlertText.gameObject.SetActive(false);
		adminRegistrationAlertText.gameObject.SetActive(false);

		UserName.text = "";
		Password.text = "";
		RepeatPassword.text = "";
		AdminRegistrationCode.text = "";
		isAdmin.isOn = false;
	}

	void HandleSubmit (object sender, EventArgs<UnityEngine.EventSystems.PointerEventData> e)
	{
		if(validate()){
			if(UserInfoTable.Instance.Create(UserName.text, Password.text, isAdmin.isOn)){
				displayAlert(Alert, "successfully created user.");
				ClearAll();
			}else{
				displayAlert(userNameAlertText, "Name already used");
			}
		}
	}

	void displayAlert(Text alert, string message){
		alert.gameObject.SetActive(true);
		alert.text = message;
	}

	bool validate(){
		bool ret = true;
		//check password length
		if (Password.text.Length < 8){
			displayAlert(passwordAlertText, "Password should be at least 8 characters");
			ret = false;
		}

		if(Password.text != RepeatPassword.text){
			displayAlert(repeatPasswordAlertText, "Two password should be the same");
			ret = false;
		}

		if(isAdmin.isOn && AdminRegistrationCode.text != "this is some magical text 1234 *(&"){
			displayAlert(adminRegistrationAlertText, "invalid admin registration code");
			ret = false;
		}
		return ret;
	}
}
