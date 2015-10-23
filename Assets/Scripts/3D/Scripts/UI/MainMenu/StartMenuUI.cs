using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ResourcePath("UI/StartMenu")]
public class StartMenuUI : UIMonoBehaviour, ITemporaryUI<string,object> {
	public event System.EventHandler<EventArgs<object>> Complete;
	
	public void Initialize (string args1)
	{
		//
	}
	
	public void Close ()
	{
		Destroy(gameObject);
		Complete.Raise(this, new EventArgs<object>(null));
	}
	
	public void SignUpButtonClicked() {
        if (GameSettings.Instance.Local) {
            PlayerData.Initialize(new PlayerData());
            PlayerData.Instance.PersonalData.StartPlayTime = System.DateTime.Now;
            PlayerData.Instance.PersonalData.SetName("LocalPlayer");
            Application.LoadLevel("PlayerCustomizer");
        } else {
            UILibrary.SignupMenu.Get(null);
            Close();
        }
	}

	public void LogInButtonClicked() {
        if (GameSettings.Instance.Local) {
            PlayerData.Instance.PersonalData.StartPlayTime = System.DateTime.Now;
            Application.LoadLevel("Start");
        } else {
            UILibrary.SigninMenu.Get(null);
            Close();
        }
	}

    public void SettingsButtonClicked() {
        UILibrary.Settings.Get(null);
    }

}
