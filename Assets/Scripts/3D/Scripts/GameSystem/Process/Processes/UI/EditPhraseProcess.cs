using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EditPhraseProcess : IProcess<PhraseSequence, PhraseSequence> {

    public ProcessExitCallback OnExit { get; set; }

    ITemporaryUI<PhraseSequence, PhraseSequence> ui;

    public void Initialize(PhraseSequence param1) {
        ui = UILibrary.PhraseEditor.Get(param1);
        ui.Complete += ui_Complete;
    }

    void ui_Complete(object sender, EventArgs<PhraseSequence> e) {
        if (e == null) {
            Exit(null);
        } else {
            Exit(e.Data);
        }
    }

    public void ForceExit() {
        Exit(null);
    }

    void Exit(PhraseSequence args) {
        if (ui != null) {
            ui.Close();
        }
        OnExit.Raise(this, args);
    }

}
