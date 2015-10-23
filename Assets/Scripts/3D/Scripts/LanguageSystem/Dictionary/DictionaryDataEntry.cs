using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DictionaryDataEntry {

	AuxiliaryDictionaryDataEntry auxData;

	public int ID { get; set; }
	public string Kanji { get; set; }
	public string Kana { get; set; }
	public List<string> English { get; set; }
	public PartOfSpeech PartOfSpeech { get; set; }

	public string EnglishSummary {
		get {
			var s = "";
			foreach (var e in English) {
				s += e + "; ";
			}
			if (s.Length >= 2) {
				s = s.Substring (0, s.Length - 2);
			}
			return s;
		}
	}

	public AuxiliaryDictionaryDataEntry AuxiliaryData {
		get {
			return auxData;
		}
	}

	public bool HasAuxiliaryData {
		get {
			return auxData != null;
		}
	}

	public DictionaryDataEntry(){
		ID = -1;
		Kanji = "";
		Kana = "";
		English = new List<string> ();
	}

	public DictionaryDataEntry(int id, string kanji, string kana, List<string> english, PartOfSpeech partOfSpeech){
		ID = id;
		Kanji = kanji;
		Kana = kana;
		English = english;
		PartOfSpeech = partOfSpeech;
	}

	public void SetAuxiliaryData(AuxiliaryDictionaryDataEntry auxData){
		this.auxData = auxData;
	}

    public string GetPreferredTranslation() {
        if (AuxiliaryData != null) {
            if (AuxiliaryData.PreferredTranslation != null) {
                return AuxiliaryData.PreferredTranslation;
            } 
        }
        if (English.Count == 0) {
            return "NULL";
        }
        return English[0];
    }

    public PartOfSpeech GetPartOfSpeech() {
        if (AuxiliaryData != null) {
            if (AuxiliaryData.PartOfSpeech != PartOfSpeech.Unclassified) {
                return AuxiliaryData.PartOfSpeech;
            }
        }
        return PartOfSpeech;
    }

    public List<string> GetAllTranslations() {
        var t = new List<string>(English);
        if (AuxiliaryData != null) {
            if (AuxiliaryData.PreferredTranslation != null) {
                t.Add(AuxiliaryData.PreferredTranslation);
            }
        }
        return t;
    }

}
