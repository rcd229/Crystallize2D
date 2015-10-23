using UnityEngine;
using System.Collections;

public class ItemGameData : ISerializableDictionaryItem<int>, ISetableKey<int> {

    public int Key {
        get {
            return ItemID;
        }
    }

    public int ItemID { get; set; }
    public PhraseSequence Name { get; set; }

    public ItemGameData() {
        ItemID = -1;
        Name = new PhraseSequence();
    }

    public ItemGameData(int id) : this(){
        ItemID = id;
    }

    public void SetKey(int key) {
        ItemID = key;
    }

}
