using UnityEngine;
using System;
using System.Collections;

public class PhraseEventArgs : EventArgs {

	public static new PhraseEventArgs Empty { get; private set; }

	static PhraseEventArgs(){
		Empty = new PhraseEventArgs ();
	}

	public IWordContainer PhraseContainer { get; set; }

    public PhraseSequence Phrase { get; set; }
    public PhraseSequenceElement Word {
        get {
            return Phrase.PhraseElements[0];
        }
    }
	public string Translation { get; set; }

	PhraseEventArgs(){
	}

	public PhraseEventArgs (IWordContainer phraseContainer){
		//PhraseData = phraseContainer.PhraseData;
		PhraseContainer = phraseContainer;

        Phrase = new PhraseSequence();
        Phrase.Add(phraseContainer.Word); //PhraseData.GetPhraseSequence().PhraseElements[0]);
	}

    public PhraseEventArgs(PhraseSequenceElement word) {
        Phrase = new PhraseSequence();
        Phrase.Add(word);
    }

    public PhraseEventArgs(PhraseSequence phrase) {
        Phrase = phrase;
    }

}
