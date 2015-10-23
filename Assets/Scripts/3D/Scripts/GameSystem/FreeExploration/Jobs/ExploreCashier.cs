using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData{
	public class ExploreCashier : Cashier {

		protected override void PrepareGameData ()
		{
			base.PrepareGameData ();
			Initialize(JobID.ExploreCashier, "ExploreCashier", 0, 1000);
			job.Tasks = new List<JobTaskGameData>();
			AddTask<ExploreCashierSellGoods>();
			job.Hide = true;
		}
	}
}
