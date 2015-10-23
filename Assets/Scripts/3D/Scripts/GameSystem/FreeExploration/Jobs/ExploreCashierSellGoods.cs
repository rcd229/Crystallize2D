using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class ExploreCashierSellGoods : CashierSellGoods {

		protected override void PrepareGameData ()
		{
			base.PrepareGameData ();
			SetProcess<CashierExploreProcess>();
		}
	}
}
