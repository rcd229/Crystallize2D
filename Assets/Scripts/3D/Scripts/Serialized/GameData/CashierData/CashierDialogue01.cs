using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class CashierDialogue01 : StaticSerializedDialogueGameData {
		protected override void PrepareGameData() {

//			AddActor("Cashier");
			AddActor("Customer01");
			
			AddAnimation(new GestureDialogueAnimation("Bow"));
			AddLine("[greeting]");

		}
	}
}
