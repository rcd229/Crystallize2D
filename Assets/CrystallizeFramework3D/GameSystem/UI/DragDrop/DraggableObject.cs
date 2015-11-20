using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class DraggableObject : MonoBehaviour, IPointerClickHandler, IInitializable<PhraseSequence>, IDragHandler, IBeginDragHandler {
    public PhraseSequence Phrase { get; set; }

    public void Initialize(PhraseSequence phrase) {
        Phrase = phrase;
    }

    public void OnDrag(PointerEventData eventData) { }

    public virtual void OnBeginDrag(PointerEventData eventData) {

    }

    public virtual void OnPointerClick(PointerEventData eventData) { }
}