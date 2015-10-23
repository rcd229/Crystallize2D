using UnityEngine;
using System.Collections;

public class PhraseJobRequirementGameData : JobRequirementGameData {

    public PhraseSequence Phrase { get; set; }

    public PhraseJobRequirementGameData() {
        Phrase = new PhraseSequence();
    }

    public PhraseJobRequirementGameData(PhraseSequence phrase) : this() {
        Phrase = phrase;
    }

    public override bool IsFulfilled() {
        if (Phrase.IsWord) {
            return PlayerData.Instance.WordStorage.ContainsFoundWord(Phrase.Word);
        } else {
            return PlayerData.Instance.PhraseStorage.ContainsPhrase(Phrase);
        }
    }

}