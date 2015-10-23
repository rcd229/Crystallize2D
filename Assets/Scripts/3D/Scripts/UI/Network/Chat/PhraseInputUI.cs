using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseInputUI : MonoBehaviour, IUIComponent<PhraseSequence> {

    public GameObject wordPrefab;
    public GameObject popupPrefab;
    public RectTransform wordParent;
    public RectTransform popupParent;
    public InputField input;
    public bool canAddFromDictionary = true;

    List<GameObject> wordInstances = new List<GameObject>();
    List<GameObject> popupInstances = new List<GameObject>();
    List<PhraseSequence> words = new List<PhraseSequence>();

    Dictionary<char, PhraseSequence> phrases = new Dictionary<char, PhraseSequence>();
    bool quitting = false;
    int childCount = 0;
    
    string lastText;
    bool inputConsumed = false;

    public event EventHandler<PhraseEventArgs> OnPhraseChanged;
    public event EventHandler<PhraseEventArgs> OnPhraseEntered;

    public PhraseSequence Value {
        get { return GetPhrase(); }
        set {
            if (!PhraseSequence.PhrasesEquivalent(value, GetPhrase())) {
                Initialize(value);
            }
        }
    }

    public void Initialize(PhraseSequence phrase) {
        if (phrase != null) {
            foreach (var word in phrase.PhraseElements) {
                AddWord(new PhraseSequence(word));
            }
        }
    }

    void Start() {
        popupParent.gameObject.SetActive(false);
    }

    void Update() {
        if (input.isFocused) {
            PlayerManager.LockMovement(this);

            if (!inputConsumed && Input.GetKeyDown(KeyCode.Backspace)) {
                RemoveLastWord();
            }
            inputConsumed = false;

            if (!lastText.IsEmptyOrNull() && (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))) {
                input.text = lastText.Replace("+", "").Replace("=", "");
                UILibrary.Dictionary.Get(input.text);
            }
        } else {
            PlayerManager.UnlockMovement(this);
        }
    }

    public void TextChanged(string text) {
        if (text.Length > 0 && phrases.ContainsKey(text[text.Length - 1])) {
            AddWord(phrases[text[text.Length - 1]]);
            input.text = "";
        } else {
            if (text.Length == 0) {
                UpdateWords(null);
            } else {
                var allWords = from r in PlayerData.Instance.Reviews.Reviews
                               select r.Item;
                var words = from w in allWords
                            where w.GetText(JapaneseTools.JapaneseScriptType.Romaji).StartsWith(text)
                            orderby w.GetText(JapaneseTools.JapaneseScriptType.Romaji).Length
                            select w;
                UpdateWords(words.Take(5));
            }

            if (lastText != text) {
                inputConsumed = true;
            }
            lastText = text;
        }

        OnPhraseChanged.Raise(this, new PhraseEventArgs(GetPhrase()));
    }

    public void EnterLine(string text) {
        if (words.Count > 0 || text.Length > 0) {
            OnPhraseEntered.Raise(this, new PhraseEventArgs(GetPhrase()));

            //wordInstances.DestroyAndClear();
            //words.Clear();
        } 
        //input.text = "";
    }

    public void Clear() {
        wordInstances.DestroyAndClear();
        words.Clear();
        input.text = "";
    }

    PhraseSequence GetPhrase() {
        var phrase = new PhraseSequence();
        foreach (var word in words) {
            phrase.Add(word);
        }
        if (!input.text.IsEmptyOrNull()) {
            phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, input.text));
        }
        return phrase;
    }

    void RemoveLastWord(){
        if(wordInstances.Count > 0){
            Destroy(wordInstances.Last());
            wordInstances.RemoveAt(wordInstances.Count - 1);
            words.RemoveAt(words.Count - 1);
        }
    }

    void AddWord(PhraseSequence word) {
        words.Add(word);
        var instance = Instantiate<GameObject>(wordPrefab);
        instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(word);
        instance.transform.SetParent(wordParent, false);
        wordInstances.Add(instance);
    }

    void UpdateWords(IEnumerable<PhraseSequence> words) {
        phrases = new Dictionary<char, PhraseSequence>();
        if (words == null) {
            popupInstances.DestroyAndClear();
            return;
        }

        childCount = 1;
        popupParent.gameObject.SetActive(popupInstances.Count > 0);
        UIUtil.GenerateChildren(words, popupInstances, popupParent, GenerateWordChild);
        for (int i = 0; i < popupInstances.Count; i++) {
            popupInstances[i].transform.SetAsFirstSibling();
        }
    }

    GameObject GenerateWordChild(PhraseSequence phrase) {
        var instance = Instantiate<GameObject>(popupPrefab);
        instance.transform.Find("NumberText").GetComponent<Text>().text = childCount + ":";
        phrases[childCount.ToString()[0]] = phrase;
        childCount++;
        instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(phrase);
        return instance;
    }
}
