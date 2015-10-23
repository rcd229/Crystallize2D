using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneDresser : MonoBehaviour, IInteractableSceneObject {

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        UILibrary.Equipment.Get(null, (s, e) => callback(s, e), parent);
    }
}
