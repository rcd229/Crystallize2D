using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HomePlayerData : ISerializableDictionaryItem<int>, ISetableKey<int> {

    public int HomeID { get; set; }
    public bool Unlocked {get; set;}

    public int Key {
        get { return HomeID; }
    }

    public HomePlayerData() {
        HomeID = -1;
        Unlocked = false;
    }

    public HomePlayerData(int id) : this() {
        HomeID = id;
    }

    public HomePlayerData(int jobID, bool unlocked) {
        HomeID = jobID;
    }

    public void SetKey(int key) {
        HomeID = key;
    }

}
