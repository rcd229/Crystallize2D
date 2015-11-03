using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UIPanel : UIMonoBehaviour, IPanel {

    //Coroutine fadeOut;

    public virtual void Close() {
        Destroy(gameObject);
    }

    public virtual void SetVisible(bool visible) {
        gameObject.SetActive(visible);
    }

    public virtual void SetInteractive(bool interactive) {
        if (interactive) {
            canvasGroup.alpha = 1f;
        } else {
            canvasGroup.alpha = 0.5f;
        }
    }

    //void FadeOut() {
    //    fadeOut = StartCoroutine(FadeOutCoroutine());
    //}

    //IEnumerator FadeOutCoroutine() {
    //    canvasGroup.interactable = false;
    //    while (canvasGroup.alpha > 0) {
    //        canvasGroup.alpha -= Time.deltaTime;

    //        yield return null;
    //    }

    //    fadeOut = null;
    //}

}
