using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemResourceGameData {

    public int itemID;
    public Sprite icon;

    public ItemResourceGameData(int itemID) {
        this.itemID = itemID;
    }

}
