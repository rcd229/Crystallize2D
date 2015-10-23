using UnityEngine;
using System.Collections;

namespace CrystallizeData {
    public class VisitRestaurant01 : StaticSerializedTaskGameData<JobTaskGameData> {

        protected override void PrepareGameData() {
            Initialize("Visit the restaurant", "RestaurantTest", "Observer");
            SetProcess<StandardConversationProcess>();
            SetDialogue<RestaurantDialogue01>();
        }

    }
}