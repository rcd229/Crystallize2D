﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

public class Object2DSceneResourceManager : MonoBehaviour {
    const string Object2DBasePrefab = "Object2DBase";
    const string Object2DPrefabsDirectory = "Object2DPrefabs/";

    float gridSize = 0.5f;

    string _stage {
        get {
            return Tile2DSceneResourceManager.Instance.currentLevel.levelname;
        }
    }

    static Object2DSceneResourceManager _instance;
    public static Object2DSceneResourceManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("Object2DSceneResourceManager").AddComponent<Object2DSceneResourceManager>();
            }
            return _instance;
        }
    }

    Dictionary<Vector2int, GameObject> resources = new Dictionary<Vector2int, GameObject>();

    public static GameObject[] GetGraphicPrefabs() {
        return Resources.LoadAll<GameObject>(Object2DPrefabsDirectory);
    }

    public void LoadLevel(GameLevel2D level) {
        foreach (var r in resources.Values) {
            Destroy(r);
        }
        resources = new Dictionary<Vector2int, GameObject>();

        LoadAll(level.levelname);
    }

    public void Create(Object2D obj) {
        Place(obj);
        Save(obj);
    }

    public void Place(Object2D obj) {
        var pos = GetGridPosition(obj.Position);
        DeleteAt(pos);

        resources[pos] = ConstructObject2DInstance(obj);
    }

    public Object2D GetAt(Vector2 pos) {
        return GetAt(GetGridPosition(pos));
    }

    public Object2D GetAt(Vector2int pos) {
        if (resources.ContainsKey(pos)) {
            var obj = resources[pos].GetComponent<Object2DComponent>().Object;
            return obj;
        } else {
            return null;
        }
    }

    public void Save(Object2D obj) {
        Object2DLoader.Save(_stage, obj);
    }

    public void DeleteAt(Vector2 pos) {
        DeleteAt(GetGridPosition(pos));
    }

    public void DeleteAt(Vector2int pos) {
        if (resources.ContainsKey(pos)) {
            var obj = resources[pos].GetComponent<Object2DComponent>().Object;
            Destroy(resources[pos]);
            resources.Remove(pos);
            Object2DLoader.Delete(_stage, obj);
        }
    }

    public void Delete(Object2D obj) {
        DeleteAt(obj.Position);
    }

    public void LoadAll(string level) {
        var objects = Object2DLoader.LoadAll(level);
        foreach (var obj in objects) {
            Place(obj);

            if (obj is SceneChangeTrigger2D) {
                var p = Vector2int.Snap(((Vector2)obj.Position) * 2f) + Vector2int.One;
                CollisionMap2D.Instance.SetValue(p, 0, false);
                CollisionMap2D.Instance.SetValue(p + Vector2int.Right,0, false);
                CollisionMap2D.Instance.SetValue(p + Vector2int.Up, 0, false);
                CollisionMap2D.Instance.SetValue(p + Vector2int.One, 0, false);
            }
        }
    }

    public void UnloadAll() {
        foreach (var r in resources) {
            Destroy(r.Value);
        }
        resources.Clear();
    }

    GameObject ConstructObject2DInstance(Object2D obj) {
        var baseObject = GameObjectUtil.GetResourceInstance(Object2DBasePrefab);
        baseObject.GetOrAddComponent<Object2DComponent>().Initialize(obj);
        baseObject.transform.position = GetWorldPosition(GetGridPosition(obj.Position));
        baseObject.name = string.Format("{0} ({1})", obj.Name, obj.GetType());

        var graphicObject = GameObjectUtil.GetResourceInstance(Object2DPrefabsDirectory + obj.PrefabName);
        graphicObject.transform.SetParent(baseObject.transform);
        graphicObject.transform.localPosition = Vector3.zero;

        //if (obj is IHasTrigger) {
        //    var c = baseObject.AddComponent<BoxCollider2D>();
        //    c.isTrigger = true;
        //}

        if (obj is IHasCollider) {
            graphicObject.AddComponent<BoxCollider2D>();
        }

        if (Object2DBehaviourMap.Instance.HasTypeFor(obj.GetType())) {
            baseObject.AddComponent(Object2DBehaviourMap.Instance.GetTypeFor(obj.GetType()));
        }

        foreach (var i in baseObject.GetInterfacesInChildren<IInitializable<Object2D>>()) {
            i.Initialize(obj);
        }

        return baseObject;
    }

    public Vector2 GetWorldPosition(Vector2int pos) {
        return pos.ToVector2() * gridSize;
    }

    public Vector2int GetGridPosition(Vector2 pos) {
        return Vector2int.Snap(pos / gridSize);
    }

}
