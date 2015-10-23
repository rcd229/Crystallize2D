using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerDataMessage : MessageBase {

    public string name;
    public byte[] data;

    public PlayerDataMessage() { }

    public PlayerDataMessage(string name, byte[] data) {
        this.name = name;
        this.data = data;
    }

}
