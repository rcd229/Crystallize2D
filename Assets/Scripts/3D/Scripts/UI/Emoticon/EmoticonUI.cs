using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EmoticonInitArgs {
    public Transform Target{get; set;}
    public EmoticonType Type { get; set; }
    public EmoticonInitArgs(Transform target, EmoticonType type) {
        this.Target = target;
        this.Type = type;
    }
}

[ResourcePath("UI/Element/EmoticonBubble")]
public class EmoticonUI : UIPanel, ITemporaryUI<EmoticonInitArgs, object> {

    public event EventHandler<EventArgs<object>> Complete;

    public AnimationCurve sizeCurve;
    public Transform target;

    public void Initialize(EmoticonInitArgs args1) {
        if (args1.Target.GetComponent<DialogueActor>()) {
            args1.Target.GetComponent<DialogueActor>().SetPhrase(null);
        }

        target = args1.Target.GetSpeechBubbleTarget();
        GetComponent<Image>().sprite = args1.Type.Image;
    }

    public void Close() {
        Complete.Raise(this, null);
        Destroy(gameObject);
    }

    IEnumerator Start() {
        for (float t = 0; t < 1f; t += 4f * Time.deltaTime) {
            transform.localScale = sizeCurve.Evaluate(t) * Vector3.one;
            yield return null;
        }
        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(.75f);

        for (float t = 0; t < 1f; t += 4f * Time.deltaTime) {
            canvasGroup.alpha = 1f - t;
            yield return null;
        }

        Close();
    }

    void Update() {
        transform.position = Camera.main.WorldToScreenPoint(target.position);
    }

}
