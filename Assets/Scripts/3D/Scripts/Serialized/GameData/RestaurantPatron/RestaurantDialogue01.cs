using UnityEngine;
using System.Collections;

namespace CrystallizeData {
    public class RestaurantDialogue01 : StaticSerializedDialogueGameData {

        protected override void PrepareGameData() {
            AddActor("Waiter");
            AddActor("Customer");

            AddAnimation(new GestureDialogueAnimation("Bow"));
            AddLine("Welcome to the restaurant. How many in your party?", 0);
            AddLine("x people.", 1);
            AddLine("Please, this way.", 0);
        }

    }
}