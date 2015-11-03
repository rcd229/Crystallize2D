using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

[CustomPropertyDrawer(typeof(ScenePhraseSequence))]
public class ScenePhraseSequencePropertyDrawer : PropertyDrawer {

    PhraseSequence phrase;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        var obj = property.serializedObject.targetObject.GetType().GetField(property.name);
        var sps = obj.GetValue(property.serializedObject.targetObject) as ScenePhraseSequence;
        if (sps.CachedText == null) {
            sps.CachedText = sps.Get().GetText();
        }

        if (GUI.Button(position, sps.CachedText)) {
            if (Event.current.button == 0) {
                phrase = sps.Get();
                PhraseEditorWindow.Open(phrase, () => SetValue(property));
            } else {
                phrase = sps.Get();
                PhraseTranslatorEditorWindow.Open(phrase, () => SetValue(property));
            }
        }

        EditorGUI.EndProperty();
    }

    void SetValue(SerializedProperty property) {
        var obj = property.serializedObject.targetObject.GetType().GetField(property.name);
        var sps = obj.GetValue(property.serializedObject.targetObject) as ScenePhraseSequence;
        sps.Set(phrase);
        EditorUtility.SetDirty(property.serializedObject.targetObject);
        phrase = null;
    }

}
