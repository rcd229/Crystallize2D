using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HomeDoor : MonoBehaviour, IInteractableSceneObject, IInteractableProcessTerminator {
    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        callback(this, null);
    }
}
