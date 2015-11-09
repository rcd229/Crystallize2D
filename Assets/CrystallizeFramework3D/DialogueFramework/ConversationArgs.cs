using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ConversationArgs {

    public static ConversationArgs ExitArgs(GameObject target, DialogueSequence dialogue, bool clearOnClose = true) {
        return new ConversationArgs(target, dialogue, null, false, clearOnClose);
    }

    public static ConversationArgs OpenSegmentArgs(GameObject target, DialogueSequence dialogue, ContextData context) {
        return new ConversationArgs(target, dialogue, context, true, false);
    }

    public GameObject[] Targets { get; set; }
    public DialogueSequence Dialogue { get; set; }
    public bool DoCamera { get; set; }
    public ContextData Context { get; set; }
    public StringMap ActorMap { get; set; }

    public GameObject Target {
        get {
            if (Targets == null || Targets.Length == 0) return null;
            return Targets[0];
        }
    }

    public bool HasMultipleTargets {
        get {
            return Targets.Length > 1;
        }
    }

    public bool PlayImmediately { get; set; }
    public bool ClearOnClose { get; set; }

    ConversationArgs(DialogueSequence dialogue) {
        this.Targets = new GameObject[0];
        this.Dialogue = dialogue;
        this.DoCamera = true;
        this.PlayImmediately = false;
        this.ClearOnClose = true;
        this.ActorMap = new StringMap();
        this.Context = new ContextData();
    }

    public ConversationArgs(GameObject target, DialogueSequence dialogue) : this(dialogue) {
        this.Targets = new GameObject[1];
        this.Targets[0] = target;
    }

    public ConversationArgs(GameObject[] targets, DialogueSequence dialogue) : this(dialogue) {
        this.Targets = targets;
        this.DoCamera = false;
    }

    public ConversationArgs(GameObject target, DialogueSequence dialogue, ContextData context) : this(target, dialogue) {
        this.Context = context;
    }

    public ConversationArgs(GameObject target, DialogueSequence dialogue, ContextData context, bool playImmediately)
        : this(target, dialogue) {
        this.Context = context;
        this.PlayImmediately = playImmediately;
    }

    public ConversationArgs(GameObject target, DialogueSequence dialogue, ContextData context, bool playImmediately, bool clearOnClose) 
    : this(target, dialogue, context,playImmediately){
        ClearOnClose = clearOnClose;
    }

}
