using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ResourcePath("UI/SigninMenu")]
public class SigninMenuUI : UIMonoBehaviour, ITemporaryUI<string, object> {
    public event System.EventHandler<EventArgs<object>> Complete;

    public InputField usernameField;
    public InputField passwordField;
    public Text noticeText;

    string enteredName;

    public void Initialize(string args1) {

    }

    public void Close() {
        Destroy(gameObject);
        Complete.Raise(this, new EventArgs<object>(null));
    }

    public void ButtonClicked() {
        if (enteredName.IsEmptyOrNull()) {
            enteredName = usernameField.text;
            CrystallizeNetwork.Client.RequestPlayerDataFromServer(new UsernamePasswordPair(usernameField.text, passwordField.text), HandleNameResponse);
        } else {
            noticeText.text = "Please wait.";
        }
    }

    public void BackButtonClicked() {
        UILibrary.StartMenu.Get("");
        Close();
    }

    void HandleNameResponse(PlayerData result) {
        if (result != null) {
            PlayerData.Initialize(result);
            PlayerData.Instance.PersonalData.StartPlayTime = System.DateTime.Now;
            Application.LoadLevel("Start");
        } else {
            noticeText.text = "Username or password is invalid.";
            enteredName = null;
        }
    }
}
