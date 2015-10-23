using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData{
	public class ExploreGuide  : PlacePointer01{
		protected override void PrepareGameData ()
		{
			base.PrepareGameData ();
			Initialize (JobID.ExploreGuide, "Explore Guide 1");
			job.Tasks = new List<JobTaskGameData>();
			AddTask<ExploreGuideTask1>();
			job.Hide = true;
		}

	}
}
