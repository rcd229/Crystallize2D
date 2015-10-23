using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class HorizontalScrollArea : UIMonoBehaviour {

    public Scrollbar scrollbar;
    public RectTransform container;
    public RectTransform mask;
    public RectTransform content;
    public RectOffset padding;

    float overflow = 0;
    float lastContentWidth;
    float lastContentHeight;

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
            UpdateScrollLocation(scrollbar.value);
        }

        if (overflow > 0 && rectTransform.GetScreenRect().Contains(Input.mousePosition)) {
            var scrollForce = Input.GetAxis("Mouse ScrollWheel");
            scrollbar.value -= scrollForce * 1000f / overflow;
        }
    }

    void RefreshScroll() {
        var width = content.rect.width + padding.left + padding.right;

        if (width > container.rect.width) {
            scrollbar.gameObject.SetActive(true);
            scrollbar.size = container.rect.width / width;
            mask.GetComponent<LayoutElement>().preferredWidth = container.rect.width;
        } else {
            scrollbar.gameObject.SetActive(false);

            mask.GetComponent<LayoutElement>().preferredWidth = width;
            content.transform.localPosition = new Vector2(padding.left, padding.bottom);
        }

        mask.GetComponent<LayoutElement>().preferredHeight = content.rect.height + padding.top + padding.bottom;

        overflow = (width - container.rect.width) + padding.left + padding.right;
        lastContentWidth = content.rect.width;
        lastContentHeight = content.rect.height;
    }

    public void UpdateScrollLocation(float pos) {
        var offset = overflow * pos;
        content.transform.localPosition = new Vector2(padding.left - offset, padding.bottom);
    }

}
