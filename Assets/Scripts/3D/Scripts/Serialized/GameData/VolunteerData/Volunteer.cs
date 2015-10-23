using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class Volunteer : StaticSerializedJobGameData {
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			Initialize(JobID.Volunteer1, "Volunteer");
			AddTask<VolunteerHelpFindPlace>();
            
			job.Requirements.Add(new PreviousJobRequirementGameData(JobID.PetFeeder1));
			job.Requirements.Add(new PreviousJobRequirementGameData(JobID.Guide1));
			job.AddRequirement(GetPhrase("hungry"));
			job.AddRequirement(GetPhrase("thirsty"));
			job.AddRequirement(GetPhrase("restaurant"));
			job.AddRequirement(GetPhrase("coffee shop"));
			job.TaskSelector = new VariationListSelectorGameData(2);

            job.Hide = true;
		}

		#endregion


	}
}