using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[ResourcePath("UI/MultipleChoicePhraseReview")]
public class MultipleChoicePhraseReviewUI : BaseReviewUI<PhraseSequence>, IDebugMethods {

    static int[] standardWordIDs = new int[] { 1323080, 1387990, 1387990, 1206900 };

    public GameObject buttonPrefab;
    public GameObject solutionPrefab;
    public Text itemText;
    public Text questionText;
    public Text rankText;
    public Image backgroundImage;
    public RectTransform buttonParent;
    public RectTransform itemButton;
    public List<GameObject> debugObjects = new List<GameObject>();

    List<GameObject> instances = new List<GameObject>();

    public override void Initialize(object param1) {
        foreach (var obj in debugObjects) {
            obj.SetActive(false);
        }
        Refresh();
    }

    protected override void Refresh() {
        base.Refresh();

        PlayerData.Instance.Reviews.GetNewReviews();
        var reviews = PlayerData.Instance.Reviews.GetCurrentReviews();

        if (reviews.Count > 0 && count < 20) {
            ActiveReview = reviews[UnityEngine.Random.Range(0, reviews.Count)];
            DisplayReview(ActiveReview);

            if (ActiveReview.Item.IsWord) {
                var audioPlayer = itemButton.gameObject.GetOrAddComponent<ClickAudioPlayerUI>();
                audioPlayer.PlayAudio(ActiveReview.Item.Word.GetText(JapaneseTools.JapaneseScriptType.Kana));
            } else {
                var audioPlayer = itemButton.gameObject.GetOrAddComponent<ClickAudioPlayerUI>();
                audioPlayer.PlayAudio(ActiveReview.Item.GetText(JapaneseTools.JapaneseScriptType.Kanji));
            }
        } else {
            Exit();
        }
    }

#if UNITY_EDITOR
    void Update(){
        if (Input.GetKeyDown(KeyCode.S)) {
            if (ActiveReview != null) {
                DisplayResult(true);
            } else {
                Refresh();
            }
        }
    }
#endif

    void Clear() {
        foreach (var b in instances) {
            Destroy(b);
        }
    }

    void DisplayReview(ItemReviewPlayerData<PhraseSequence> review) {
        itemText.text = PlayerDataConnector.GetText(review.Item);//.GetText(JapaneseTools.JapaneseScriptType.Romaji);
        questionText.text = "What does this mean?";
        backgroundImage.color = Color.clear;
        List<PhraseSequence> choices = new List<PhraseSequence>();
        if (review.Item.IsWord) {
            choices = GetWordChoices(review.Item);
        } else {
            choices = GetPhrasesChoices(review.Item);
        }
        rankText.text = "Rank " + review.Rank;
        UIUtil.GenerateChildren(choices, instances, buttonParent, GetButtonInstance);
    }

    GameObject GetButtonInstance(PhraseSequence p) {
        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.GetComponentInChildren<Text>().text = p.Translation;
        instance.AddComponent<DataContainer>().Store(p);
        instance.GetComponent<UIButton>().OnClicked += MultipleChoiceReviewUI_OnClicked;
        return instance;
    }

    void MultipleChoiceReviewUI_OnClicked(object sender, EventArgs e) {
        var p = ((Component)sender).GetComponent<DataContainer>().Retrieve<PhraseSequence>();
        DisplayResult(PhraseSequence.PhrasesEquivalent(p, ActiveReview.Item));
    }

    void DisplayResult(bool correct) {
        //Debug.Log("Displaying result"); 
        Clear();
        var r = ActiveReview;

        var t = "";
        if (ActiveReview.Item.IsWord) {
            t = ActiveReview.Item.Word.GetTranslation();
        } else {
            t = ActiveReview.Item.Translation;
        }

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
        rankText.text = "Rank " + r.Rank;

        if (correct) {
            backgroundImage.color = GUIPallet.Instance.successColor;
            questionText.text = "Right! The correct answer is...";
            SoundEffectManager.Play(SoundEffectType.PositiveFeedback);
            SetResult(1);
        } else {
            backgroundImage.color = GUIPallet.Instance.failureColor;
            questionText.text = "Wrong. The correct answer is...";
            SoundEffectManager.Play(SoundEffectType.NegativeFeedback);
            SetResult(0);
        }
    }

    void Continue_OnClicked(object sender, EventArgs e) {
        Continue();
    }

    List<PhraseSequence> GetPhrasesChoices(PhraseSequence phrase) {
        var phrases = new List<PhraseSequence>(PlayerData.Instance.PhraseStorage.Phrases.Where((p) => !PhraseSequence.PhrasesEquivalent(phrase, p)));
        int count = 0;
        while (phrases.Count < 4) {
            var w = new PhraseSequenceElement(standardWordIDs[count], 0);
            var p = new PhraseSequence(w);
            p.Translation = w.GetTranslation();
            phrases.Add(p);
            count++;
        }

        var selected = phrases.PickN(4).ToList();
        selected.Add(phrase);
        selected.Shuffle();
        return selected;
    }

    List<PhraseSequence> GetWordChoices(PhraseSequence phrase) {
        var words = PlayerData.Instance.WordStorage.FoundWords.Select((w) => new PhraseSequenceElement(w, 0)).Where((w) => phrase.Word.WordID != w.WordID).ToList();
        int count = 0;
        while (words.Count < 4) {
            words.Add(new PhraseSequenceElement(standardWordIDs[count], 0));
            count++;
        }

        var selected = words.PickN(4);
        selected.Add(phrase.Word);
        selected.Shuffle();
        return selected.Select((w) => new PhraseSequence(w)).ToList();
    }

    protected override PhraseReviewSessionResultArgs GetExitArgs() {
        return new PhraseReviewSessionResultArgs(results.Values);
    }

    #region DEBUG
    public IEnumerable<NamedMethod> GetMethods() {
        return NamedMethod.Collection(ShowPanel);
    }

    public string ShowPanel(string input) {
        foreach (var obj in debugObjects) {
            obj.SetActive(true);
        }
        return "enabled";
    }
    #endregion

}
