using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

namespace CrystallizeData {
    public class StandardDialogueTask : StaticSerializedTaskGameData<JobTaskGameData> {
        protected override void PrepareGameData() {
            Initialize(Name, "RestaurantTest", "Observer");
            SetProcess<StandardConversationProcess>();
        }
    }

    public class FindPersonTask : StaticSerializedTaskGameData<JobTaskGameData> {
        protected override void PrepareGameData() {
            Initialize(Name, "RestaurantTest", "Observer");
            SetProcess<FindPersonProcess>();
        }
    }
}
