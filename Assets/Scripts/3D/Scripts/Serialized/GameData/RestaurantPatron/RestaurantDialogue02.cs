using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData {
    public class RestaurantDialogue02 : StaticSerializedDialogueGameData {

        BranchRef right = new BranchRef();
        BranchRef wrong = new BranchRef();

        protected override void PrepareGameData() {
            AddActor("Waiter");

            AddLine("Welcome to the restaurant.");
            AddLine("What will you eat?");

            right.Index = AddLine("good choice");
            Break();

            wrong.Index = AddLine("that's not a food");
            Break();
        }

        List<TextMenuItem> GetItems() {
            var item1 = new TextMenuItem();
            item1.text = "table";
			var item2 = new TextMenuItem();
            item2.text = "beef";
            return new List<TextMenuItem>(new TextMenuItem[]{item1, item2});
        }

        int SelectNext(TextMenuItem selected) {
            if (selected.text == "beef") {
                return right.Index;
            } else {
                return wrong.Index;
            }
        }

    }
}