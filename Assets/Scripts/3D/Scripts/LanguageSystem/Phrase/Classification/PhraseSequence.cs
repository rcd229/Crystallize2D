using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JapaneseTools;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

public static class PhraseSequenceExtensions {
    public static bool ContainsEquivalentPhrase(this IEnumerable<PhraseSequence> phrases, PhraseSequence phrase) {
        foreach (var p in phrases) {
            if (PhraseSequence.PhrasesEquivalent(phrase, p)) {
                return true;
            }
        }
        return false;
    }

    public static bool ContainsEquivalentPhrase(this IEnumerable<PhraseSequence> phrases, PhraseSequence phrase, out PhraseSequence item) {
        foreach (var p in phrases) {
            if (PhraseSequence.PhrasesEquivalent(phrase, p)) {
                item = p;
                return true;
            }
        }
        item = null;
        return false;
    }

    public static PhraseSequence GetComparableSubset(this PhraseSequence p) {
        var comparables = from elem in p.PhraseElements
                          where elem.Comparable
                          select elem;
        PhraseSequence phrase = new PhraseSequence();
        phrase.Translation = p.Translation;
        foreach (var e in comparables) {
            phrase.Add(e);
        }
        return phrase;
    }

    public static void AddValidWords(this HashSet<PhraseSequenceElement> set, PhraseSequence phrase) {
        if (phrase.IsWord && phrase.Word.IsDictionaryWord) {
            set.Add(phrase.Word);
        } else {
            foreach (var elem in phrase.GetElements()) {
                if (elem.IsDictionaryWord) {
                    set.Add(elem);
                }
            }
        }
    }

    public static IEnumerable<PhraseSequence> AggregateWords(this IEnumerable<PhraseSequence> phrases) {
        HashSet<PhraseSequenceElement> phraseSequenceSet = new HashSet<PhraseSequenceElement>(new PhraseElementComparerator());
        foreach (var p in phrases) {
            phraseSequenceSet.AddValidWords(p);
        }
        return phraseSequenceSet.Select(w => new PhraseSequence(w));
    }

    public static PhraseSequence Flatten(this IEnumerable<PhraseSequence> phrases) {
        var p = new PhraseSequence();
        foreach (var w in phrases) {
            p.Add(w);
        }
        return p;
    }
}

public class PhraseSequence : Prefixable {

    static int GetNextComparableElement(PhraseSequence p, int start) {
        while (start < p.PhraseElements.Count) {
            if (p.PhraseElements[start].Comparable) {
                return start;
            }
            start++;
        }
        return -1;
    }



    public static bool PhrasesEquivalent(PhraseSequence a, PhraseSequence b) {
        if (a == null || b == null) {
            return false;
        }

        if (a.ComparableElementCount != b.ComparableElementCount) {
            return false;
        }

        if (a.ComparableElementCount == 0) {
            return a.GetText() == b.GetText();
        }

        int indexA = 0;
        int indexB = 0;
        while (true) {
            indexA = GetNextComparableElement(a, indexA);
            indexB = GetNextComparableElement(b, indexB);
            if (indexA == -1) {
                return indexB == -1;
            }

            if (!PhraseSequenceElement.StrictCompare(a.PhraseElements[indexA], b.PhraseElements[indexB])) {
                return false;
            }

            indexA++;
            indexB++;
        }
    }

    public string Translation { get; set; }
    public List<PhraseSequenceElement> PhraseElements { get; set; }

    [JsonIgnore]
    public bool IsWord {
        get {
            return ComparableElementCount == 1;
        }
    }

    [JsonIgnore]
    public PhraseSequenceElement Word {
        get {
            foreach (var e in PhraseElements) {
                if (e.Comparable) {
                    return e;
                }
            }
            return null;//PhraseElements[0];
        }
    }

    [JsonIgnore]
    public bool IsEmpty {
        get {
            return PhraseElements.Count == 0;
        }
    }

    [JsonIgnore]
    public int ComparableElementCount {
        get {
            if (PhraseElements == null) return 0;
            int c = 0;
            for (int i = 0; i < PhraseElements.Count; i++) {
                if (PhraseElements[i].Comparable) {
                    c++;
                }
            }
            return c;
        }
    }

    [JsonIgnore]
    public bool HasContextData {
        get {
            foreach (var e in PhraseElements) {
                if (e.ElementType == PhraseSequenceElementType.ContextSlot) {
                    return true;
                }
            }
            return false;
        }
    }

    public PhraseSequence() {
        PhraseElements = new List<PhraseSequenceElement>();
    }

    public PhraseSequence(string text)
        : this() {
        var pe = new PhraseSequenceElement(PhraseSequenceElementType.Text, text);
        Add(pe);
        Translation = text;
    }

    public PhraseSequence(PhraseSequence original) {
        Translation = original.Translation;
        PhraseElements = new List<PhraseSequenceElement>(original.PhraseElements);
    }

    public PhraseSequence(PhraseSequenceElement word)
        : this() {
        Add(word);
        Translation = word.GetTranslation();
    }

    public List<PhraseSequenceElement> GetElements() {
        return PhraseElements;
    }

    public List<DictionaryDataEntry> GetWords() {
        var l = new List<DictionaryDataEntry>();
        foreach (var ele in PhraseElements) {
            l.Add(DictionaryData.Instance.GetEntryFromID(ele.WordID));
        }
        return l;
    }

