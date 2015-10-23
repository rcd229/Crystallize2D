using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LineDialogueElementProcess : EnumeratorProcess<DialogueState, DialogueState> {

    ITemporaryUI clickToContinueUI;

    public ProcessExitCallback OnExit { get; set; }
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public override IEnumerator<SubProcess> Run(DialogueState args) {
        var target = args.GetTarget();
        var context = new ContextData();
        var a = target.GetComponentInSelfOrChild<DialogueActor>();
        if (a) { context = a.GetOrCreateRandomContext().Context.OverrideWith(args.Context); }

        target.GetComponentInChildren<DialogueActor>().SetLine(args.GetElement<LineDialogueElement>().Line, context, args.ReducesConfidence);

        yield return Get(AwaitNext(args));

        Exit(args.NextElement());
    }

    protected virtual IEnumerator<SubProcess> AwaitNext(DialogueState args) {
        clickToContinueUI = UILibrary.ClickToContinue.Get(null);
        yield return Get(ProcessLibrary.ListenForInput, new InputListenerArgs(InputType.EnvironmentClick));
    }

    protected override void BeforeExit() {
        clickToContinueUI.CloseIfNotNull();
    }

}