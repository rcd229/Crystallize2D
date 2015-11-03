using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class OneWayTrigger2D : MonoBehaviour {
    public Trigger2D entry;
    public Trigger2D exit;

    HashSet<Collider2D> inEntryColliders = new HashSet<Collider2D>();
    HashSet<Collider2D> inExitColliders = new HashSet<Collider2D>();

    bool inEntry { get { return inEntryColliders.Count > 0; } }
    bool inExit { get { return inExitColliders.Count > 0; } }

    public event EventHandler OnEntered;

    void Start() {
        entry.OnEntered += entry_TriggerEntered;
        entry.OnExited += entry_OnExited;
        exit.OnEntered += exit_OnEntered;
        exit.OnExited += exit_OnExited;
    }

    void exit_OnExited(object sender, EventArgs<Collider2D> e) { if (IsPlayer(e.Data)) inExitColliders.Remove(e.Data); }
    void exit_OnEntered(object sender, EventArgs<Collider2D> e) { if (IsPlayer(e.Data)) inExitColliders.Add(e.Data); }
    void entry_OnExited(object sender, EventArgs<Collider2D> e) { if (IsPlayer(e.Data)) inEntryColliders.Remove(e.Data); }

    void entry_TriggerEntered(object sender, EventArgs<Collider2D> e) {
        //Debug.Log("Entered: " + e.Data + "; " + IsPlayer(e.Data) + "; entry " + inEntry + "; exit " + inExit);
        if (IsPlayer(e.Data)) {
            if (!inEntry && !inExit) {
                Debug.Log("raised event");
                OnEntered.Raise(this, EventArgs.Empty);
            }
            inEntryColliders.Add(e.Data);
        }
    }

    bool IsPlayer(Collider2D collider) {
        if (!collider.attachedRigidbody) return false;
        return collider.attachedRigidbody.tag == TagLibrary.Player;
    }
}
