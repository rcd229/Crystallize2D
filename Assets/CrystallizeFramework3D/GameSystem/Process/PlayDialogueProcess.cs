﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayDialogueProcess : EnumeratorProcess<PlayDialogueContext, object> {
    static readonly ProcessFactoryRef<PlayDialogueContext, object> LineProcessFactory
        = new ProcessFactoryRef<PlayDialogueContext, object>();
    static readonly ProcessFactoryRef<List<BranchDialogueElementLink>, int> PromptProcessFactory
        = new ProcessFactoryRef<List<BranchDialogueElementLink>, int>();
    public static readonly ProcessFactoryRef<PlayDialogueContext, object> EndDialogueProcess
        = new ProcessFactoryRef<PlayDialogueContext, object>();

    public static void Initialize<T1, T2>()
    where T1 : IProcess<PlayDialogueContext, object>, new()
    where T2 : IProcess<List<BranchDialogueElementLink>, int>, new() {
        LineProcessFactory.Set<T1>();
        PromptProcessFactory.Set<T2>();
    }

    public static void Initialize<T1, T2, T3>()
        where T1 : IProcess<PlayDialogueContext, object>, new()
        where T2 : IProcess<List<BranchDialogueElementLink>, int>, new()
        where T3 : IProcess<PlayDialogueContext, object>, new() {
        LineProcessFactory.Set<T1>();
        PromptProcessFactory.Set<T2>();
        EndDialogueProcess.Set<T3>();
    }

    public override IEnumerator<SubProcess> Run(PlayDialogueContext context) {
        if (!context.Dialogue.Elements.ContainsKey(0)) {
            Debug.Log("Dialouge is empty");
            yield break;
        } else {
            while (context.CurrentElement != null) {
                var next = context.CurrentElement.DefaultNextID;
                if (context.CurrentElement is LineDialogueElement) {
                    yield return Get(LineProcessFactory, context);
                } else if (context.CurrentElement is BranchDialogueElement) {
                    var branches = ((BranchDialogueElement)context.CurrentElement).Branches;
                    var result = Get(PromptProcessFactory, branches);
                    yield return result;
                    if (result.Data >= 0 && result.Data < branches.Count) {
                        next = branches[result.Data].NextID;
                    } else {
                        next = -1;
                    }
                } else {
                    Debug.LogWarning("No handler found for " + context.GetType());
                }
                context.MoveTo(next);
            }
        }

        if(EndDialogueProcess.Factory != null) {
            yield return Get(EndDialogueProcess, context);
        }
    }
}
