using UnityEngine;
using System.Collections;

public class PhraseClickedEventArgs : System.EventArgs {

    public PhraseSequence Phrase { get; set; }
    public PhraseSequenceElement Word {
        get {
            return Phrase.Word;
        }
    }
    public string Destination { get; set; }

    public PhraseClickedEventArgs(PhraseSequenceElement word, string destination) {
        this.Phrase = new PhraseSequence(word);
        this.Destination = destination;
    }

    public PhraseClickedEventArgs(PhraseSequence phrase, string destination) {
        this.Phrase = phrase;
        this.Destination = destination;
    }

}
