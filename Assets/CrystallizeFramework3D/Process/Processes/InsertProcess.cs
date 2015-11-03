using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class InsertProcess<I, O> : IProcess<I, O> {

    ProcessFactoryRef<I, O> original = new ProcessFactoryRef<I,O>();
    IProcessGetter inserted;
    I storedArgs;

    public ProcessExitCallback OnExit { get; set; }

    public InsertProcess(IProcessGetter inserted, ProcessFactory<I> original) {
        this.inserted = inserted;
        this.original.Factory = original;
        //Debug.Log("Playing inserted");
    }

    public void Initialize(I param1) {
        storedArgs = param1;
        inserted.Get(InsertedProcessCallback, this);
    }

    void InsertedProcessCallback(object sender, object args) {
        Debug.Log("orig fact: " + original.Factory);
        if (original.Factory != null) {
            original.Get(storedArgs, OriginalProcessCallback, this);
        } else {
            Exit((O)args);
        }
    }

    void OriginalProcessCallback(object sender, O args) {
        Exit(args);
    }

    public void ForceExit() {
        Exit(default(O));
    }

    void Exit(O args) {
        OnExit.Raise(this, args);
    }

}
