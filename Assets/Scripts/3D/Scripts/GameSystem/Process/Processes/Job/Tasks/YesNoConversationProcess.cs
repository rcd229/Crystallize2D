using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class YesNoArgs {
    public PhraseSequence Prompt { get; set; }
    public bool Center { get; set; }

    public YesNoArgs(PhraseSequence prompt, bool center = false) {
        this.Prompt = prompt;
        this.Center = center;
    }
}

public class YesNoConversationProcess : IProcess<YesNoArgs, bool> {

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(YesNoArgs args) {
        var phrases = new List<PhraseSequence>();
        phrases.Add(new PhraseSequence("Yes"));
        phrases.Add(new PhraseSequence("No"));
        var cArgs = new PhraseSelectorInitArgs(phrases, args.Prompt.GetText(), false);
        cArgs.UseTranslation = true;
        cArgs.Center = args.Center;
        UILibrary.PhraseSelector.Get(cArgs, HandleChoice, this);
    }

    void HandleChoice(object sender, EventArgs<PhraseSequence> data) {
        if (data.Data.GetText().ToLower().Trim() == "yes") {
            Exit(true);
        } else {
            Exit(false);
        }
    }

    public void ForceExit() {    }

    void Exit(bool val) {
        OnExit.Raise(this, val);
    }
    
}
