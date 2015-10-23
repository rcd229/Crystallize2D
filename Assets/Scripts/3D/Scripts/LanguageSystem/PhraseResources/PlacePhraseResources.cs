using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlacePhraseResources : DefaultPhrasePipelineBuilder, IPhraseResources {

    public const string Tokyo = "Tokyo";
    public const string Kyoto = "Kyoto";
    public const string Osaka = "Osaka";
    public const string Yokohama = "Yokohama";
    public const string Nagoya = "Nagoya";

    public static string[] GetCityKeys() {
        return new string[] { Tokyo, Kyoto, Osaka, Yokohama, Nagoya };
    }

    public static IEnumerable<PhraseSequence> GetCities() {
        return GetCityKeys().Select(k => GetPhrase(k));
    }

    public string SetKey { get { return GetType().ToString(); } }
    public IEnumerable<string> GetPhraseKeys() { return GetCityKeys(); }
}
