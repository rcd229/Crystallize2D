using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[JobProcessType]
public class StandardConversationProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public ProcessExitCallback OnExit { get; set; }

    GameObject person;
    JobTaskRef task;

    public void Initialize(JobTaskRef param1) {
        task = param1;
        //Debug.Log(task.Variation);
        person = new SceneObjectRef(task.Data.Actor).GetSceneObject();
        ProcessLibrary.Conversation.Get(new ConversationArgs(person, task.Data.Dialogue), HandleConversationExit, this);
    }

    void HandleConversationExit(object sender, object obj) {
        var money = UnityEngine.Random.Range(1, 100);
        PlayerDataConnector.AddMoney(money);
        var ui = UILibrary.MessageBox.Get(string.Format("You found {0} yen on the ground.", money));
        ui.Complete += ui_Complete;
    }

    void ui_Complete(object sender, EventArgs<object> e) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
