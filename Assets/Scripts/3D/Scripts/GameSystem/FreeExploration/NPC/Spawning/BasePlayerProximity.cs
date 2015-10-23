using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using Util;

public abstract class BasePlayerProximity : MonoBehaviour {
    const float Overflow = 0.5f;

    protected abstract float GridSize { get; }
    protected abstract int Radius { get; }

    protected HashSet<Vector2int> previousCells = new HashSet<Vector2int>();
    protected HashSet<Vector2int> _newCells = new HashSet<Vector2int>();
    protected Rect regionRect;
    protected Rect homeRect;

    protected abstract Vector2 GetPlayerPosition();
    protected abstract void RaiseProximityChanged(HashSet<Vector2int> newCells);

    void Update() {
        if (!PlayerManager.Instance.PlayerGameObject) {
            return;
        }

        if (!homeRect.Contains(GetPlayerPosition())) {
            RecalculateHome();
        }
    }

    void RecalculateHome() {
        var player = GetPlayerPosition();
        var x = Mathf.FloorToInt(player.x / GridSize);
        var y = Mathf.FloorToInt(player.y / GridSize);
        var p = new Vector2int(x, y);

        homeRect = new Rect(GridSize * p, GridSize * Vector2.one);
        homeRect = homeRect.Border(GridSize * Overflow);

        var regionX = GridSize * p.x - GridSize * Radius;
        var regionY = GridSize * p.y - GridSize * Radius;
        var size = (Radius + Radius + 1) * GridSize * Vector2.one;
        regionRect = new Rect(regionX, regionY, size.x, size.y);

        RaiseProximityChanged(GetNewCells(p));
    }

    HashSet<Vector2int> GetNewCells(Vector2int point) {
        var currentCells = new HashSet<Vector2int>();
        for (int x = point.x - Radius; x <= point.x + Radius; x++) {
            for (int y = point.y - Radius; y <= point.y + Radius; y++) {
                currentCells.Add(new Vector2int(x, y));
            }
        }
        var newCells = new HashSet<Vector2int>(currentCells);
        newCells.ExceptWith(previousCells);
        previousCells = currentCells;

        _newCells = newCells;
        return newCells;
    }
    
}
