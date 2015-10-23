using UnityEngine;
using System.Collections;


namespace CrystallizeData{
	public class VolunteerDialogue02 : StaticSerializedDialogueGameData {
		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			AddActor("Player");
			AddLine ("You should go to [place]");
		}
		#endregion
		
	}
}