using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using JapaneseTools;

public class DictionaryUI : MonoBehaviour {

	const int MaxResults = 16;

	public GameObject entryPrefab;
	public InputField inputField;
	public RectTransform entryParent;
	public Text kanaOutput;

	List<GameObject> entryInstances = new List<GameObject>();

	// Use this for initialization
	void Start () {
		transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
	}

	public void UpdateKanaOutput(){
		kanaOutput.text = string.Format ("({0})", KanaConverter.Instance.ConvertToHiragana (inputField.text));
	}

	public void UpdateEntries(){
		foreach (var e in entryInstances) {
			Destroy(e);
		}
		entryInstances.Clear ();

		var searchString = inputField.text;
		if (searchString == "") {
			return;
		}

		var entries = DictionaryData.SearchDictionary (searchString);
		var count = Mathf.Min (MaxResults, entries.Count);
		for(int i = 0; i < count; i++){
			AddEntry(entries[i]);
		}
	}

	void AddEntry(DictionaryDataEntry entry){
		var instance = Instantiate (entryPrefab) as GameObject;
		instance.transform.SetParent (entryParent);
		instance.GetComponent<DictionaryEntryUI> ().Initiallize (entry.Kana, KanaConverter.Instance.ConvertToRomaji (entry.Kana), entry.EnglishSummary);

		entryInstances.Add (instance);
	}

	public void Close(){
		Destroy (gameObject);
	}

}
