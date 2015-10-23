using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class NPCCharacterData {
    public static NPCCharacterData GetRandom() {
        var charData = new NPCCharacterData();
        charData.Guid = Guid.NewGuid();
        charData.Name = RandomNameGenerator.GetRandomMaleName();
        charData.Appearance = AppearancePlayerData.GetRandom();
        charData.Animation = NPCAnimationData.Normal;
        return charData;
    }

    public Guid Guid { get; private set; }
    public string Name { get; set; }
    public AppearancePlayerData Appearance { get; set; }
    public NPCAnimationData Animation { get; set; }
    public FacialAnimationType FacialAnimationType { get; set; }
}
