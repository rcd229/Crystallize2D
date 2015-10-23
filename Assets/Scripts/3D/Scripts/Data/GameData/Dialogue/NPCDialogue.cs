using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCDialogue : ISerializableDictionaryItem<int> {

    public int Key {
        get {
            return ID;
        }
    }

    public int ID { get; set; }
    public List<DialogueActorLine> Lines { get; set; }
    public List<int> ActorIndicies { get; set; }

	public NPCDialogue() {
        ID = -1;
        Lines = new List<DialogueActorLine>();
        ActorIndicies = new List<int>();
    }

    public NPCDialogue(int id)
        : this() {
        ID = id;
    }

    public int GetActorIndex(DialogueActorLine line) {
        var i = Lines.IndexOf(line);
        while (ActorIndicies.Count < Lines.Count) {
            ActorIndicies.Add(0);
        }
        return ActorIndicies[i];
    }

    public void SetActorIndex(DialogueActorLine line, int value) {
        var i = Lines.IndexOf(line);
        while (ActorIndicies.Count < Lines.Count) {
            ActorIndicies.Add(0);
        }
        ActorIndicies[i] = value;
    }

    public void AddLine() {
        ActorIndicies.Add(Lines.Count % 2);
        Lines.Add(new NPCActorLine());
    }

}
