using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TransformData {
    public int ConnectionID { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public TransformData() { }

    public TransformData(int connectionID, Vector3 position, Quaternion rotation) {
        this.ConnectionID = connectionID;
        this.Position = position;
        this.Rotation = rotation;
    }

    //public void SetNextTransform(int connectionID, Vector3 nextPosition, Quaternion nextRotation) {
    //    ConnectionID = connectionID;
    //    Position = nextPosition;
    //    Rotation = nextRotation;
    //}
}

public class PlayerTransformTable {

    Dictionary<int, TransformData> playerTransforms = new Dictionary<int, TransformData>();

    public IEnumerable<TransformData> Transforms { get { return playerTransforms.Values; } }

    public List<TransformData> GetTransforms() {
        return new List<TransformData>(playerTransforms.Values);
    }

    public void Remove(int playerID) {
        playerTransforms.Remove(playerID);
    }

    public void Update(TransformData data) {
        playerTransforms[data.ConnectionID] = data;
    }

    public void Update(IEnumerable<TransformData> data) {
        playerTransforms.Clear();
        foreach (var td in data) {
            playerTransforms[td.ConnectionID] = td;
        }
    }

}
