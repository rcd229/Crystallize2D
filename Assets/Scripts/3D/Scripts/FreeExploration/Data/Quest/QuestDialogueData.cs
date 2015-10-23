using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class QuestDialogueData {
    public NPCID NPCID { get; set; }
    public List<QuestDialogueState> DialogueStates { get; set; }

    public QuestDialogueData() {
        DialogueStates = new List<QuestDialogueState>();
    }
}
