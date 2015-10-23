
using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class GameObjectArgs : EventArgs {

    public GameObject Target { get; set; }

    public GameObjectArgs(GameObject target) {
        Target = target;
    }

}
