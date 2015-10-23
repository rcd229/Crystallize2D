using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UIFadeInAndOut : UIMonoBehaviour, ITemporaryUI<string, object> {

    public float duration = 1f;

    bool fade = true;

    IEnumerator Start() {
        if (fade) {
            yield return StartCoroutine(UIUtil.FadeInAndOutRoutine(canvasGroup));

            Close();
        }
    }


    public void Close() {
        Complete.Raise(this, null);
        Destroy(gameObject);
    }

    public void Initialize(string param1) {
        GetComponentInChildren<Text>().text = param1;
        if (param1.Length > 0) {
            if (param1[0] == '-') {
                fade = false;
                GetComponentInChildren<Text>().text = param1.Substring(1);
            }
        } 
    }

    public event EventHandler<EventArgs<object>> Complete;
}
