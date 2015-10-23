using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public static class ConfidenceDialogueEventExtensions {

    public static int GetConfidenceCost(this DialogueSequence dialogue) {
        var conf = 0;
        foreach (var e in dialogue.Events) {
            if (e.IsAction<int>(PlayerDataConnector.AddConfidence)) {
                conf += ((ActionDialogueEvent<int>)e).Arg1;
            }
        }
        return conf;
    }

}
