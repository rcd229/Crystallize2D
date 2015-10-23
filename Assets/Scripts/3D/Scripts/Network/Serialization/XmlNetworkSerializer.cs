using UnityEngine;
using System.Collections;
using Util.Serialization;

public class XmlNetworkSerializer<T> : INetworkSerializer<T, byte[]> where T : class {

    public byte[] Serialize(T data) {
        return new StringNetworkSerializer().Serialize(Serializer.SaveToXmlString<T>(data));
    }

    public T Deserialize(byte[] data) {
        return Serializer.LoadFromXmlString<T>(new StringNetworkSerializer().Deserialize(data));
    }

}
