using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

[CustomPropertyDrawer(typeof(DialogueSequenceUnityXml))]
public class DialogueSequencePropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (GUI.Button(position, label.text)) 
        {
            DialogueSequenceEditorWindow.Open(property.FindPropertyRelative("xmlString").stringValue, 
                (s) => SetValue(property, s));
        }

        EditorGUI.EndProperty();
    }

    void SetValue(SerializedProperty property, string val)
    {
        var f = property.serializedObject.targetObject.GetType().GetField(property.name);
        ((DialogueSequenceUnityXml)f.GetValue(property.serializedObject.targetObject)).xmlString = val;
        EditorUtility.SetDirty(property.serializedObject.targetObject);
    }

}
