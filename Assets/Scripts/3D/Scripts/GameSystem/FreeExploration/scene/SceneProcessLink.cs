using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneProcessLink : MonoBehaviour, IInteractableSceneObject {

    Action<ProcessExitCallback<object>, IProcess> process;

    public void Initialize(Action<ProcessExitCallback<object>, IProcess> process) {
        this.process = process;
    }

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        if (process == null) {
            callback(this, null);
        } else {
            process.Raise(callback, parent);
        }
    }

}
