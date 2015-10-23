using UnityEngine;
using System.Collections;

public class LinearDialogueHolder : ISerializableDictionaryItem<int> {

    public int Key {
        get {
            return GlobalID;
        }
        set {
            GlobalID = value;
        }
    }

    public int GlobalID { get; set; }
    public int DialogueID { get; set; }

    public LinearDialogueHolder() {
        GlobalID = -1;
        DialogueID = -1;
    }

    public LinearDialogueHolder(int gid, int did) {
        GlobalID = gid;
        DialogueID = did;
    }

}
