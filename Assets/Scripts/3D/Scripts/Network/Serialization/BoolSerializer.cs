using UnityEngine;
using System;
using System.Collections;

public class BoolSerializer : INetworkSerializer<bool, byte[]> {
    public byte[] Serialize(bool data) { return new byte[]{ Convert.ToByte(data) }; }
    public bool Deserialize(byte[] data) { return Convert.ToBoolean(data[0]); }
}
