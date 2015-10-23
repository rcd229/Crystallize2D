using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UIFadeOut : UIMonoBehaviour {

    public float duration = 1f;

    IEnumerator Start() {
        for(float t = 0; t < 1f; t += Time.deltaTime / duration){
            canvasGroup.alpha = 1f - t;

            yield return null;
        }
    }

}
