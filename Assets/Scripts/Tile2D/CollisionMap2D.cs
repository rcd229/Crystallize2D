using UnityEngine;
using System.Collections.Generic;
using Util;

public class CollisionMap2D : MonoBehaviour {

    static CollisionMap2D _instance;
    public static CollisionMap2D Instance {
        get {
            if(!_instance) {
                _instance = new GameObject("CollisionMap").AddComponent<CollisionMap2D>();
            }
            return _instance;
        }
    }

    const string ColliderPrefab = "CollisionTile";
    const float TileSize = 0.5f;
    static readonly Vector2 Offset = new Vector2(-0.25f, -0.25f);

    GameObject _prefab;
    TileMap2D collisionTiles = new TileMap2D(autoSave: false);

    Dictionary<Vector2int, GameObject> resources = new Dictionary<Vector2int, GameObject>();

    GameObject GetPrefab() {
        if (!_prefab) {
            _prefab = Resources.Load<GameObject>(ColliderPrefab);
        }
        return _prefab;
    }

    public void SetVisualsEnabled(bool enabled) {
        foreach(var r in GetComponentsInChildren<Renderer>()) {
            r.enabled = enabled;
        }
    }

    public void Refresh() {
        Clear();
        //var s = "";
        //var c = 0;
        foreach(var t in collisionTiles.GetTiles()) {
            //s += t.type + ";\t";
            //if(t.type > 0) {
            //    c++;
            //}
            SetTile(t.position, t.type);
        }
        //Debug.Log(c + "\n" + s);
    }

    public void Clear() {
        foreach(var r in resources.Values) {
            GameObject.Destroy(r);
        }
        resources = new Dictionary<Vector2int, GameObject>();
    }

    public void SetValue(Vector2int pos, int val, bool update = true) {
        collisionTiles.SetValue(pos, (byte)val);

        if (update) {
            SetTile(pos, val);
        }
    }

    void SetTile(Vector2int pos, int val) {
        if (resources.ContainsKey(pos)) {
            if(val == 0) {
                GameObject.Destroy(resources[pos]);
                resources.Remove(pos);
            }
        } else {
            if(val != 0) {
                var r = GameObject.Instantiate(GetPrefab());
                resources[pos] = r;
                r.transform.position = pos.ToVector2() * TileSize + Offset;
                r.transform.parent = transform;
            }
        }
    }

}
