using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DebugGameObject : MonoBehaviour {

    void Awake() {
        if (!GameSettings.Instance.IsDebug) {
            gameObject.SetActive(false);
        }
    }

}
