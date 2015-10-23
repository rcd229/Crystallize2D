using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class WordCollectionPlayerData {

    public List<int> FoundWords { get; set; }

    public WordCollectionPlayerData() {
        FoundWords = new List<int>();
    }

    public bool AddFoundWord(PhraseSequenceElement word) {
        return AddFoundWord(word.WordID);
    }

    public bool AddFoundWord(int wordID) {
        if (!FoundWords.Contains(wordID)) {
            FoundWords.Add(wordID);

            // TODO: should not be here, need to keep events out of data classes
            CrystallizeEventManager.PlayerState.RaiseGameEvent(this, System.EventArgs.Empty);

            DataLogger.LogTimestampedData("Found", wordID.ToString());
            return true;
        }
        return false;
    }

    public void RemoveFoundWord(int wordID) {
        if (FoundWords.Contains(wordID)) {
            FoundWords.Remove(wordID);
        }
    }

    public bool ContainsFoundWord(PhraseSequenceElement word) {
        return ContainsFoundWord(word.WordID);
    }

    public bool ContainsFoundWord(int wordID) {
        return FoundWords.Contains(wordID);
    }

}