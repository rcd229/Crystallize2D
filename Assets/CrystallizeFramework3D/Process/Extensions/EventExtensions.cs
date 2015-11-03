using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static partial class EventExtensions {
    public static void Raise<I, O>(this ProcessExitCallback eventHandler, IProcess<I, O> sender, object args) {
        if (eventHandler != null) {
            var e = eventHandler;
            e(sender, args);
        }
    }

    public static void Raise<T>(this ProcessExitCallback<T> eventHandler, object sender, T args) {
        if (eventHandler != null) {
            var e = eventHandler;
            e(sender, args);
        }
    }
}
