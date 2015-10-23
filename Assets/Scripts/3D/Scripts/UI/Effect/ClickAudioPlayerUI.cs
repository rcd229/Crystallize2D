using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ClickAudioPlayerUI : MonoBehaviour, IPointerClickHandler {

    public string audioText = "";
    public bool isMale = true;

    public void PlayAudio(string text) {
        audioText = text;
        if (!audioText.IsEmptyOrNull()) {
            AudioLoader.PlayAudio(audioText, isMale);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        PlayAudio(audioText);
    }

}
