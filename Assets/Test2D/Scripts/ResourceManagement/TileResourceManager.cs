using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using Util;

public class TileResourceManager  {
    public const float GridSize = 0.32f;
    const string TileResourcePath = "2D/TileBlock";

    public static int ScreenRadius {
        get {
            var size = Mathf.Max(1f, Camera.main.aspect) * Camera.main.orthographicSize / (TileResourceManager.GridSize * TileMap2D.MapSize);
            return Mathf.CeilToInt(size);
        }
    }

    public static Vector2 GetWorldPositionFromMapPosition(Vector2int mapPosition) {
        return new Vector2(mapPosition.x, mapPosition.y) * GridSize;
    }

    public static Vector2int GetMapPositionFromWorldPosition(Vector2 worldPosition) {
        return new Vector2int(Mathf.FloorToInt((worldPosition.x + GridSize * 0.5f) / GridSize), Mathf.FloorToInt((worldPosition.y + GridSize * 0.5f) / GridSize));
    }


    static GameObject tilePrefab;
    static TileResourceManager _instance;

    public static TileResourceManager Instance {
        get {
            if (_instance == null) {
                _instance = new TileResourceManager();
            }
            return _instance;
        }
    }

    static TileResourceManager() {
        tilePrefab = Resources.Load<GameObject>(TileResourcePath);
    }

    Transform tileParent;
    Dictionary<Vector2int, Dictionary<Vector2int, GameObject>> resources = new Dictionary<Vector2int, Dictionary<Vector2int, GameObject>>();

    TileResourceManager() {
        tileParent = new GameObject("Tiles").transform;
        PlayerProximity2D.Instance.OnProximityChanged += Instance_OnProximityChanged;
    }

    void Instance_OnProximityChanged(object sender, ProximityArgs2D e) {
        Refresh();
    }

    public void AddTile(Vector2int mapPos) {
        var reducedPos = TileMap2D.GetReducedPoint(mapPos);
        if (!resources.ContainsKey(reducedPos)) {
            resources[reducedPos] = new Dictionary<Vector2int, GameObject>();
        }

        if (!resources[reducedPos].ContainsKey(mapPos)) {
            var obj = GameObject.Instantiate<GameObject>(tilePrefab);
            obj.transform.parent = tileParent;
            obj.transform.position = GetWorldPositionFromMapPosition(mapPos);
            resources[reducedPos][mapPos] = obj;
        }
    }

    public void RemoveTile(Vector2int mapPos) {
        var reducedPos = TileMap2D.GetReducedPoint(mapPos);
        if (resources.ContainsKey(reducedPos)) {
            if (resources[reducedPos].ContainsKey(mapPos)) {
                GameObject.Destroy(resources[reducedPos][mapPos]);
                resources.Remove(mapPos);
            }
        }
    }

    public void Refresh() {
        CoroutineManager.Instance.StartCoroutine(RefreshCoroutine());
    }

    IEnumerator RefreshCoroutine() {
        var newVisibleAreas = new HashSet<Vector2int>(GetVisibleAreas());
        var keys = new HashSet<Vector2int>(resources.Keys);
        keys.ExceptWith(newVisibleAreas);
        newVisibleAreas.ExceptWith(keys);

        foreach (var key in newVisibleAreas) {
            foreach (var p in TileMap2D.pathInstance.GetPositions(key)) {
                AddTile(p);
            }
            yield return null;
        }

        foreach (var key in newVisibleAreas)
        {
            foreach (var p in TileMap2D.envirInstance.GetPositions(key))
            {
                AddTile(p);
            }
            yield return null;
        }

        foreach (var key in keys) {
            foreach (var go in resources[key].Values) {
                GameObject.Destroy(go);
            }
            resources.Remove(key);
            yield return null;
        }
    }

    public IEnumerable<Vector2int> GetVisibleAreas() {
        var center = TileMap2D.GetReducedPoint(GetMapPositionFromWorldPosition(PlayerManager.Instance.PlayerGameObject.transform.position));
        var radius = ScreenRadius;

        var positions = new List<Vector2int>();
        for (int px = center.x - radius; px <= center.x + radius; px++) {
            for (int py = center.y - radius; py <= center.y + radius; py++) {
                positions.Add(new Vector2int(px, py));
            }
        }
        return positions;
    }

}
