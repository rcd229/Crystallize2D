using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class PointPlaceDialogue01 : StaticSerializedDialogueGameData {
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			AddActor("Asker");
			AddLine("I want to go to [place]");
		}
		#endregion
		
	}
}
