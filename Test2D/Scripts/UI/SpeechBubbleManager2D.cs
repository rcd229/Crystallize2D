using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SpeechBubbleManager2D {
    static SpeechBubbleManager2D _instance;
    public static SpeechBubbleManager2D Instance {
        get {
            if (_instance == null) {
                _instance = new SpeechBubbleManager2D();
            }
            return _instance;
        }
    }

    Dictionary<Transform, SpeechBubble2D> instances = new Dictionary<Transform, SpeechBubble2D>();

    public void Add(Transform target, PhraseSequence phrase) {
        Remove(target);
        var instance = GameObjectUtil.GetResourceInstanceFromAttribute<SpeechBubble2D>();
        instance.Initialize(target, phrase);
        instances[target] = instance;
    }

    public void Remove(Transform target) {
        if (instances.ContainsKey(target)) {
            GameObject.Destroy(instances[target].gameObject);
        }
        instances.Remove(target);
    }
}
