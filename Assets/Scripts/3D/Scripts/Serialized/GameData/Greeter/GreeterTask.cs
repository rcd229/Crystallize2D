using UnityEngine;
using System.Collections;

namespace CrystallizeData {
    public class GreeterTask01 : StaticSerializedTaskGameData<JobTaskGameData> {

        protected override void PrepareGameData() {
            Initialize(Name, "SchoolPool", "Boss");
            SetProcess<GreeterProcess01>();
        }

    }

    public class GreeterTask02 : StaticSerializedTaskGameData<JobTaskGameData> {

        protected override void PrepareGameData() {
            Initialize(Name, "StreetSession", "Customer");
            SetProcess<GreeterProcess>();
        }

    }
}