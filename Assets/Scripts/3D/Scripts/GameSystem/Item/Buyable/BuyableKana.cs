using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BuyableKana : IBuyable {

    const string ViewedSet = "KanaViewed";

    public static readonly BuyableKana VowelHiragana = Hiragana("Vowel", "Unlock the 5 hiragana with no initial sound.", "あいうえお");
    public static readonly BuyableKana KHiragana = Hiragana("K-initial", "Unlock the 5 hiragana beginning with a 'k' sound.", "かきくけこ", VowelHiragana);
    public static readonly BuyableKana GHiragana = Hiragana("G-initial", "Unlock the 5 hiragana beginning with a 'g' sound. These are modified versions of the 'k' series kana.", "がぎぐげご", KHiragana);
    public static readonly BuyableKana SHiragana = Hiragana("S-initial", "Unlock the 5 hiragana beginning with a 's' sound.", "さしすせそ", KHiragana, NHiragana, MHiragana, RHiragana);
    public static readonly BuyableKana ZHiragana = Hiragana("Z-initial", "Unlock the 5 hiragana beginning with a 'z' sound. These are modified versions of the 's' series kana.", "ざじずぜぞ", SHiragana);
    public static readonly BuyableKana THiragana = Hiragana("T-initial", "Unlock the 5 hiragana beginning with a 't' sound.", "たちつてと", KHiragana, NHiragana, MHiragana, RHiragana);
    public static readonly BuyableKana DHiragana = Hiragana("D-initial", "Unlock the 5 hiragana beginning with a 'd' sound. These are modified versions of the 't' series kana.", "だぢづでど", THiragana);
    public static readonly BuyableKana NHiragana = Hiragana("N-initial", "Unlock the 5 hiragana beginning with a 'n' sound.", "なにぬねの", VowelHiragana);
    public static readonly BuyableKana HHiragana = Hiragana("H-initial", "Unlock the 5 hiragana beginning with a 'h' sound.", "はひふへほ", KHiragana, NHiragana, MHiragana, RHiragana);
    public static readonly BuyableKana BHiragana = Hiragana("B-initial", "Unlock the 5 hiragana beginning with a 'b' sound. These are modified versions of the 'h' series kana.", "ばびぶべぼ", HHiragana);
    public static readonly BuyableKana PHiragana = Hiragana("P-initial", "Unlock the 5 hiragana beginning with a 'p' sound. These are modified versions of the 'h' series kana.", "ぱぴぷぺぽ", HHiragana);
    public static readonly BuyableKana MHiragana = Hiragana("M-initial", "Unlock the 5 hiragana beginning with a 'm' sound.", "まみむめも", VowelHiragana);
    public static readonly BuyableKana YHiragana = Hiragana("Y-initial", "Unlock the 3 hiragana beginning with a 'y' sound.", "やゆよ", SHiragana, THiragana, HHiragana);
    public static readonly BuyableKana RHiragana = Hiragana("R-initial", "Unlock the 5 hiragana beginning with a 'r' sound.", "らりるれろ", VowelHiragana);
    public static readonly BuyableKana WHiragana = Hiragana("W-initial and 'n'", "Unlock the 2 hiragana beginning with a 'w' sound and 'n'.", "わをん", SHiragana, THiragana, HHiragana);

    public static readonly BuyableKana KyHiragana = HiraganaCombo("Ky-initial", "Unlock the 3 hiragana beginning with a 'ky' sound.", "きゃ,きゅ,きょ,", KHiragana, YHiragana);
    public static readonly BuyableKana ShyHiragana = HiraganaCombo("Shy-initial", "Unlock the 3 hiragana beginning with a 'shy' sound.", "しゃ,しゅ,しょ,", SHiragana, YHiragana);
    public static readonly BuyableKana ChyHiragana = HiraganaCombo("Chy-initial", "Unlock the 3 hiragana beginning with a 'chy' sound.", "ちゃ,ちゅ,ちょ,", THiragana, YHiragana);
    public static readonly BuyableKana NyHiragana = HiraganaCombo("Ny-initial", "Unlock the 3 hiragana beginning with a 'ny' sound.", "にゃ,にゅ,にょ,", NHiragana, YHiragana);
    public static readonly BuyableKana HyHiragana = HiraganaCombo("Hy-initial", "Unlock the 3 hiragana beginning with a 'hy' sound.", "ひゃ,ひゅ,ひょ,", HHiragana, YHiragana);
    public static readonly BuyableKana MyHiragana = HiraganaCombo("My-initial", "Unlock the 3 hiragana beginning with a 'my' sound.", "みゃ,みゅ,みょ,", MHiragana, YHiragana);
    public static readonly BuyableKana RyHiragana = HiraganaCombo("Ry-initial", "Unlock the 3 hiragana beginning with a 'ry' sound.", "りゃ,りゅ,りょ,", RHiragana, YHiragana);

    static List<BuyableKana> _values;
    public static IEnumerable<BuyableKana> Values { get { return _values; } }

    static BuyableKana Hiragana(string prefex, string description, string kana, params BuyableKana[] prereqs) {
        var kanaList = "(";
        foreach (var k in kana.ToCharArray()) {
            kanaList += k + "・";
        }
        kanaList = kanaList.Substring(0, kanaList.Length - 1) + ")";
        description = kanaList + "\n\n" + description;

        description += "\n\n<i>Hiragana is one of the 3 Japanese alphabets. Mastering hiragana will give you a salary bonus.</i>";
        return new BuyableKana(1000, prefex + " hiragana", description, kana, prereqs);
    }

    static BuyableKana HiraganaCombo(string prefex, string description, string kana, params BuyableKana[] prereqs) {
        var kanaList = "(";
        foreach (var k in kana.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)) {
            kanaList += k + "・";
        }
        kanaList = kanaList.Substring(0, kanaList.Length - 1) + ")";
        description = kanaList + "\n\n" + description;

        description += "\n\n<i>These kana use a combination of a small 'y' hiragana with a consonant sound to produce a different sound. Mastering hiragana will give you a salary bonus.</i>";
        return new BuyableKana(1000, prefex + " hiragana", description, kana, prereqs);
    }

    public int Cost { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public BuyableAvailability Availability { get { return GetAvailable(); } }
    public List<char> Kana { get; private set; }

    public bool Viewed {
        get { return PlayerData.Instance.Flags.GetOrCreateFlagSet(ViewedSet).Contains(KanaString); }
        set { PlayerData.Instance.Flags.GetOrCreateFlagSet(ViewedSet).Add(KanaString); }
    }

    string KanaString {
        get {
            var s = "";
            foreach (var c in Kana) {
                s += c;
            }
            return s;
        }
    }

    List<BuyableKana> prerequisites = new List<BuyableKana>();

    BuyableKana(int cost, string name, string description, string kana, params BuyableKana[] prereqs) {
        Cost = cost;
        Name = name;
        Description = description;
        Kana = new List<char>(kana.ToCharArray());

        if (_values == null) {
            _values = new List<BuyableKana>();
        }
        _values.Add(this);

        foreach (var pre in prereqs) {
            if (pre != null) {
                prerequisites.Add(pre);
            }
        }
    }

    public void AfterBuyItem() {
        foreach (var c in Kana) {
            PlayerData.Instance.KanaReviews.AddReview(c.ToString());
        }
    }

    public BuyableAvailability GetAvailable() {
        if (PlayerData.Instance.KanaReviews.ContainsReview(Kana[0].ToString())) {
            return BuyableAvailability.Purchased;
        } else if (PrerequisitesFulfilled()) {
            return BuyableAvailability.Available;
        }
        return BuyableAvailability.Locked;
    }

    public bool PrerequisitesFulfilled() {
        foreach (var p in prerequisites) {
            if (p.Availability != BuyableAvailability.Purchased) {
                return false;
            }
        }
        return true;
    }

}
