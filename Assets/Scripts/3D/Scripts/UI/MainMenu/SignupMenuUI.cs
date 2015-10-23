using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ResourcePath("UI/SignupMenu")]
public class SignupMenuUI : UIMonoBehaviour, ITemporaryUI<string,object> {
	public event System.EventHandler<EventArgs<object>> Complete;

	public InputField usernameField;
	public InputField passwordField;
	public InputField confirmPassworldField;
	public Text noticeText;

    string enteredName;

	public void Initialize (string args1)
	{

	}

	public void Close ()
	{
		Destroy(gameObject);
		Complete.Raise(this, new EventArgs<object>(null));
	}
	
	public void ButtonClicked(){
        if (enteredName.IsEmptyOrNull()) {
            if (passwordField.text == confirmPassworldField.text) {
                enteredName = usernameField.text;
                CrystallizeNetwork.Client.RequestNameFromServer(new UsernamePasswordPair(usernameField.text, passwordField.text), HandleNameResponse);
            } else {
                noticeText.text = "Passwords do not match. Please try again.";
            }
        } else {
            noticeText.text = "Please wait.";
        }
	}

    public void BackButtonClicked() {
        UILibrary.StartMenu.Get("");
        Close();
    }

	void HandleNameResponse (bool result)
	{
		if (result) {
            UILibrary.SurveyPrompt.Set<SurveyUI>();
            UILibrary.SurveyPrompt.Get(null, HandleSurveyResponse, null);
		} else {
			noticeText.text = "Username is invalid or has already been taken. Please select a new username.";
		    enteredName = null;
        }
	}

    void HandleSurveyResponse(object sender, EventArgs<object> args) {
        PlayerData.Initialize(new PlayerData());
        PlayerData.Instance.PersonalData.StartPlayTime = System.DateTime.Now;
        PlayerData.Instance.PersonalData.SetName(enteredName);
        Application.LoadLevel("PlayerCustomizer");
    }

}
