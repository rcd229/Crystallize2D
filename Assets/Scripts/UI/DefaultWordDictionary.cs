using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DefaultWordDictionary {
    
    static DefaultWordDictionary _instance;
    public static DefaultWordDictionary Instance {
        get {
            if(_instance == null) {
                _instance = new DefaultWordDictionary();
            }
            return _instance;
        }
    }

    public PrefixTree<PhraseSequence> Dictionary { get; private set; }

    DefaultWordDictionary() {
        Dictionary = new PrefixTree<PhraseSequence>(
            DictionaryData.Instance.Entries
            .Select(e => new PhraseSequence(new PhraseSequenceElement(e.ID, 0)))
            );
    }

}
