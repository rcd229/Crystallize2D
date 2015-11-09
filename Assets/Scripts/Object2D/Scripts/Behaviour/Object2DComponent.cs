using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class Object2DComponent : MonoBehaviour, IInitializable<Object2D> {
    public Object2D Object { get; private set; }

    public void Initialize(Object2D obj) {
        this.Object = obj;
        Debug.Log(obj);
    }
}
