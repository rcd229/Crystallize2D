﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class NPC2D : Object2D, IHasCollider {
    public string DialogueKey { get; set; }

    //public DialogueSequence Dialogue {
    //    get {
    //        return DialoguePipeline.GetDialogue(DialogueKey);
    //    }
    //}
    //public List<DialogueEntry2D> Dialogues { get; set; }

    //[JsonIgnore]
    //public DialogueEntry2D Dialogue {
    //    get {
    //        if(Dialogues.Count == 0) {
    //            Dialogues.Add(new DialogueEntry2D());
    //        }
    //        return Dialogues[0];
    //    }
    //}

    public NPC2D() {
        DialogueKey = "";
    //    Dialogues = new List<DialogueEntry2D>();
    }
}
