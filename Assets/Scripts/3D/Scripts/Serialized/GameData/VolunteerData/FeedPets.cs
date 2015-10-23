using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class FeedPets : StaticSerializedTaskGameData<PetFeederTaskData> {
		#region implemented abstract members of StaticGameData

		protected override void PrepareGameData ()
		{
			var hungryPhrase = GetPhrase("hungry");
			var thirstyPhrase = GetPhrase("thirsty");
			var tiredPhrase = GetPhrase("tired");
			var boredPhrase = GetPhrase("bored");

			//set question and answer data
			task.AddQA(hungryPhrase, new PhraseSequence("fish"), new Sprite());
			task.AddQA(thirstyPhrase, new PhraseSequence("milk"), new Sprite());
			task.AddQA (tiredPhrase, new PhraseSequence("bed"), new Sprite ());
			task.AddQA (boredPhrase, new PhraseSequence("toy"), new Sprite ());

			//other initialization
			Initialize("PetFeederTask", "PetFeederTest1", "PetOwner");
			SetProcess<PetFeederProcess>();
			SetDialogue<PetFeederDialogue01>();
			SetProps(0, 1, 5, 7);
		}

		#endregion


	}
}