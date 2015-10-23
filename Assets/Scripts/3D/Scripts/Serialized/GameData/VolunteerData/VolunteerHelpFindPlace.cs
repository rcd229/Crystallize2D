using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class VolunteerHelpFindPlace : StaticSerializedTaskGameData<VolunteerTaskData> {
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			//set dialogue
			SetDialogue<VolunteerDialogue01>();
			//set question and answer data
			var restaurantPhrase = GetPhrase("restaurant");
			var coffeeShopPhrase = GetPhrase("coffee shop");
			var hotelPhrase = GetPhrase("hotel");
			var theatrePhrase = GetPhrase("theatre");
			var hungryPhrase = GetPhrase("hungry");
			var thirstyPhrase = GetPhrase("thirsty");
			var tiredPhrase = GetPhrase("tired");
			var boredPhrase = GetPhrase("bored");

			task.AddQA(hungryPhrase, restaurantPhrase);
			task.AddQA(thirstyPhrase, coffeeShopPhrase);
			task.AddQA (tiredPhrase, hotelPhrase);
			task.AddQA (boredPhrase, theatrePhrase);
			//initialize player dialogue
			SetAnswerDialogue<VolunteerDialogue02>();
			//other initialization
			Initialize("Volunteer task", "VolunteerTest", "Asker");
			SetProcess<VolunteerProcess>();
		}

		#endregion

		protected void SetAnswerDialogue<T> () where T: CrystallizeData.StaticSerializedDialogueGameData, new(){
			task.AnswerDialogue = new T().GetDialogue();
		}
	}
}