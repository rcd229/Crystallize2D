using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PhraseSlotsUI : MonoBehaviour {

    public Text wordSlotsText;
    public Text phraseSlotsText;

    void Start() {

    }

    void Update() {
        wordSlotsText.text = string.Format("{0}/{1}", PlayerDataConnector.RemainingWordCount, PlayerData.Instance.Proficiency.Words);
        phraseSlotsText.text = string.Format("{0}/{1}", PlayerDataConnector.RemainingPhraseCount, PlayerData.Instance.Proficiency.Phrases);
    }

}
