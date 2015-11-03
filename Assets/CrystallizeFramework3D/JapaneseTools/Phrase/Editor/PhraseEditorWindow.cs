using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using JapaneseTools;
using Newtonsoft.Json;

public class PhraseEditorWindow : EditorWindow {

    class KeyStringMap {
        public string Normal { get; private set; }
        public string Shift { get; private set; }

        public KeyStringMap(string normal, string shift) {
            Normal = normal;
            Shift = shift;
        }
    }

	/** IME Keyboard configuration ******************************************/
	public static int displayLines  = 10;
	public static KeyCode nextPage = KeyCode.RightArrow;
	public static KeyCode prevPage = KeyCode.LeftArrow;
	public static KeyCode nextEntry = KeyCode.DownArrow;
	public static KeyCode prevEntry = KeyCode.UpArrow;

    static Dictionary<KeyCode, KeyStringMap> keyMapping = new Dictionary<KeyCode, KeyStringMap>();

    static PhraseEditorWindow() {
        keyMapping[KeyCode.Comma] = new KeyStringMap(",", "<");
        keyMapping[KeyCode.Period] = new KeyStringMap(".", ">");
        keyMapping[KeyCode.Slash] = new KeyStringMap("/", "?");
        keyMapping[KeyCode.Minus] = new KeyStringMap("-", "_");
        keyMapping[KeyCode.Space] = new KeyStringMap(" ", "\t");
        keyMapping[KeyCode.BackQuote] = new KeyStringMap("!", "~");
    }

	DictionaryDataEntry[] OnDisplay = new DictionaryDataEntry[displayLines];
	int offset = 0;
	DictionaryDataEntry[] currentList = new DictionaryDataEntry[0];

	PhraseSequenceElement[] conjugateDisplay = new PhraseSequenceElement[displayLines];
	int conjugateOffset = 0;
	PhraseSequenceElement[] currentConjugations = new PhraseSequenceElement[0];

	private bool focusSwitch = false;
	private bool onConjugate = false;
	private DictionaryDataEntry toConjugate = null;
	/** END *****************************************************************/

	bool keepFocus = true;

	PhraseSequence phraseSequence = new PhraseSequence();
    Action onClose;
	string lastImeText = "";
	string imeText = "";

	int selected = 0;
    int cursorPosition = 0;

	[MenuItem("Crystallize/Phrase IME")]
	public static void Open(){
		//var window = 
        GetWindow<PhraseEditorWindow>();
	}

	public static void Open(PhraseSequence phraseSequence){
		var window = GetWindow<PhraseEditorWindow>();
		window.phraseSequence = phraseSequence;
        window.onClose = null;
	}

    public static void Open(PhraseSequence phrase, Action action) {
        var window = GetWindow<PhraseEditorWindow>();
        window.phraseSequence = phrase;
        window.onClose = action;
    }

	void Awake(){
		keepFocus = true;
	}
	//public override void OnGUI (Rect rect)
	void OnGUI()
	{	
		if(keepFocus)
			Focus ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical();

		imeText = DrawPhrase (phraseSequence, imeText);
		if (onConjugate) {
			if(toConjugate == null){
				DrawTenWordForms (selected);
			}
			else
				DrawTenWordForms(toConjugate, false, selected);
		}
		else if (selected >= phraseSequence.PhraseElements.Count) {
			DrawTenFileterWords(phraseSequence, offset);
		} 
		else if (phraseSequence.PhraseElements [selected].IsDictionaryWord) {
			onConjugate = true;
			DrawTenWordForms (selected);
		} 
		else {
			//DrawSlotEditor(selected);
		}

		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();

		/** keyBorad event loop ****************************************/
		HandleKeyboardEvents ();
	}

	private void HandleKeyboardEvents(){
		bool canRead = true;
		if(Event.current.type == EventType.Repaint){
			canRead = true;
		}
		if(Event.current.type == EventType.Layout){
			canRead = false;
		}
		if(canRead){
			if (Event.current.type == EventType.KeyDown) {
				//listen to key events
				KeyCode key = Event.current.keyCode;
				ListenToScroll(key);
				ListenToTyping(key);
				ListenToControl(key);
			}
			//reset entry when typing
			if(lastImeText != imeText){
				selected = phraseSequence.PhraseElements.Count;
				//lastImeText = imeText;
				//offset = 0;
			}
		}
	}
	/**    IME UI KeyBoard Event Handling *******************************************************/

