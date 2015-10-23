using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public class JapaneseTextEntryUI : MonoBehaviour  {

	public GameObject wordPrefab;
	public GameObject popupPrefab;
	public RectTransform wordParent;
	public RectTransform popupParent;

	public InputField input;

	List<GameObject> wordInstances = new List<GameObject>();
	List<GameObject> popupInstances = new List<GameObject>();
	List<PhraseSequenceElement> words = new List<PhraseSequenceElement>();

	
	Dictionary<char, PhraseSequence> phrases = new Dictionary<char, PhraseSequence>();
	PrefixTree<PhraseSequence> dictionary;
	int childCount = 0;

	void Awake(){
		//TODO a better way?
		while(!DefaultWordDictionary.Done){
		}
		dictionary = DefaultWordDictionary.Dict;
	}
	void Update() {
		if (input.isFocused) {
			input.GetComponent<Image>().color = Color.white;
		} else {
			input.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
		}
	}

	void AddWord(PhraseSequence word) {
		words.Add(word.Word);
		var instance = Instantiate<GameObject>(wordPrefab);
		instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(word);
		instance.transform.SetParent(wordParent, false);
		wordInstances.Add(instance);
	}

	public void AddPresetWords(List<PhraseSequence> words){
		foreach(var w in words){
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

	public void TextChanged(string text) {
		if (text.Length > 0 && phrases.ContainsKey(text[text.Length - 1])) {
			AddWord(phrases[text[text.Length - 1]]);
			input.text = "";
			return;
		}
		
		if (text.Length == 0) {
			UpdateWords(null);
		} else {
			var words = from w in dictionary.withPrefix(text)
					orderby w.getPrefixableText().Length
					select w;
//			var words = allWords;
//			var wordList = words.ToList();
			UpdateWords(words.Take(5));
		}
	}
	
	public void EnterLine(string text) {
		input.text = "";
	}

	public List<PhraseSequence> Compile(){
		return words.ConvertAll(s => new PhraseSequence(s));
	}
}
