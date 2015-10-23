using UnityEngine;
using System;
using System.Collections;

public static class EventExtensions {

    public static void Raise(this Action a) {
        if (a != null) {
            a();
        }
    }

    public static void Raise<T>(this Action<T> a, T args) {
        if (a != null) {
            a(args);
        }
    }

    public static void Raise<T1, T2>(this Action<T1, T2> a, T1 arg1, T2 arg2){
        if (a != null) {
            a(arg1, arg2);
        }
    }

    public static T1 Raise<T1>(this Func<T1> func) {
        if (func != null) {
            return func();
        }
        return default(T1);
    }

    public static void Raise(this EventHandler eventHandler, object sender, EventArgs args) {
        if (eventHandler != null) {
            eventHandler(sender, args);
        }
    }

    public static void Raise<T>(this EventHandler<T> eventHandler, object sender, T args) where T : EventArgs {
        if (eventHandler != null) {
            eventHandler(sender, args);
        }
    }

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
