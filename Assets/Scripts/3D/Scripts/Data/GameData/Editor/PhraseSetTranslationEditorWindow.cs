using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PhraseSetTranslationEditorWindow : EditorWindow {

    [MenuItem("Crystallize/Game Data/Translate phrases")]
    public static void Open() {
        var w = GetWindow<PhraseSetTranslationEditorWindow>();
        w.Initialize();
    }

    Vector2 scroll;

    void Initialize() {
        PhraseSetCollectionGameData.LoadAll();
    }

    void OnGUI() {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var ps in PhraseSetCollectionGameData.GetPhraseSets()) {
            foreach (var p in ps.Phrases) {
                p.Translation = EditorGUILayout.TextField(p.GetText(), p.Translation);
            }
        }

        EditorGUILayout.EndScrollView();
    }

}
