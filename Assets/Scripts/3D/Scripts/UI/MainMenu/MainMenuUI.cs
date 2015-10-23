using UnityEngine;
using System;
using System.Collections;

[ResourcePath("UI/MainMenu")]
public class MainMenuUI : UIPanel, ITemporaryUI<object,object> {

    static GameObject _instance;

    public event EventHandler<EventArgs<object>> Complete;

    bool initialized = false;

    public void Initialize(object param1) {
        if (_instance) {
            Destroy(gameObject);
            return;
        }
        MainCanvas.main.PushLayer();
        MainCanvas.main.Add(transform);
        initialized = true;
    }

    public void SaveGame() {
        PlayerDataLoader.Save();
    }

    public void ReturnToTitle() {
        foreach (var go in GameObject.FindObjectsOfType<GameObject>()) {
            Destroy(go);
        }
        Application.LoadLevel("ChooseScene");
    }

    public void OpenSettings() {
        UILibrary.Settings.Get(null);
    }

    public void QuitGame() {
        Application.Quit();
    }

    void OnDestroy() {
        if (initialized) {
            MainCanvas.main.PopLayer();
            Complete.Raise(this, null);
        }
    }

}
