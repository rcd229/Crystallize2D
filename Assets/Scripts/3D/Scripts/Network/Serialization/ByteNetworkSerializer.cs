using UnityEngine;
using System.Collections;

public class ByteNetworkSerializer : INetworkSerializer<byte[], byte[]> {
    public byte[] Serialize(byte[] data) { return data; }
    public byte[] Deserialize(byte[] data) { return data; }
}
