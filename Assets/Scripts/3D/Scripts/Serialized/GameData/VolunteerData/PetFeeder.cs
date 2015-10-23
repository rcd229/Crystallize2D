using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class PetFeeder : StaticSerializedJobGameData {
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			Initialize(JobID.PetFeeder1, "PetFeeder");
			AddTask<FeedPets>();
			job.TaskSelector = new VariationListSelectorGameData(2);

            job.Hide = true;
			job.formalLevel = JobFormalLevel.Versatile;
			AddLine(PetFeederProcess.goodJob);
			AddLine(PetFeederProcess.OkJob);
			AddLine(PetFeederProcess.badJob);
			AddLine(PetFeederProcess.terribleJob);
		}
		#endregion
		
	}
}
