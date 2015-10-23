using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using JapaneseTools;

[ResourcePath("UI/SettingsMenu")]
public class SettingsMenu : UIMonoBehaviour, ITemporaryUI<object, object> {

    public ToggleGroup group;
    public Toggle romajiToggle;
    public Toggle kanaToggle;
    public JapaneseScriptType selected = JapaneseScriptType.Romaji;

    bool isRomaji =true;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(object args1) {
        MainCanvas.main.PushLayer();
        MainCanvas.main.Add(transform);

        isRomaji = ((PlayerData.Alive && PlayerData.Instance.ScriptType == JapaneseScriptType.Romaji) || GameSettings.Instance.TextMode == 2);
        CoroutineManager.Instance.WaitAndDo(InitToggle);
    }

    void InitToggle() {
        if (isRomaji) {
            romajiToggle.isOn = true;
            kanaToggle.isOn = false;
            group.NotifyToggleOn(romajiToggle);
        } else {
            romajiToggle.isOn = false;
            kanaToggle.isOn = true;
            group.NotifyToggleOn(kanaToggle);
        }
    }

    public void Close() {
        Destroy(gameObject);
        MainCanvas.main.PopLayer();
        Complete.Raise(null, null);
    }

    public void RomajiToggleValueChanged(bool value) {
        Debug.Log("value changed: " + value);
        if (PlayerData.Alive) {
            if (value) {
                PlayerData.Instance.ScriptType = JapaneseScriptType.Romaji;
            } else {
                PlayerData.Instance.ScriptType = JapaneseScriptType.Kana;
            }
        }

        if (value) {
            GameSettings.Instance.TextMode = 2;
        } else {
            GameSettings.Instance.TextMode = 1;
        }
        Debug.Log("Setting is: " + GameSettings.Instance.TextMode + "; " + PlayerData.Instance.ScriptType);
    }

}
