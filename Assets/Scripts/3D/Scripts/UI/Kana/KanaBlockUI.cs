using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using JapaneseTools;

public class KanaBlockUI : MonoBehaviour, IInitializable<string> {

    public Text romajiText;
    public Image hiraganaImage;
    public Image katakanaImage;

    public void Initialize(string hiragana) {
        GetComponent<ClickAudioPlayerUI>().audioText = hiragana;
        romajiText.text = KanaConverter.Instance.ConvertToRomaji(hiragana);

        if (PlayerData.Instance.KanaReviews.ContainsReview(hiragana)) {
            hiraganaImage.GetComponentInChildren<Text>().text = hiragana;
        } else {
            hiraganaImage.GetComponent<CanvasGroup>().alpha = 0.5f;
            hiraganaImage.GetComponentInChildren<Text>().text = "?";
        }

        var katakana = KanaConverter.Instance.ConvertToKatakana(hiragana);
        if (PlayerData.Instance.KanaReviews.ContainsReview(katakana)) {
            katakanaImage.GetComponentInChildren<Text>().text = katakana;
        } else {
            katakanaImage.GetComponent<CanvasGroup>().alpha = 0.5f;
            katakanaImage.GetComponentInChildren<Text>().text = "?";
        }
    }

}
