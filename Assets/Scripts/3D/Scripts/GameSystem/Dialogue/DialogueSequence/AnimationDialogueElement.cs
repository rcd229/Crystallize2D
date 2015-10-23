using UnityEngine;
using System.Collections;

public class AnimationDialogueElement : DialogueElement {

    public DialogueAnimation Animation { get; set; }

    public override ProcessFactoryRef<DialogueState, DialogueState> Factory {
        get {
            var f = new ProcessFactoryRef<DialogueState, DialogueState>();
            f.Set<AnimationDialogueElementProcess>();
            return f;
        }
    }

    public AnimationDialogueElement()
        : base() {

    }

    public override string ToString() {
        return "AnimationDialogueElement[" + Animation + "]";
    }

}