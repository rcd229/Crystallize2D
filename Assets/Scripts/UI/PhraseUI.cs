using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PhraseUI : UIMonoBehaviour, IInitializable<PhraseSequence> {

    const float MaxSpeed = 2000f;
    const float PopInDuration = 0.25f;

    public Text phraseText;
    public AnimationCurve curve;
    public bool doColor = true;
    public bool hasDefinition = false;

    public event EventHandler<EventArgs<PhraseSequence>> Complete;

    public PhraseSequence phrase;

    public void Initialize(PhraseSequence phrase) {
        this.phrase = phrase;
        if (phrase != null) {
            phraseText.text = phrase.GetText((JapaneseTools.JapaneseScriptType)GameSettings.Instance.TextMode);
            if (hasDefinition) {
                phraseText.text += "\t[" + phrase.Word.GetTranslation() + "]";
            }
                //PlayerDataConnector.GetText(phrase);
            //if (phrase.IsWord && doColor) {
            //    phraseText.color = GUIPallet.Instance.GetColorForWordCategory(phrase.Word.GetPhraseCategory()).Lighten(0.5f);
            //}
        }
    }

}
