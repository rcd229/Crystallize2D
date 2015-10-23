using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WordDataEditorWindow : EditorWindow {

	[MenuItem("Crystallize/Word data editor")]
	public static void Open(){
		var window = GetWindow<WordDataEditorWindow>();
		
		window.Initiallize ();
	}

    //ReorderableList functionList;
    //ReorderableList parameterList;
    //ReorderableList tagList;
	GameData gameData;

	string filterText = "";
	string lastFilterText = "";
	DictionaryDataEntry activeEntry;
	AuxiliaryDictionaryDataEntry activeAuxEntry;
	Vector2 tagScrollPosition;
	Vector2 scrollPosition;
	List<DictionaryDataEntry> searchedEntries = new List<DictionaryDataEntry>();

    string[] itemStrings;
    List<int> itemIDs;

	void Initiallize(){
		gameData = GameData.Instance;
        //functionList = GetReorderableList (gameData.PhraseClassData.PhraseFunctions, "Phrase functions");
        //parameterList = GetReorderableList (gameData.PhraseClassData.PhraseParameters, "Phrase parameters");
        //tagList = GetReorderableList (gameData.PhraseClassData.Tags, "Tags");

        var items = (from i in gameData.TradeData.Items.Items select i);
        itemStrings = (from i in items select i.Name.GetText()).ToArray();
        itemIDs = (from i in items select i.ItemID).ToList();
	}

	ReorderableList GetReorderableList(List<string> list, string title){
		var uiList = new ReorderableList (list, typeof(string));
		uiList.drawHeaderCallback = (a) => DrawHeaderCallback(title, a);
		uiList.onAddCallback = OnAddCallback;
		uiList.drawElementCallback = (a, b, c, d) => DrawElementCallback(list, a, b, c, d);
		return uiList;
	}

	void DrawHeaderCallback(string title, Rect rect){
		EditorGUI.LabelField (rect, title);
	}

	void OnAddCallback(ReorderableList list){
		list.list.Add ("NEW ITEM");
	}

	void DrawElementCallback(List<string> list, Rect rect, int index, bool isActive, bool isFocused){
		EditorGUI.LabelField (rect, index.ToString ());
		rect.x += 20f;
		rect.width -= 20f;
		list [index] = EditorGUI.TextField (rect, list [index]);
	}

	void OnGUI(){
		EditorGUILayout.BeginHorizontal ();

		tagScrollPosition = EditorGUILayout.BeginScrollView (tagScrollPosition);
        //EditorGUILayout.BeginVertical ();

        ////EditorGUILayout.BeginVertical ();
        ////functionList.DoLayoutList ();
        ////EditorGUILayout.EndVertical ();

        ////EditorGUILayout.BeginVertical ();
        ////parameterList.DoLayoutList ();
        ////EditorGUILayout.EndVertical ();

        ////EditorGUILayout.BeginVertical ();
        ////tagList.DoLayoutList ();
        ////EditorGUILayout.EndVertical ();

        //EditorGUILayout.EndVertical ();
		EditorGUILayout.EndScrollView ();

		EditorGUILayout.BeginVertical (GUILayout.Width(400f));

		if (GUILayout.Button ("Update all dictionary entries")) {
			DictionaryData.UpdateAllDictionaryEntriesFromSource();
		}

		DrawSearchBox ();

		DrawActiveEntryBox ();

		DrawAuxiliaryDataBox ();

		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndHorizontal ();
	}

	void DrawSearchBox(){
		EditorGUILayout.BeginVertical (GUI.skin.box);
		EditorGUILayout.BeginHorizontal ();

		filterText = EditorGUILayout.TextField (filterText);
		if (filterText != lastFilterText) {
			searchedEntries = DictionaryData.Instance.FilterEntriesFromRomaji(filterText);
			lastFilterText = filterText;
		}

		EditorGUILayout.EndHorizontal ();

		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, GUILayout.MaxHeight(200f));

		EditorGUILayout.BeginVertical ();
		foreach(var e in searchedEntries){
			if(GUILayout.Button(e.Kanji)){
				activeEntry = e;
			}
		}
		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();
	}

	void DrawActiveEntryBox(){
		EditorGUILayout.BeginVertical (GUI.skin.box);

		if (activeEntry == null) {
			EditorGUILayout.LabelField ("NULL");
		} else {
            EditorGUILayout.TextField("ID", activeEntry.ID.ToString());
			EditorGUILayout.LabelField ("Kanji", activeEntry.Kanji);
			EditorGUILayout.LabelField ("Kana", activeEntry.Kana);
			EditorGUILayout.LabelField ("English", "");
			EditorGUI.indentLevel++;
			foreach(var e in activeEntry.English){
				EditorGUILayout.LabelField ("", e);
			}
            EditorGUILayout.LabelField("PoS", activeEntry.PartOfSpeech + "; " + activeEntry.PartOfSpeech.GetCategory()+ "; " + activeEntry.GetPartOfSpeech());
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.EndVertical ();
	}

	void DrawAuxiliaryDataBox(){
		EditorGUILayout.BeginVertical (GUI.skin.box);

        //var selectionStrings = new string[gameData.PhraseClassData.Tags.Count + 1];
        //selectionStrings [0] = "None";
        //System.Array.Copy (gameData.PhraseClassData.Tags.ToArray (), 0, selectionStrings, 1, gameData.PhraseClassData.Tags.Count);

		if (activeEntry == null) {
			EditorGUILayout.LabelField ("NULL");
		} else if (!activeEntry.HasAuxiliaryData) {
			if(GUILayout.Button("Add auxiliary data")){
				DictionaryData.Instance.UpdateAuxiliaryData(new AuxiliaryDictionaryDataEntry(activeEntry.ID));
			}
		} else {
			var aux = activeEntry.AuxiliaryData;

            var s = aux.PreferredTranslation;
            if(s == null){
                s = "";
            }
            s = EditorGUILayout.TextField("Preferred English", s);
            if(s != ""){
                aux.PreferredTranslation = s;
            }

            aux.PartOfSpeech = (PartOfSpeech)EditorGUILayout.EnumPopup("PartOfSpeech", aux.PartOfSpeech);

            int selectedItem = itemIDs.IndexOf(aux.ItemID);
            int newSelectedItem = EditorGUILayout.Popup("Item", selectedItem, itemStrings);
            if (selectedItem != newSelectedItem) {
                aux.ItemID = itemIDs[newSelectedItem];
            }

			EditorGUILayout.LabelField ("Tags", "");
			EditorGUI.indentLevel++;
            //foreach(var e in aux.TagIDs){
            //    if(GUILayout.Button (gameData.PhraseClassData.Tags[e])){
            //        aux.TagIDs.Remove(e);
            //    }
            //}
            //var selected = EditorGUILayout.Popup(0, selectionStrings);
            //if (selected != 0) {
            //    aux.TagIDs.Add(selected - 1);
            //    //Debug.Log("Selected " + selected);
            //}

			EditorGUI.indentLevel--;

			if(GUILayout.Button("Remove auxiliary data")){
				DictionaryData.Instance.RemoveAuxiliaryData(activeEntry.ID);
			}
		}
		
		EditorGUILayout.EndVertical ();
	}

}
