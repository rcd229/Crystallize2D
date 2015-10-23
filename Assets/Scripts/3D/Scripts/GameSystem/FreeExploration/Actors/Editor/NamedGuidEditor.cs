using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public abstract class NamedGuidEditor<T> : Editor where T : UniqueID {

    int index = 0;
    List<NamedGuid> npcs = new List<NamedGuid>();
    string[] names = new string[0];

    protected abstract string ValueLabel { get; }

    protected virtual IEnumerable<NamedGuid> IDs { get { return NamedGuid.GetIDs<T>(); } }

    SceneGuid<T> Target { get { return (SceneGuid<T>)target; } }

    void OnEnable() {
        npcs = new List<NamedGuid>(IDs);
        names = npcs.Select(nm => nm.Name).ToArray();
        var guid = Target.Guid;
        if (NamedGuid.ContainsID<T>(guid)) {
            for(int i = 0; i < npcs.Count; i++){
                if(npcs[i].Guid == guid){
                    index= i;
                    break;
                }
            }
        }
    }

    public override void OnInspectorGUI() {
        if (names.Length > 0) {
            index = EditorGUILayout.Popup("Value", index, names);
            EditorGUILayout.LabelField("Guid", npcs[index].Guid.ToString());
            var oldGuid = Target.Guid;
            if (oldGuid != npcs[index].Guid) {
                ((SceneGuid<T>)target).Guid = npcs[index].Guid;
                EditorUtility.SetDirty(target);
            }
        }
    }

}
