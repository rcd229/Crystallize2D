using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public class JapaneseTextEntryUI : MonoBehaviour {

    public GameObject wordPrefab;
    public GameObject popupPrefab;
    public RectTransform wordParent;
    public RectTransform popupParent;

    public InputField input;
    public Text popupText;

    List<GameObject> wordInstances = new List<GameObject>();
    List<GameObject> popupInstances = new List<GameObject>();
    List<PhraseSequenceElement> words = new List<PhraseSequenceElement>();


    Dictionary<char, PhraseSequence> phrases = new Dictionary<char, PhraseSequence>();
    PrefixTree<PhraseSequence> dictionary;
    int childCount = 0;

    int offset = 0;

    bool isConjugating = false;

    void Awake() {
        dictionary = DefaultWordDictionary.Instance.Dictionary;
    }

    void Start() {
        input.onValueChange.AddListener((s) => TextChanged(s));
        input.onEndEdit.AddListener((s) => EnterLine(s));
    }

    void Update() {
        if (input.isFocused) {
            input.GetComponent<Image>().color = Color.white;

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.PageUp)) {
                offset = Mathf.Max(0, offset - 5);
                UpdateWords();
                Debug.Log(offset);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.PageDown)) {
                offset = (offset + 5);
                UpdateWords();
                Debug.Log(offset);
            }
        }
        else {
            input.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
        }
    }

    void AddWord(PhraseSequence word) {
        words.Add(word.Word);
        var instance = Instantiate<GameObject>(wordPrefab);
        instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(word);
        instance.transform.SetParent(wordParent, false);
        wordInstances.Add(instance);
        offset = 0;
    }

    public void AddPresetWords(List<PhraseSequence> words) {
        foreach (var w in words) {
            AddWord(w);
        }
    }

    void UpdateWords(IEnumerable<PhraseSequence> words) {

        phrases = new Dictionary<char, PhraseSequence>();
        if (words == null) {
            popupInstances.DestroyAndClear();
            return;
        }
        childCount = 1;
        UIUtil.GenerateChildren(words, popupInstances, popupParent, GenerateWordChild);
        for (int i = 0; i < popupInstances.Count; i++) {
            popupInstances[i].transform.SetAsFirstSibling();
        }
        popupParent.gameObject.SetActive(popupInstances.Count > 0);
    }

    GameObject GenerateWordChild(PhraseSequence phrase) {
        var instance = Instantiate<GameObject>(popupPrefab);
        instance.transform.Find("NumberText").GetComponent<Text>().text = childCount + ":";
        phrases[childCount.ToString()[0]] = phrase;
        childCount++;
        instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(phrase);
        return instance;
    }

    public void TextChanged(string text) {
        if (text.Length > 0 && phrases.ContainsKey(text[text.Length - 1])) {
            PhraseSequence candidate = phrases[text[text.Length - 1]];

            AddWord(candidate);
            input.text = "";
            return;
        }

        if (text.Length == 0) {
            UpdateWords(null);
        }
        else {
            UpdateWords();
        }
    }

    void UpdateWords() {
        var words = from w in dictionary.withPrefix(input.text)
                    orderby w.getPrefixableText().Length
                    select w;
        var c = words.Count();
        if (c > 5) {
            offset = Mathf.Clamp(offset, 0, words.Count() - 5);
        }
        else {
            offset = 0;
        }
        popupText.text = ((offset / 5) + 1) + "/" + Mathf.CeilToInt(c / 5f);
        UpdateWords(words.Skip(offset).Take(5));
    }

    public void EnterLine(string text) {
        input.text = "";
    }

    public List<PhraseSequence> Compile() {
        return words.ConvertAll(s => new PhraseSequence(s));
    }
}
