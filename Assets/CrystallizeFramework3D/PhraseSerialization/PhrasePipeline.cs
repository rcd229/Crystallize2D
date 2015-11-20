using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PhrasePipeline {
    static Dictionary<string, PhraseSequence> phrases = new Dictionary<string, PhraseSequence>();

    public static readonly Dictionary<string, List<string>> PhraseSets = new Dictionary<string, List<string>>();

    public static PhraseSequence GetPhrase(string phraseKey, bool isTest = false) {
        return GetPhrase("Default", phraseKey, isTest);
    }

    public static PhraseSequence GetPhrase(string setKey, string phraseKey, bool isTest) {
        if (isTest) {
            return new PhraseSequence(phraseKey);
        }

        var key = phraseKey.ToLower();
        //Debug.Log("Adding: " + setKey + "; " + key + "; " + isTest);
        if (phrases.ContainsKey(key)) {
            return phrases[key];
        } else {
            var p = PhraseSetCollectionGameData.GetOrCreateItem(setKey).GetPhrase(phraseKey);
            
            //GameDataInitializer.AddPhrase(setKey, phraseKey);
            if (!PhraseSets.ContainsKey(setKey)) {
                PhraseSets[key] = new List<string>();
            }
            PhraseSets[key].Add(phraseKey);

            phrases[key] = p;
            return p;
        }
    }
}
