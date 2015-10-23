using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HomeRef : IntIDRef<HomeGameData, HomePlayerData> {

    public HomeGameData GameDataInstance {
        get {
            return GameData.Instance.Homes.Get(ID);
        }
		set{}
    }

    public HomePlayerData PlayerDataInstance {
        get {
            return PlayerData.Instance.Homes.GetOrCreateItem(ID);
        }
		set{}
    }

    public HomeRef(int id) : base(id) { }

}
