using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DictionarySearchWindow : EditorWindow {

    [MenuItem("Crystallize/Dictionary search")]
    public static void Open() {
        //var window = 
        GetWindow<DictionarySearchWindow>();
    }

    string searchString = "";
    int id;

    void OnGUI() {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        id = EditorGUILayout.IntField("ID", id);
        if (GUILayout.Button("Search", GUILayout.Width(100f))) {
            var entries = DictionaryData.SearchDictionaryForID(id);
            DictionaryEntrySelectionWindow.Open(entries);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        searchString = EditorGUILayout.TextField("String (no kanji)", searchString);
        if (GUILayout.Button("Search", GUILayout.Width(100f))) {
            var entries = DictionaryData.SearchDictionary(searchString);
            DictionaryEntrySelectionWindow.Open(entries);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

}