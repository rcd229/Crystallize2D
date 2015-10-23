using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneReview : MonoBehaviour, IInteractableSceneObject {

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        ProcessLibrary.Review.Get(null, callback, parent);
    }

}
