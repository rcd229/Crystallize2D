using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Util.Serialization;

public class DialogueSequenceEditorWindow : EditorWindow {

    public static void Open(DialogueSequence dialogue) {
        var window = GetWindow<DialogueSequenceEditorWindow>();
        window.Initialize(dialogue);
    }

    public static void Open(string initial, Action<string> setString)
    {
        var window = GetWindow<DialogueSequenceEditorWindow>();
        window.Initialize(initial, setString);
    }

    DialogueSequence dialogue;
    Action<string> setString;

    List<Type> newElementTypes;
    string[] newElementTypeStrings;

    string[] elementStrings;
    int[] elementIDs;

    void Initialize(DialogueSequence dialogue) {
        this.dialogue = dialogue;
        setString = null;
        GetElementList();

        newElementTypes = (from t in Assembly.GetAssembly(typeof(DialogueSequence)).GetTypes()
                           where t.IsSubclassOf(typeof(DialogueElement)) select t).ToList();
        newElementTypes.Insert(0, typeof(object));
        newElementTypeStrings = newElementTypes.Select((e) => e.ToString()).ToArray();
        newElementTypeStrings[0] = "Null";
    }

    void Initialize(string xmlString, Action<string> setString)
    {
        if (xmlString != "")
        {
            try
            {
                dialogue = Serializer.LoadFromXmlString<DialogueSequence>(xmlString);
            }
            catch
            {

            }
        }

        if (dialogue == null)
        {
            dialogue = new DialogueSequence();
        }

        this.setString = setString;

        GetElementList();
    }

    void GetElementList()
    {
        elementStrings = new string[dialogue.Elements.Items.Count + 1];
        elementIDs = new int[dialogue.Elements.Items.Count + 1];

        var names = dialogue.Elements.Items.Select((e) => string.Format("[{0}] {1}", e.ID, e.ToString())).ToArray();
        var ids = dialogue.Elements.Items.Select((e) => e.ID).ToArray();

        elementStrings[0] = "NULL";
        System.Array.Copy(names, 0, elementStrings, 1, names.Length);

        elementIDs[0] = -1;
        System.Array.Copy(ids, 0, elementIDs, 1, ids.Length);
    }

    void OnGUI()
    {
        EditorUtilities.DrawProperty(dialogue, typeof(DialogueSequence).GetProperty("Actors"));

        foreach (var e in dialogue.Elements.Items)
        {
            DrawElement(e);

            e.ActorIndex = EditorGUILayout.Popup("Actor", e.ActorIndex, GetPersonChoices());

            if (GUILayout.Button("-")) {
                dialogue.Elements.Remove(e.ID);
                break;
            }
        }

        var selected = EditorGUILayout.Popup("Add element", 0, newElementTypeStrings);
        if (selected != 0) {
            dialogue.GetNewDialogueElement(newElementTypes[selected]);
            GetElementList();
        }
        /*if (GUILayout.Button("Add element"))
        {
            dialogue.GetNewDialogueElement();
            GetElementList();
        }*/

        if (GUILayout.Button("Save"))
        {
            Debug.Log("Set string");
            if (setString != null) {
                setString(Serializer.SaveToXmlString<DialogueSequence>(dialogue));
            }
        }

        EditorGUILayout.LabelField(Serializer.SaveToXmlString<DialogueSequence>(dialogue));
    }

    void DrawElement(DialogueElement element)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorUtilities.DrawObject(element);

        EditorGUILayout.EndVertical();
    }

    string[] GetPersonChoices() {
        if (dialogue.Actors.Count == 0) {
            return new string[] { "default" };
        }
        return dialogue.Actors.Select((e) => e.Name).ToArray();
    }

    int GetIndex(int id)
    {
        for (int i = 0; i < elementIDs.Length; i++)
        {
            if (elementIDs[i] == id)
            {
                return i;
            }
        }
        return -1;
    }

    int GetID(string label, int originalID)
    {
        var index = GetIndex(originalID);
        var newIndex = EditorGUILayout.Popup(label, index, elementStrings);
        return elementIDs[newIndex];
    }

}
