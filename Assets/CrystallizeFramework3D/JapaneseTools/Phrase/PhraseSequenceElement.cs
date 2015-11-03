using UnityEngine;
using System.Collections;
using JapaneseTools;
using System.Collections.Generic;
using Newtonsoft.Json;

public enum PhraseSequenceElementType {
    FixedWord = 0,
    Text = 101,
    ContextSlot = 102,
    TaggedSlot = 103,
    Wildcard = 104
}

public class PhraseSequenceElement {
	public static long timer;
    public static bool StrictCompare(PhraseSequenceElement e1, PhraseSequenceElement e2) {
        if (!e1.Comparable) {
            return false;
        }

        if (!e2.Comparable) {
            return false;
        }

        if (e1.ElementType != e2.ElementType) {
            return false;
        }

        switch (e1.ElementType) {
            case PhraseSequenceElementType.FixedWord:
                return e1.WordID == e2.WordID;
            case PhraseSequenceElementType.ContextSlot:
            case PhraseSequenceElementType.TaggedSlot:
                return e1.Text == e2.Text;
            case PhraseSequenceElementType.Wildcard:
                return e2.ElementType == PhraseSequenceElementType.Wildcard;
        }
        return false;
    }

    public static bool WildcardCompare(PhraseSequenceElement e1, PhraseSequenceElement e2) {
        if (e1.ElementType == PhraseSequenceElementType.Wildcard) {
            return true;
        }

        if (e2.ElementType == PhraseSequenceElementType.Wildcard) {
            return true;
        }

        return StrictCompare(e1, e2);
    }

    public static bool IsEqual(PhraseSequenceElement e1, PhraseSequenceElement e2, bool enforceForm = false) {
        if (e1 == null || e2 == null) {
            return e1 == e2;
        }

        if (e1.WordID >= 1000000) {
            if (enforceForm) {
                return e1.WordID == e2.WordID && e1.FormID == e2.FormID;
            } else {
                return e1.WordID == e2.WordID;
            }
        } else {
            return e1.ElementType == e2.ElementType && e1.Text == e2.Text;
        }
    }

    public int WordID { get; set; }
    public int FormID { get; set; }
    public string Text { get; set; }
	public string RomajiText{get;set;}
    public List<string> Tags { get; set; }

    [JsonIgnore]
    public bool IsFixedWord {
        get {
            return WordID > 100;
        }
    }

    [JsonIgnore]
    public bool IsDictionaryWord {
        get {
            return WordID >= 1000000;
        }
    }

    [JsonIgnore]
    public bool IsPlainText {
        get {
            return WordID == (int)PhraseSequenceElementType.Text;
        }
    }

    [JsonIgnore]
    public bool Comparable {
        get {
            return WordID != (int)PhraseSequenceElementType.Text;
        }
    }

    public PhraseSequenceElementType ElementType {
        get {
            //if (WordID == 101) {
            //    return PhraseSequenceElementType.Text;
            //}

            //if(WordID == 102){
            //    return PhraseSequenceElementType.ContextSlot;
            //}

            //if (WordID == 103) {
            //    return PhraseSequenceElementType.TaggedSlot;
            //}

            //if (WordID == 104) {
            //    return PhraseSequenceElementType.Wildcard;
            //}

            if (WordID >= 1000000) {
                return PhraseSequenceElementType.FixedWord;
            } else {
                if (System.Enum.IsDefined(typeof(PhraseSequenceElementType), WordID)) {
                    return (PhraseSequenceElementType)WordID;
                }
            }

            return PhraseSequenceElementType.Text;
        }
    }

    public PhraseSequenceElement() {
        Tags = new List<string>();
    }

    public PhraseSequenceElement(int wordID, int formID)
        : this() {
        WordID = wordID;
        FormID = formID;

        //var dd = DictionaryData.Instance.GetEntryFromID(WordID);
        //if (dd != null) {
        //    if (dd.HasAuxiliaryData) {
        //        foreach (var tag in dd.AuxiliaryData.TagIDs) {
        //            Tags.Add(GameData.Instance.PhraseClassData.Tags[tag]);
        //        }
        //    }
        //}
    }

    public PhraseSequenceElement(PhraseSequenceElementType type, string text)
        : this() {
        WordID = (int)type;
        Text = text;
		if(type == PhraseSequenceElementType.Text){
			RomajiText = GetText(JapaneseScriptType.Romaji);
		}
    }

    public string GetText(JapaneseScriptType scriptType) {
        switch (ElementType) {
            case PhraseSequenceElementType.Text:
                switch (scriptType) {
                    case JapaneseScriptType.Romaji:
						if(RomajiText.IsEmptyOrNull()){ 
							return KanaConverter.Instance.ConvertToRomaji(Text);
						}else{
					return RomajiText;
						}
                    default:
                        return Text;
                }
            case PhraseSequenceElementType.FixedWord:
                var e = DictionaryData.Instance.GetEntryFromID(WordID);
                return ConjugationTool.GetForm(e, FormID, scriptType);
            case PhraseSequenceElementType.TaggedSlot:
                return "<" + Text + ">";
            case PhraseSequenceElementType.ContextSlot:
                return "[" + Text + "]";
            case PhraseSequenceElementType.Wildcard:
                return "*";
        }
        return Text;
    }

    public string GetText(JapaneseScriptType scriptType, ContextData context) {
        switch (ElementType) {
            case PhraseSequenceElementType.Text:
                //Debug.Log("IS TEXT");
                return Text;
            case PhraseSequenceElementType.FixedWord:
                var e = DictionaryData.Instance.GetEntryFromID(WordID);
                return ConjugationTool.GetForm(e, FormID, scriptType);
            case PhraseSequenceElementType.TaggedSlot:
                if (Tags.Count > 0) {
                    var s = "";
                    foreach (var t in Tags) {
                        s += "<" + t + "> ";
                    }
                    return s;
                } else {
                    return "<>";
                }
            case PhraseSequenceElementType.ContextSlot:
                //Debug.Log(Text);
                return context.Get(Text).Data.GetText(scriptType);

            case PhraseSequenceElementType.Wildcard:
                return "*";
        }
        return Text;
    }

    public string GetTranslation() {
        if (WordID < 1000000) {
            return GetText();
        }
        return DictionaryData.Instance.GetEntryFromID(WordID).GetPreferredTranslation();
    }

    public string GetPlayerText() {
        return KanaConverter.Instance.ConvertToRomaji(GetKanaText());
    }

    public string GetText() {
        return GetText(JapaneseScriptType.Kanji);
    }

    public string GetKanaText() {
        return GetText(JapaneseScriptType.Kana);
    }

    public string GetKanaText(ContextData context) {
        return GetText(JapaneseScriptType.Kana, context);
    }

    public PhraseCategory GetPhraseCategory() {
        if (WordID >= 1000000) {
            return DictionaryData.Instance.GetEntryFromID(WordID).GetPartOfSpeech().GetCategory();
        } else {
            if (Text[0] < 'A') {
                return PhraseCategory.Punctuation;
            }
            return PhraseCategory.Unknown;
        }
    }

    public void AddTag(string tag) {
        if (!Tags.Contains(tag.ToLower())) {
            Tags.Add(tag.ToLower());
        }
    }

    public bool ContainsTag(string tag) {
        var lower = tag.ToLower();
        foreach (var t in Tags) {
            if (t.ToLower() == lower) {
                return true;
            }
        }
        return false;
    }

}
