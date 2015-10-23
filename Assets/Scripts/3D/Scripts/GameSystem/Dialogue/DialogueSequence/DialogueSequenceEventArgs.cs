using UnityEngine;
using System.Collections;

public class DialogueSequenceEventArgs : System.EventArgs {

    public DialogueSequence Dialogue { get; set; }
    public int CurrentElement { get; set; }

    public DialogueSequenceEventArgs(DialogueSequence dialogue, int currentElement)
    {
        Dialogue = dialogue;
        CurrentElement = currentElement;
    }

}
