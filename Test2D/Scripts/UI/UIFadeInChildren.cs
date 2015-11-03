using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UIFadeInChildren : MonoBehaviour {

    void OnEnable() {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn() {
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0;
        }

        for (int i = 0; i < transform.childCount; i++) {
            for (float t = 0; t < 1f; t += Time.deltaTime) {
                transform.GetChild(i).GetComponent<CanvasGroup>().alpha = t;
                yield return null;
            }
            transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1f;

            yield return new WaitForSeconds(1f);
        }
    }

}
