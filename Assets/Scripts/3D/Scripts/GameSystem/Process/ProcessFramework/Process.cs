using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Process<I, O> : IProcess<I,O> {

    public virtual ProcessExitCallback OnExit { get; set; }

    public virtual void ForceExit() {
        throw new NotImplementedException();
    }

    public virtual void Initialize(I param1) {
        throw new NotImplementedException();
    }
}

public static class ProcessExt {

    public static void Link<I,O>(ref ProcessRequestHandler<I, O> requester, GetProcessInstance<I, O> getProcess) {
        requester = (s, e) => ProcessRequestHandler(getProcess, s, (ProcessRequestEventArgs<I, O>)e);
    }

    static void ProcessRequestHandler<I, O>(GetProcessInstance<I, O> getProcessInstance, object sender, ProcessRequestEventArgs<I, O> args) {
        var process = getProcessInstance();

        if (args.Parent != null) {
            ProcessExitCallback forceChildExit = (s, e) => process.ForceExit();
            ProcessExitCallback detachChild = (s, e) => {
                args.Parent.OnExit -= forceChildExit;
                Debug.Log("Detached child: " + process);
            };
            process.OnExit += detachChild;
            args.Parent.OnExit += forceChildExit;
        }

        if (args.SetChild != null) {
            args.SetChild(process);
        }

        ProcessExitCallback castCallback = (s, e) => args.Callback(s, (O)e);
        process.OnExit += castCallback;

        //Debug.Log("Started: " + process);
        //process.OnExit += (s, e) => Debug.Log("Ended: " + process);

        process.Initialize(args.Data);
    }

    public static void TryForceExit(this IProcess process) {
        if (process != null) {
            process.ForceExit();
        }
    }

    public static void ExitTo<T>(this IProcess process, IProcess parent, object key, T args) {
        var keyString = key.ToString();
        foreach (var m in process.GetType().GetMethods()) {
            foreach (var a in Attribute.GetCustomAttributes(m)) {
                var callbackAttribute = a as ProcessCallbackAttribute;
                if (callbackAttribute != null) {
                    if (callbackAttribute.Key == keyString) {
                        m.Invoke(parent, new object[]{ process, args });
                        return;
                    }
                }
            }
        }
    }

    //public static void AddListener<I, O>(this ITemporaryUI<I, O> ui, IProcess parent, EventHandler<EventArgs<O>> callback) {
        
    //}

}