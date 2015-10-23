using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class JobPanelEntryUI : MonoBehaviour, IInitializable<IJobRef> {

    public Text jobText;
    public Scrollbar progressScrollbar;
    public Text salaryText;
    public Text completeText;
    public Image buttonImage;
    public Image promotionImage;
    public RectTransform wordParent;
    public GameObject textPrefab;
    public GameObject requiredWordPrefab;
    public Color completeColor = Color.blue;
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.yellow;

    public IJobRef Job { get; set; }

    List<GameObject> instances = new List<GameObject>();

    public void Initialize(IJobRef job) {
        Job = job;
        salaryText.text = "¥" + job.GameDataInstance.Money;
        jobText.text = job.GameDataInstance.Name;
        if (!job.PlayerDataInstance.Unlocked) {
            buttonImage.color = lockedColor;
            completeText.text = "locked";
            progressScrollbar.size = 1f;
            progressScrollbar.handleRect.GetComponent<Image>().color = lockedColor;
            GetComponent<Image>().color = lockedColor;
        } else {
			//Debug.Log (eventsText);
            var progress = job.Progress();
            if (progress == 1f) {
                progressScrollbar.size = 1f;
                progressScrollbar.handleRect.GetComponent<Image>().color = completeColor;
                completeText.text = "mastered";
            } else {
                progressScrollbar.size = progress;
                progressScrollbar.handleRect.GetComponent<Image>().color = unlockedColor;
                completeText.text = "";
            }

            if (job.PlayerDataInstance.Promoted) {
                promotionImage.color = unlockedColor;
                promotionImage.GetComponent<HelpOnMouseOver>().helpText = "You have a promotion in this job!";
            } else {
                promotionImage.color = Color.gray;
                promotionImage.GetComponent<HelpOnMouseOver>().helpText = "This job hasn't been promoted yet. Continue to do the job and you will have the opportunity to be promoted.";
            }
            //eventsText.text = job.ViewedEventsString();
        }

        var phrases = job.GameDataInstance.GetPhraseRequirements();
        if (phrases.Count() == 0) {
            var instance = Instantiate<GameObject>(textPrefab);
            instance.GetComponentInChildren<Text>().text = "None";
            instance.GetComponentInChildren<Text>().color = Color.gray;
            instance.transform.SetParent(wordParent);
        } else {
            UIUtil.GenerateChildren<PhraseJobRequirementGameData>(phrases, instances, wordParent, GetChild);
        }

        GetComponentInChildren<HoverJobMissingWordsUI>().Initialize(job);
    }

    GameObject GetChild(PhraseJobRequirementGameData p) {
        var instance = Instantiate<GameObject>(requiredWordPrefab);
        instance.GetInterface<IInitializable<PhraseJobRequirementGameData>>().Initialize(p);
        return instance;
    }

}