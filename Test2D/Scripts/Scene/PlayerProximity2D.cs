using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

public class ProximityArgs2D : EventArgs {
    public Rect PlayerArea { get; private set; }
    public HashSet<Vector2int> NewCells { get; private set; }

    public ProximityArgs2D(Rect playerArea, HashSet<Vector2int> cells) {
        this.PlayerArea = playerArea;
        this.NewCells = cells;
    }
}

public class PlayerProximity2D : BasePlayerProximity {
    static PlayerProximity2D _instance;
    public static PlayerProximity2D Instance {
        get {
            if (!_instance) _instance = new GameObject("PlayerProximity").AddComponent<PlayerProximity2D>();
            return _instance;
        }
    }

    public event EventHandler<ProximityArgs2D> OnProximityChanged;

    protected override float GridSize { get { return TileResourceManager.GridSize * TileMap2D.MapSize; } }
    protected override int Radius { get { return TileResourceManager.ScreenRadius; } }

    protected override Vector2 GetPlayerPosition() {
        return PlayerManager.Instance.PlayerGameObject.transform.position;
    }

    protected override void RaiseProximityChanged(HashSet<Vector2int> newCells) {
        OnProximityChanged.Raise(this, new ProximityArgs2D(homeRect, newCells));
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue.SetTransparency(0.5f);
        Gizmos.DrawCube(regionRect.center, new Vector3(regionRect.width, regionRect.width, 0.5f));
        Gizmos.DrawCube(homeRect.center, new Vector3(GridSize, GridSize, 0.55f));
        Gizmos.DrawCube(homeRect.center, new Vector3(homeRect.width, homeRect.height,  0.6f));

        Gizmos.color = Color.red.SetTransparency(0.5f);
        foreach (var c in _newCells) {
            var center = GridSize * c.ToVector2().ToVector3XZ()
                + 0.5f * GridSize * Vector3.one.ToXZ();
            Gizmos.DrawCube(center, new Vector3(GridSize, 0.55f, GridSize));
        }
    }

}
