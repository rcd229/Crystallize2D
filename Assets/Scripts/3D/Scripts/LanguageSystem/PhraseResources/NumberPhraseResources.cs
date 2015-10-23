using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NumberPhraseResources : DefaultPhrasePipelineBuilder, IPhraseResources {
    public static readonly string[] Numbers0to9 = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    public static readonly string[] Numbers10to90 = new string[] { "10", "20", "30", "40", "50", "60", "70", "80", "90" };

    public static PhraseSequence GetPhraseForNumber(int number) {
        if (number > 99 || number < 0) {
            throw new NotImplementedException("Numbers higher than 99 have not yet been implemented");
        }
        if (number < 10) {
            return GetPhrase(Numbers0to9[number]);
        } else {
            var p = new PhraseSequence();
            p.Add(GetPhrase(Numbers10to90[(number / 10) - 1]));
            if (number % 10 != 0) {
                p.Add(GetPhrase(Numbers0to9[number % 10]));
            }
            p.Translation = number.ToString();
            return p;
        }
    }

    public string SetKey { get { return GetType().ToString(); } }
    public IEnumerable<string> GetPhraseKeys() { return Numbers0to9.Concat(Numbers10to90); }
}
