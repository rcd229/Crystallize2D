using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class Actor2D : MonoBehaviour, IInteractableSceneObject {
    public ScenePhraseSequence _phrase = new ScenePhraseSequence();

    public void BeginInteraction(ProcessExitCallback<object> callback, IProcess parent) {
        SpeechBubbleManager2D.Instance.Add(transform, _phrase.Get());
        callback.Raise(null, null);
    }
}
