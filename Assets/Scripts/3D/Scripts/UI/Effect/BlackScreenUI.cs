using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BlackScreenInitArgs {

    public bool StartBlack { get; private set; }
    public bool FadeAutomatically { get; private set; }

    public BlackScreenInitArgs(bool startBlack = false, bool fadeAutomatically = true) {
        this.StartBlack = startBlack;
        this.FadeAutomatically = fadeAutomatically;
    }

}

[ResourcePath("UI/Element/BlackScreen")]
public class BlackScreenUI : UIMonoBehaviour, ITemporaryUI<object, object> {

    static BlackScreenUI _instance;
    public static BlackScreenUI Instance {
        get {
            if (!_instance) {
                _instance = GameObjectUtil.GetResourceInstanceFromAttribute<BlackScreenUI>();
                MainCanvas.main.Add(_instance.transform, CanvasBranch.None);
            }
            return _instance;
        }
    }

    public static BlackScreenUI GetInstance() {
        return Instance;
    }

    public event EventHandler<EventArgs<object>> Complete;

    Coroutine coroutine;
    ProcessExitCallback callback;

    public void Initialize(object param1) {
        Initialize(0.25f, 1f, 0.25f);
    }

    public void Initialize(float dur1, float dur2, float dur3) {
        gameObject.SetActive(true);
        MainCanvas.main.Add(transform, CanvasBranch.Persistent);
        //Debug.Log("fading in and out");
        SetCoroutine(FadeInAndOut(dur1, dur2, dur3));
    }

    void OnEnable() {
        SetAlpha(0);
    }

    public void SetAlpha(float f) {
        canvasGroup.alpha = f;
    }

    public void FadeIn(float duration, ProcessExitCallback callback) {
        //Debug.Log("Fading in");
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        SetCoroutine(FadeSequence(duration, false), callback);
    }

    public void FadeOut(float duration, ProcessExitCallback callback) {
        //Debug.Log("fading out");
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        SetCoroutine(FadeSequence(duration, true), callback);
    }

    IEnumerator FadeSequence(float duration, bool toOpaque) {
        //Debug.Log("fading: " + toOpaque);
        var speed = 1f / duration;
        for (float t = 0; t < 1f; t += Time.deltaTime * speed) {
            if (toOpaque) {
                canvasGroup.alpha = t;
            } else {
                canvasGroup.alpha = 1f - t;
            }
            yield return null;
        }
        if (toOpaque) {
            canvasGroup.alpha = 1f;
        } else {
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        RaiseCallback();
    }

    IEnumerator FadeInAndOut(float d1, float d2, float d3) {
        yield return StartCoroutine(FadeSequence(d1, true));

        Complete.Raise(this, null);
        Complete = null;
        yield return new WaitForSeconds(d2);

        yield return StartCoroutine(FadeSequence(d3, false));
    }

    void SetCoroutine(IEnumerator coroutine, ProcessExitCallback callback) {
        if (this.coroutine != null) {
            StopCoroutine(this.coroutine);
            RaiseCallback();
        }
        this.callback = callback;
        this.coroutine = StartCoroutine(coroutine);
    }

    void RaiseCallback() {
        var c = callback;
        callback = null;
        if (c != null) {
            c(this, null);
        }
    }


    public void Close() {
        gameObject.SetActive(false);
    }

   
}
