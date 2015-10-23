using UnityEngine;
using UnityEditor;
using System.Collections;

public class AreaEditorWindow : EditorWindow {

    [MenuItem("Crystallize/Areas")]
    public static void Open() {
        var window = GetWindow<AreaEditorWindow>();
        window.Initialize();
    }

    NavigationGameData navData;
    Vector2 scroll;

    void Initialize(){
        navData = GameData.Instance.NavigationData;
    }

    void OnGUI(){
        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach(var a in navData.Areas.Items){
            DrawArea(a);
        }
        if (GUILayout.Button("Add new area")) {
            navData.AddNewArea();
        }

        EditorGUILayout.EndScrollView();
    }

    void DrawArea(AreaGameData area){
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorUtilities.DrawObject(area);

        EditorGUILayout.EndVertical();
    }

}