    public void Add(PhraseSequenceElement element) {
        PhraseElements.Add(element);
    }

    public void Add(PhraseSequence phrase) {
        foreach (var e in phrase.PhraseElements) {
            PhraseElements.Add(e);
        }
    }

    public void UpdateAt(int index, PhraseSequenceElement element) {
        PhraseElements.RemoveAt(index);
        PhraseElements.Insert(index, element);
    }

    public void RemoveAt(int index) {
        PhraseElements.RemoveAt(index);
    }

    public string GetText(JapaneseScriptType scriptType = JapaneseScriptType.Kanji) {
		StringBuilder sb = new StringBuilder();
        if (PhraseElements.Count > 0) {
            sb.Append(PhraseElements[0].GetText(scriptType));
        }

        for (int i = 1; i < PhraseElements.Count; i++) {
            if (scriptType == JapaneseScriptType.Romaji) {
                sb.Append(" ");
            }
            sb.Append(PhraseElements[i].GetText(scriptType));
        }
        return sb.ToString();
    }

    public List<string> GetSuppliedContextData() {
        var suppliedContext = new List<string>();
        foreach (var p in PhraseElements) {
            if (p.ElementType == PhraseSequenceElementType.ContextSlot) {
                suppliedContext.Add(p.Text);
            }
        }
        return suppliedContext;
    }

    public bool FulfillsTemplate(PhraseSequence template) {
        var cleanPhrase = new PhraseSequence();
        foreach (var e in this.PhraseElements) {
            if (e.GetPhraseCategory() != PhraseCategory.Punctuation) {
                cleanPhrase.PhraseElements.Add(e);
            }
        }

        var cleanTemplate = new PhraseSequence();
        foreach (var e in template.PhraseElements) {
            if (e.GetPhraseCategory() != PhraseCategory.Punctuation) {
                cleanTemplate.PhraseElements.Add(e);
            }
        }

        //Debug.Log("Cleaned: " + cleanPhrase.GetText());

        if (cleanPhrase.PhraseElements.Count != cleanTemplate.PhraseElements.Count) {
            //Debug.Log("Count mismatch");
            return false;
        }

        for (int i = 0; i < cleanTemplate.PhraseElements.Count; i++) {
            if (cleanTemplate.PhraseElements[i].ElementType == PhraseSequenceElementType.FixedWord) {
                if (cleanTemplate.PhraseElements[i].WordID != cleanPhrase.PhraseElements[i].WordID) {
                    //Debug.Log("Word mismatch: " + i + "; " + template.PhraseElements[i].WordID + "; " + this.PhraseElements[i].WordID);
                    return false;
                }
            }

            //if (template.PhraseElements[i].ElementType == PhraseSequenceElementType.ContextSlot) {
            //    if (!cleanPhrase.PhraseElements[i].Tags.Contains(template.PhraseElements[i].Text)) {
            //        //Debug.Log("Context mismatch" + i + "; " + template.PhraseElements[i].WordID + "; " + this.PhraseElements[i].WordID);
            //        return false;
            //    }
            //}

            if (cleanTemplate.PhraseElements[i].ElementType == PhraseSequenceElementType.TaggedSlot) {
                if (cleanPhrase.PhraseElements[i].GetPhraseCategory().ToString().ToLower() != cleanTemplate.PhraseElements[i].Text.ToLower()) {
                    //Debug.Log("Context mismatch" + i + "; " + template.PhraseElements[i].WordID + "; " + this.PhraseElements[i].WordID);
                    return false;
                }
            }
        }
        return true;
    }

    public bool ContainsWord(PhraseSequenceElement word, bool enforceForm = false) {
        foreach (var w in PhraseElements) {
            if (PhraseSequenceElement.IsEqual(word, w, enforceForm)) {
                return true;
            }
        }
        return false;
    }

    public PhraseSequence InsertContext(ContextData context) {
        if (context == null) {
            return this;
        }

        var p = new PhraseSequence();
        p.Translation = Translation;
        foreach (var w in PhraseElements) {
            if (w.ElementType == PhraseSequenceElementType.ContextSlot) {
                var cd = context.Get(w.Text);
                if (cd != null) {
                    PhraseSequence replace = cd.Data;
                    foreach (var e in replace.PhraseElements) {
                        p.Add(e);
                    }
                    p.Translation = p.Translation.Replace("[" + w.Text + "]", HelperGetTranslation(replace));
                } else {
                    p.Add(w);
                }
            } else {
                p.Add(w);
            }
        }
        return p;
    }

    public List<PhraseSequence> GetContextWords(ContextData context) {
        var contextWords = new List<PhraseSequence>();
        foreach (var w in PhraseElements) {
            if (w.ElementType == PhraseSequenceElementType.ContextSlot) {
                var cd = context.Get(w.Text);
                if (cd != null) {
                    contextWords.Add(cd.Data);
                }
            }
        }
        return contextWords;
    }

	public string getPrefixableText(){
		return GetText(JapaneseScriptType.Romaji);
	}

    string HelperGetTranslation(PhraseSequence sequence) {
        var t = "";
        if (sequence.IsWord) {
            t = sequence.Word.GetTranslation();
        } else {
            t = sequence.Translation;
        }
        return t;
    }

}
