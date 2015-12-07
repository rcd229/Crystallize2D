using UnityEngine;
using System.Collections.Generic;
using System;

public class WordInventory : PlayerDataElement {

    int _availableSlots;
    public int AvailableSlots {
        get { return _availableSlots; }
        set {
            _availableSlots = value;
            RaiseDataChanged();
        }
    }

    public List<PhraseSequence> Words { get; set; }

    public WordInventory() {
        AvailableSlots = 4;
        Words = new List<PhraseSequence>();
    }

    public PhraseSequence GetElement(int index) {
        if (index < 0 || index >= Words.Count) {
            return null;
        }

        return Words[index];
    }

    public void SetElement(int index, PhraseSequence word) {
        if (index < 0) {
            throw new System.IndexOutOfRangeException("Word index must be greater than 0");
        }

        while (Words.Count <= index) {
            Words.Add(null);
        }
        Words[index] = word;

        RaiseDataChanged();
    }
}
