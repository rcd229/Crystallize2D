using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class LearnGrammarProcess : IProcess<JobTaskRef, object> {

    public const string DesuPhrase = "to be [object]";
    public const string SubjectDesuPhrase = "[subject] is a [object]";

    public const string Student = "student";
    public const string Teacher = "teacher";
    public const string Cat = "cat";
    public const string Dog = "dog";

    public const string I = "i";
    public const string You = "you";
    public const string He = "he";
    public const string She = "she";

    public ProcessExitCallback OnExit { get; set; }

    JobTaskRef task;
    DialogueSequence dialogue;

    public void Initialize(JobTaskRef param1) {
        task = param1;
        var person = new SceneObjectRef(task.Data.Actor).GetSceneObject();
        ProcessLibrary.Conversation.Get(new ConversationArgs(person, task.Data.Dialogue), ConversationExit, this);
    }

    void ConversationExit(object sender, object obj) {
        //var blackout = UILibrary.BlackScreen.Get(null);
        //blackout.Complete += blackout_Complete;
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

    void ChooseSession() {

    }

}
