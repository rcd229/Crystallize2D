using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour {

    void Start() {
        if (Screen.fullScreen) {
            var res = Screen.resolutions[Screen.resolutions.Length - 1];
            Screen.SetResolution(res.width, res.height, true);
        }

        if (GameSettings.Instance.Local) {
            RPCFunctions.IsLocalOnly = true;
            Application.LoadLevel("Title");
            return;
        }

        if (GameSettings.Instance.IsServer) {
            Application.LoadLevel("Server");
        } else {
            //CrystallizeNetwork.InitializeClient();
            //Application.LoadLevel("MultiplayerLobby");
            Application.LoadLevel("Title");
        }
    }

}