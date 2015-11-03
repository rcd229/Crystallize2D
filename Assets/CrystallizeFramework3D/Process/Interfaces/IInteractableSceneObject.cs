using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public interface IInteractableSceneObject {
    bool enabled { get; set; }
    void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent);
}
