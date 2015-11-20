using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PostCompilerStringComparer : IEqualityComparer<string>{
	#region IEqualityComparer implementation
	
	public bool Equals (string x, string y)
	{
		return x.Equals(y, StringComparison.OrdinalIgnoreCase);
	}
	
	public int GetHashCode (string obj)
	{
		return obj.ToLower().GetHashCode();
	}
	
	#endregion
}

public class PhraseSetGameData : ISerializableDictionaryItem<string> {

    public string Name { get; set; }
    public List<PhraseSequence> Phrases { get; set; }

    public string Key {
        get { return Name; }
    }

    public PhraseSetGameData() {
        Name = "";
        Phrases = new List<PhraseSequence>();
    }

    public PhraseSetGameData(string name) : this(){
        Name = name;
    }

    public void SetPhrase(int index, PhraseSequence phrase) {
        while (Phrases.Count <= index) {
            Phrases.Add(new PhraseSequence());
        }

        Phrases[index] = phrase;
    }

    public PhraseSequence GetPhrase(int index) {
        return Phrases[index];
    }

    public PhraseSequence GetPhrase(string translation) {
		var choices = (from p in Phrases
		               where p.Translation.ToLower().Equals(translation, System.StringComparison.OrdinalIgnoreCase)
		               select p);
		var phraseChoice = choices.FirstOrDefault();
		if(Name == PhraseSetCollectionGameData.Default.Name){
			return phraseChoice;
		}
		else{
			if(phraseChoice != null){
				return phraseChoice;
			}
			else{
				return PhraseSetCollectionGameData.Default.GetPhrase(translation);
			}
		}
    }

    public PhraseSequence GetOrCreatePhrase(string translation) {
        var p = GetPhrase(translation);
        if (p == null) {
            p = new PhraseSequence();
            p.Translation = translation;
            Phrases.Add(p);
        }
        return p;
    }   

    public PhraseSequence GetPhrase(string translation, out bool isDefault) {
        var choices = (from p in Phrases
                       where p.Translation.ToLower().Equals(translation.ToLower(), System.StringComparison.OrdinalIgnoreCase)
                       select p);
        var phraseChoice = choices.FirstOrDefault();
        if (Name == PhraseSetCollectionGameData.Default.Name) {
            isDefault = true;
            return phraseChoice;
        } else {
            if (phraseChoice != null) {
                isDefault = false;
                return phraseChoice;
            } else {
                isDefault = true;
                return PhraseSetCollectionGameData.Default.GetPhrase(translation);
            }
        }
    }

    public PhraseSequence GetOrCreatePhrase(int index) {
        while (Phrases.Count <= index) {
            Phrases.Add(new PhraseSequence());
        }

        return Phrases[index];
    }

    public IEnumerable<PhraseSequence> AggregateAllWords() {
        HashSet<PhraseSequenceElement> phraseSequenceSet = new HashSet<PhraseSequenceElement>(new PhraseElementComparerator());
        foreach (var p in Phrases) {
            phraseSequenceSet.AddValidWords(p);
        }
        var allWords = from word in phraseSequenceSet
                       where !PlayerDataConnector.ContainsLearnedItem(new PhraseSequence(word))
                       select new PhraseSequence(word);
        return allWords;
    }

    public IEnumerable<PhraseSequence> AggregateAllWordsAndPhrases() {
        var allPhrases = new HashSet<PhraseSequence>(AggregateAllWords(), new PhraseComparerator());
        allPhrases.UnionWith(Phrases);
        return allPhrases;
    } 

}