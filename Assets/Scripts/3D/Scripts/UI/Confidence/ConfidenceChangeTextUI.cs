using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[ResourcePath("UI/Element/ConfidenceText")]
public class ConfidenceChangeTextUI : UIMonoBehaviour {

    const float Speed = 1f;

    public AnimationCurve heightCurve;
    public AnimationCurve rotationCurve;
    public Color negativeColor = Color.red;
    public Color positiveColor = Color.green;
    public int amount = -1;

    float xExtent = 0;

    void Awake() {
        canvasGroup.alpha = 0;
    }

    IEnumerator Start() {
        //Debug.Log("conf changed");
        MainCanvas.main.Add(transform, CanvasBranch.None);
        yield return null;
        

        if (amount > 0) {
            GetComponent<Text>().text = //"confidence +" + 
                "+" + amount.ToString();
            GetComponent<Text>().color = positiveColor;

            yield return null;

            xExtent = rectTransform.rect.width * 0.5f;
            //Debug.Log(xExtent);

            for (float t = 0; t < 1f; t += Time.deltaTime * Speed) {
                transform.position = GetScreenPosition() + Vector3.up * t * 24f;
                canvasGroup.alpha = 3f * (1f - t);

                yield return null;
            }
        } else {
            GetComponent<Text>().text = //"confidence " + 
                amount.ToString();
            GetComponent<Text>().color = negativeColor;

            yield return null;

            xExtent = rectTransform.rect.width * 0.5f;
            //Debug.Log(xExtent);

            for (float t = 0; t < 1f; t += Time.deltaTime * Speed) {
                transform.position = GetScreenPosition() + Vector3.up * heightCurve.Evaluate(t) * 24f;
                transform.rotation = Quaternion.Euler(0, 0, rotationCurve.Evaluate(t) * 45f);
                canvasGroup.alpha = 3f * (1f - t);

                yield return null;
            }
        }

        Destroy(gameObject);
    }

    Vector3 GetScreenPosition() {
        Vector2 pos = Vector2.zero;
        var confTarget = GameObject.FindGameObjectWithTag(TagLibrary.ConfidenceTarget);
        if (confTarget) {
            pos = confTarget.GetComponent<RectTransform>().GetCenter();
        } else {
            var target = PlayerManager.Instance.PlayerGameObject;
            pos = Camera.main.WorldToScreenPoint(target.transform.position + Vector3.up * 1.9f);
        }

        if (pos.x < xExtent) {
            pos.x = xExtent;
        }
        return pos;
    }

}
