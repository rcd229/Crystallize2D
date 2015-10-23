using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInventoryPlayerData {

    const int MaxCount = 4;

    public List<int> ItemIDs { get; set; }

    public ItemInventoryPlayerData() {
        ItemIDs = new List<int>();
    }

    public bool AddItem(int itemID) {
        for (int i = 0; i < Mathf.Min(ItemIDs.Count, MaxCount); i++) {
            if (ItemIDs[i] == 0) {
                ItemIDs[i] = itemID;
                return true;
            }
        }

        if (ItemIDs.Count < MaxCount) {
            ItemIDs.Add(itemID);
            return true;
        }

        return false;
    }

    public void SetItem(int index, int itemID) {
        while (ItemIDs.Count <= index) {
            ItemIDs.Add(0);
        }
        ItemIDs[index] = itemID;
    }

    public int GetItem(int index) {
        if (index >= ItemIDs.Count) {
            return 0;
        }

        return ItemIDs[index];
    }

}
