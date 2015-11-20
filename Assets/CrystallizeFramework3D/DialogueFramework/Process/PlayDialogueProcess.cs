using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayDialogueProcess : EnumeratorProcess<DialogueSequence, object> {
    static readonly ProcessFactoryRef<DialogueElement, DialogueElement> LineProcessFactory
        = new ProcessFactoryRef<DialogueElement, DialogueElement>();
    static readonly ProcessFactoryRef<IEnumerable<DialogueElement>, DialogueElement> PromptProcessFactory
        = new ProcessFactoryRef<IEnumerable<DialogueElement>, DialogueElement>();


    public static void Initialize<T1, T2>()
        where T1 : IProcess<DialogueElement, DialogueElement>, new()
        where T2 : IProcess<IEnumerable<DialogueElement>, DialogueElement>, new() {
        LineProcessFactory.Set<T1>();
        PromptProcessFactory.Set<T2>();
    }

    public override IEnumerator<SubProcess> Run(DialogueSequence args) {
        var next = args.Elements.Get(0);
        yield return Get(LineProcessFactory, args.Elements.Get(0));
    }
}