	//manage control keys on the window
	void ListenToControl(KeyCode key){
		if (key == KeyCode.Escape) {
			if(focusSwitch)
				keepFocus = false;
		}
		if (key == KeyCode.Return && imeText == "") {
            if (onClose != null) {
                onClose();
            }
            Close();
		}
	}
	
	//managing response to keyboard entries about inputs
	void ListenToTyping(KeyCode key){
		//deleting text
		if (key == KeyCode.Backspace || key == KeyCode.Delete) {
			//if no imetext, delete the entry in front of cursor
			if(imeText == "" && cursorPosition != 0){
				setCursorPosition(cursorPosition - 1);
				phraseSequence.RemoveAt(cursorPosition);
			}
			//if there is imetext, delete one letter from it
			else{
				onConjugate = false;
				toConjugate = null;
				imeText = imeText.Substring(0, Mathf.Max(0, imeText.Length - 1));
			}
		}
		//entering letters into imeText
		if(IsText(key)){//key.ToString().Length == 1 && IsAlphabet(key.ToString()[0])){
			//set onConjugate and toConjugate as such for every action that will change imeText
			onConjugate = false;
			toConjugate = null;
            imeText += GetText(key);
		}
		/** Potential key strokes that can be used for entering into text ***/
		//TODO maybe enable punctuations
		/********************************************************************/

		/** selection key strokes *****/
		int index;

		//conjugate words selection when modifying existing verbs
		if (IsNumeric (key.ToString (), out index) 
            && onConjugate 
		    && selected < phraseSequence.PhraseElements.Count
		    && phraseSequence.PhraseElements [selected].IsDictionaryWord) {
			if(index >= 0 && index < conjugateDisplay.Length && conjugateDisplay[index] != null){
				phraseSequence.UpdateAt(selected, conjugateDisplay[index]);
			}
		}

		else if(IsNumeric(key.ToString(), out index) && imeText != ""){
			//conjugate words selection for newly typed words
			if(onConjugate){
				if(index >= 0 && index < conjugateDisplay.Length && conjugateDisplay[index] != null){
					phraseSequence.PhraseElements.Insert (cursorPosition, conjugateDisplay[index]);
					imeText = "";
					toConjugate = null;
					onConjugate = false;
					setCursorPosition(cursorPosition + 1);
				}
			}
			//Japanese word selection
			else{
				if(index >= 0 && index < OnDisplay.Length && OnDisplay[index] != null){
					if(hasConjugate(OnDisplay[index])){
						toConjugate = OnDisplay[index];
						//Debug.Log (toConjugate);
						onConjugate = true;
					}
					else{
						phraseSequence.PhraseElements.Insert (cursorPosition, new PhraseSequenceElement(OnDisplay[index].ID, 0));
						imeText = "";
						setCursorPosition(cursorPosition + 1);
					}
				}
			}
		}

		/**   HotKeys for selection when imeText isn't empty  ********************/
		//TODO maybe change hotkey to ctrl
		else if (key == KeyCode.Tab && imeText != "") {
			var element = new PhraseSequenceElement (PhraseSequenceElementType.Text, imeText);
			//phrase.Add (new PhraseSequenceElement(PhraseSequenceElementType.Text, imeText));
			imeText = "";
			phraseSequence.PhraseElements.Insert (cursorPosition, element);
			setCursorPosition(cursorPosition + 1);
		}
		else if (key == KeyCode.LeftControl && imeText != "") {
			var element = new PhraseSequenceElement (PhraseSequenceElementType.ContextSlot, imeText);
			//phrase.Add (new PhraseSequenceElement(PhraseSequenceElementType.Text, imeText));
			imeText = "";
			phraseSequence.PhraseElements.Insert (cursorPosition, element);
			setCursorPosition(cursorPosition + 1);
		}
		else if (key == KeyCode.Tab && imeText == "") {
			var element = new PhraseSequenceElement (PhraseSequenceElementType.Wildcard, "");
			//phrase.Add (new PhraseSequenceElement(PhraseSequenceElementType.Text, imeText));
			imeText = "";
			phraseSequence.PhraseElements.Insert (cursorPosition, element);
			setCursorPosition(cursorPosition + 1);
		}
	}

