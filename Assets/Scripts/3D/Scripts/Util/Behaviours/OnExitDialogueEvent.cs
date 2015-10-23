using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class OnExitDialogueEvent : MonoBehaviour {
    public static void Raise(GameObject target) {
        var e = target.GetComponent<OnExitDialogueEvent>();
        if (e) {
            e.Raise();
        }
    }

    public event EventHandler OnExitDialogue;

    public void Raise() {
        OnExitDialogue.Raise(this, EventArgs.Empty);
        OnExitDialogue = null;
    }
}
