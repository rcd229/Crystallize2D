using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SceneInputHandler2D : MonoBehaviour {
    static SceneInputHandler2D _instance;
    public static SceneInputHandler2D Instance {
        get {
            if(!_instance) {
                _instance = new GameObject("SceneInputHandler").AddComponent<SceneInputHandler2D>();
                TransformPath.Add(_instance.transform, "UI", "Input");
            }
            return _instance;
        }
    }

    public event EventHandler<EventArgs<GameObject>> OnObjectClicked;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit.transform && IsTarget(hit.transform.gameObject)) {
                OnObjectClicked.Raise(this, new EventArgs<GameObject>(hit.transform.gameObject));
            }
        }
    }

    bool IsTarget(GameObject target) {
        return target.GetInterface<IInteractableSceneObject>() != null;
    }
}
