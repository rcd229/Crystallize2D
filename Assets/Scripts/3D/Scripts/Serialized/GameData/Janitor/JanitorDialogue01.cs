using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class JanitorDialogue01 : StaticSerializedDialogueGameData {

        protected override void PrepareGameData() {
            //AddActor("");
            AddAnimation(new EquipDialogueAnimation("Broom"));
            AddLine("Ah, the new recruit has arrived.");
            AddLine("Let's get started.");
            AddLine("Let's clean this place up.");
            AddAnimation(new GestureDialogueAnimation(AnimatorState.Give));
        }

    }
}
