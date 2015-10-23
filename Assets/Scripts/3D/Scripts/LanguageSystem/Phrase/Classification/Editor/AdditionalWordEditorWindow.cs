using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AdditionalWordEditorWindow : EditorWindow {

    const int StartID = 10000000;

    DictionaryDataEntry selectedEntry;
    List<DictionaryDataEntry> entries;

    [MenuItem("Crystallize/Dictionary/New word")]
    public static void Open() {
        var w = GetWindow<AdditionalWordEditorWindow>();
        w.Initialize();
    }

    void Initialize() {
        entries = new List<DictionaryDataEntry>();
        for (int i = DictionaryData.AdditionalEntryStartID; i < DictionaryData.AdditionalEntryStartID + 10000; i += 10) {
            var e = DictionaryData.Instance.GetEntryFromID(i);
            /*if(e == null){
                break;
            }*/
            if (e != null) {
                entries.Add(e);
            }
        }
    }

    void OnGUI() {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        foreach (var e in entries) {
            if (GUILayout.Button(string.Format("[{0}] {1} ({2})", e.ID, e.Kanji, e.GetPreferredTranslation()))) {
                selectedEntry = e;
            }
        }

        if (GUILayout.Button("Add...")) {
            AddNewWord();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        if (selectedEntry != null) {
            EditorUtilities.DrawObject(selectedEntry);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    void AddNewWord() {
        selectedEntry = DictionaryData.Instance.AddNewEntry();
        Initialize();
    }

}
