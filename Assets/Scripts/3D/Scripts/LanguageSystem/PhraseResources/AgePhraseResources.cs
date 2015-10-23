using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class AgePhraseResources : DefaultPhrasePipelineBuilder, IPhraseResources {
    public const string AgePhrase = "[number] years old";
    public const string IAm_YearsOld = "I am [age] years old";

    public static PhraseSequence GetYearsOldPhrase(int age) {
        var c = new ContextData();
        c.Set("number", NumberPhraseResources.GetPhraseForNumber(age));
        return GetPhrase(AgePhrase).InsertContext(c);
    }

    public static PhraseSequence GetAgePhrase(int age) {
        var c = new ContextData();
        c.Set("number", NumberPhraseResources.GetPhraseForNumber(age));
        c.Set("age", GetPhrase(AgePhrase).InsertContext(c));
        return GetPhrase(IAm_YearsOld).InsertContext(c);
    }

    public static IEnumerable<PhraseSequence> GetAgeRange(int min, int max) {
        var list = new List<PhraseSequence>();
        for (int i = min; i <= max; i++) {
            list.Add(GetYearsOldPhrase(i));
        }
        return list;
    }

    public static IEnumerable<string> GetAgeRangeKeys(int min, int max) {
        return GetAgeRange(min, max).Select(p => p.Translation);
    }

    public IEnumerable<string> GetPhraseKeys() {
        return new string[] { AgePhrase, IAm_YearsOld };
    }

    public string SetKey {
        get { return GetType().ToString(); }
    }
}
