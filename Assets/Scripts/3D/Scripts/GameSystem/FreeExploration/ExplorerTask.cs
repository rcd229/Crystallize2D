using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class ExplorerTask : StaticSerializedTaskGameData<JobTaskGameData> {
		protected override void PrepareGameData() {
			Initialize("Explore freely", "FreeExploreSession", "Actor");
			SetProcess<FreeExploreProcess>();
//			SetProcess<ConfidenceFreeExploreProcess>();
			
		}
	}
}
