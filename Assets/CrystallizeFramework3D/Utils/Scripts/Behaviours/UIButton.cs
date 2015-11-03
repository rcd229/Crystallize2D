using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIButton : MonoBehaviour, IPointerClickHandler {

    public event EventHandler<EventArgs<PointerEventData>> OnClicked;

    public void OnPointerClick(PointerEventData eventData) {
        OnClicked.Raise(this, new EventArgs<PointerEventData>(eventData));
    }

}
