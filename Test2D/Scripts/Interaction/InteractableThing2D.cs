using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SceneThing2D))]
public class InteractableThing2D : MonoBehaviour, IInteractableSceneObject {

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        var interactions = InteractionLoader2D.Load(GetComponent<SceneThing2D>().Thing).Where(i => i.IsAvailable());
        var interaction = interactions.FirstOrDefault();
        if (interaction != null) {
            new DialogueProcess2D().Run(new DialogueProcess2DInitArgs(gameObject, interaction), callback, parent);
        }
    }
}
