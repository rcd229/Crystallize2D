using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class Object2DComponent : MonoBehaviour {
    public Object2D Object { get; private set; }

    public void Initialize(Object2D obj) {
        this.Object = obj;

        if (obj is IHasTrigger) {
            var c = gameObject.AddComponent<BoxCollider2D>();
            c.isTrigger = true;
        }

        if (Object2DBehaviourMap.Instance.HasTypeFor(obj.GetType())) {
            gameObject.AddComponent(Object2DBehaviourMap.Instance.GetTypeFor(obj.GetType()));
        }
    }
}
