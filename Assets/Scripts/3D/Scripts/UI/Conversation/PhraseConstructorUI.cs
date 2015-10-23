using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseConstructorArgs {
    public string Help { get; set; }
    public PhraseSequence Phrase { get; set; }
    public List<PhraseSequence> EnteredPhrase { get; set; }
    public List<PhraseSequence> AvailableWords { get; set; }
    public ContextData Context { get; set; }
    public bool UseSlots { get; set; }
    public int EmptySlots { get; set; }

    public PhraseConstructorArgs(PhraseSequence phrase, List<PhraseSequence> availableWords) {
        this.Phrase = phrase;
        this.AvailableWords = availableWords;
        this.Help = "Say \"" + PlayerDataConnector.GetTranslation(phrase) + "\"";
        this.Context = new ContextData();
        this.EmptySlots = phrase.ComparableElementCount;
    }

    public PhraseConstructorArgs(PhraseSequence phrase, List<PhraseSequence> availableWords, int emptySlots)
        : this(phrase, availableWords) {
        this.EmptySlots = Mathf.Clamp(emptySlots, 0, phrase.ComparableElementCount);
    }

    public PhraseConstructorArgs(PhraseSequence phrase, List<PhraseSequence> availableWords, string help)
        : this(phrase, availableWords) {
        this.Help = help;
    }
}

[ResourcePath("UI/PhraseConstructor")]
public class PhraseConstructorUI : UIMonoBehaviour, ITemporaryUI<PhraseConstructorArgs, List<PhraseSequence>> {

    public GameObject wordPrefab;
    public GameObject emptyPrefab;
    public GameObject textPrefab;
    public Button confirmButton;
    public PhraseConstructorDropAreaUI inventoryBackground;
    public RectTransform helpMessage;
    public RectTransform inventoryParent;
    public RectTransform sayParent;
    public RectTransform sayBackground;

    // TODO: integrate with tutorial system
    public GameObject tutorial1;
    public GameObject tutorial2;

    public event EventHandler<EventArgs<List<PhraseSequence>>> Complete;

    string help;
    PhraseSequence phrase;
    List<GameObject> empties = new List<GameObject>();
    GameObject dragged = null;
    List<GameObject> phraseWords = new List<GameObject>();
    Dictionary<GameObject, PhraseSequence> instances = new Dictionary<GameObject, PhraseSequence>();

    PhraseSequence comparablePhraseWords;

    public void Initialize(PhraseConstructorArgs param1) {
        if (!PlayerData.Instance.Tutorial.GetTutorialViewed("PhraseConstructor")) {
            tutorial1.gameObject.SetActive(true);
            tutorial2.gameObject.SetActive(false);
        } else {
            tutorial1.gameObject.SetActive(false);
            tutorial2.gameObject.SetActive(false);
        }

        help = param1.Help;
        helpMessage.GetComponentInChildren<Text>().text = param1.Help;
        phrase = param1.Phrase;
        HashSet<int> emptySlots = GetRandomEmptySlots(phrase.ComparableElementCount, param1.EmptySlots);
        var cmpEleIndex = 0;
        List<PhraseSequence> availableCopy = new List<PhraseSequence>(param1.AvailableWords);
        foreach (var w in phrase.InsertContext(param1.Context).PhraseElements) {
            if (w.GetPhraseCategory() != PhraseCategory.Punctuation) {//w.IsDictionaryWord || w.ElementType == PhraseSequenceElementType.ContextSlot || ) {
                var e = Instantiate<GameObject>(emptyPrefab);
                e.transform.SetParent(sayParent, false);
                e.GetComponent<PhraseConstructorDropAreaUI>().OnDropped += PhraseConstructorUI_OnDropped;
                e.name = "Empty0" + cmpEleIndex;
                //				mapEmptyIndexToPhraseIndex[empties.Count] = cmpEleIndex;
                empties.Add(e);
                phraseWords.Add(null);
                if (!emptySlots.Contains(cmpEleIndex) && param1.EnteredPhrase == null) {
                    var replacePhrase = (from ws in availableCopy
                                         where PhraseSequence.PhrasesEquivalent(ws, new PhraseSequence(w))
                                         select ws).FirstOrDefault();
                    Debug.Log("rep phr: " + replacePhrase + "; " + w.GetText() + "; " + w.ElementType + "; ");
                    availableCopy.Remove(replacePhrase);
                    var t = CreateChild(replacePhrase);
                    TryAddWordToSay(cmpEleIndex, t);
                }
                cmpEleIndex++;
            } else {
                var t = Instantiate<GameObject>(textPrefab);
                t.transform.SetParent(sayParent, false);
                t.GetComponent<Text>().text = w.GetText(JapaneseTools.JapaneseScriptType.Romaji);
            }
        }
        UIUtil.GenerateChildren(availableCopy, inventoryParent, CreateChild);
        inventoryBackground.OnDropped += PhraseConstructorUI_OnDropped;

        if (param1.EnteredPhrase != null) {
            int i = 0;
            foreach (var w in param1.EnteredPhrase) {
                var go = (from kv in instances
                          where PhraseSequence.PhrasesEquivalent(kv.Value, w)
                          select kv)
                          .FirstOrDefault();
                TryAddWordToSay(i, go.Key);
                i++;
            }
        }

        //        if (phraseWords.Count < empties.Count) {
        if (!CanEnableConfirm()) {
            confirmButton.interactable = false;
        } else {
            confirmButton.interactable = true;
        }
    }

