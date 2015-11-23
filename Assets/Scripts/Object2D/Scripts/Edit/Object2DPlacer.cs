using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class Object2DPlacer : MonoBehaviour {

    int graphicIndex = 0;
    string currentPrefabName;
    GameObject[] graphicPrefabs;
    public static Object2DPlacer placer;

    int objectTypeIndex = 0;
    Type[] objectTypes;

    public int GraphicIndex {
        get {
            return graphicIndex;
        }
        set {
            graphicIndex = value;
            var prefab = graphicPrefabs.GetSafely(value);
            if (prefab != null) {
                currentPrefabName = prefab.name;
            }
        }
    }

    void Awake() {
        placer = this;
        graphicPrefabs = Object2DSceneResourceManager.GetGraphicPrefabs();
        GraphicIndex = 0;

        objectTypes = Object2D.GetTypes();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            GraphicIndex = (GraphicIndex + 1) % graphicPrefabs.Length;
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            objectTypeIndex = (objectTypeIndex + 1) % objectTypes.Length;
            Debug.Log("Type is: " + objectTypes[objectTypeIndex]);
        }

        if (!UIUtil.MouseOverUI()) {
            if (Input.GetMouseButtonDown(0)) {
                var pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var obj = (Object2D)Activator.CreateInstance(objectTypes[objectTypeIndex]);
                obj.PrefabName = currentPrefabName;
                obj.Position = pos;
                Object2DSceneResourceManager.Instance.Create(obj);
            }

            if (Input.GetMouseButtonDown(1)) {
                var pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var obj = Object2DSceneResourceManager.Instance.GetAt(pos);
                Object2DEditorResourceManager.Instance.OpenEditorFor(obj);
            }
        }
    }

    public void SetIndex(int index)
    {
        GraphicIndex = index;
    }

}