	//managing response to keyboard entries about scrolling
	void ListenToScroll(KeyCode key) {
		if(key == nextPage){
			if(imeText == ""){
				setCursorPosition(cursorPosition + 1);
			}
			else if (onConjugate){
				conjugateOffset = Mathf.Min(conjugateOffset + displayLines, currentConjugations.Length - displayLines);
			}
			else
				offset = Mathf.Min(offset + displayLines, currentList.Length - displayLines);
		}
		if(key == prevPage){
			if(imeText == ""){
				setCursorPosition(cursorPosition - 1);
			}
			else if (onConjugate){
				conjugateOffset = Mathf.Max(0, conjugateOffset - displayLines);
			}
			else
				offset = Mathf.Max(0, offset - displayLines);
		}
		if(key == nextEntry){
			if(onConjugate){
				conjugateOffset = Mathf.Min(conjugateOffset + 1, currentConjugations.Length  - displayLines);
			}
			else
				offset = Mathf.Min(offset + 1, currentList.Length - displayLines);
		}
		if(key == prevEntry){
			if(onConjugate){
				conjugateOffset = Mathf.Max(0, conjugateOffset - 1);
			}
			else
				offset = Mathf.Max(0, offset - 1);
		}
	}

	/** IME UI helper ****************************************************************************/

	//key input helpers
    bool IsText(KeyCode kc) {
        var s = kc.ToString();
        if (s.Length == 1) {
            if(IsAlphabet(s[0])){
                return true;
            }
        }

        return keyMapping.ContainsKey(kc);
    }

    string GetText(KeyCode kc) {
        var s = kc.ToString();
        if (s.Length == 1) {
            if (IsAlphabet(s[0])) {
                if (Event.current.shift) {
                    return s.ToUpper();
                } else {
                    return s.ToLower();
                }
            }
        }

        if (keyMapping.ContainsKey(kc)) {
            if (Event.current.shift) {
                return keyMapping[kc].Shift;
            } else {
                return keyMapping[kc].Normal;
            }
        }

        return kc.ToString();
    }

	bool IsAlphabet(char c){
		if (((int)c >= (int)'A' && (int)c <= (int)'Z') || ((int)c >= (int)'a' && (int)c <= (int)'z'))
			return true;
		return false;
	}

	bool IsNumeric(string s, out int index){
		if (s.Length == 6 && s.Substring (0, 5) == "Alpha") {
			int dec = (int) s[5];
			if(dec >= (int) '1' && dec <= (int) '9'){
				index = dec - (int) '1';
				return true;
			}
			else if(dec == (int) '0'){
				index = 9;
				return true;
			}
		}
		index = -1;
		return false;
	}

	void setCursorPosition(int newPos){
		if (newPos < 0)
			newPos = 0;
		if (newPos >= phraseSequence.PhraseElements.Count)
			newPos = phraseSequence.PhraseElements.Count;
		cursorPosition = newPos;
	}

	private void createTextField(){
		//TODO highlight text entry area
//		Color oldColor = GUI.color;
//		GUI.color = Color.blue;
		GUILayout.Label (imeText, GUILayout.ExpandWidth(false), GUILayout.MinWidth(20));
//		GUI.color = oldColor;
		if(imeText != lastImeText){
			lastImeText = imeText;
			offset = 0;
		}
	}
	/** end of key input helpers **/

