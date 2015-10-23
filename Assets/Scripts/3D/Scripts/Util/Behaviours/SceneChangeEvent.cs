using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneChangeEvent : MonoBehaviour {

    static SceneChangeEvent _instance;
    public static SceneChangeEvent Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("SceneChangeEvent").AddComponent<SceneChangeEvent>();
                GameObject.DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public event EventHandler SceneChanged;

    void OnLevelWasLoaded(int level) {
        Debug.Log("Scene changed to " + level);
        CoroutineManager.Instance.WaitAndDo(() => SceneChanged.Raise(this, EventArgs.Empty));
    }
}
