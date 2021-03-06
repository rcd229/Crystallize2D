﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SubProcess { }

public class SubProcess<T> : SubProcess {
    public T Data { get; set; }
}

public class EnumSubProcess : SubProcess {
    public IEnumerator<SubProcess> enumerator { get; private set; }
    public EnumSubProcess(IEnumerator<SubProcess> enumer) { this.enumerator = enumer; }
}

public class WaitSubProcess : SubProcess { }

public abstract class EnumeratorProcess<I, O> : IProcess<I, O> {

    public ProcessExitCallback OnExit { get; set; }

    protected object result;
    protected virtual O ExitArgs { get; set; }

    Stack<IEnumerator<SubProcess>> current = new Stack<IEnumerator<SubProcess>>();
    SubProcess currentSubProcess;

    public void Initialize(I param1) {
        ExitArgs = default(O);
        current.Push(Run(param1));
        Continue();
    }

    protected void Exit() {
        BeforeExit();
        current = null;
        Debug.Log("exiting with: " + ExitArgs);
        OnExit.Raise(this, ExitArgs);
    }

    protected void Exit(O args) {
        BeforeExit();
        current = null;
        OnExit.Raise(this, args);
    }

    public virtual void ForceExit() { }

    public abstract IEnumerator<SubProcess> Run(I args);
    protected virtual void BeforeExit() { }

    protected SubProcess<O1> Get<I1, O1>(ProcessFactoryRef<I1, O1> factory, I1 args) {
        var sp = new SubProcess<O1>();
        factory.Get<I1, O1>(args, (s, e) => Callback<O1>(sp, e), this);
        return sp;
    }

    protected SubProcess<O1> Get<I1, O1>(UIFactoryRef<I1, O1> factory, I1 args) {
        var sp = new SubProcess<O1>();
        factory.Get(args, (s, e) => Callback(sp, e.GetDataSafely()), this);
        return sp;
    }

    protected EnumSubProcess Get(IEnumerator<SubProcess> enumerator) {
        var sp = new EnumSubProcess(enumerator);
        return sp;
    }

    protected WaitSubProcess Wait() {
        return new WaitSubProcess();
    }

    void Callback<T>(SubProcess<T> sp, T args) {
        if (current == null || current.Count == 0) return;
        result = args;
        sp.Data = args;
        if (args is O) {
            ExitArgs = (O)(object)args;
        }
        Continue();
    }

    protected void Continue() {
        //Debug.Log(Time.time + "Continuing.");
        if (current.Peek().MoveNext()) {
            if (current != null && current.Peek().Current is EnumSubProcess) {
                //Debug.Log(Time.time + "Pushing enumerator.");
                current.Push((current.Peek().Current as EnumSubProcess).enumerator);
                Continue();
            }
        } else if (current != null) {
            current.Pop();
            if (current.Count == 0) {
                //Debug.Log(Time.time + "Stack is empty.");
                Exit();
            } else {
                //Debug.Log(Time.time + "Continuing on substack.");
                Continue();
            }
        }
    }

    protected void Continue(object sender, EventArgs args) {
        Continue();
    }

    protected void ContinueCallback(object sender, object args) {
        Continue();
    }

}
