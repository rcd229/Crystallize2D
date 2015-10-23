using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ConfidenceUI : MonoBehaviour {

    const float MoveSpeed = 1f;

    public float size = 0;
    public Scrollbar scrollbar;

    void OnEnable() { 
        CrystallizeEventManager.UI.OnSpeechBubbleOpen += HandleSpeechBubbleOpen;
    }

    void OnDisable() {
        CrystallizeEventManager.UI.OnSpeechBubbleOpen -= HandleSpeechBubbleOpen;
    }

    void Update() {
        size = (float)PlayerData.Instance.Session.Confidence / PlayerData.Instance.Proficiency.Confidence;
        scrollbar.size = Mathf.MoveTowards(scrollbar.size, size, Time.deltaTime * MoveSpeed);
    }

    void HandleSpeechBubbleOpen(object sender, SpeechBubbleRequestedEventArgs args) {
        var speechBubble = (sender as GameObject).GetComponent<SpeechBubbleUI>();
        if (!speechBubble.Speaker.CompareTag(TagLibrary.Player)) {
            StartCoroutine(WrongWordSequence(speechBubble));
        }
        Debug.Log("Subtracting confidence");
    }

    IEnumerator WrongWordSequence(SpeechBubbleUI speechBubble) {
        var wordParent = speechBubble.wordsPanel.transform;
        for (int i = 0; i < wordParent.childCount; i++) {
            var word = wordParent.GetChild(i).GetComponent<SpeechBubbleWordUI>();
            if (word && word.Word.IsDictionaryWord && !PlayerDataConnector.ContainsLearnedItem(word.Word)) {
                var instance = GameObjectUtil.GetResourceInstanceFromAttribute<PopupGlowUI>();
                instance.target = wordParent.GetChild(i);
                PlayerDataConnector.AddConfidence(-1);
                yield return new WaitForSeconds(0.75f);
            }
        }
    }

}
