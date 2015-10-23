using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public abstract class NetworkRequestBase {
    protected static int _requestID = 0;

    public int RequestID { get; protected set; }

    public abstract void Run();
    public abstract void HandleResponse(RequestMessage message);
}

public class NetworkRequest<O> : NetworkRequestBase {

    NetworkClient client;
    byte channel;
    ServerRequestType requestType;
    string key;
    Action<O> callback;
    INetworkSerializer<O, string> stringSerializer;
    INetworkSerializer<O, byte[]> byteSerializer;

    NetworkRequest(NetworkClient client, byte channel, ServerRequestType requestType, string key, Action<O> callback) {
        this.client = client;
        this.channel = channel;
        this.key = key;
        this.requestType = requestType;
        this.callback = callback;
        this.RequestID = _requestID;
        _requestID++;
    }

    public NetworkRequest(NetworkClient client, byte channel, ServerRequestType requestType, string key, Action<O> callback, INetworkSerializer<O, string> serializer)
        : this(client, channel, requestType, key, callback) {
        this.stringSerializer = serializer;
    }

    public NetworkRequest(NetworkClient client, byte channel, ServerRequestType requestType, string key, Action<O> callback, INetworkSerializer<O, byte[]> serializer)
        : this(client, channel, requestType, key, callback) {
        this.byteSerializer = serializer;
    }

    public override void Run() {
        client.SendByChannel(CrystallizeMessageType.GenericRequest, new RequestMessage(requestType, RequestID, key), channel);
    }

    public override void HandleResponse(RequestMessage message) {
        if (stringSerializer != null) {
            RaiseCallback(stringSerializer.Deserialize(message.StringValue));
        } else if (byteSerializer != null) {
            RaiseCallback(byteSerializer.Deserialize(message.payload));
        } else {
            Debug.LogError("No serializer!");
        }
    }

    protected void RaiseCallback(O data) {
        callback.Raise(data);
        callback = null;
    }

}

public class NetworkRequest : NetworkRequest<string> {
    class StringSerializer : INetworkSerializer<string, string> {
        public string Serialize(string data) { return data; }
        public string Deserialize(string data) { return data; }
    }

    public NetworkRequest(NetworkClient client, byte channel, ServerRequestType requestType, string key, Action<string> callback) :
        base(client, channel, requestType, key, callback, new StringSerializer()) { }
}