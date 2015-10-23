using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinearDialogueEnumerator : IEnumerator<DialogueActorLine> {

    int index = 0;
    public LinearDialogueSection Dialogue { get; set; }

    public LinearDialogueEnumerator(LinearDialogueSection dialogue) {
        index = -1;
        this.Dialogue = dialogue;
    }

    public DialogueActorLine Current {
        get {
            if(index < 0){
                return null;
            }

            if(index >= Dialogue.Lines.Count){
                return null;
            }

            return Dialogue.Lines[index];
        }
    }

    public void Dispose() {
        Dialogue = null;
    }

    object IEnumerator.Current {
        get { return Current; }
    }

    public bool MoveNext() {
        index++;
        return index < Dialogue.Lines.Count;
    }

    public void Reset() {
        index = 0;
    }

}
