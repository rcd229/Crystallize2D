using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlainWordUI : MonoBehaviour, IInitializable<PhraseSequence> {

    public Text wordText;
    public Image wordImage;

    public void Initialize(PhraseSequence word) {
        //Debug.Log("Word is")
        wordText.text = PlayerDataConnector.GetText(word);//.GetPlayerText();

        if (word != null && word.IsWord) {
            Color c = GUIPallet.Instance.GetColorForWordCategory(word.Word.GetPhraseCategory());
            c = Color.Lerp(c, Color.white, 0.5f);
            wordImage.GetComponentInChildren<Text>().color = c;
        }
    }

}
