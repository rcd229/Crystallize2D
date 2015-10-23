using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class GameDataDictionaryEditorWindow<T> : EditorWindow 
    where T : IHasID, ISerializableDictionaryItem<int>, ISetableKey<int>, new() {

    protected abstract DictionaryCollectionGameData<T> Dictionary { get; }

    Vector2 scroll;

    void OnGUI() {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var j in Dictionary.Items) {
            DrawItem(j);
            if (GUILayout.Button("-")) {
                Dictionary.Remove(j.ID);
                break;
            }

            EditorGUILayout.Space();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+")) {
            Dictionary.AddNewItem();
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    void DrawItem(T j) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorUtilities.DrawObject(j);

        EditorGUILayout.EndVertical();
    }

}
