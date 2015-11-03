using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using JapaneseTools;

public class DictionaryEntrySelectionWindow : EditorWindow {

    static GUIStyle style;

	public static void Open(List<DictionaryDataEntry> entries){
		var window = GetWindow<DictionaryEntrySelectionWindow>();

        style = new GUIStyle(GUI.skin.button);
        style.alignment = TextAnchor.MiddleLeft;
		window.entries = entries;
        window.filterText = "";
		foreach (var e in entries) {
			window.entryStrings[e] = string.Format("[{0}] {1} ({2}) {3}", e.ID, e.Kanji, e.Kana, 
                e.EnglishSummary.Truncate(30));
		}
	}

	Vector2 scrollPosition;
	List<DictionaryDataEntry> entries = new List<DictionaryDataEntry>();
	Dictionary<DictionaryDataEntry, string> entryStrings = new Dictionary<DictionaryDataEntry, string>();
    string filterText = "";

	void OnGUI(){
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);
		EditorGUILayout.BeginVertical ();

        filterText = EditorGUILayout.TextField("Filter", filterText);

        var sortedEntries = from e in entries
                            where e.English.Contains(filterText) || KanaConverter.Instance.ConvertToRomaji(e.Kana).Contains(filterText)
                            orderby e.Kana.Length, e.Kana
                            select e;

		foreach (var entry in sortedEntries) {
			if(GUILayout.Button(entryStrings[entry])){
				DictionaryData.Instance.UpdateEntry(entry);
			}
		}

		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndScrollView ();
	}

}
