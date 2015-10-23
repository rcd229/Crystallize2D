using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class KeyedPhraseSequence : UniqueNameGameData {

    public PhraseSequence Phrase { get; set; }

    public KeyedPhraseSequence() : base() {    }

    public KeyedPhraseSequence(string key, PhraseSequence phrase) {
        Name = key;
        Phrase = phrase;
    }

}
