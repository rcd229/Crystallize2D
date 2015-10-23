using UnityEngine;
using System.Collections;

namespace CrystallizeData {
    public class MeetOnStreet01 : StaticSerializedTaskGameData<JobTaskGameData> {

        protected override void PrepareGameData() {
            Initialize("Take a walk", "RestaurantTest", "Observer");
            SetProcess<StandardConversationProcess>();
            SetDialogue<HelloDialogue01>();
        }

    }
}