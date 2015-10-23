using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DictionaryEntryUI : MonoBehaviour {

	public Text kanaText;
	public Text romajiText;
	public Text englishText;

	public void Initiallize(string kana, string romaji, string english){
		kanaText.text = kana;
		romajiText.text = romaji;
		englishText.text = english;
	}

}
