using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class PhraseConstructorDraggableWordUI : PlainWordUI, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {

    public bool Dragging { get; set; }

    public event EventHandler OnDragStarted;
    public event EventHandler OnDragStopped;
    public event EventHandler OnClicked;

    public void OnBeginDrag(PointerEventData eventData) {
        Dragging = true;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        OnDragStarted.Raise(this, EventArgs.Empty);
    }

    public void OnDrag(PointerEventData eventData) {
        
    }

    public void OnEndDrag(PointerEventData eventData) {
        OnDragStopped.Raise(this, EventArgs.Empty);
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Dragging = false;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!Dragging) {
            //Debug.Log("clicked");
            OnClicked.Raise(this, EventArgs.Empty);
        }
    }

}