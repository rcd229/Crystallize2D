using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PhraseSelectionSubProcess : IProcess<PhraseSelectorInitArgs, PhraseSequence> {

    public ProcessExitCallback OnExit { get; set; }

    PhraseSelectorInitArgs args;

    public void Initialize(PhraseSelectorInitArgs args) {
        this.args = args;
        Begin(null, null);
    }

    public void ForceExit() { }

    void Begin(object sender, EventArgs<object> e) {
        UILibrary.PhraseSelector.Get(args, ui_Complete, this);
    }

    void ui_Complete(object sender, EventArgs<PhraseSequence> e) {
        //Debug.Log("Handling " + e.Data.GetText());
        if (e.Data.GetText().Trim() == "?") {
            HandlePhrasePanelExit(sender, e.Data);
        } else if (args.UseTranslation || e.Data.ComparableElementCount == 0) {
            HandlePhrasePanelExit(sender, e.Data);
        } else if (!PlayerDataConnector.CanSelectPhrase(e.Data)) {
            var needed = PlayerDataConnector.GetNeededWords(e.Data);
            foreach (var nw in needed) {
                Debug.Log("n");
            }
            UILibrary.NeededWords.Get(needed, Begin, this);
        } else if (PlayerDataConnector.NeedToConstructPhrase(e.Data)) {
            UILibrary.PhraseConstructor.Get(PlayerDataConnector.GetConstructorArgsForPhrase(e.Data), ConstructorExit, this);
        } else {
            HandlePhrasePanelExit(sender, e.Data);
        }
    }

    void ConstructorExit(object sender, EventArgs<List<PhraseSequence>> args) {
        HandlePhrasePanelExit(sender, args.Data.Flatten());
    }

    void HandlePhrasePanelExit(object sender, PhraseSequence args) {
        Exit(args);
    }

    void Exit(PhraseSequence phrase) {
        OnExit.Raise(this, phrase);
    }

}
