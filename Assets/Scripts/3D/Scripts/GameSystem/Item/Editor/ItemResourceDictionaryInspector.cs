using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ItemResourceDictionary))]
public class ItemResourceDictionaryInspector : Editor {

    void OnEnable() {

    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();

        if (GUILayout.Button("Open items...")) {
            ItemEditorWindow.Open((ItemResourceDictionary)target);
        }
    }

}
