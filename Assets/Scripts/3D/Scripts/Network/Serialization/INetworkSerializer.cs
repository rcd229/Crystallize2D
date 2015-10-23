using UnityEngine;
using System.Collections;

public interface INetworkSerializer<I, O> {
    O Serialize(I data);
    I Deserialize(O data);
}
