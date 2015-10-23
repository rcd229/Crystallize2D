using UnityEngine;
using System.Collections;

public class EnvironmentPhraseComparison : MonoBehaviour {
	public PhraseSequence Phrase;
	public PhraseSequence CorrectPhrase;
	public bool isCorrect{
		get{
			return PhraseSequence.PhrasesEquivalent(Phrase, CorrectPhrase);
		}
	}
}
