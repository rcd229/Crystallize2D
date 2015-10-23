using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class KeyListener : MonoBehaviour, ITemporaryUI<KeyCode, object> {

    public static KeyListener GetInstance() {
        return new GameObject("KeyListener").AddComponent<KeyListener>();
    }

    public event EventHandler<EventArgs<object>> Complete;

    KeyCode key;

    public void Initialize(KeyCode args1) {
        this.key = args1;
    }

    public void Close() {
        Destroy(gameObject);
    }

    void Update() {
        if (Input.GetKeyDown(key)) {
            Complete.Raise(this, null);
            Close();
        }
    }

}
