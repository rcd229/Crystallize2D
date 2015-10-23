using UnityEngine;
using System.Collections;

public class TitleProcess : MonoBehaviour {

    public GameObject connectingMessage;

    // Use this for initialization
    IEnumerator Start() {
        if (!GameSettings.Instance.Local) {
            while (!CrystallizeNetwork.Connected) {
                yield return null;
            }
        }
        connectingMessage.SetActive(false);

        yield return null;

        LinkUI();
        //		var ui = UILibrary.PrestartMainMenu.Get(null);
        //		ui.Complete += HandleMainMenuComplete;
        UILibrary.StartMenu.Get(null, null, null);
    }

    void HandleMainMenuComplete(object sender, EventArgs<bool> e) {
        var isNewGame = e.Data;
        if (isNewGame) {
            Application.LoadLevel("PlayerCustomizer");
        } else {
            GameObject.Destroy(((UIPanel)sender).gameObject);
            if (GameSettings.Instance.Local) {
                Application.LoadLevel("Start");
                return;
            }
            var ui = UILibrary.PlayerLoader.Get(null);
            ui.Complete += HandleLoadPlayerComplete;
            ((PlayerNameEntryUI)ui).Canceled += HandleCanceled;
        }
    }

    void HandleCanceled(object sender, EventArgs<object> e) {
        Application.LoadLevel("PreStart");
    }

    void HandlePlayerLoaderCanceled(object sender, EventArgs<object> e) {
        GameObject.Destroy(((UIPanel)sender).gameObject);
        var ui = UILibrary.PrestartMainMenu.Get(null);
        ui.Complete += HandleMainMenuComplete;
    }

    void HandleLoadPlayerComplete(object sender, EventArgs<PlayerData> e) {
        Application.LoadLevel("Start");
    }

    void LinkUI() {
        UILibrary.PlayerLoader.Set(PlayerNameEntryUI.GetInstance);
        UILibrary.PrestartMainMenu.Set(GameMainMenuUI.GetInstance);

        UILibrary.StartMenu.Set<StartMenuUI>();
        UILibrary.SignupMenu.Set<SignupMenuUI>();
        UILibrary.SigninMenu.Set<SigninMenuUI>();
        UILibrary.Settings.Set<SettingsMenu>();
    }

    void LinkProcess() {

    }

}
