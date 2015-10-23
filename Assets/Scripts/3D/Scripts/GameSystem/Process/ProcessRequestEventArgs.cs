using UnityEngine; 
using System;
using System.Collections;

public class ProcessRequestEventArgs : EventArgs {

}

public class ProcessRequestEventArgs<I, O> : ProcessRequestEventArgs {

    public I Data { get; set; }
    public ProcessExitCallback<O> Callback { get; set; }
    public IProcess Parent { get; set; }
    public Action<IProcess> SetChild { get; set; }

    public ProcessRequestEventArgs(I data, ProcessExitCallback<O> callback, IProcess parent) {
        Data = data;
        Callback = callback;
        Parent = parent;
    }

    public ProcessRequestEventArgs(I data, ProcessExitCallback<O> callback, IProcess parent, Action<IProcess> setChild) : this(data, callback, parent) {
        SetChild = setChild;
    }

}
