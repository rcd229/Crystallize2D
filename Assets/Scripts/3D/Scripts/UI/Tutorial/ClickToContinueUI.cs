using UnityEngine;
using System.Collections;

[ResourcePath("UI/Tutorial/ClickToContinue")]
public class ClickToContinueUI : BaseTemporaryUI<object, object>  {

    void Awake() {
        GetComponent<CanvasGroup>().alpha = 0;
    }

    IEnumerator Start() {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(UIUtil.FadeInRoutine(GetComponent<CanvasGroup>(), 0.5f));
    }

    void Update() {
        if (SpeechBubbleUI.LastSpeechBubble) {
            transform.SetParent(SpeechBubbleUI.LastSpeechBubble.transform, false);
        }
    }

}