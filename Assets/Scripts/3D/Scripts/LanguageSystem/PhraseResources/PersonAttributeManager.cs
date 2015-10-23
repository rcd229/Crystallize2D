using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PersonAttributes : IPhraseResources {

    public const string Person = "person";
    public const string Boy = "Boy";
    public const string Girl = "Girl";
    public const string RedHaired = "red hair";
    public const string BlackHaired = "black hair";
    public const string BrownHaired = "brown hair";
    public const string BlondHaired = "blond hair";
    public const string BlueHaired = "blue hair";
    public const string GreenHaired = "green hair";
    public const string GrayHaired = "gray hair";
    public const string Shorts = "Shorts";
    public const string Pants = "Pants";
    public const string ShortSleevedShirt = "short sleeved shirt";
    public const string LongSleevedShirt = "long sleeved shirt";

    public static string[] MaleFemale() {
        return new string[] { Boy, Girl };
    }

    public static string[] PersonStrings() {
        return new string[] {
            Person, Boy, Girl
        };
    }

    public static string[] HairStrings() {
        return new string[] {
            RedHaired, BlackHaired, BrownHaired, BlondHaired, BlueHaired, GreenHaired, GrayHaired
        };
    }

    public static string[] ClothsStrings() {
        return new string[]{
            Shorts, Pants, ShortSleevedShirt, LongSleevedShirt
        };
    }

    public static IEnumerable<string> AllStrings() {
        var list = new List<string>();
        list.AddRange(PersonStrings());
        list.AddRange(HairStrings());
        list.AddRange(ClothsStrings());
        return list;
    }

    public static List<GameObject> GetNewActorInstances(List<Vector3> targets, string personKey, string itemKey, string attributeKey) {
        var personTag = "";
        switch (personKey) {
            case Boy:
                personTag = "Male";
                break;
            case Girl:
                personTag = "Female";
                break;
        }

        var actors = new List<GameObject>();
        if (IsLegs(itemKey)) {
            actors = DialogueActorUtil.GetActorsForTargetsWithTag(targets, personTag, attributeKey, "", "", "", itemKey);
        } else {
            actors = DialogueActorUtil.GetActorsForTargetsWithTag(targets, personTag, attributeKey, "", "", itemKey);
        }
        return actors;
    }

    static bool IsLegs(string item) {
        if (item == Shorts || item == Pants) {
            return true;
        }
        return false;
    }

    public static PhraseSequence GetPhrase(string key) {
        var inst = new PersonAttributes();
        return PhrasePipeline.GetPhrase(inst.SetKey, key, false);
    }

    public string SetKey {
        get { return "PersonAttributes"; }
    }

    public IEnumerable<string> GetPhraseKeys() {
        return AllStrings();
    }

}
