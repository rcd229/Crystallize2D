using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OccupationPhraseResources : DefaultPhrasePipelineBuilder, IPhraseResources {
    public const string Student = "student";
    public const string Teacher = "teacher";
    public const string Chef = "chef";
    public const string ShopAssistant = "shop assistant";
    public const string CompanyEmployee = "company employee";

    public static string[] GetOccupationKeys() {
        return new string[] { Student, Teacher, Chef, ShopAssistant, CompanyEmployee };
    }

    public static IEnumerable<PhraseSequence> GetOccupations() {
        return GetOccupationKeys().Select(k => GetPhrase(k));
    }

    public string SetKey { get { return GetType().ToString(); } }
    public IEnumerable<string> GetPhraseKeys() { return GetOccupationKeys(); }
}
