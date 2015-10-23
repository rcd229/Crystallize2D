using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HelpOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public string helpText = "";

    public void OnPointerEnter(PointerEventData eventData) {
        if (helpText != "") {
            var h = 0.5f * GetComponent<RectTransform>().rect.height;
            var pos = GetComponent<RectTransform>().GetCenter() + Vector2.up * h;
            UIMouseOverPanel.GetInstance().Initialize(helpText, pos);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (helpText != "") {
            UIMouseOverPanel.GetInstance().Close();
        }
    }
}
