using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PhraseSequenceEquivalentComparator : IEqualityComparer<PhraseSequence>{
	#region IEqualityComparer implementation

	public bool Equals (PhraseSequence x, PhraseSequence y)
	{
		return PhraseSequence.PhrasesEquivalent(x, y);
	}

	public int GetHashCode (PhraseSequence obj)
	{
		return obj.GetText(JapaneseTools.JapaneseScriptType.Romaji).ToLower().GetHashCode();
	}

	#endregion


}

public class PhraseElementComparerator : IEqualityComparer<PhraseSequenceElement> {

    public bool Equals(PhraseSequenceElement x, PhraseSequenceElement y) {
        return PhraseSequenceElement.IsEqual(x, y); //x.GetText(JapaneseTools.JapaneseScriptType.Romaji).Equals(y.GetText(JapaneseTools.JapaneseScriptType.Romaji));
    }

    public int GetHashCode(PhraseSequenceElement obj) {
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


