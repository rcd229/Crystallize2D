using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobEditorWindow : EditorWindow  {

    [MenuItem("Crystallize/Game Data/Jobs")]
    static void Open() {
        GetWindow<JobEditorWindow>();
    }

    protected SerializableDictionary<JobID, JobGameData> Dictionary { 
        get { return GameData.Instance.Jobs; } 
    }

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
        //if (GUILayout.Button("+")) {
        //    Dictionary.AddNewItem();
        //}
        GUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    void DrawItem(JobGameData j) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorUtilities.DrawObject(j);

        EditorGUILayout.EndVertical();
    }

}
