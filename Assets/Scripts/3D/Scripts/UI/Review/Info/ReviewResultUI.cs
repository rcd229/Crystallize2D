using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ReviewResultUI : UIMonoBehaviour, ITemporaryUI<PhraseReviewSessionResultArgs, PhraseReviewSessionResultArgs> {

    const string ResourcePath = "UI/ReviewResult";
    public static ReviewResultUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<ReviewResultUI>(ResourcePath);
    }

    public GameObject wordPrefab;
    public GameObject phrasePrefab;
    public GameObject experienceEffectPrefab;
    public RectTransform itemParent;
    public Text experienceText;

    public event EventHandler<EventArgs<PhraseReviewSessionResultArgs>> Complete;

    PhraseReviewSessionResultArgs args;
    int count = 0;
    IEnumerable<SessionReviewEntry<PhraseSequence>> completedReviews = new List<SessionReviewEntry<PhraseSequence>>();
    List<GameObject> instances = new List<GameObject>();
    List<PhraseSequence> reviewQueue = new List<PhraseSequence>();
    Dictionary<PhraseSequence, GameObject> itemTargets = new Dictionary<PhraseSequence, GameObject>();

    CollectUI collectUI;

    public void Initialize(PhraseReviewSessionResultArgs param1) {
        args = param1;
        completedReviews = param1.SessionReviews;
        count = 0;//args.Before;
        UpdateText(0);
        CrystallizeEventManager.Input.OnLeftClick += Input_OnLeftClick;

        collectUI = CollectUI.GetInstance();
    }

    public void Close() {
        Complete.Raise(this, new EventArgs<PhraseReviewSessionResultArgs>(args));
        Destroy(gameObject);
    }

    IEnumerator Start() {
        var allPhrases = PlayerData.Instance.WordStorage.FoundWords.Select((w) => new PhraseSequence(new PhraseSequenceElement(w, 0))).ToList();
        allPhrases.AddRange(PlayerData.Instance.PhraseStorage.Phrases);
        UIUtil.GenerateChildren(allPhrases, instances, itemParent, CreateChild);

        yield return null;

        float time = 0.5f;
        if (reviewQueue.Count > 0) {
            time = Mathf.Clamp(1f / reviewQueue.Count, 0.05f, 0.5f);
        }

        foreach (var p in reviewQueue) {
            CreateEffect(itemTargets[p]);
            Debug.Log(p.GetText() + "; " + itemTargets[p]);
            UpdateText(1);

            yield return new WaitForSeconds(time);
        }
    }

    void Input_OnLeftClick(object sender, EventArgs e) {
        Close();
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnLeftClick -= Input_OnLeftClick;
    }

    GameObject CreateChild(PhraseSequence phrase) {
        GameObject instance = null;
        if (phrase.IsWord) {
            instance = Instantiate<GameObject>(wordPrefab);
            instance.GetComponent<Image>().color = GUIPallet.Instance.GetColorForWordCategory(phrase.Word.GetPhraseCategory());
        } else {
            instance = Instantiate<GameObject>(phrasePrefab);
        }
        instance.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(phrase);
        if (WasReviewed(phrase)) {
            reviewQueue.Add(phrase);
            itemTargets[phrase] = instance;
        }
        return instance;
    }

    void CreateEffect(GameObject target) {
        var instance = MainCanvas.main.AddNew(experienceEffectPrefab);
        instance.transform.SetParent(MainCanvas.main.transform, false);
        instance.transform.position = target.GetComponent<RectTransform>().GetCenter();
        Debug.Log(target.GetComponent<RectTransform>().GetCenter());
    }

    bool WasReviewed(PhraseSequence phrase) {
        foreach (var r in completedReviews) {
            if (PhraseSequence.PhrasesEquivalent(r.Review.Item, phrase)) {
                return true;
            }
        }
        return false;
    }

    void UpdateText(int toAdd) {
        var prevLvl = ProficiencyPlayerData.GetReviewLevel(count);
        count += toAdd;
        var newLvl = ProficiencyPlayerData.GetReviewLevel(count);
        if (prevLvl != newLvl) {
            collectUI.CreateAnimatedSlotItem(GetComponent<RectTransform>());
        }
        Debug.Log("lvl: " + prevLvl + "; " + newLvl);
        experienceText.text = string.Format("Next word slot: {0}/{1}", ProficiencyPlayerData.GetReviewLevelExperience(count), ProficiencyPlayerData.GetNextLevelExperienceFromExperience(count));
    }

}