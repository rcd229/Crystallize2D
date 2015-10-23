using UnityEngine;
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

public class WaitSubProcess : SubProcess {}

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

    protected SubProcess<O> Get<I, O>(ProcessFactoryRef<I, O> factory, I args) {
        var sp = new SubProcess<O>();
        factory.Get<I, O>(args, (s, e) => Callback<O>(sp, e), this);
        return sp;
    }

    protected SubProcess<EventArgs<O>> Get<I, O>(UIFactoryRef<I, O> factory, I args) {
        var sp = new SubProcess<EventArgs<O>>();
        factory.Get(args, (s, e) => Callback<EventArgs<O>>(sp, e), this);
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
