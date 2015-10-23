using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class VerticalScrollArea : UIMonoBehaviour {

    public Scrollbar scrollbar;
    public RectTransform mask;
    public RectTransform content;
    public bool reverse = false;
    public float fixedHeight = -1f;
    public float paddingBottom = 200f;
    public float paddingTop = 200f;
    //public float maxHeight = 600f;

    float overflow = 0;
    float lastContentWidth;
    float lastContentHeight;

    float MaxHeight {
        get {
            if (fixedHeight > 0) {
                return fixedHeight;
            }
            return Screen.height - paddingBottom - paddingTop;
        }
    }

    IEnumerator Start() {
        scrollbar.onValueChanged.AddListener(UpdateScrollLocation);
        yield return null;
        yield return null;
        scrollbar.value = 0.001f;
    }

    void Update() {
        var r = content.rect;
        if (lastContentWidth != r.width || lastContentHeight != r.height) {
            RefreshScroll();
        }

        if (overflow > 0 && rectTransform.GetScreenRect().Contains(Input.mousePosition)) {
            var scrollForce = Input.GetAxis("Mouse ScrollWheel");
            scrollbar.value -= scrollForce * 1000f / overflow;// *0.1f;
        }
    }

    void RefreshScroll() {
        var height = content.rect.height + 16f;

        if (height > MaxHeight) {
            scrollbar.gameObject.SetActive(true);
            scrollbar.size = MaxHeight / height;
            mask.GetComponent<LayoutElement>().preferredHeight = MaxHeight;
        } else {
            scrollbar.gameObject.SetActive(false);

            if (fixedHeight > 0) {
                mask.GetComponent<LayoutElement>().preferredHeight = MaxHeight;
            } else {
                mask.GetComponent<LayoutElement>().preferredHeight = height;
            }

            if (reverse) {
                content.transform.localPosition = new Vector2(2f, 2f);
            } else {
                content.transform.localPosition = new Vector2(2f, -2f);
            }
        }

        mask.GetComponent<LayoutElement>().preferredWidth = content.rect.width + 12f;

        overflow = -(MaxHeight - (content.rect.height + 16f));
        lastContentWidth = content.rect.width;
        lastContentHeight = content.rect.height;
    }

    public void UpdateScrollLocation(float pos) {
        var offset = -overflow * pos;
        if (reverse) {
            offset = -offset;
        }
        content.transform.localPosition = new Vector2(2f, -2f - offset);
    }

}
