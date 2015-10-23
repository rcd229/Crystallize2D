using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

public class ProximityArgs : EventArgs {
    public Rect PlayerArea { get; private set; }
    public List<Rect> NewCells { get; private set; }

    public ProximityArgs(Rect playerArea, List<Rect> cells) {
        this.PlayerArea = playerArea;
        this.NewCells = cells;
    }
}

public class PlayerProximity : BasePlayerProximity {

    static PlayerProximity _instance;
    public static bool Alive { get { return _instance; } }
    public static PlayerProximity Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("PlayerProximity").AddComponent<PlayerProximity>();
            }
            return _instance;
        }
    }

    protected override float GridSize { get { return 8f; } }
    protected override int Radius { get { return 3; } }

    public event EventHandler<ProximityArgs> ProximityChanged;

    protected override Vector2 GetPlayerPosition() {
        return PlayerManager.Instance.PlayerGameObject.transform.position.XZToVector2();
    }

    protected override void RaiseProximityChanged(HashSet<Vector2int> newCells) {
        var newRects = new List<Rect>();
        foreach (var c in newCells) {
            newRects.Add(new Rect(GridSize * c, GridSize * Vector2.one));
        }
        ProximityChanged.Raise(this, new ProximityArgs(regionRect, newRects));
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue.SetTransparency(0.5f);
        Gizmos.DrawCube(regionRect.center.ToVector3XZ(), new Vector3(regionRect.width, 0.5f, regionRect.width));
        Gizmos.DrawCube(homeRect.center.ToVector3XZ(), new Vector3(GridSize, 0.55f, GridSize));
        Gizmos.DrawCube(homeRect.center.ToVector3XZ(), new Vector3(homeRect.width, 0.6f, homeRect.height));

        Gizmos.color = Color.red.SetTransparency(0.5f);
        foreach (var c in _newCells) {
            var center = GridSize * c.ToVector2().ToVector3XZ()
                + 0.5f * GridSize * Vector3.one.ToXZ();
            Gizmos.DrawCube(center, new Vector3(GridSize, 0.55f, GridSize));
        }
    }

}
