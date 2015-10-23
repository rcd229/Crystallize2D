using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class ContextActionArgs {

    public string Text { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsTemporary { get; set; }

    public ContextActionArgs(string text, bool isAvailable) {
        Text = text;
        IsAvailable = isAvailable;
        IsTemporary = false;
    }

    public ContextActionArgs(string text, bool isAvailable, bool isTemporary) : this(text, isAvailable)  {
        IsTemporary = isTemporary;
    }

}

[ResourcePath("UI/ContextActionStatusPanel")]
public class ContextActionStatusPanelUI : UIMonoBehaviour, ITemporaryUI<ContextActionArgs, object> {

    static ContextActionStatusPanelUI _instance;

    public Text text;
    bool started = false;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(ContextActionArgs param1) {
        if (_instance) {
            Destroy(_instance.gameObject);
        }
        _instance = this;

        if(text){
			text.text = param1.Text;
		}

        if (param1.IsTemporary) {
            SetCoroutine(UIUtil.FadeInAndOutRoutine(canvasGroup, 0.1f, 2f, 0.5f));
        } else {
            if (param1.IsAvailable) {
                SetCoroutine(UIUtil.FadeInRoutine(canvasGroup, 0.1f));
            } else {
                SetCoroutine(null);
                canvasGroup.alpha = 0;
                //SetCoroutine(UIUtil.FadeOutRoutine(canvasGroup));
            }
        }
    }

    public void Close() {
        SetCoroutine(ExitSequence());
    }

    void Start() {
        canvasGroup.alpha = 0;
    }

    void OnEnable() {
        if (started) {
            Destroy(gameObject);
        }
    }

    IEnumerator ExitSequence() {
        started = true;
        while (canvasGroup.alpha > 0) {
            canvasGroup.alpha -= Time.deltaTime * 2f;

            yield return null;
        }

        Destroy(gameObject);
    }

    

}
