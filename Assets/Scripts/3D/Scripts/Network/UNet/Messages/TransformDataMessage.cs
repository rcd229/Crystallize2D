using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TransformDataMessage : MessageBase {
    public int connectionID = 0;
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;

    public TransformDataMessage() { }

    public TransformDataMessage(int connectionID, Vector3 position, Quaternion rotation) {
        this.connectionID = connectionID;
        this.position = position;
        this.rotation = rotation;
    }

    public TransformData GetTransformData() {
        return new TransformData(connectionID, position, rotation);
    }
}

public class TransformListDataMessage : MessageBase {

    public List<TransformData> transforms = new List<TransformData>();

    public TransformListDataMessage() {    }

    public TransformListDataMessage(List<TransformData> transforms) {
        this.transforms = transforms;
    }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(transforms.Count);
        foreach (var td in transforms) {
            writer.Write(td.ConnectionID);
            writer.Write(td.Position);
            writer.Write(td.Rotation);
        }
    }

    public override void Deserialize(NetworkReader reader) {
        var c = reader.ReadInt32();
        for (int i = 0; i < c; i++) {
            var td = new TransformData();
            td.ConnectionID = reader.ReadInt32();
            td.Position = reader.ReadVector3();
            td.Rotation = reader.ReadQuaternion();
            transforms.Add(td);
        }
    }

}
