using UnityEngine;
using System.Collections;

public class PartnerSaidPhraseEventArgs : System.EventArgs {

    public string PlayerGuid { get; set; }
    public PhraseSequence Phrase { get; set; }

    public PartnerSaidPhraseEventArgs(string guid, PhraseSequence phrase) {
        this.PlayerGuid = guid;
        this.Phrase = phrase;
    }

}
