using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystallizeData {
    class StudentDialogue03 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Person01");
            AddActor("Person02");

            AddMessage("During lunchtime, you overhear two people talking.");
            AddAnimation(new GestureDialogueAnimation("PointOnce"), 0);
            AddLine("Um... I forgot again.", 0);
            AddLine("What is this?", 0);
            AddLine("This is a desk.", 1);
            AddLine("Thanks!", 0);
            AddLine("Try your best, okay?", 1);
            AddLine("Okay!", 0);
            AddMessage("After hearing the conversation, you continue to attend classes for the rest of the day.");
        }
    }
}
