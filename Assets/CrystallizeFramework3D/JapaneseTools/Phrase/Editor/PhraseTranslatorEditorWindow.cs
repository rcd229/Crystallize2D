using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PhraseTranslatorEditorWindow : EditorWindow {

    public static void Open(PhraseSequence phrase, Action onComplete = null) {
        var window = GetWindow<PhraseTranslatorEditorWindow>();
        window.phrase = phrase;
        window.onComplete = onComplete;
    }

    Action onComplete;
    PhraseSequence phrase;

    void OnGUI() {
        EditorGUILayout.LabelField("Text", phrase.GetText());
        phrase.Translation = EditorGUILayout.TextField("Translation", phrase.Translation);

        if (GUILayout.Button("Enter")) {
            if (onComplete != null) {
                onComplete();
            }
            Close();
        }
    }

}
