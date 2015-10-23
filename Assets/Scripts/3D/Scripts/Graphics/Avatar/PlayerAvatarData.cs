using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlayerAvatarData {

    public string Name { get; set; }
    public AppearancePlayerData Appearance { get; set; }

    public PlayerAvatarData() {
        this.Name = "";
        this.Appearance = new AppearancePlayerData();
    }

    public PlayerAvatarData(string name, AppearancePlayerData appearance) {
        this.Name = name;
        this.Appearance = appearance;
    }

}
