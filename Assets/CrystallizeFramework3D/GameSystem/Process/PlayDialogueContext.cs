using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayDialogueContext {
    public DialogueSequence Dialogue { get; set; }
    public DialogueElement CurrentElement { get; set; }
    public List<GameObject> Actors { get; set; }

    public GameObject Actor {
        get {
            return Actors.FirstOrDefault();
        }
    }

    public PlayDialogueContext(DialogueSequence dialogue, params GameObject[] actors) {
        Dialogue = dialogue;
        Actors = actors.ToList();
    }
}
