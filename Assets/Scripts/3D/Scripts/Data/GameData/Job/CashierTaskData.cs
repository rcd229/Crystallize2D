using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CashierTaskData : InferenceTaskGameData {
	//number of items a customer will buy
	public int NumItem { get; set;}

	public PhraseSequence andPhrase;
	public PhraseSequence yenPhrase;
//	public DialogueActorLine priceLine;

	public List<ValuedItem> ShopLists { get; set;}

	public CashierTaskData() : base() {
		NumItem = 0;
		ShopLists = new List<ValuedItem> ();
	}
}
