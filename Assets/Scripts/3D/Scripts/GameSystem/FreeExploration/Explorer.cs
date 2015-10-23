using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class Explorer : StaticSerializedJobGameData {

		protected override void PrepareGameData() {
			Initialize(JobID.Explorer, "Explorer", 0, 1000);
			AddTask<ExplorerTask>();
			job.formalLevel = JobFormalLevel.Formal;
			//set promotion criteria
			if (PromotionTaskData.UsePromotion) {
				job.PromotionReq = 15;
				job.PromotionMistakes = 1;
				job.PromotionTask = job.Tasks[0];
			}
			
			job.TaskSelector = new JobTaskSelectorGameData();
			job.Money = 0;;
			
			job.Hide = true;
		}
	}
}
