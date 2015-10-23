using UnityEngine;
using System.Collections;

namespace CrystallizeData{
	public class PointPlace01 : StaticSerializedTaskGameData<PointPlaceTaskData> {
		public const string restaurant = "restaurant";
		public const string coffeeShop = "coffee shop";
		public const string groceryStore = "grocery store";

		public const string goodCoffee = "Thank you! The coffee is great!";
		public const string badSushi = "I am not coming here again.";
		public const string helpful = "Thank you for finding me the store.";

		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			task = new PointPlaceTaskData ();
			//set dialogue
			SetDialogue<PointPlaceDialogue01>();
			//get AfterChats
			var goodCoffeePhrase = GetPhrase(goodCoffee);
			var badSushiPhrase = GetPhrase(badSushi);
			var helpfulPhrase = GetPhrase(helpful);
			//set question and answer data
			var restaurantPhrase = GetPhrase(restaurant);
			var coffeeShopPhrase = GetPhrase(coffeeShop);
			var storePhrase = GetPhrase(groceryStore);
			task.AddQAAndAfterChat(restaurantPhrase, restaurantPhrase, badSushiPhrase);
			task.AddQAAndAfterChat(coffeeShopPhrase, coffeeShopPhrase, goodCoffeePhrase);
			task.AddQAAndAfterChat (storePhrase, storePhrase, helpfulPhrase);
			//street targets
			task.StreetTarget.Add(coffeeShop);
			task.StreetTarget.Add(restaurant);
			task.StreetTarget.Add(groceryStore);
			//other initialization
			Initialize("Point Place Task 1", "StreetSession", "Asker");
			SetProcess<PointPlaceProcess>();
		}
		#endregion
		
	}

	public class PointPlace02 : StaticSerializedTaskGameData<PointPlaceTaskData> {
		public const string classroom1 = "classroom 1";
		public const string classroom2 = "classroom 2";
		public const string restroom = "restroom";
		public const string staircase = "staircase";
		public const string exit = "exit";

		public const string notLateForClass = "Thank you for not letting me be late for class";
		public const string helpALot = "You saved me!";

		#region implemented abstract members of StaticGameData
		protected override void PrepareGameData ()
		{
			task = new PointPlaceTaskData ();
			//set dialogue
			SetDialogue<PointPlaceDialogue01>();
			//set afterchat phrase
			var notLatePhrase = GetPhrase(notLateForClass);
			var helpALotPhrase = GetPhrase(helpALot);
			//set question and answer data
			var classroom1Phrase = GetPhrase(classroom1);
			var classroom2Phrase = GetPhrase(classroom2);
			var restroomPhrase = GetPhrase(restroom);
			var staircasePhrase = GetPhrase(staircase);
			var exitPhrase = GetPhrase(exit);
			//TODO : enable when we have the phrase
			task.AddQAAndAfterChat(classroom1Phrase, classroom1Phrase, notLatePhrase);
			task.AddQAAndAfterChat(restroomPhrase, restroomPhrase, helpALotPhrase);
			task.AddQAAndAfterChat (classroom2Phrase, classroom2Phrase, notLatePhrase);
			task.AddQAAndAfterChat (staircasePhrase, staircasePhrase, helpALotPhrase);
			task.AddQAAndAfterChat (exitPhrase, exitPhrase, helpALotPhrase);
			//street targets TODO : change according to scene
			task.StreetTarget.Add(classroom1);
			task.StreetTarget.Add(classroom2);
			task.StreetTarget.Add(restroom);
			task.StreetTarget.Add(exit);
			task.StreetTarget.Add(staircase);
			//other initialization
			Initialize("Point Place Task 2", "SchoolHallway2", "Asker");
			SetProcess<PointPlaceProcess>();
		}
		#endregion
		
	}
}