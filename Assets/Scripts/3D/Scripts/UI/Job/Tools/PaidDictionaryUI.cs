using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PaidDictionaryUI : MonoBehaviour, IPhraseDropHandler {

    public Image wordImage;
    public Text translationText;

    int cost = 1000;

    void Start() {
        cost = PlayerData.Instance.Session.BaseMoney;
        translationText.text = "<i>Drag here to look up (¥" + cost +")</i>";
    }

    public void HandlePhraseDropped(PhraseSequence phrase) {
        if (PlayerData.Instance.Money < cost || !phrase.IsWord) {
            SoundEffectManager.Play(SoundEffectType.NegativeFeedback);
        } else {
            wordImage.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(phrase);
            wordImage.GetComponentInChildren<Text>().color = Color.black.Lighten(0.1f);
            var t = phrase.Translation;
            if (phrase.IsWord) {
                wordImage.color = GUIPallet.Instance.GetColorForWordCategory(phrase.Word.GetPhraseCategory()).Lighten(0.5f);
                t = phrase.Word.GetTranslation();
            } else {
                wordImage.color = Color.white;
            }
            translationText.text = t;// "\n\n<i>Drag here to look up (¥1000)</i>";

            PlayerDataConnector.CollectItem(phrase);

            SoundEffectManager.Play(SoundEffectType.Buy);
            PlayerDataConnector.AddMoney(-cost);
        }
    }
}
