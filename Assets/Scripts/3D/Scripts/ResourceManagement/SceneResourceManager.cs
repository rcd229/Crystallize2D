using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneResourceManager : MonoBehaviour {

    static SceneResourceManager _instance;
    public static SceneResourceManager Instance {
        get {
            if (!_instance) {
                Debug.Log("Making new scene resources.");
                _instance = new GameObject("SceneResourceManager").AddComponent<SceneResourceManager>();
            }
            return _instance;
        }
    }

    Dictionary<Guid, List<GameObject>> objectResources = new Dictionary<Guid, List<GameObject>>();

    public void SetResources(Guid key, IEnumerable<GameObject> resources) {
        var newRes = new HashSet<GameObject>();
        if (resources != null) {
            newRes.UnionWith(resources);
        }

        if (objectResources.ContainsKey(key) && objectResources[key] != null) {
            foreach (var obj in objectResources[key]) {
                if (newRes.Contains(obj)) {
                    Debug.Log("Reusing resource: " + obj);
                } else {
                    Destroy(obj);
                }
            }
            objectResources[key].Clear();
        }

        if (resources == null) {
            objectResources.Remove(key);
        } else {
            objectResources[key] = new List<GameObject>(resources);
        }
    }

}
