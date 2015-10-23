using UnityEngine;
using UnityEditor;
using System.Collections;
using JapaneseTools;

[CustomEditor(typeof(KanaConverterTest))]
public class KanaConverterTestEditor : Editor {

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		var tester = (KanaConverterTest)target;
		EditorGUILayout.LabelField ("Romaji:\t" + KanaConverter.Instance.ConvertToRomaji (tester.text));
		EditorGUILayout.LabelField ("Hiragana:\t" + KanaConverter.Instance.ConvertToHiragana (tester.text));
		EditorGUILayout.LabelField ("Katakana:\t" + KanaConverter.Instance.ConvertToKatakana (tester.text));
	}

}
