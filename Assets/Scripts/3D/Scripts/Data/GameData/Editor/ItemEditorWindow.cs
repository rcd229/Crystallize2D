using UnityEngine;
using UnityEditor;
using System.Collections;

public class ItemEditorWindow : EditorWindow {

    [MenuItem("Crystallize/Items")]
    public static void Open() {
        //var window = 
        GetWindow<ItemEditorWindow>();
    }

    public static void Open(ItemResourceDictionary dict) {
        var window = GetWindow<ItemEditorWindow>();

        window.Initialize(dict);
    }

    ItemResourceDictionary resourceDictionary;

    void Initialize(ItemResourceDictionary dict) {
        resourceDictionary = dict;
    }

    void OnGUI() {
        EditorGUILayout.BeginVertical();

        foreach (var i in GameData.Instance.TradeData.Items.Items) {
            DrawItem(i);
        }

        if (GUILayout.Button("Add item")) {
            GameData.Instance.TradeData.AddNewItem();
        }

        EditorGUILayout.EndVertical();
    }

    void DrawItem(ItemGameData item) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField(item.ItemID.ToString());

        if (GUILayout.Button(item.Name.GetText())) {

        }

        if (resourceDictionary) {
            var r = resourceDictionary.GetOrCreateItemResources(item.ItemID);
            r.icon = EditorGUILayout.ObjectField("Icon", r.icon, typeof(Sprite), false) as Sprite;
            EditorUtility.SetDirty(resourceDictionary);
        }

        EditorGUILayout.EndVertical();
    }

}
