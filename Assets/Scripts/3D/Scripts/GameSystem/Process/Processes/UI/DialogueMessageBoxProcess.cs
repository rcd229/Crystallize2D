using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MessageDialogueElementProcess : IProcess<DialogueState, DialogueState> {

    DialogueState state;
    ITemporaryUI<string, object> instance;

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(DialogueState param1) {
        state = param1;
        instance = UILibrary.MessageBox.Get(((MessageDialogueElement)param1.GetElement()).Message);
        instance.Complete += HandleComplete;
    }

    public void ForceExit() {
        Exit();
    }

    void HandleComplete(object sender, EventArgs e) {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, new DialogueState(state.GetElement().DefaultNextID, state.Dialogue, state.Context, state.ActorMap, state.GameObjects));
    }

}
