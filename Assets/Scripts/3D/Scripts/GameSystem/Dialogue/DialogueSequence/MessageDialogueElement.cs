using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MessageDialogueElement : DialogueElement {

    public string Message { get; set; }

    public override ProcessFactoryRef<DialogueState, DialogueState> Factory {
        get {
            var f = new ProcessFactoryRef<DialogueState, DialogueState>();
            f.Set<MessageDialogueElementProcess>();
            return f;
        }
    }

    public MessageDialogueElement()
        : base() {
        Message = "";
    }

    public MessageDialogueElement(string message)
        : this() {
        Message = message;
    }

}
