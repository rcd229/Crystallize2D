using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class DialogueActorLine {

    public PhraseSequence Phrase { get; set; }
    public int AudioClipID { get; set; }

    public DialogueActorLine() {
        Phrase = new PhraseSequence();
        AudioClipID = -1;
    }

    public DialogueActorLine(PhraseSequence phrase) : this() {
        Phrase = phrase;
    }

}
