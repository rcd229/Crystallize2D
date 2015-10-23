using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class WordSelectionProcess : IProcess<PhraseSequenceElement, PhraseSequenceElement> {

    public ProcessExitCallback OnExit { get; set; }

    ITemporaryUI<PhraseSequenceElement, PhraseSequenceElement> ui;

    public void Initialize(PhraseSequenceElement param1) {
        ui = UILibrary.WordSelector.Get(param1);
        ui.Complete += ui_Complete;
    }

    public void ForceExit() {
        Exit(null);
    }

    void ui_Complete(object sender, EventArgs<PhraseSequenceElement> e) {
        Exit(e.Data);
    }

    void Exit(PhraseSequenceElement word) {
        ui.Close();
        OnExit.Raise(this, word);
    }

}
