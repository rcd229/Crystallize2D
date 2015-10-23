using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseInputUIRequestEventArgs : UIRequestEventArgs {

	public PhraseSequence PhraseSequence { get; set; }
    public PlayerActorLine PlayerLine { get; set; }

    public PhraseInputUIRequestEventArgs(GameObject menuParent, PhraseSequence phraseSequence)
        : base(menuParent) {
        PhraseSequence = phraseSequence;
    }

    public PhraseInputUIRequestEventArgs(GameObject menuParent, PlayerActorLine line)
        : base(menuParent) {
            PhraseSequence = line.Phrase;
            PlayerLine = line;
    }

}
