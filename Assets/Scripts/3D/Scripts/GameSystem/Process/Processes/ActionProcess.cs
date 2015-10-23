using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ActionProcess<I> : IProcess<I, object> {

    public ProcessExitCallback OnExit { get; set; }

    Action<I> action;

    public ActionProcess(Action<I> action) {
        this.action = action;
    }

    public void Initialize(I param1) {
        //Debug.Log("Calling");
        action(param1);
        CoroutineManager.Instance.WaitAndDo(Exit);
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
