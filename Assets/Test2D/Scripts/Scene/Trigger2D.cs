using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class Trigger2D : MonoBehaviour {
    public event EventHandler<EventArgs<Collider2D>> OnEntered;
    public event EventHandler<EventArgs<Collider2D>> OnExited;

    void OnTriggerEnter2D(Collider2D collider){
        OnEntered.Raise(this, new EventArgs<Collider2D>(collider));
    }

    void OnTriggerExit2D(Collider2D collider) {
        OnExited.Raise(this, new EventArgs<Collider2D>(collider));
    }
}
