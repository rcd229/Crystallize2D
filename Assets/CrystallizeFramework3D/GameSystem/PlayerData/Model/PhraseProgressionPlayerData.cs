using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseProgressionPlayerData {

    public PhraseSequence Phrase { get; set; }
    public HashSet<int> UsedOnActors { get; set; }

    public int Step {
        get {
            return UsedOnActors.Count;
        }
    }

    public PhraseProgressionPlayerData() {
        Phrase = new PhraseSequence();
        UsedOnActors = new HashSet<int>();
    }

    public void AddActor(int globalID) {
        if (!UsedOnActors.Contains(globalID)) {
            UsedOnActors.Add(globalID);
        }
    }

}
