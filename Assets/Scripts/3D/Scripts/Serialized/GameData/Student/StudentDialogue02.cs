using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystallizeData {
    class StudentDialogue02 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Person01");
            AddActor("Person02");

            AddMessage("During lunchtime, you overhear two people talking.");
            AddAnimation(new GestureDialogueAnimation("PointOnce"), 0);
            AddLine("What is this?", 0);
            AddLine("This is a desk.", 1);
            AddLine("I see.", 0);
            AddLine("Do you remember?", 1);
            AddLine("Um...", 0);
            AddMessage("After hearing the conversation, you continue to attend classes for the rest of the day.");
        }
    }
}
