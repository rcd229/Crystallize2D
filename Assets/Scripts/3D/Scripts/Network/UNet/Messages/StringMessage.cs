using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections; 
using System.Collections.Generic;

public class StringMessage : MessageBase {

    public string text = "";

    public StringMessage() { }

    public StringMessage(string s) {
        text = s;
    }

}
