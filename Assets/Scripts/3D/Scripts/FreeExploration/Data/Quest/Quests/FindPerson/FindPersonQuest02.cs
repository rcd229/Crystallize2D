using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[SerializedQuestAttribute]
public class FindPersonQuest02 : BaseFindPersonQuest {
    public override NPCID NPCID { get { return new NPCID("8a520ef5b8f34419a8153920e405aa7f"); } }
    public override string SeekPromptKey { get { return "I'm looking for a [attribute] [person]"; } }
    public override int RewardMoney { get { return 1000; } }
    public override int Tier { get { return 1; } }

    public override NPCCharacterData CharacterData {
        get {
            var charData = new NPCCharacterData();
            charData.Name = "Looking for someone";
            charData.Appearance = new AppearancePlayerData();
            charData.Appearance.Gender = (int)AppearanceGender.Female;
            charData.Appearance.TopMaterial = (int)AppearanceShirt01Material.Stripe;
            charData.Animation = NPCAnimationData.Relaxed;
            return charData;
        }
    }

    public override void GetKeys(out string personKey, out string attributeKey) {
        personKey = PersonAttributes.PersonStrings().GetRandom();
        attributeKey = PersonAttributes.HairStrings().GetRandom();
    }
}
