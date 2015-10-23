using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class _SceneSayPhrase : MonoBehaviour {

    public PhraseSequence phrase;

    public void Say() {
        GetComponent<DialogueActor>().SetPhrase(phrase, false);
    }

}
