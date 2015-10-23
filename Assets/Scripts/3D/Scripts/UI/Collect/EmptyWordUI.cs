using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EmptyWordUI : MonoBehaviour {

    public RectTransform child;

    IEnumerator Start() {
        yield return null;
        SetMinWidth();
    }

    void SetMinWidth() {
        GetComponent<LayoutElement>().minWidth = child.rect.width + 12f;
    }


}