    HashSet<int> GetRandomEmptySlots(int range, int num_slots) {
        num_slots = Math.Min(num_slots, range);
        HashSet<int> ret = new HashSet<int>();
        for (int i = 0; i < num_slots; i++) {
            int k = UnityEngine.Random.Range(0, range);
            while (ret.Contains(k)) {
                k = UnityEngine.Random.Range(0, range);
            }
            ret.Add(k);
        }
        return ret;
    }

    void PhraseConstructorUI_OnDropped(object sender, EventArgs e) {
        if (dragged) {
            Debug.Log("sender: " + sender);
            var i = empties.IndexOf(((Component)sender).gameObject);
            if (i >= 0) {
                TryAddWordToSay(i, dragged);
            } else {
                AddWordToInventory(dragged);
            }
        }
    }

    GameObject CreateChild(PhraseSequence word) {
        var i = Instantiate<GameObject>(wordPrefab);
        i.GetInterface<IInitializable<PhraseSequence>>().Initialize(word);
        i.GetComponent<PhraseConstructorDraggableWordUI>().OnDragStarted += Word_Dragged;
        i.GetComponent<PhraseConstructorDraggableWordUI>().OnClicked += PhraseConstructorUI_OnClicked;
        instances[i] = word;
        return i;
    }

    //void Start() {
    //    var sps = GetComponent<EnvironmentPhrase>();
    //    var words = sps.phrase.Get().PhraseElements.Select((pe) => new PhraseSequence(pe)).ToList();
    //    words.AddRange(new List<PhraseSequence>(words));
    //    Initialize(new PhraseConstructorArgs(sps.phrase.Get(), words, ));
    //}


    public void Close() {
        Exit();
        Destroy(gameObject);
    }

    public void Confirm() {
        Close();
    }

    void PhraseConstructorUI_OnClicked(object sender, EventArgs args) {
        //Debug.Log("clicked");
        var c = ((Component)sender);
        if (c.transform.parent == sayParent) {
            AddWordToInventory(c.gameObject);
        } else {
            TryAddWordToSay(GetFirstOpenIndex(), c.gameObject);
        }
    }

    int GetFirstOpenIndex() {
        int i = 0;
        while (i < phraseWords.Count) {
            if (phraseWords[i] == null) {
                return i;
            }
            i++;
        }
        return -1;
    }

