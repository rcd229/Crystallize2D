using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class PhraseElementComparerator : IEqualityComparer<PhraseSequenceElement> {

	public bool Equals (PhraseSequenceElement x, PhraseSequenceElement y)
	{
        return PhraseSequenceElement.IsEqual(x, y); //x.GetText(JapaneseTools.JapaneseScriptType.Romaji).Equals(y.GetText(JapaneseTools.JapaneseScriptType.Romaji));
	}

	public int GetHashCode (PhraseSequenceElement obj)
	{
        int hash = obj.WordID.GetHashCode();
        if (obj.Text != null) {
            hash = hash * 31 + obj.Text.GetHashCode();
        }
        return hash;
	}

}

public class PhraseComparerator : IEqualityComparer<PhraseSequence> {

    public bool Equals(PhraseSequence x, PhraseSequence y) {
        return PhraseSequence.PhrasesEquivalent(x, y); //x.GetText(JapaneseTools.JapaneseScriptType.Romaji).Equals(y.GetText(JapaneseTools.JapaneseScriptType.Romaji));
    }

    public int GetHashCode(PhraseSequence obj) {
        int hash = obj.ComparableElementCount.GetHashCode();
        foreach (var e in obj.PhraseElements) {
            if (e.IsDictionaryWord) {
                hash = (hash * 7) + e.WordID.GetHashCode();
            }
        }
        return hash;
    }

}

public class PhrasePrint : MonoBehaviour {
	string fileSuffix = ".txt";
	// Use this for initialization
	void Start () {
		var LoggingDirectory = "CrystallizeWords/";
		var path = Application.dataPath;
		var dir = Directory.GetParent(path).Parent;
		path = dir.FullName + "/" + LoggingDirectory;
		if(!Directory.Exists(path)){
			Directory.CreateDirectory(path);
		}
		PrintPhrases(path, 0, JapaneseTools.JapaneseScriptType.Romaji, false);
		PrintPhrases(path, 5, JapaneseTools.JapaneseScriptType.Romaji, false, false);
        PrintWords(path, 3, JapaneseTools.JapaneseScriptType.Kanji, true);
        PrintWords(path, 4, JapaneseTools.JapaneseScriptType.Romaji, false);
		PrintPhrases(path, 2, JapaneseTools.JapaneseScriptType.Kanji, true);
	}
	

	void PrintPhrases(string path, int index, JapaneseTools.JapaneseScriptType type, bool useContext, bool translate = true){
		using(StreamWriter sw = new StreamWriter(path + "wordsTranslation" + index + fileSuffix, false)){
			foreach (var p in PhraseSetCollectionGameData.Default.Phrases){
				if(useContext || !p.HasContextData){
					string output;
					if(translate)
                        output = string.Format("{0}\t{1}", p.GetText(type), p.Translation);
					else{
						if(!p.IsWord){
                            output = string.Format("{0}", p.GetText(type));
						}
						else{
							output = "";
							continue;
						}
					}
					sw.WriteLine(output);
				}
			}
			sw.Close();
		}
	}

	void PrintWords(string path, int index, JapaneseTools.JapaneseScriptType type, bool translate){
		using(StreamWriter sw = new StreamWriter(path + "wordsTranslation" + index + fileSuffix, false)){
			HashSet<PhraseSequenceElement> words = new HashSet<PhraseSequenceElement>(new PhraseElementComparerator());
			foreach (var p in PhraseSetCollectionGameData.Default.Phrases){
				foreach(var word in p.PhraseElements){
					if(!words.Contains(word) && word.IsDictionaryWord){
						words.Add(word);
					}
				}
			}

			var allWords = (from w in words
//			                orderby w.GetTranslation().Length
			                select w);
			foreach(var e in allWords){
				string output;
				if(translate)
                    output = string.Format("{0}\t{1}", e.GetText(type), e.GetTranslation());
				else
                    output = string.Format("{0}", e.GetText(type));
				sw.WriteLine(output);
			}
			sw.Close();
		}
	}

}
