using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class FloatingNameManager2D {

    static FloatingNameManager2D _instance;
    public static FloatingNameManager2D Instance {
        get {
            if (_instance == null) {
                _instance = new FloatingNameManager2D();
            }
            return _instance;
        }
    }

    Dictionary<Transform, FloatingName2D> names = new Dictionary<Transform, FloatingName2D>();

    public void Add(Transform target, PhraseSequence phrase) {
        if (names.ContainsKey(target)) {
            GameObject.Destroy(names[target].gameObject);
            names.Remove(target);
        }

        var instance = GameObjectUtil.GetResourceInstanceFromAttribute<FloatingName2D>();
        instance.Initialize(target);
        instance.Initialize(phrase);
        names[target] = instance;
    }

}
