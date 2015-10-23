using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewDebugStateUI : MonoBehaviour {

    public Text stateText;

    void Start() {
        //var phraseSet = GameData.Instance.PhraseSets.GetItem("RestaurantDialogue02");
        //foreach (var p in phraseSet.Phrases) {
        //    PlayerDataConnector.CollectPhrase(p);
        //    foreach (var w in p.PhraseElements) {
        //        if (w.ElementType == PhraseSequenceElementType.FixedWord) {
        //            PlayerDataConnector.CollectWord(w);
        //        }
        //    }
        //}
    }

    void Update() {
        stateText.text = string.Format("Reviews: {0}\n{1}", PlayerData.Instance.Reviews.Reviews.Count, ReviewTimeManager.GetTime());
    }

    public void AddMinutes(int minutes) {
        ReviewTimeManager.AddTime(0, 0, minutes);
    }

    public void AddHours(int hours) {
        ReviewTimeManager.AddTime(0, hours, 0);
    }

    public void AddDays(int days) {
        ReviewTimeManager.AddTime(days, 0, 0);
    }

}
