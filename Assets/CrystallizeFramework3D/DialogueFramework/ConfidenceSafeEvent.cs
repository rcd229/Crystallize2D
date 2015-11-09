using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ConfidenceSafeEvent : IDialogueEvent {

    public static readonly ConfidenceSafeEvent Instance = new ConfidenceSafeEvent();

    public void RaiseEvent() {    }

    public int Key { get; private set; }

    public void SetKey(int key) {
        Key = key;
    }
}
