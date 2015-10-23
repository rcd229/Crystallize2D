using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrystallizeData{
	public class CashierSellGoods : StaticSerializedTaskGameData<CashierTaskData> {

		protected override void PrepareGameData() {

			task = new CashierTaskData();
			var lines = task.Lines;
			//initialize inference dialogues, should be done in serialized files
//			lines.Add(createLine("Hi", "all" ,"Normal", "Morning", "Evening"));
			lines.Add(createLine("Hello", "all", "Normal", "Morning", "Evening"));
			lines.Add(createLine( "Can I help you?", "cashier", "Normal", "Morning", "Evening"));
			lines.Add(createLine("Good Morning", "all", "Morning"));
			lines.Add(createLine("Good Evening", "all", "Evening"));
			//initialize shop lists
			task.ShopLists.Add (createValuedItem("book", 500));
			task.ShopLists.Add (createValuedItem("umbrella", 200));
			task.ShopLists.Add (createValuedItem("water", 200));
			task.ShopLists.Add (createValuedItem("bento", 600));
			task.ShopLists.Add (createValuedItem("flower", 400));
			task.ShopLists.Add (createValuedItem("tea", 600));

			task.andPhrase = GetPhrase("and");
			task.yenPhrase = GetPhrase("yen");

			Initialize("Sell goods as cashier", "StreetSession", "Customer01");
			SetProcess<CashierProcess>();
			AddDialogues<CashierDialogue01>();
			AddDialogues<CashierDialogue02>();
		}

		//Convinience functions for task creation
		InferenceDialogueLine createLine(string text, string tag, params string[] category){
			InferenceDialogueLine line = new InferenceDialogueLine(new List<string>(category), tag);
			line.Phrase = GetPhrase(text);
			return line;
		}
		
		ValuedItem createValuedItem(string t, int i){
			ValuedItem item = new ValuedItem();
			item.Text = GetPhrase(t);
			item.Value = i;
			item.ValueText = GetPhrase(i.ToString());
			return item;
		}
	}

	public class OpenCashierTask : StaticSerializedTaskGameData<CashierTaskData> {
		
		protected override void PrepareGameData() {
			
			task = new CashierTaskData();
			var lines = task.Lines;
			//initialize inference dialogues, should be done in serialized files
			//			lines.Add(createLine("Hi", "all" ,"Normal", "Morning", "Evening"));
			lines.Add(createLine("Hello", "all", "Normal", "Morning", "Evening"));
			lines.Add(createLine( "Can I help you?", "cashier", "Normal", "Morning", "Evening"));
			lines.Add(createLine("Good Morning", "all", "Morning"));
			lines.Add(createLine("Good Evening", "all", "Evening"));
			//initialize shop lists
			task.ShopLists.Add (createValuedItem("book", 500));
			task.ShopLists.Add (createValuedItem("umbrella", 200));
			task.ShopLists.Add (createValuedItem("water", 200));
			task.ShopLists.Add (createValuedItem("bento", 600));
			task.ShopLists.Add (createValuedItem("flower", 400));
			task.ShopLists.Add (createValuedItem("tea", 600));
			
			task.andPhrase = GetPhrase("and");
			task.yenPhrase = GetPhrase("yen");
			task.AreaName = "CashierArea";
			
			Initialize("Sell goods as cashier", "StreetSession", "Customer01");
			SetProcess<OpenCashierProcess>();
			AddDialogues<CashierDialogue02>();
			SetDialogue<CashierStartDialogue>();
		}
		
		//Convinience functions for task creation
		InferenceDialogueLine createLine(string text, string tag, params string[] category){
			InferenceDialogueLine line = new InferenceDialogueLine(new List<string>(category), tag);
			line.Phrase = GetPhrase(text);
			return line;
		}
		
		ValuedItem createValuedItem(string t, int i){
			ValuedItem item = new ValuedItem();
			item.Text = GetPhrase(t);
			item.Value = i;
			item.ValueText = GetPhrase(i.ToString());
			return item;
		}
	}
	
	public class CashierStartDialogue : StaticSerializedDialogueGameData {
		
		protected override void PrepareGameData() {
			//AddActor("");
			AddLine("Ah, the new recruit has arrived.");
			AddLine("Let's get started.");
			AddLine("Help the customers");
			AddAnimation(new GestureDialogueAnimation(AnimatorState.Stand));
		}
		
	}
}
