using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerInventoryUI : UIMonoBehaviour, ITemporaryUI<PhraseReviewSessionResultArgs, PhraseReviewSessionResultArgs> {

    const string ResourcePath = "UI/PlayerInventory";
    public static PlayerInventoryUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<PlayerInventoryUI>(ResourcePath);
    }

    public GameObject wordPrefab;
    public GameObject phrasePrefab;
    public RectTransform itemParent;

    public event EventHandler<EventArgs<PhraseReviewSessionResultArgs>> Complete;

    PhraseReviewSessionResultArgs args;
    List<GameObject> instances = new List<GameObject>();

    public void Initialize(PhraseReviewSessionResultArgs param1) {
        args = param1;
    }

    public void Close() {
        Complete.Raise(this, new EventArgs<PhraseReviewSessionResultArgs>(args));
        Destroy(gameObject);
    }

    void Start() {
        var allPhrases = PlayerData.Instance.WordStorage.FoundWords.Select((w) => new PhraseSequence(new PhraseSequenceElement(w, 0))).ToList();
        allPhrases.AddRange(PlayerData.Instance.PhraseStorage.Phrases);
        foreach (var phrase in PlayerData.Instance.Session.TodaysCollectedWords) {
            var toRemove = new List<PhraseSequence>(allPhrases.Where((p) => PhraseSequence.PhrasesEquivalent(p, phrase)));
            foreach (var tr in toRemove) {
                allPhrases.Remove(tr);
            }
        }
        UIUtil.GenerateChildren(allPhrases, instances, itemParent, CreateChild);
    }

    GameObject CreateChild(PhraseSequence phrase) {
        GameObject instance = null;
        if (phrase.IsWord) {
            instance = Instantiate<GameObject>(wordPrefab);
            instance.GetComponent<RankedStoredItemUI>().Initialize(
                GUIPallet.Instance.GetColorForWordCategory(phrase.Word.GetPhraseCategory()));
        } else {
            instance = Instantiate<GameObject>(phrasePrefab);
        }
        instance.GetComponent<RankedStoredItemUI>().Initialize(PlayerData.Instance.Reviews.GetOrCreateReview(phrase).GetLevel());
        instance.GetComponentInChildren<Text>().text = PlayerDataConnector.GetText(phrase);
        return instance;
    }

    public void Add(PhraseSequence phrase) {
        var c = CreateChild(phrase);
        c.transform.SetParent(itemParent, false);
    }

}