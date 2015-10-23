using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Xml.Serialization;

public static class ActionDialogueEventExtensions {
    public static bool IsAction(this IDialogueEvent e, Action action) {
        if (e is ActionDialogueEvent && ((ActionDialogueEvent)e).Event.Method.Name == action.Method.Name) {
            return true;
        } else {
            return false;
        }
    }

    public static bool IsAction<T1>(this IDialogueEvent e, Action<T1> action) {
        if (e is ActionDialogueEvent<T1> && ((ActionDialogueEvent<T1>)e).Event.Method.Name == action.Method.Name) {
            return true;
        } else {
            return false;
        }
    }

    public static bool IsAction<T1, T2>(this IDialogueEvent e, Action<T1, T2> action) {
        if (e is ActionDialogueEvent<T1, T2> && ((ActionDialogueEvent<T1, T2>)e).Event.Method.Name == action.Method.Name) {
            return true;
        } else {
            return false;
        }
    }
}

public class ActionDialogueEvent : BaseDialogueEvent {

    public static ActionDialogueEvent Get(Action action) {
        return new ActionDialogueEvent(action);
    }

    public static ActionDialogueEvent<T1> Get<T1>(Action<T1> action, T1 arg1) {
        return new ActionDialogueEvent<T1>(action, arg1);
    }

    public static ActionDialogueEvent<T1, T2> Get<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2) {
        return new ActionDialogueEvent<T1, T2>(action, arg1, arg2);
    }

    [XmlIgnore]
    public Action Event { get; private set; }

    public ActionDialogueEvent(Action callback) : base () {
        this.Event = callback;
    }

    public override void RaiseEvent() {
        Event.Raise();
    }
}

public class ActionDialogueEvent<T1> : BaseDialogueEvent {

    [XmlIgnore]
    public Action<T1> Event { get; private set; }
    public T1 Arg1 {get; private set;}

    public ActionDialogueEvent(Action<T1> callback, T1 arg1)
        : base() {
        this.Event = callback;
        this.Arg1 = arg1;
    }

    public override void RaiseEvent() {
        Event.Raise(Arg1);
    }

}

public class ActionDialogueEvent<T1, T2> : BaseDialogueEvent {

    [XmlIgnore]
    public Action<T1, T2> Event { get; private set; }
    public T1 Arg1 { get; private set; }
    public T2 Arg2 { get; private set; }

    public ActionDialogueEvent(Action<T1, T2> callback, T1 arg1, T2 arg2)
        : base() {
        this.Event = callback;
        this.Arg1 = arg1;
        this.Arg2 = arg2;
    }

    public override void RaiseEvent() {
        Event.Raise(Arg1, Arg2);
    }

}
