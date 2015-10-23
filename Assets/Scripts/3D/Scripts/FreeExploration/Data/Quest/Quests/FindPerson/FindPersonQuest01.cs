using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[SerializedQuestAttribute]
public class FindPersonQuest01 : BaseFindPersonQuest {
    public override NPCID NPCID { get { return new NPCID("3d91b9f026b14c45b5447a3c2bca2633"); } }
    public override string SeekPromptKey { get { return "I'm looking for a [person]"; } }
    public override int RewardMoney { get { return 500; } }
    public override int Tier { get { return 0; } }

    public override NPCCharacterData CharacterData {
        get {
            var charData = new NPCCharacterData();
            charData.Name = "Looking for someone";
            charData.Appearance = new AppearancePlayerData();
            charData.Appearance.Gender = (int)AppearanceGender.Female;
            charData.Animation = NPCAnimationData.Relaxed;
            return charData;
        }
    }

    public override void GetKeys(out string personKey, out string attributeKey) {
        personKey = PersonAttributes.MaleFemale().GetRandom();
        attributeKey = "";//PersonAttributes.PersonStrings().GetRandom();
    }
}
