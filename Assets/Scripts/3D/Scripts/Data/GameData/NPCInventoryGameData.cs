using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCInventoryGameData : ISerializableDictionaryItem<int> {

    public int Key {
        get {
            return GlobalID;
        }
    }

    public int GlobalID { get; set; }
    public List<int> ItemIDs = new List<int>();

    public NPCInventoryGameData() {
        GlobalID = -1;
        ItemIDs = new List<int>();
    }

    public NPCInventoryGameData(int globalID) {
        GlobalID = globalID;
    }

}
