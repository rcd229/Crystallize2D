using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class CashierDialogue02 : StaticSerializedDialogueGameData {
		protected override void PrepareGameData() {
//			AddActor("Cashier");
//			AddActor("Customer01");
			

			AddLine("I want to buy [item]");
		}
	}
}