	/** UI Draw Functions **************************************************************************/
	public string DrawPhrase(PhraseSequence phrase, string imeText){
		EditorGUILayout.BeginVertical ();

		/** Translation fields **/
//		var s = phrase.Translation;
//		if (s == null) {
//			s = "";
//		}
//		GUI.SetNextControlName("translationCtrl");
//		s = EditorGUILayout.TextField ("Translation", s, GUILayout.ExpandWidth(false), GUILayout.Width(500f));
//		if (s != "") {
//			phrase.Translation = s;
//		}

		/** Focus switch on the window **/
//		EditorGUILayout.BeginHorizontal ();
//
//		//set to focus back on the window
//		if (GUILayout.Button ("Focus On Window", GUILayout.ExpandWidth (false))) {
//			GUI.FocusControl(null);
//			if(focusSwitch)
//				keepFocus = true;
//		}
//		//set to remove focus from the window
//		if (GUILayout.Button ("Focus Away From Window", GUILayout.ExpandWidth (false))) {
//			if(focusSwitch)
//				keepFocus = false;
//		}
//
//		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();

		int i = 0;
        for (; i < phrase.PhraseElements.Count; i++) {
            if (cursorPosition == i) {

				createTextField();
            } else {
//                if (GUILayout.Button("o", GUILayout.ExpandWidth(false))) {
//					setCursorPosition(i);
//                }
            }

            var word = phrase.PhraseElements[i];

            if (GUILayout.Button(word.GetText(), GUILayout.ExpandWidth(false))) {
				onConjugate = false;
                selected = i;
            }

//            if (GUILayout.Button("x", GUILayout.ExpandWidth(false))) {
//                phrase.RemoveAt(i);
//				setCursorPosition(cursorPosition - 1);
//                break;
//            }
        }

        if(cursorPosition >= phrase.PhraseElements.Count){
			setCursorPosition(phrase.PhraseElements.Count);
			createTextField();
        } else {
//            if(GUILayout.Button("o", GUILayout.ExpandWidth(false))){
//				setCursorPosition(phrase.PhraseElements.Count);
//            }
        }
		
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		return imeText;
	}

	void DrawWordForms(int wordIndex){
		if (wordIndex < phraseSequence.PhraseElements.Count) {
			var entry = DictionaryData.Instance.GetEntryFromID (phraseSequence.PhraseElements[wordIndex].WordID);
			foreach(var form in ConjugationTool.GetForms(entry)){
				var label = form.GetText();
				if (GUILayout.Button (label)) {
					phraseSequence.UpdateAt(selected, form);
				}
			}
		}
	}

	private bool hasConjugate(DictionaryDataEntry e){
		//e itself is always returned, check if any other form exists
		return ConjugationTool.GetForms (e).Length > 1;
	}

	void DrawTenWordForms(int selected){
		var entry = DictionaryData.Instance.GetEntryFromID (phraseSequence.PhraseElements[selected].WordID);
		DrawTenWordForms (entry, true, selected);
	}

	void DrawTenWordForms(DictionaryDataEntry entry, bool update, int selected){
		currentConjugations = ConjugationTool.GetForms (entry);
		if (conjugateOffset + displayLines >= currentConjugations.Length)
			conjugateOffset = Mathf.Max (0, currentConjugations.Length - displayLines);
		if (conjugateOffset < 0)
			conjugateOffset = 0;
		for (int i = 0; i < displayLines; i++) {
			if (conjugateOffset + i >= currentConjugations.Length || conjugateOffset + i < 0)
				conjugateDisplay[i] = null;
			else{
				var e = currentConjugations [conjugateOffset + i];
				var label = string.Format ("{0}:   {1}", i + 1, e.GetText());
				GUIStyle buttonStyle = new GUIStyle();
				buttonStyle.alignment = TextAnchor.LowerLeft;
				if (GUILayout.Button (label, buttonStyle)) {
					if(update){
						phraseSequence.UpdateAt(selected, e);
						break;
					}
					else{
						phraseSequence.PhraseElements.Insert (cursorPosition, e);
						imeText = "";
						toConjugate = null;
						break;
					}
				}
				conjugateDisplay [i] = e;
			}
		}

	}

    //void DrawSlotEditor(int wordIndex){
    //    var tag = DrawTags (phraseSequence.PhraseElements [wordIndex].Tags);
    //    if (tag != null) {
    //        phraseSequence.PhraseElements[wordIndex].AddTag(tag);
    //    }
    //}

