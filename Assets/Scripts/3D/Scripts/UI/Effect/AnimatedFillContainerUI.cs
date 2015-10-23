using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class AnimatedFillContainerUI : UIMonoBehaviour {

    public float speed = 1000f;

    void Update() {
        var parent = rectTransform.parent as RectTransform;
        rectTransform.offsetMin = Vector2.MoveTowards(rectTransform.offsetMin, Vector2.zero, speed * Time.deltaTime);
        rectTransform.offsetMax = Vector2.MoveTowards(rectTransform.offsetMax, parent.sizeDelta, speed * Time.deltaTime);
    }

}
