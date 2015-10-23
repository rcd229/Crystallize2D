using UnityEngine;
using System;
using System.Collections;

public class PhraseInputEventArgs : EventArgs {

	public PhraseSequence Phrase { get; set; }
	public ContextData ContextData { get; set; }

	public PhraseInputEventArgs(PhraseSequence phrase){
		Phrase = phrase;
	}

    public PhraseInputEventArgs(PhraseSequence phrase, ContextData context)
        : this(phrase) {
		Phrase = phrase;
		ContextData = context;
	}

}
