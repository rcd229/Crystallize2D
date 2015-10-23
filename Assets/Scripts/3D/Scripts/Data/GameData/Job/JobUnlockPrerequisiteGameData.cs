using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JobUnlockPrerequisiteGameData {

    public PhraseSequence Words { get; set; }
    public List<string> Jobs { get; set; }

    public JobUnlockPrerequisiteGameData() {
        Words = new PhraseSequence();
        Jobs = new List<string>();
    }

}