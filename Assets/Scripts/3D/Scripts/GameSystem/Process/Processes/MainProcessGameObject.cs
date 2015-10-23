using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MainProcessGameObject : MonoBehaviour {

    void Awake() {
        gameObject.SetActive(!ProcessInitializer.Running);
    }

}
