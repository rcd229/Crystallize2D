using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DialogueProcess2DInitArgs {
    public GameObject Target { get; private set; }
    public DialogueSegment2D Dialogue { get; private set; }
    
    public DialogueProcess2DInitArgs(GameObject target, DialogueSegment2D dialogue) {
        this.Target = target;
        this.Dialogue = dialogue;
    }
}

public class DialogueProcess2D : EnumeratorProcess<DialogueProcess2DInitArgs, object> {
    public override IEnumerator<SubProcess> Run(DialogueProcess2DInitArgs args) {
        foreach (var element in args.Dialogue.Dialogue.Elements.Items) {
            if (element is LineDialogueElement) {
                SpeechBubbleManager2D.Instance.Add(args.Target.transform, ((LineDialogueElement)element).Line.Phrase);
                yield return Get(ProcessLibrary.ListenForInput, new InputListenerArgs(InputType.EnvironmentClick));
            }
        }
        SpeechBubbleManager2D.Instance.Remove(args.Target.transform);
    }
}
