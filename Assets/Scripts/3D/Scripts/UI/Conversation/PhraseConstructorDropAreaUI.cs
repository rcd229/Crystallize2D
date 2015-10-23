using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class PhraseConstructorDropAreaUI : MonoBehaviour, IDropHandler {

    public event EventHandler OnDropped;

    public void OnDrop(PointerEventData eventData) {
        OnDropped.Raise(this, EventArgs.Empty);
    }

}