using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerActorLine : DialogueActorLine {

    public bool OverrideGivenWords { get; set; }
    public bool ProvideMissingWordsMessage { get; set; }
    public bool UseAutomaticProgression { get; set; }
    public List<bool> GivenWords { get; set; }

    public PlayerActorLine()
        : base() {
        ProvideMissingWordsMessage = true;
        GivenWords = new List<bool>();
    }

    public void SetOverrideGivenWords(bool val) {
        if (val) {
            if (GivenWords == null) {
                GivenWords = new List<bool>();
            }
            UseAutomaticProgression = false;
        } else {
            GivenWords = null;
        }
        OverrideGivenWords = val;
    }

    public void SetUseAutomaticProgression(bool val) {
        if (val) {
            GivenWords = null;
            OverrideGivenWords = false;
        }
        UseAutomaticProgression = val;
    }

    public bool GetWordGiven(int index) {
        if (GivenWords.Count <= index) {
            return true;
        }

        return GivenWords[index];
    }

    public void SetWordGiven(int index, bool given) {
        if(GivenWords.Count < index && given){
            return;
        }

        while (GivenWords.Count <= index) {
            GivenWords.Add(true);
        }

        GivenWords[index] = given;
    }

    public List<int> GetMissingWords() {
        var mws = new List<int>();

        if (UseAutomaticProgression) {
            var missing = PlayerDataConnector.GetMissingWords(Phrase);
            for (int i = 0; i < Phrase.PhraseElements.Count; i++) {
                if (missing[i]) {
                    mws.Add(Phrase.PhraseElements[i].WordID);
                }
            }
        } else if (OverrideGivenWords) {
            for (int i = 0; i < Phrase.PhraseElements.Count; i++) {
                if (!GetWordGiven(i)) {
                    mws.Add(Phrase.PhraseElements[i].WordID);
                }
            }
        } else {
            for (int i = 0; i < Phrase.PhraseElements.Count; i++) {
                var e = Phrase.PhraseElements[i];

                if (e.GetPhraseCategory() == PhraseCategory.Unknown) {
                    continue;
                }

                if (e.GetPhraseCategory() == PhraseCategory.Particle) {
                    continue;
                }

                if (e.GetPhraseCategory() == PhraseCategory.Punctuation) {
                    continue;
                }

                mws.Add(e.WordID);
            }
        }
        return mws;
    }

}
