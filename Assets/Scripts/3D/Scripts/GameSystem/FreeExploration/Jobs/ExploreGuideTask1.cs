using UnityEngine;
using System.Collections;

namespace CrystallizeData{ 
	public class ExploreGuideTask1 : PointPlace01 {

		protected override void PrepareGameData ()
		{
			base.PrepareGameData ();
			SetProcess<PointPlaceExploreProcess>();
		}
	}
}
