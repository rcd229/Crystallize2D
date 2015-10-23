using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[ResourcePath("UI/MultipleChoiceKanaReview")]
public class MultipleChoiceKanaReviewUI : BaseReviewUI<string> {

    public GameObject buttonPrefab;
    public GameObject solutionPrefab;
    public Text questionText;
    public Image backgroundImage;
    public RectTransform itemButton;
    public RectTransform buttonParent;

    List<GameObject> instances = new List<GameObject>();

    public override void Initialize(object param1) {
        Refresh();
    }

    protected override void Refresh() {
        base.Refresh();
        var reviews = PlayerData.Instance.KanaReviews.GetCurrentReviews();

        if (reviews.Count > 0) {
            ActiveReview = reviews[UnityEngine.Random.Range(0, reviews.Count)];
            DisplayReview(ActiveReview);
        } else {
            Exit();
        }
    }

    void Clear() {
        foreach (var b in instances) {
            Destroy(b);
        }
    }

    void DisplayReview(ItemReviewPlayerData<string> review) {
        //itemText.text = "🔊"; //PlayerDataConnector.GetText(review.Item);//.GetText(JapaneseTools.JapaneseScriptType.Romaji);
        var audioPlayer = itemButton.GetComponent<ClickAudioPlayerUI>();
        if (!audioPlayer) {
            audioPlayer = itemButton.gameObject.AddComponent<ClickAudioPlayerUI>();
        }
        audioPlayer.PlayAudio(review.Item);
        itemButton.GetComponentInChildren<Text>().text = JapaneseTools.KanaConverter.Instance.ConvertToRomaji(review.Item);

        questionText.text = "Which kana is this?";
        backgroundImage.color = Color.white;
        List<string> choices = new List<string>(PlayerData.Instance.KanaReviews.GetCollectedKana().Select(c => c.ToString()));
        choices = CollectionExtensions.RandomSubsetWithValue(choices, review.Item, 5);
        UIUtil.GenerateChildren(choices, instances, buttonParent, GetButtonInstance);
    }

    GameObject GetButtonInstance(string s) {
        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.GetComponentInChildren<Text>().text = s;
        instance.AddComponent<DataContainer>().Store(s);
        instance.GetComponent<UIButton>().OnClicked += MultipleChoiceReviewUI_OnClicked;
        return instance;
    }

    void MultipleChoiceReviewUI_OnClicked(object sender, EventArgs e) {
        var s = ((Component)sender).GetComponent<DataContainer>().Retrieve<string>();
        DisplayResult(s == ActiveReview.Item);
    }

    void DisplayResult(bool correct) {
        Clear();

        var t = ActiveReview.Item;

        var solutionInstance = Instantiate<GameObject>(solutionPrefab);
        //Debug.Log(t == null);
        solutionInstance.GetComponentInChildren<Text>().text = t;
        solutionInstance.transform.SetParent(buttonParent.transform, false);
        instances.Add(solutionInstance);

        var continueButtonInstance = Instantiate<GameObject>(buttonPrefab);
        continueButtonInstance.GetComponentInChildren<Text>().text = "Continue";
        continueButtonInstance.GetComponent<UIButton>().OnClicked += Continue_OnClicked;
        continueButtonInstance.transform.SetParent(buttonParent.transform, false);
        instances.Add(continueButtonInstance);

        if (correct) {
            backgroundImage.color = GUIPallet.Instance.successColor.Lighten(0.5f);
            questionText.text = "Right! The correct answer is...";
            SetResult(1);
        } else {
            backgroundImage.color = GUIPallet.Instance.failureColor.Lighten(0.5f);
            questionText.text = "Wrong. The correct answer is...";
            SetResult(0);
        }

        ActiveReview = null;
    }

    void Continue_OnClicked(object sender, EventArgs e) {
        Continue();
    }

    protected override PhraseReviewSessionResultArgs GetExitArgs() {
        return new PhraseReviewSessionResultArgs(null);
    }

}
