using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystallizeData {
    class GreetingDialogue14 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Person01");
            AddActor("Person02");

            AddMessage("During your touring, you overhear two people talking.");
//            AddAnimation(new GestureDialogueAnimation("Bow"), 0);
            AddLine("good morning", 1);
            AddLine("good morning", 0);
            AddMessage("After hearing the conversation, you continue to tour around for the rest of the day.");
        }
    }
}
