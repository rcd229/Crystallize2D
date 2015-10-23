using UnityEngine;
using System;
using System.Collections;

public abstract class ProcessExitEventArgs : EventArgs {

}

public class ProcessExitEventArgs<T> : ProcessExitEventArgs {

    public T Data { get; set; }

    public ProcessExitEventArgs(T data) {
        Data = data;
    }

}
