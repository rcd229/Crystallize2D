using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface UIFactoryRef { }

public static class UIFactoryRefExtensions {
    public static void Initialize<I, O>(this ITemporaryUI<I, O> ui, I args, EventHandler<EventArgs<O>> callback, IProcess parent) {
        if (ui is Component) {
            if (ui is IHasCanvasBranch) {
                MainCanvas.main.Add(((Component)ui).transform, ((IHasCanvasBranch)ui).Branch);
            } else {
                MainCanvas.main.Add(((Component)ui).transform);
            }
        }

        if (ui is IDebugMethods) {
            var methods = ((IDebugMethods)ui).GetMethods().ToArray();
            DebugMenuListener.AddContextMethods(methods);
            ui.Complete += (s, e) => DebugMenuListener.RemoveContextMethods(methods);
        }

        if (callback != null) {
            EventHandler<EventArgs<O>> handler = null;

            ProcessExitCallback exitCallback = (s, e) => {
                ui.Complete -= handler;
                Debug.Log("Detaching UI callback: " + ui + "; " + parent);
            };

            handler = (s, e) => {
                if (parent != null) parent.OnExit -= exitCallback;
                callback(s, e);
                //Debug.Log("Detaching process callback: " + i + "; " + parent);
            };

            ui.Complete += handler;
            if (parent != null) {
                parent.OnExit += exitCallback;
            }
        }

        ui.Initialize(args);
    }
}

public class UIFactoryRef<I, O> : UIFactoryRef {

    public UIFactory<I, O> Factory { get; set; }

    public ITemporaryUI<I, O> Get(I args, EventHandler<EventArgs<O>> callback = null, IProcess parent = null) {
        if (Factory == null) {
            Debug.LogError(this + " has not been set!");
        }
        var i = Factory.GetInstance();
        i.Initialize(args, callback, parent);
        return i;
    }

    public void Set(Func<ITemporaryUI<I, O>> getter) {
        Factory = new UIFactory<I, O>(getter);
    }

    public void Set<T>() where T : Component, ITemporaryUI<I, O> {
        var attr = typeof(T).GetAttribute<ResourcePathAttribute>();
        UnityEngine.Debug.Assert(attr != null);

        Factory = new UIFactory<I, O>(() => GameObjectUtil.GetResourceInstance<T>(attr.ResourcePath));
    }

}