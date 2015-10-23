using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class PlacePointer01 : StaticSerializedJobGameData{
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			Initialize (JobID.Guide1, "Guide 1");
			AddTask<PointPlace01>();
			job.formalLevel = JobFormalLevel.Versatile;
			AddLine(PointPlace01.coffeeShop);
			AddLine(PointPlace01.groceryStore);
			AddLine(PointPlace01.restaurant);
			AddLine(PointPlaceProcess.thanks);
			AddLine(PointPlace01.badSushi);
			AddLine(PointPlace01.goodCoffee);
			AddLine(PointPlace01.helpful);
			job.TaskSelector = new VariationListSelectorGameData(3);
			job.AddRequirement(JobID.Tourist2);
			job.AddRequirement(GetPhrase("thank you"));
			//promotion
			job.PromotionReq = 12;
			job.PromotionMistakes = 1;
			job.PromotionTask = job.Tasks[0];
			//
			job.Money = 500;
            job.Hide = false;
		}
		#endregion
	}

	public class PlacePointer02 : StaticSerializedJobGameData{
		protected override void PrepareGameData ()
		{
			Initialize (JobID.Guide2, "Guide 2");
			job.formalLevel = JobFormalLevel.Versatile;
			AddTask<PointPlace02>();
//			AddLine(PointPlace01.coffeeShop);
//			AddLine(PointPlace01.groceryStore);
//			AddLine(PointPlace01.restaurant);
			AddLine(PointPlaceProcess.thanks);
			AddLine(PointPlace02.classroom1);
			AddLine(PointPlace02.classroom2);
			AddLine(PointPlace02.restroom);
			AddLine(PointPlace02.exit);
			AddLine(PointPlace02.staircase);

			AddLine(PointPlace02.helpALot);
			AddLine(PointPlace02.notLateForClass);
			job.TaskSelector = new VariationListSelectorGameData(3);
			job.AddRequirement(JobID.Guide1);
			job.AddRequirement(GetPhrase("to go"));
			//promotion
			job.PromotionReq = 18;
			job.PromotionMistakes = 2;
			job.PromotionTask = job.Tasks[0];
			//
			job.Money = 1500;
			job.Hide = false;
		}
	}
}