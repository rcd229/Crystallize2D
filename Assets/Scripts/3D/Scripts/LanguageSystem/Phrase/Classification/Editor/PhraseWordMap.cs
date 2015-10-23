using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class PhraseWordMap : MonoBehaviour {

    [MenuItem("Crystallize/Print word map")]
    public static void PrintWordMap() {
        List<PhraseSequence> words;
        List<PhraseSequence> phrases;
        GetAllWordsAndPhrases(out words, out phrases);
        //words = words.OrderBy(w => w.Word.GetText(JapaneseTools.JapaneseScriptType.Kana)).ToList();
        var map = new Dictionary<PhraseSequence, List<PhraseSequence>>();
        foreach (var w in words) {
            map[w] = phrases.Where(p => p.ContainsWord(w.Word)).ToList();
        }

        var s = "Word map";
        var ordered = from kv in map
                      orderby kv.Value.Count, kv.Key.GetText(JapaneseTools.JapaneseScriptType.Kana)
                      select kv;
        foreach (var item in ordered) {
            s += "\n" + item.Key.GetText(JapaneseTools.JapaneseScriptType.Kanji) + ":";
            foreach (var p in item.Value) {
                s += "\t" + p.GetText(JapaneseTools.JapaneseScriptType.Kanji);
            }
        }
        Debug.Log(s);
    }

    static IEnumerable<string> GetAllCompiledPhraseSets() {
        return GameDataInitializer.phraseSets.Keys;
    }

    static List<string> GetCompiledPhraseSetKeys(string setKey) {
        return GameDataInitializer.phraseSets[setKey];
    }

    public static void GetAllWordsAndPhrases(out List<PhraseSequence> words, out List<PhraseSequence> phrases) {
        words = new List<PhraseSequence>();
        phrases = new List<PhraseSequence>();
        HashSet<int> wordIDs = new HashSet<int>();
        foreach (var setKey in GetAllCompiledPhraseSets()) {
            var ps = PhraseSetCollectionGameData.GetOrCreateItem(setKey);
            var keys = GetCompiledPhraseSetKeys(setKey);
            for (int i = 0; i < keys.Count; i++) {
                var p = ps.GetPhrase(keys[i]);
                if (p.IsWord) {
                    if (p.Word.IsDictionaryWord) {
                        if (wordIDs.Add(p.Word.WordID)) {
                            words.Add(p);
                        }
                    }
                } else {
                    foreach (var w in p.PhraseElements) {
                        if (w.IsDictionaryWord) {
                            if (wordIDs.Add(w.WordID)) {
                                words.Add(p);
                            }
                        }
                    }

                    if (p.ComparableElementCount != 0) {
                        if (!phrases.ContainsEquivalentPhrase(p)) {
                            phrases.Add(p);
                        }
                    }
                }
            }
        }
    }

}
