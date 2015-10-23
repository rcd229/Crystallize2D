using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseProgression {

    public PhraseSequence Phrase { get; set; }
    public List<List<bool>> MissingWords { get; set; }

    public int Steps {
        get {
            return MissingWords.Count;
        }
    }

    public PhraseProgression() {
        Phrase = new PhraseSequence();
        MissingWords = new List<List<bool>>();
    }

    public void AddStep() {
        MissingWords.Add(new List<bool>());
    }

    public void RemoveLastStep() {
        if (MissingWords.Count > 0) {
            MissingWords.RemoveAt(MissingWords.Count - 1);
        }
    }

    public bool GetWordMissing(int step, int word) {
        int index = Mathf.Clamp(step, 0, MissingWords.Count - 1);
        //if (step < MissingWords.Count) {
        if (word < MissingWords[index].Count) {
            return MissingWords[index][word];
        }
        return false;
        //}
        //return false;
    }

    public void SetWordMissing(int step, int word, bool value) {
        while (MissingWords.Count <= step) {
            AddStep();
        }

        while (MissingWords[step].Count <= word) {
            MissingWords[step].Add(false);
        }

        MissingWords[step][word] = value;
    }

}
