using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public static class EventArgsExtensions {
    public static T GetDataSafely<T> (this EventArgs<T> e) {
        if(e == null) {
            return default(T);
        }
        return e.Data;
    }
}

public class EventArgs<T> : System.EventArgs, IEventArgs<T> {

    public T Data { get; private set; }

    public EventArgs(T data) {
        Data = data;
    }

}
