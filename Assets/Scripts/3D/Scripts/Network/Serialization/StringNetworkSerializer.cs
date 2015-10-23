using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class StringNetworkSerializer : INetworkSerializer<string, byte[]> {

    public static readonly StringNetworkSerializer Instance = new StringNetworkSerializer();

    public byte[] Serialize(string data) {
        return System.Text.Encoding.UTF8.GetBytes(data);
    }

    public string Deserialize(byte[] data) {
        return System.Text.Encoding.UTF8.GetString(data);
    }

}
