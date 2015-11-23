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
        CurrentElement = Dialogue.Elements.Get(0);
        Actors = actors.ToList();
    }

    public void MoveTo(int elementId) {
        if (Dialogue.Elements.ContainsKey(elementId)) {
            CurrentElement = Dialogue.GetElement(elementId);
        } else {
            CurrentElement = null;
        }
    }
}
