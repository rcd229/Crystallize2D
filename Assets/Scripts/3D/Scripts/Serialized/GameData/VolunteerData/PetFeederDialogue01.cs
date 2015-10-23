using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class PetFeederDialogue01 : StaticSerializedDialogueGameData{
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			AddActor("PetOwner");
			AddLine("the cat is [need]");
		}

		#endregion



	}
}
