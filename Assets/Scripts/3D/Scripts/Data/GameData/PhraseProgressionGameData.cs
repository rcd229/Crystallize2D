using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseProgressionGameData {

    public List<PhraseProgression> Progressions { get; set; }

    public PhraseProgressionGameData() {
        Progressions = new List<PhraseProgression>();
    }

    public PhraseProgression GetProgression(PhraseSequence phrase) {
        foreach (var p in Progressions) {
            if (PhraseSequence.PhrasesEquivalent(p.Phrase, phrase)) {
                return p;
            }
        }
        return null;
    }

    public PhraseProgression GetOrCreateProgression(PhraseSequence phrase) {
        var p = GetProgression(phrase);
        if (p == null) {
            p = new PhraseProgression();
            p.Phrase = phrase;
            Progressions.Add(p);
        }
        return p;
    }

}
