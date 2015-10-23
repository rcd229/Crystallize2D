using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/Element/PopupGlow")]
public class PopupGlowUI : UIMonoBehaviour, IInitializable<RectTransform> {

    const float Speed = 2f;

    public Transform target;

    public void Initialize(RectTransform param1) {
        this.target = param1;
    }

    IEnumerator Start() {
        transform.SetParent(target.transform, false);
        //transform.localScale = Vector3.zero;

        //for (float t = 0; t < 1f; t += Time.deltaTime * Speed) {
        //    transform.localScale = t * Vector3.one;
        //    yield return null;
        //}
        for (float t = 0; t < 1f; t += Time.deltaTime * Speed) {
            canvasGroup.alpha = Mathf.PingPong(5f * t, 1f); //t;
            //transform.localScale = (1f + 0.25f * Mathf.PingPong(2f * t, 1f)) * Vector3.one;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(0.25f);

        for (float t = 0; t < 1f; t += Time.deltaTime * 2f) {
            transform.localScale = new Vector3(1f + t, 1f + 0.5f * t, 0);
            canvasGroup.alpha = 1f - t;
            yield return null;
        }

        Destroy(gameObject);
    }

}
