using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EventArgs<T> : System.EventArgs, IEventArgs<T> {

    public T Data { get; private set; }

    public EventArgs(T data) {
        Data = data;
    }

}
