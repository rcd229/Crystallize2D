using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class InputEventManager : MonoBehaviour {
    static InputEventManager _instance;
    public static InputEventManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("InputEvents").AddComponent<InputEventManager>();
            }
            return _instance;
        }
    }

    InputEvents events = new InputEvents();
    public InputEvents Events {
        get {
            return events;
        }
    }

    
}
