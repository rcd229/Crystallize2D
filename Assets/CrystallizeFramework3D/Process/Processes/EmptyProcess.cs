using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EmptyProcess<I, O> : IProcess<I, O> {

    public ProcessExitCallback OnExit{ get; set; }

    public void ForceExit() {
        
    }

    public void Initialize(I param1) {
        CoroutineManager.Instance.WaitAndDo(Exit);
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
