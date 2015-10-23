using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DestroyEvent : MonoBehaviour {

    public static DestroyEvent Get(GameObject obj) {
        if (obj.GetComponent<DestroyEvent>()) {
            return obj.GetComponent<DestroyEvent>();
        }
        return obj.AddComponent<DestroyEvent>();
    }

    public event EventHandler<GameObjectArgs> Destroyed;

    bool closing = false;

    void OnDestroy() {
        if (!closing) {
            Destroyed.Raise(this, new GameObjectArgs(gameObject));
            Destroyed = null;
        }
    }

    void OnApplicationQuit() {
        closing = true;
    }

}
