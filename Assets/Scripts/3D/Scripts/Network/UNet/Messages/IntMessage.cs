using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections; 
using System.Collections.Generic;

public class IntMessage : MessageBase {

    public int val = 0;

    public IntMessage() { }

    public IntMessage(int val) {
        this.val = val;
    }

}
