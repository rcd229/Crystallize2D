using UnityEngine;
using System.Collections;

public class WordContainer : IWordContainer {

    public GameObject gameObject {
        get { return null; }
    }

    public PhraseSequenceElement Word {
        get;
        set;
    }

    public WordContainer(PhraseSequenceElement element) {
        Word = element;
    }

}
