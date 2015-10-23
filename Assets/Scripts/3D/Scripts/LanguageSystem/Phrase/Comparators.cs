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


