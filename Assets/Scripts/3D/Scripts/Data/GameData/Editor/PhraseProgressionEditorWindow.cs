using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PhraseProgressionEditorWindow : EditorWindow {

    [MenuItem("Crystallize/Phrase progression")]
    public static void Open() {
        var window = GetWindow<PhraseProgressionEditorWindow>();
        window.Initialize();
    }

    Vector2 scrollPosition;
    //Dictionary<DialogueActorLine, List<DialogueActorLine>> playerLines = new Dictionary<DialogueActorLine, List<DialogueActorLine>>();

    PlayerActorLine selectedLine;

    void Initialize() {
        //playerLines = EditorUtilities.GetAggregatedPlayerLines();
    }

    void OnGUI() {
        EditorGUILayout.BeginHorizontal();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        //foreach (var l in playerLines.Keys) {
        //    if (GUILayout.Button(GetPhraseString(l.Phrase))) {
        //        selectedLine = l as PlayerActorLine;
        //        //PhraseEditorWindow.Open(l.Phrase);
        //    }
        //}

        EditorGUILayout.EndScrollView();

        if (selectedLine != null) {
            DrawProgressions();
        }

        EditorGUILayout.EndHorizontal();
    }

    string GetPhraseString(PhraseSequence phrase) {
        var s = "";
        foreach (var pe in phrase.PhraseElements) {
            s += string.Format("{0}({1}) ", pe.GetText(), pe.WordID);
        }
        return s;
    }

    void DrawProgressions() {
        EditorGUILayout.BeginVertical();

        var p = GameData.Instance.ProgressionData.PhraseProgression.GetOrCreateProgression(selectedLine.Phrase);
        for (int i = 0; i < p.Steps; i++) {
            DrawMissingWords(p, i);
        }

        if (GUILayout.Button("Add step")) {
            p.AddStep();
        }

        EditorGUILayout.EndVertical();
    }

    void DrawMissingWords(PhraseProgression prog, int step) {
        GUILayout.BeginHorizontal();
        for (int i = 0; i < prog.Phrase.PhraseElements.Count; i++) {
            var w = prog.Phrase.PhraseElements[i];
            var s = w.GetText();
            var missing = prog.GetWordMissing(step, i);
            if (missing) {
                s = "{" + s + "}";
            }

            if (GUILayout.Button(s)) {
                prog.SetWordMissing(step, i, !missing);
            }
        }
        GUILayout.EndHorizontal();
    }

}
