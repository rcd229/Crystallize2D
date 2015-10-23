using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneInteractableObject : MonoBehaviour, IInteractableSceneObject {

    public Action<ProcessExitCallback<object>, IProcess> HandleInteraction { get; set; }

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        if (HandleInteraction == null) {
            callback(this, null);
        } else {
            HandleInteraction(callback, parent);
        }
    }
}
