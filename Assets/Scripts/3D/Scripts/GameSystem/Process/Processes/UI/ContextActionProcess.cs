using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ContextActionProcess : IProcess<string, object> {

    public ProcessExitCallback OnExit { get; set; }

    ITemporaryUI<string, string> instance;

    public void Initialize(string param1) {
        instance = UILibrary.ContextActionButton.Get(param1);
        instance.Complete += instance_Complete;
    }

    void instance_Complete(object sender, EventArgs<string> e) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit(this, null);
    }

}
