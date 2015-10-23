using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class RPCRequest {
    public int RequestID { get; protected set; }

    public RPCRequest(int requestID) {
        this.RequestID = requestID;
    }

    public abstract void DoResponse(byte[] data);
}

public class RPCRequest<O> : RPCRequest {

    INetworkSerializer<O, byte[]> serializer;
    Action<O> action;

    public RPCRequest(int requestID, INetworkSerializer<O, byte[]> serializer, Action<O> action) : base(requestID) {
        this.serializer = serializer;
        this.action = action;
    }

    public override void DoResponse(byte[] data) {
        var outData = serializer.Deserialize(data);
        action(outData);
    }

}
