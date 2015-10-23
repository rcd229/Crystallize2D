using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class GreeterDialogue01 : StaticSerializedDialogueGameData {

        BranchRef b1 = new BranchRef();
        BranchRef b2 = new BranchRef();
        BranchRef b3 = new BranchRef();

        protected override void PrepareGameData() {
            AddActor("Customer");

            b1.Prompt = GetPhrase("hello");
            b2.Prompt = GetPhrase("goodbye");
            b3.Prompt = GetPhrase("welcome");

            AddMessage("Customers flow into the shop.");
            AddBranch(new BranchRef[] { b1, b2, b3 });
            
            b1.Index = AddAnimation(new GestureDialogueAnimation("Bow"));
            AddLine("hello");
            Break();

            b2.Index = AddLine("What?");
            Break();

            b3.Index = AddAnimation(new GestureDialogueAnimation("Surprise"));
            AddLine("Your Japanese is great!");
        }
    }
}
