using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class RequestMessage : MessageBase {
    public int requestType = 0;
    public int requestID = 0;
    public byte[] payload;

    public int ConnectionID { get; set; }

    public string StringValue {
        get {
            return Encoding.UTF8.GetString(payload);
        }
    }

    public RequestMessage() { }

    public RequestMessage(ServerRequestType requestType, int requestID, byte[] data) {
        this.requestType = (int)requestType;
        this.requestID = requestID;
        this.payload = data;
    }

    public RequestMessage(ServerRequestType requestType, int requestID, string data)
        : this(requestType, requestID, Encoding.UTF8.GetBytes(data)) { }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(requestType);
        writer.Write(requestID);
        writer.WriteBytesFull(payload);
    }

    public override void Deserialize(NetworkReader reader) {
        requestType = reader.ReadInt32();
        requestID = reader.ReadInt32();
        payload = reader.ReadBytesAndSize();
    }

}