    void Word_Dragged(object sender, EventArgs args) {
        //Debug.Log("dragging");
        var word = (PhraseConstructorDraggableWordUI)sender;
        word.transform.SetParent(transform, false);
        word.GetComponent<LayoutElement>().ignoreLayout = true;
        var index = phraseWords.IndexOf(word.gameObject);
        if (phraseWords.IndexInRange(index)) {
            phraseWords[index] = null;
            UpdateEmpties();
        }
        StartCoroutine(DragCoroutine(word, index));
    }

    void TryAddWordToSay(int wordIndex, GameObject word) {
        //Debug.Log("Adding to say: " + wordIndex);
        if (!phraseWords.IndexInRange(wordIndex)) {
            RejectWord();
        } else {
            word.transform.SetParent(sayParent, false);
            word.transform.SetSiblingIndex(empties[wordIndex].transform.GetSiblingIndex());
            phraseWords[wordIndex] = word;
            UpdateEmpties();
            //Debug.Log(GetPhrase().GetText());
        }
    }

    void AddWordToInventory(GameObject word) {
        word.transform.SetParent(inventoryParent.transform, false);
        var i = phraseWords.IndexOf(word);
        if (phraseWords.IndexInRange(i)) {
            phraseWords[i] = null;
        }
        UpdateEmpties();
    }

    void UpdateEmpties() {
        foreach (var e in empties) {
            e.SetActive(false);
        }

        for (int i = 0; i < empties.Count; i++) {
            if (phraseWords[i] == null) {
                empties[i].SetActive(true);
            }
        }

        if (!CanEnableConfirm()) {
            confirmButton.interactable = false;
        } else {
            //if (!PlayerData.Instance.Tutorial.GetTutorialViewed("PhraseConstructor")) {
            //    tutorial1.gameObject.SetActive(false);
            //    tutorial2.gameObject.SetActive(true);
            //    PlayerData.Instance.Tutorial.SetTutorialViewed("PhraseConstructor");
            //} 
            confirmButton.interactable = true;
        }
    }

    void RejectWord() {
        SetCoroutine(RejectCoroutine());
    }

    IEnumerator RejectCoroutine() {
        helpMessage.GetComponentInChildren<Text>().text = "The sentence is full!";
        var i = sayBackground.GetComponent<Image>();
        var c = i.color;
        for (float t = 0; t < 1f; t += Time.deltaTime * 8f) {
            i.color = Color.Lerp(c, Color.yellow, t);
            yield return null;
        }

        for (float t = 0; t < 1f; t += Time.deltaTime * 8f) {
            i.color = Color.Lerp(c, Color.yellow, 1f - t);
            yield return null;
        }
        i.color = c;

        yield return new WaitForSeconds(3f);

        helpMessage.GetComponentInChildren<Text>().text = help;
    }

    IEnumerator DragCoroutine(PhraseConstructorDraggableWordUI word, int phrasePosition) {
        dragged = word.gameObject;
        while (word.Dragging) {
            //Debug.Log("dragging");
            word.transform.position = Input.mousePosition;
            yield return null;
        }

        yield return null;
        Debug.Log("drag stopped");

        if (dragged.transform.parent != sayParent &&
            dragged.transform.parent != inventoryParent) {
            if (phrasePosition < 0) {
                AddWordToInventory(word.gameObject);
            } else {
                TryAddWordToSay(phrasePosition, word.gameObject);
            }
        }
        word.GetComponent<LayoutElement>().ignoreLayout = false;
        dragged = null;
    }

    List<PhraseSequence> GetPhrase() {
        var p = new List<PhraseSequence>();
        foreach (var g in phraseWords) {
            if (g != null) {
                p.Add(instances[g]);
            }
        }
        return p;
    }

    void Exit() {
        var p = GetPhrase();
        DataLogger.LogTimestampedData("ConstructedPhrase", p.Flatten().GetText());
        Complete.Raise(this, new EventArgs<List<PhraseSequence>>(p));
    }

    bool CanEnableConfirm() {
        var filled = (from p in phraseWords
                      where p != null
                      select p).Count();
        return filled >= empties.Count;
    }

}
