using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Object2DEditorBase : MonoBehaviour, IInitializable<Object2D> {
    protected Object2D Object { get; private set; }
    protected Object2DEditorControls Controls { get { return GetComponent<Object2DEditorControls>(); } }

    public void Initialize(Object2D obj) {
        Object = obj;
        ConstructEditor();
    }

    public abstract void ConstructEditor();

}
