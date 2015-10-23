using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/DialogueConfidence")]
public class DialogueConfidenceUI : UIPanel, ITemporaryUI<object, object> {

    const float MoveSpeed = 1f;
    const float ConfidenceMultiplier = 25f;

    public Text amountText;
    public RectTransform reserveText;
    public Image totalImage;
    public Image currentImage;

    public float size = 0;
    public float lastSize = -1;
    int lastReserve = -1;

    int total = 0;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(object param1) { }

    void OnEnable() {
        CrystallizeEventManager.UI.OnSpeechBubbleOpen += HandleSpeechBubbleOpen;
        CrystallizeEventManager.PlayerState.ConfidenceChanged += PlayerState_ConfidenceChanged;
    }

    void OnDisable() {
        if (CrystallizeEventManager.Alive) {
            if (total > 0) {
                PlayerDataConnector.AddConfidence(-1);
            }
            CrystallizeEventManager.UI.OnSpeechBubbleOpen -= HandleSpeechBubbleOpen;
            CrystallizeEventManager.PlayerState.ConfidenceChanged -= PlayerState_ConfidenceChanged;
        }
    }

    void Update() {
        var current = PlayerData.Instance.Session.Confidence;
        var total = PlayerData.Instance.Proficiency.Confidence;
        size = (float)current / total;
        currentImage.fillAmount = Mathf.MoveTowards(currentImage.fillAmount, size, Time.deltaTime * MoveSpeed);
        if (size != lastSize) {
            //totalImage.GetComponent<LayoutElement>().preferredWidth = Mathf.Min(total * ConfidenceMultiplier, 500f);
            amountText.text = string.Format("{0}/{1}", current, total);
        }

        var reserve = PlayerData.Instance.Proficiency.ReserveConfidence;
        if (lastReserve != reserve) {
            if (reserve == 0) {
                reserveText.gameObject.SetActive(false);
            } else {
                reserveText.gameObject.SetActive(true);
                reserveText.GetComponentInChildren<Text>().text = string.Format("+{0}", PlayerData.Instance.Proficiency.ReserveConfidence);
            }
            lastReserve = PlayerData.Instance.Proficiency.ReserveConfidence;
        }

        lastSize = size;
    }

    void HandleSpeechBubbleOpen(object sender, SpeechBubbleRequestedEventArgs args) {
        if (args.ReduceConfidence) {
            var senderGO = sender as GameObject;
            if (senderGO) {
                var speechBubble = senderGO.GetComponent<SpeechBubbleUI>();
                if (speechBubble && !speechBubble.Speaker.CompareTag(TagLibrary.Player)) {
                    StartCoroutine(WrongWordSequence(speechBubble));
                }
            }
        }
        //Debug.Log("Subtracting confidence");
    }

    IEnumerator WrongWordSequence(SpeechBubbleUI speechBubble) {
        int total = 0;
        var wordParent = speechBubble.wordsPanel.transform;
        for (int i = 0; i < wordParent.childCount; i++) {
            var word = wordParent.GetChild(i).GetComponent<SpeechBubbleWordUI>();
            if (word && word.Word.IsDictionaryWord && !PlayerDataConnector.ContainsLearnedItem(word.Word)) {
                total++;
            }
        }

        for (int i = 0; i < wordParent.childCount; i++) {
            var word = wordParent.GetChild(i).GetComponent<SpeechBubbleWordUI>();
            if (word && word.Word.IsDictionaryWord && !PlayerDataConnector.ContainsLearnedItem(word.Word)) {
                var instance = GameObjectUtil.GetResourceInstanceFromAttribute<PopupGlowUI>();
                //Debug.Log("Making instance: " + instance);
                instance.target = wordParent.GetChild(i);
                PlayerDataConnector.AddConfidence(-1);
                total--;
                yield return new WaitForSeconds(0.5f);
            }

            if (!wordParent) {
                break;
            }
        }
    }

    void PlayerState_ConfidenceChanged(object sender, EventArgs<int> e) {
        //Debug.Log("conf changed");
        GameObjectUtil.GetResourceInstance<ConfidenceChangeTextUI>("UI/Element/ConfidenceText").amount = e.Data;
    }

}