    //string DrawTags(List<string> tags){
    //    var selectionStrings = new string[GameData.Instance.PhraseClassData.Tags.Count + 1];
    //    selectionStrings [0] = "None";
    //    System.Array.Copy (GameData.Instance.PhraseClassData.Tags.ToArray (), 0, selectionStrings, 1, GameData.Instance.PhraseClassData.Tags.Count);

    //    foreach(var e in tags){
    //        if(GUILayout.Button (e)){
    //            tags.Remove(e);
    //            break;
    //        }
    //    }

    //    var selected = EditorGUILayout.Popup(0, selectionStrings);
    //    if(selected != 0){
    //        return selectionStrings[selected];
    //    }
    //    return null;
    //}

	/** tentative method to draw words
	 *  draw ten words on the screen
	 *  Pre: PhraseSquence phrase, int offset, the index of the first element to be drawn (starting at 0)
	 */ 
	void DrawTenFileterWords(PhraseSequence phrase, int offset){
		int prevCount = phrase.PhraseElements.Count;
		if (imeText != "") {
			PhraseSequenceElement element = null;
			
			if (imeText [0] == '*') {
				if (GUILayout.Button ("Add slot...")) {
					element = new PhraseSequenceElement (0, 0);
					imeText = "";
					//phrase.Add (new PhraseSequenceElement(0, 0));
				}
			} else {

				currentList = DictionaryData.Instance.FilterEntriesFromRomaji (imeText).ToArray ();
				if (offset + displayLines >= currentList.Length)
					offset = Mathf.Max (0, currentList.Length - displayLines);
				if (offset < 0)
					offset = 0;
				for (int i = 0; i < displayLines; i++) {
					if (offset + i >= currentList.Length || offset + i < 0)
						OnDisplay[i] = null;
					else{
						var e = currentList [offset + i];
						string label = string.Format ("{0}:   [{1}] {2} ({3}) {4}", i + 1, e.ID, e.Kanji, e.Kana, e.EnglishSummary);
						GUIStyle buttonStyle = new GUIStyle();
						buttonStyle.alignment = TextAnchor.LowerLeft;
						if (GUILayout.Button (label, buttonStyle)) {
							element = new PhraseSequenceElement (e.ID, 0);
							//phrase.Add (new PhraseSequenceElement(e.ID, 0));
							imeText = "";
							break;
						}
						OnDisplay [i] = e;
					}
				}

				string buttonLabel = string.Format("{0}  [Hotkey: {1}]", "Add as plain text", ",");
				if (GUILayout.Button (buttonLabel)) {
					element = new PhraseSequenceElement (PhraseSequenceElementType.Text, imeText);
					//phrase.Add (new PhraseSequenceElement(PhraseSequenceElementType.Text, imeText));
					imeText = "";
				}
				buttonLabel = string.Format("{0}  [Hotkey: {1}]", "Add as context data", ".");
				if (GUILayout.Button (buttonLabel)) {
					element = new PhraseSequenceElement (PhraseSequenceElementType.ContextSlot, imeText);
					//phrase.Add (new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, imeText));
					imeText = "";
				}
				
//				if (GUILayout.Button ("Add as tag")) {
//					element = new PhraseSequenceElement (PhraseSequenceElementType.TaggedSlot, imeText);
//					//phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.TaggedSlot, imeText));
//					imeText = "";
//				}
				buttonLabel = string.Format("{0}  [Hotkey: {1}]", "Add as wildcard", "/");
				if (GUILayout.Button (buttonLabel)) {
					element = new PhraseSequenceElement (PhraseSequenceElementType.Wildcard, imeText);
					//phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Wildcard, imeText));
					imeText = "";
				}

				if (element != null) {
					phrase.PhraseElements.Insert (cursorPosition, element);
				}
				
				if (GUILayout.Button ("Search...")) {
					var searchResultList = DictionaryData.SearchDictionaryWithStartingRomaji (imeText);
					DictionaryEntrySelectionWindow.Open (searchResultList);
				}
			}
		} 
		
		if (prevCount != phrase.PhraseElements.Count) {
			setCursorPosition(cursorPosition + 1);
		}
	}
	


}
