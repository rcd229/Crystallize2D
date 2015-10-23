using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneObjectRef {

    SceneObjectGameData obj;

    public SceneObjectRef(SceneObjectGameData obj) {
        this.obj = obj;
    }

    public GameObject GetSceneObject() {
        return GameObject.Find(obj.Name);
    }

}
