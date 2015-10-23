using UnityEngine;
using System.Collections;

public class LineDialogueElement : DialogueElement {

    public DialogueActorLine Line { get; set; }

    public LineDialogueElement() : base(){
        Line = new DialogueActorLine();
    }

    public LineDialogueElement(PhraseSequence phrase) : this() {
        Line = new DialogueActorLine(phrase);
    }

    public override string ToString() {
        return "LineDialogueElement[" + Line.Phrase.ToString() + "]";
    }

    public override ProcessFactoryRef<DialogueState, DialogueState> Factory {
        get {
            return ProcessLibrary.DialogueLine;
        }
    }

}