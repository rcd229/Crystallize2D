using UnityEngine;
using System.Collections;

public class ConversationSegmentProcess : IProcess<ConversationArgs, DialogueState> {

    public ProcessExitCallback OnExit { get; set; }

    ConversationArgs args;

    public void Initialize(ConversationArgs args) {
        //if (args.Target && !args.ActorMap.ContainsKey("[default]")) {
        //    args.ActorMap.Set(new StringMapItem("[default]", args.Target.name));
        //}

        //Debug.Log("args: " + args.Dialogue);
        this.args = args;
        CoroutineManager.Instance.WaitAndDo(
                () => SetDialogueElement(new DialogueState(0, args.Dialogue, args.Context, args.ActorMap, args.Targets))
                );
    }

    void SetDialogueElement(DialogueState dialogueState) {
        var e = dialogueState.GetElement(args.Dialogue);

        args.Dialogue.RaiseEvents(e.ID);

        if (e is DialogueSequence) {
            Debug.Log("is dialogue sequence; ");
            var subD = (DialogueSequence)e;
            ProcessLibrary.ConversationSegment.Get(new ConversationArgs(args.Target, subD), HandleTurnExit, this);
            return;
        } else {
            var f = e.Factory;
            if (f == null) {
                Debug.LogError("Factory for " + e + " cannot be null");
            } else {
                f.Get(dialogueState, HandleTurnExit, this);
                return;
            }
        }

        int id = -1;
        if (e != null) {
            id = e.DefaultNextID;
        }
        HandleTurnExit(null, new DialogueState(id, args.Dialogue, null, null));
    }

    void HandleTurnExit(object sender, DialogueState e) {
        if (e.GetElement(args.Dialogue) == null) {
            //Debug.Log("is dialogue sequence; " + e.CurrentID);
            Exit();
            return;
        }
        //Debug.Log(e.GetElement());

        if (e.CurrentID == DialogueSequence.ConfusedExit) {
            ProcessLibrary.DialogueLine.Get(e, HandleTurnExit, this);
            return;
        }

        SetDialogueElement(new DialogueState(e.CurrentID, args.Dialogue, e.Context, e.ActorMap, e.GameObjects));
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        //Debug.Log("conv seg exit");
        OnExit.Raise(this, new DialogueState(args.Dialogue.DefaultNextID, args.Dialogue, args.Context, args.ActorMap, args.Target));
    }

}