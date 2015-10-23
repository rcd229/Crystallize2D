using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystallizeData {
    class RestaurantPatronDialogue01 : StaticSerializedDialogueGameData {
        protected override void PrepareGameData() {
            AddActor("Person03");
            AddActor("Person04");

            AddMessage("In the restaurant, you overhear two people talking.");
            AddLine("What will you eat?", 1);
            AddLine("Sushi please.", 0);
            AddLine("Okay.", 1);
            AddMessage("After hearing the conversation, you continue your meal.");
        }
    }
}