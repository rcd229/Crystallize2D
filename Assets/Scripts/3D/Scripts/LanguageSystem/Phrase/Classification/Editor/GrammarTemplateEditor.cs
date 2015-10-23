using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GrammarTemplateEditor : EditorWindow {

    [MenuItem("Crystallize/Grammar templates")]
    public static void Open() {
        var window = GetWindow<GrammarTemplateEditor>();
        window.Initialize();
    }

    Vector2 scroll;
    List<PhraseSequence> templates;

    void Initialize() {
        //templates = GameData.Instance.PhraseClassData.GrammarTemplates;
    }

    void OnGUI() {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var t in templates) {
            if (GUILayout.Button(t.GetText(JapaneseTools.JapaneseScriptType.Kanji))) {
                PhraseEditorWindow.Open(t);
            }
        }

        if (GUILayout.Button("Add...")) {
            templates.Add(new PhraseSequence());
        }

        EditorGUILayout.EndScrollView();
    }

}
