using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectLabelManager {

    static GameObjectLabelManager _instance;

    public static GameObjectLabelManager Instance {
        get{
            if(_instance == null){
                _instance = new GameObjectLabelManager();
            }
            return _instance;
        }
    }

    public static GameObject GetGameObject(string label){
        if (!Instance.gameObjects.ContainsKey(label))
        {
            Debug.Log("Not found");
            return null;
        }
        
        var l = Instance.gameObjects[label];
        if (l.Count > 1)
        {
            Debug.LogError("More than one object labeled as: " + label);
        }
        return l[0];
    }

    public static void RegisterGameObject(string label, GameObject go)
    {
        Debug.Log("Register: " + label);
        if (!Instance.gameObjects.ContainsKey(label))
        {
            Instance.gameObjects[label] = new List<GameObject>();
        }
        Instance.gameObjects[label].Add(go);
    }

    public static void UnregisterGameObject(string label, GameObject go)
    {
        Debug.Log("Unregister: " + label);
        Instance.gameObjects[label].Remove(go);
        if (Instance.gameObjects[label].Count == 0)
        {
            Instance.gameObjects.Remove(label);
        }
    }

    Dictionary<string, List<GameObject>> gameObjects = new Dictionary<string, List<GameObject>>();

}

public class LabeledGameObject : MonoBehaviour {

    public string label;

	// Use this for initialization
	void Awake () {
        GameObjectLabelManager.RegisterGameObject(label, gameObject);
	}

    void OnDestroy()
    {
        GameObjectLabelManager.UnregisterGameObject(label, gameObject);
    }

}
