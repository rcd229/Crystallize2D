using UnityEngine;
using System.Collections;

public class NPCDialogueHolder : ISerializableDictionaryItem<int> {
    
    public int Key {
        get {
            return GlobalID;
        }
    }

    public int GlobalID { get; set; }
    public int DialogueID { get; set; }

    public NPCDialogueHolder() {
        GlobalID = -1;
        DialogueID = -1;
    }

    public NPCDialogueHolder(int gid, int did) {
        GlobalID = gid;
        DialogueID = did;
    }

}
