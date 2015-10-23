using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ContextPhraseResources : DefaultPhrasePipelineBuilder, IPhraseResources {
    public const string Name = "name";
    public const string HomeTown = "hometown";
    public const string Age = "age";
    public const string Hobby = "hobby";
    public const string Occupation = "occupation";

    public static string[] GetRepeatableContext() {
        return new string[] { HomeTown, Age, Hobby, Occupation };
    }

    public static IEnumerable<string> GetAvailableKeysForContext(string context) {
        if (context == HomeTown) {
            return PlacePhraseResources.GetCityKeys();
        } else if (context == Age) {
            return AgePhraseResources.GetAgeRangeKeys(15, 35);
        } else if (context == Hobby) {
            return HobbyPhraseResources.GetHobbyKeys();
        } else if (context == Occupation) {
            return OccupationPhraseResources.GetOccupationKeys();
        } else {
            Debug.LogError(context + " is not a valid context");
            return null;
        }
    }

    public static IEnumerable<PhraseSequence> GetAvailableForContext(string context) {
        if (context == HomeTown) {
            return PlacePhraseResources.GetCities();
        } else if (context == Age) {
            return AgePhraseResources.GetAgeRange(15, 35);
        } else if (context == Hobby) {
            return HobbyPhraseResources.GetHobbies();
        } else if (context == Occupation) {
            return OccupationPhraseResources.GetOccupations();
        } else {
            Debug.LogError(context + " is not a valid context");
            return null;
        }
    }

    public string SetKey { get { return GetType().ToString(); } }
    public IEnumerable<string> GetPhraseKeys() { return new string[] { Name, HomeTown, Age, Hobby, Occupation }; }
}
