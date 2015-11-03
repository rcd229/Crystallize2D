using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Object2DEditorResourceManager: MonoBehaviour {
    const string Object2DEditorBasePrefab = "Object2DEditorBase";
    const string Object2DPrefabsDirectory = "Object2DPrefabs/";

    static Object2DEditorResourceManager _instance;
    public static Object2DEditorResourceManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("Object2DEditorResourceManager").AddComponent<Object2DEditorResourceManager>();
            }
            return _instance;
        }
    }

    GameObject current;

    public void OpenEditorFor(Object2D obj) {
        if (current) {
            Destroy(current);
        }

        if(obj != null) {
            var editor = GameObjectUtil.GetResourceInstance(Object2DEditorBasePrefab);

            if (Object2DEditorMap.Instance.HasTypeFor(obj.GetType())) {
                editor.AddComponent(Object2DEditorMap.Instance.GetTypeFor(obj.GetType()));
            }

            foreach(var initObj in editor.GetInterfacesInChildren<IInitializable<Object2D>>()) {
                initObj.Initialize(obj);
            }

            Debug.Log(MainCanvas.main);
            Debug.Log(editor);
            MainCanvas.main.Add(editor.transform);

            current = editor;
        }
    }
}