using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class GenericSpeechBubbleWordUI : MonoBehaviour, IPointerClickHandler {

	PhraseSequenceElement word;

    public PhraseSequenceElement Word {
        get {
            return word;
        }
    }

    public event EventHandler OnClicked;

    public void Initialize(PhraseSequenceElement word) {
        this.word = word;
        GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(word);
        var c = GUIPallet.Instance.GetColorForWordCategory(word.GetPhraseCategory());
        c = Color.Lerp(Color.white, c, 0.5f);
        GetComponent<Image>().color = c;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (OnClicked != null)
        {
            OnClicked(this, EventArgs.Empty);
        }
    }
}
