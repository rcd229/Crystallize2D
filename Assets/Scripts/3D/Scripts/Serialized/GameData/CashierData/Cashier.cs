using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class Cashier : StaticSerializedJobGameData {
		protected override void PrepareGameData() {
			Initialize(JobID.Cashier1, "Cashier1", 0, 1000);
			AddTask<CashierSellGoods>();
			job.AddRequirement(GetPhrase("Hello"));
			job.AddRequirement(JobID.Tourist2);
            job.AddRequirement(GetPhrase("good morning"));
            job.AddRequirement(GetPhrase("good evening"));
            job.AddRequirement(GetPhrase("plant"));
			job.formalLevel = JobFormalLevel.Formal;
			//set promotion criteria
          if (PromotionTaskData.UsePromotion) {
//			if(true){
				job.PromotionReq = 15;
                job.PromotionMistakes = 1;
				job.PromotionTask = job.Tasks[0];
            }
			//
			AddLine(CashierProcess.BadJob);
			AddLine(CashierProcess.NiceJob);
			AddLine(CashierProcess.OkayJob);
			AddLine(CashierProcess.WorkHarder);

			job.TaskSelector = new VariationListSelectorGameData(2);
			job.Money = 1500;

            job.Hide = false;
		}

	}

	public class OpenCashier : StaticSerializedJobGameData {

		protected override void PrepareGameData() {
			Initialize(JobID.OpenCashier, "Open Cashier", 0, 1000);
			AddTask<OpenCashierTask>();
			job.AddRequirement(GetPhrase("Hello"));
			job.AddRequirement(GetPhrase("good morning"));
			job.AddRequirement(GetPhrase("good evening"));
			job.AddRequirement(GetPhrase("plant"));
			job.formalLevel = JobFormalLevel.Formal;
			//set promotion criteria
			if (PromotionTaskData.UsePromotion) {
				//			if(true){
				job.PromotionReq = 15000;
				job.PromotionMistakes = 1;
				job.PromotionTask = job.Tasks[0];
			}
			job.QuestID = new CashierUnlockQuest().ID;
			//
			AddLine(CashierProcess.BadJob);
			AddLine(CashierProcess.NiceJob);
			AddLine(CashierProcess.OkayJob);
			AddLine(CashierProcess.WorkHarder);
			
			job.TaskSelector = new VariationListSelectorGameData(2);
			job.Money = 1500;
			GameDataInitializer.AddOpenJob(Name, job);
		}
	}
}
