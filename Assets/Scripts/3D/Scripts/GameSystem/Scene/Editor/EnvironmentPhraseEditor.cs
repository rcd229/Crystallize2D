using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;

[CustomEditor(typeof(EnvironmentPhrase))]
public class EnvironmentPhraseEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var ep = (EnvironmentPhrase)target;
        var p = ep.phrase.Get();
        p.Translation = EditorGUILayout.TextField(p.Translation);
        ep.phrase.Set(p);
    }

}
