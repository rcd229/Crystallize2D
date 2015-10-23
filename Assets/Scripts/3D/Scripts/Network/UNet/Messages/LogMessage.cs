using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LogMessage : MessageBase {

    public string name = "";
    public string message = "";

    public LogMessage() { }

    public LogMessage(string name, string message) {
        this.name = name;
        this.message = message;
    }

}
