using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class SceneConnectionTarget : SceneGuid<SceneData> {

    static HashSet<SceneConnectionTarget> instances = new HashSet<SceneConnectionTarget>();

    public static SceneConnectionTarget Get(SceneData scene) {
        return instances.Where(s => s.ID.guid == scene.guid).FirstOrDefault();
    }

    public override SceneData ID {
        get { return SceneData.Get(Guid); }
    }

    void OnEnable() {
        Debug.Log("Adding: " + ID.Name);
        instances.Add(this);
    }

    void OnDisable() {
        Debug.Log("Removing: " + ID.Name);
        instances.Remove(this);
    }

    #region Gizmos
    GUIStyle style;

    void OnDrawGizmos() {
        style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 18;
        style.fontStyle = FontStyle.Bold;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + transform.forward * 0.3f, 0.05f);
#if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up * .5f, ID.Name, style);
#endif
    }
    #endregion

}
