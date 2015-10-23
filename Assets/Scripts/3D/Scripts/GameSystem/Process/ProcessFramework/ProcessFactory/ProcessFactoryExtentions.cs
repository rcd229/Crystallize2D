using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

public static class ProcessFactoryExtentions {

    public static IProcess GetNested<I, O>(this ProcessFactoryRef<IProcessGetter, O> fact, ProcessFactoryRef<I, O> inner, I input, ProcessExitCallback<O> callback, IProcess parent) {
        return fact.Get<IProcessGetter, O>(
            new ProcessGetter<I, O>(inner, input),
            callback,
            parent);
    }

    public static IProcess GetNullOutput<I, O>(this ProcessFactoryRef<I, O> fact, I input, ProcessExitCallback callback, IProcess parent) {
        return fact.Get<I, O>(input, (s, e) => callback(s, e), parent);
    }

    static void Run_Internal<I, O>(this IProcess<I> process, I input, ProcessExitCallback<O> callback, IProcess parent) {
        if (process is ISettableParent) {
            ((ISettableParent)process).SetParent(parent);
        }

        ProcessExitCallback castCallback = null;
        if (parent != null) {
            ProcessExitCallback forceChildExit = (s, e) => {
                process.OnExit -= castCallback;
                process.ForceExit();
            };
            ProcessExitCallback detachChild = (s, e) => parent.OnExit -= forceChildExit;
            process.OnExit += detachChild;
            process.OnExit += (s, e) => RemoveAllEvents(process);
            parent.OnExit += forceChildExit;
        }

        if (callback != null) {
            castCallback = (s, e) => callback(s, (O)e);
            process.OnExit += castCallback;
        }

        if (process is IDebugMethods) {
            var methods = ((IDebugMethods)process).GetMethods().ToArray();
            DebugMenuListener.AddContextMethods(methods);
            process.OnExit += (s, e) => DebugMenuListener.RemoveContextMethods(methods);
        }

        process.Initialize(input);
    }

    public static void Run<I, O>(this IProcess<I, O> process, I input, ProcessExitCallback<O> callback, IProcess parent) {
        Run_Internal<I, O>(process, input, callback, parent);
    }

    public static IProcess Get<I, O>(this ProcessFactoryRef<I, O> fact, I input, ProcessExitCallback<O> callback, IProcess parent) {
        if (fact == null) {
            Debug.LogError(string.Format("Factory reference has not been intialized. ({0})", fact));
            return null;
        }

        IProcess<I> process = null;
        if (fact == null) {
            Debug.LogWarning(string.Format("Factory instance has not been intialized. Using temporary process. ({0})", fact));
            process = new TempProcess<I, O>();
        } else {
            process = fact.Factory.GetInstance(input, parent);
        }

        if (fact.Factory is IProcessModifier<I>) {
            ((IProcessModifier<I>)fact.Factory).ModifyProcess(process);
        }

        process.Run_Internal(input, callback, parent);
        return process;
        //if (process is ISettableParent) {
        //    ((ISettableParent)process).SetParent(parent);
        //}

        //ProcessExitCallback castCallback = null;
        //if (parent != null) {
        //    ProcessExitCallback forceChildExit = (s, e) => {
        //        process.OnExit -= castCallback;
        //        process.ForceExit();
        //    };
        //    ProcessExitCallback detachChild = (s, e) => parent.OnExit -= forceChildExit;
        //    process.OnExit += detachChild;
        //    process.OnExit += (s, e) => RemoveAllEvents(process);
        //    parent.OnExit += forceChildExit;
        //}

        //if (callback != null) {
        //    castCallback = (s, e) => callback(s, (O)e);
        //    process.OnExit += castCallback;
        //}

        //if (process is IDebugMethods) {
        //    var methods = ((IDebugMethods)process).GetMethods().ToArray();
        //    DebugMenuListener.AddContextMethods(methods);
        //    process.OnExit += (s, e) => DebugMenuListener.RemoveContextMethods(methods);
        //}

        ////CoroutineManager.Instance.WaitAndDo(() => 
        //process.Initialize(input);
        //return process;
    }

    public static IProcessGetter Getter<I, O>(this ProcessFactoryRef<I, O> fact, I input) {
        return new ProcessGetter<I, O>(fact, input);
    }
    //public static IProcess Get<I>(this ProcessFactoryRef fact, I input, ProcessExitCallback<I> callback, IProcess parent) {
    //    return Get<I, I>(fact, input, callback, parent);
    //}

    static void RemoveAllEvents(IProcess process) {
        process.OnExit = null;
        //var e = process.GetType().GetEvent("OnExit");
        //Debug.Log(e);
        //var f = process.GetType().GetProperty("OnExit", BindingFlags.NonPublic | BindingFlags.Instance);
        //Debug.Log(f);
        //var eh = f.GetValue(process, new object[0]) as EventHandler;

        //foreach (Delegate del in eh.GetInvocationList()) {
        //    e.GetRemoveMethod().Invoke(process, new object[] { del });
        //}
    }

}
