using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneObjectGameData {

    public string Name { get; set; }

    public SceneObjectGameData() {
        Name = "";
    }

    public SceneObjectGameData(string s) {
        Name = s;
    }

    public GameObject GetTarget() {
        return GameObject.Find(Name);
    }

    public GameObject GetTarget(StringMap actorMap) {
        if (actorMap != null) {
            if (actorMap.ContainsKey(Name)) {
                return GameObject.Find(actorMap.Get(Name).Value);
            }
        }
        return GameObject.Find(Name);
    }

}
