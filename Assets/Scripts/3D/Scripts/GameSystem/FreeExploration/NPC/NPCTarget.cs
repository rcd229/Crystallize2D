using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NPCTarget : MonoBehaviour {

    static Dictionary<Guid, NPCTarget> targets = new Dictionary<Guid, NPCTarget>();
    public static NPCTarget Get(Guid name) {
        if (targets.ContainsKey(name)) {
            return targets[name];
        }
        return null;
    }

    public GameObject NPC { get; set; }

    public bool Spawned {
        get { return NPC; }
    }

    [SerializeField]
    string Guid;

    Guid dictionaryID;

    void Start() {
        gameObject.tag = TagLibrary.NPCTarget;
    }

    void OnEnable() {
        dictionaryID = new Guid(Guid);
        if (targets.ContainsKey(dictionaryID)) {
            Debug.LogError("NPC with name: " + dictionaryID + " already exists.");
        }
        targets[dictionaryID] = this;
    }

    void OnDisable() {
        if (targets.ContainsKey(dictionaryID)) {
            targets.Remove(dictionaryID);
        }
    }

    #region Gizmos
    GUIStyle style;

#if UNITY_EDITOR
    public void SetGuid(Guid guid) {
        this.Guid = guid.ToString();
        OnDisable();
        OnEnable();
    }
#endif

    void OnDrawGizmos() {
        style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 18;
        style.fontStyle = FontStyle.Bold;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + transform.forward * 0.3f, 0.05f);
#if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up * .5f, name, style);
#endif
    }
    #endregion
}
