using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BeginConversationProcess : ConversationProcessPart, IProcess<ConversationArgs, object> {

    ConversationArgs args;

    public ProcessExitCallback OnExit { get; set; }

    public void Initialize(ConversationArgs args) {
        //Debug.Log("starting conversation");
        this.args = args;

        ResetDialogueAnimation.resetActions = new List<Action>();
        if (args.DoCamera) {
            StartCamera(args);
            CoroutineManager.Instance.WaitAndDo(() => PrepareConversation(args.Dialogue));
        } else {
            CoroutineManager.Instance.WaitAndDo(Exit);
        }
    }

    void PrepareConversation(DialogueSequence dialogue) {
        if (args.PlayImmediately) {
            ProcessLibrary.ConversationSegment.Get(args, HandleSegmentExit, this);
        } else {
            Exit();
        }
    }

    public void ForceExit() {
        Exit();
    }

    void HandleSegmentExit(object sender, object args) {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}